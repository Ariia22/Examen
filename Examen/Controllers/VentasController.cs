using Microsoft.AspNetCore.Mvc;

namespace Examen.Controllers
{
    public class VentasController: Controller
    {

        private readonly VentasService _ventasService;

        public VentasController(VentasService ventasService)
        {
            _ventasService = ventasService;
        }

        public async Task<IActionResult> VentaMenorMonto()
        {
            var ventas = await _ventasService.ObtenerVentaConMenorMontoAsync();
            return View(ventas);
        }
    }
}
