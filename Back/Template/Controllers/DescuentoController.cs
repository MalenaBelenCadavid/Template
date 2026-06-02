using Application.Exceptions;
using Application.Interfaces;
using Application.Request;
using Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Template.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DescuentoController : ControllerBase
    {
        private readonly IDescuentoService _descuentoService;

        public DescuentoController(IDescuentoService descuentoService)
        {
            _descuentoService = descuentoService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DescuentoResponse), 201)]
        public async Task<IActionResult> CreateDescuento(CrearDescuentoRequest request)
        {
            try
            {
                var result = await _descuentoService.CreateDescuento(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (ExceptionBadRequest ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DescuentoResponse), 200)]
        public async Task<IActionResult> UpdateDescuento(ModificarDescuentoRequest request, int id)
        {
            try
            {
                var result = await _descuentoService.UpdateDescuento(request, id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ExceptionNotFound ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
            catch (ExceptionBadRequest ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DescuentoResponse), 200)]
        public async Task<IActionResult> GetDescuentoById(int id)
        {
            try
            {
                var result = await _descuentoService.GetDescuentoById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ExceptionNotFound ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DescuentoResponse>), 200)]
        public async Task<IActionResult> GetAllDescuentos()
        {
            var result = await _descuentoService.GetAllDescuentos();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DescuentoResponse), 200)]
        public async Task<IActionResult> DeleteDescuento(int id)
        {
            try
            {
                var result = await _descuentoService.DeleteDescuento(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ExceptionNotFound ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
        }

        // GET api/Descuento/vigente/Reserva
        [HttpGet("vigente/{tipo}")]
        [ProducesResponseType(typeof(DescuentoResponse), 200)]
        public async Task<IActionResult> GetDescuentoActivoPorTipo(string tipo)
        {
            try
            {
                var result = await _descuentoService.GetDescuentoActivoPorTipo(tipo);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ExceptionNotFound ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
        }
    }
}
