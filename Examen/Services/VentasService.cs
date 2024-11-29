using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Examen.Models;

public class VentasService
{
    private readonly string _connectionString;

    public VentasService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<VentaConMenorMonto>> ObtenerVentaConMenorMontoAsync()
    {
        const string query = @"
            SELECT 
                CONCAT(u.nombre, ' ', u.apellidoPaterno, ' ', u.apellidoMaterno) AS NombreCompleto,
                COUNT(it.id_item_ticket) AS NumeroDeProductos, 
                t.total AS MontoTotal 
            FROM TICKETS t 
            JOIN ITEMSTICKET it ON t.id_ticket = it.id_ticket 
            JOIN USUARIOS u ON t.id_usuario = u.id_usuario 
            WHERE t.total = (SELECT MIN(total) FROM TICKETS) 
            GROUP BY u.id_usuario, u.nombre, u.apellidoPaterno, u.apellidoMaterno, t.total;";

        using (var connection = new SqlConnection(_connectionString))
        {
            var result = await connection.QueryAsync<VentaConMenorMonto>(query);
            return result.ToList();
        }
    }
}
