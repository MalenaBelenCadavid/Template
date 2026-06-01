using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Competencias;
using Application.DTOs.Request.Competencias;


namespace Template.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompetenciaController : ControllerBase
    {
        private readonly ICompetenciaService _service;
        public CompetenciaController(ICompetenciaService service)
        {
            _service = service;
        }

        [HttpPost("CrearCompetencias")]
        public async Task<IActionResult> CrearCompetencia([FromBody] CrearCompetenciaRequest request, CancellationToken ct)
        {
            var response = await _service.CrearCompetencia(request);
            return CreatedAtAction(nameof(ConsultarCompetencia), new { IdCompetencia = response }, response);
        }

        [HttpGet("{IdCompetencia}")]
        public async Task<IActionResult> ConsultarCompetencia(int IdCompetencia)
        {
            var response = await _service.ObtenerCompetenciaPorId(IdCompetencia);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> ConsultarCompetencias()
        {
            var response = await _service.ObtenerTodasLasCompetencias();
            return Ok(response);
        }

        [HttpPut("{CompetenciaId}")]
        public async Task<IActionResult> ModificarCompetencia(int CompetenciaId, [FromBody] ModificarCompetenciaRequest request, CancellationToken ct)
        {
            await _service.ModificarCompetencia(CompetenciaId, request, ct);
            return NoContent();
        }

        [HttpDelete("{CompetenciaId}")]
        public async Task<IActionResult> EliminarCompetencia(int CompetenciaId)
        {
            await _service.EliminarCompetencia(CompetenciaId);
            return NoContent();
        }
        [HttpGet("Competencias")]
        public async Task<IActionResult> ObtenerTodasLasCompetencias()
        {
            var result = await _service.ObtenerTodasLasCompetencias();

            return Ok(result);
        }
        [HttpPost("AgregarEquipo")]
        public async Task<IActionResult> AgregarEquipo([FromBody] AgregarEquipoRequest request, int idCompetencia, CancellationToken ct)
        {
            await _service.AgregarEquipo(request, idCompetencia, ct);
            return NoContent();
        }
        /*[HttpPost("GenerarFixture")]
        public async Task<IActionResult> GenerarFixture(int idCompetencia, CancellationToken ct)
        {
            await _service.GenerarFixture(idCompetencia, ct);
            return NoContent();
        }*/
        [HttpGet("DevolverPartidos")]
        public async Task<IActionResult> ObtenerPartidos(int idCompetencia, CancellationToken ct)
        {
            var result = await _service.ObtenerPartidos(idCompetencia, ct);
            return Ok(result);
        }
    }
}