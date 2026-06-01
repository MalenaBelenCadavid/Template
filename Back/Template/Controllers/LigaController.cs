using Application.Interfaces.Competencias;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Competencias;


namespace Template.Controllers
{ 
        [Route("api/v1/[controller]")]
        [ApiController]
        public class LigaController : ControllerBase
        {
            private readonly ILigaService _serviceliga;
            public LigaController(ILigaService serviceliga)
            {
                _serviceliga = serviceliga;
            }

            [HttpPost("GenerarFixture")]
            public async Task<IActionResult> GenerarFixture(int idLiga, CancellationToken ct)
            {
                await _serviceliga.GenerarFixture(idLiga, ct);
                return NoContent();
            }

            [HttpGet("ObtenerTabla/{idLiga}")]
            public async Task<IActionResult> ConsultarCompetencia(int idLiga, CancellationToken ct)
            {
                var response = await _serviceliga.ObtenerTabla(idLiga, ct);
                return Ok(response);
            }
            [HttpPut("AgregarResultado")]
            public async Task<IActionResult> CargarResultado(int idPartido, int golesLocal, int golesVis, CancellationToken ct)
            {
                await _serviceliga.CargarResultado(idPartido, golesLocal, golesVis, ct);
                return NoContent();
            }

        [HttpDelete("EliminarFixture/{idLiga}")]
            public async Task<IActionResult> EliminarFixture(int idLiga, CancellationToken ct)
            {
                await _serviceliga.EliminarFixture(idLiga, ct);
                return NoContent();
            }
            [HttpPost("RehacerFixture/{idLiga}")]
            public async Task<IActionResult> RehacerFixture(int idLiga, CancellationToken ct)
            {
                await _serviceliga.RehacerFixture(idLiga, ct);
                return NoContent();
        }
    }
}