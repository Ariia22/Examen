using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Examen.Controllers
{
    public class VentasController : Controller
    {
        private readonly IConfiguration _configuration;

        public VentasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            var ventas = new List<Venta>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CONCAT(u.nombre, ' ', u.apellidoPaterno, ' ', u.apellidoMaterno) AS NombreCompleto, 
                           COUNT(it.id_item_ticket) AS NumeroDeProductos, 
                           t.total AS MontoTotal 
                    FROM TICKETS t 
                    JOIN ITEMSTICKET it ON t.id_ticket = it.id_ticket 
                    JOIN USUARIOS u ON t.id_usuario = u.id_usuario 
                    WHERE t.total = (SELECT MIN(total) FROM TICKETS) 
                    GROUP BY u.id_usuario, u.nombre, u.apellidoPaterno, u.apellidoMaterno, t.total;";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ventas.Add(new Venta
                    {
                        NombreCompleto = reader["NombreCompleto"].ToString(),
                        NumeroDeProductos = (int)reader["NumeroDeProductos"],
                        MontoTotal = (decimal)reader["MontoTotal"]
                    });
                }
            }

            return View(ventas);
        }
    }
}
