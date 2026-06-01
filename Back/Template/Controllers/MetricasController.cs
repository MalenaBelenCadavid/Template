using Application.Interfaces.MetricasService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Template.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MetricasController : ControllerBase
    {
        private readonly IMetricasService _metricasService;

        public MetricasController(IMetricasService metricasService)
        {
            _metricasService = metricasService;
        }

        [HttpGet("ingresosPorReserva")]
        public async Task<IActionResult> IngresosPorReservas() {

            var response = await _metricasService.VerTotalDeIngresosPorReserva();
            return Ok(response);
            
        }
        [HttpGet("ingresosPorInscripcion")]
        public async Task<IActionResult> IngresosPorInscripcion()
        {

            var response = await _metricasService.VerTotalDeIngresosPorInscripciones();
            return Ok(response);

        }

        [HttpGet("canchasMasReservadas")]
        public async Task<IActionResult> MasReservadas()
        {

            var response = await _metricasService.CanchasMasReservadas();
            return Ok(response);

        }
    }
}
