using Application.DTOs.Request.Asistencia;
using Application.DTOs.Request.Clase;
using Application.Interfaces.Clase;
using Microsoft.AspNetCore.Mvc;

namespace Template.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClaseController : ControllerBase
    {
        private readonly IClaseService _service;
        public ClaseController(IClaseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> ProgramarClase([FromBody] ProgramarClasesRequest request)
        {
            var response = await _service.ProgramarClase(request);
            return CreatedAtAction(nameof(ConsultarClase), new { claseId = response.IdClase }, response);
        }
        [HttpPost("asistencia/{claseId}")]
        public async Task<IActionResult> CrearAsistencia([FromRoute] int claseId,[FromBody] List<RegistrarAsistenciaRequest> request)
        {
            var response = await _service.RegistrarAsistencia(claseId,request);
            return Ok(response);
        }
        [HttpGet("inscriptos/{claseId}")]
        public async Task<IActionResult> VerInscriptos([FromRoute] int claseId)
        {
            var response = await _service.VerInscriptos(claseId);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarClase(int id,ModificarClaseRequest request) {
            var response = await _service.ModificarClase(id,request);
            return Ok(response);
        }

        [HttpGet("{claseId}")]
        public async Task<IActionResult> ConsultarClase(int claseId) {

            var response = await _service.ConsultarClase(claseId);
            return Ok(response);

        }
        [HttpGet("clases/{profesorDni}")]
        public async Task<IActionResult> VerClasePorProfesor(int profesorDni)
        {

            var response = await _service.VerClasesPorProfesorDni(profesorDni);
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> ListarClases()
        {
            var response = await _service.ListarClases();
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarClase(int id)
        {
            var response = await _service.EliminarClase(id);
            return Ok(response);
        }



    }
}
