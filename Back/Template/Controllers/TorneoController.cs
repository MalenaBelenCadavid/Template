using Application.Interfaces.Competencias;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Competencias;


namespace Template.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TorneoController : ControllerBase
    {
        private readonly ITorneoService _serviceTorneo;
        public TorneoController(ITorneoService serviceTorneo)
        {
            _serviceTorneo = serviceTorneo;
        }

        [HttpPost("GenerarFixture")]
        public async Task<IActionResult> GenerarFixture(int idTorneo, CancellationToken ct)
        {
            await _serviceTorneo.GenerarFixture(idTorneo, ct);
            return NoContent();
        }

        [HttpPut("AgregarResultado")]
        public async Task<IActionResult> CargarResultado(int idPartido, int golesLocal, int golesVis, CancellationToken ct)
        {
            await _serviceTorneo.CargarResultado(idPartido, golesLocal, golesVis, ct);
            return NoContent();
        }
        [HttpGet("ObtenerCuadro")]
        public async Task<IActionResult> ObtenerCuadroTorneo(int idTorneo, CancellationToken ct)
        {
            var cuadro = await _serviceTorneo.ObtenerCuadroTorneo(idTorneo, ct);
            return Ok(cuadro);
        }
            [HttpPut("DescalificarEquipo")]
           public async Task<IActionResult> DescalificarEquipo(int idEquipo, CancellationToken ct)
        {
            await _serviceTorneo.DescalificarEquipo(idEquipo, ct);
            return NoContent();
        }
        [HttpDelete("EliminarEquipos")]
        public async Task<IActionResult> EliminarEquipos(int idTorneo, CancellationToken ct)
        {
            await _serviceTorneo.EliminarFixture(idTorneo, ct);
            return NoContent();
        }
        [HttpPost("RehacerFixture")]
            public async Task<IActionResult> RehacerFixture(int idTorneo, CancellationToken ct)
        {
            await _serviceTorneo.RehacerFixture(idTorneo, ct);
            return NoContent();
        }
    }
}