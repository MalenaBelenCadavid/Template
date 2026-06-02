using Application.DTOs.Request.Cobro;
using Application.DTOs.Response.Cobro;
using Application.Exceptions;
using Application.Interfaces.Cliente;
using Application.Interfaces.Cobro;

namespace Application.UseCases
{
    public class CobroService : ICobroService
    {
        private readonly ICobroCommand _cobroCommand;
        private readonly ICobroQuery _cobroQuery;
        private readonly IClienteQuery _clienteQuery;

        public CobroService(ICobroCommand cobroCommand, ICobroQuery cobroQuery,IClienteQuery clienteQuery)
        {
            _cobroCommand = cobroCommand;
            _cobroQuery = cobroQuery;
            _clienteQuery = clienteQuery;
        }

        public async Task<CobroResponse> RegistrarCobro(RegistrarCobroRequest request, CancellationToken ct = default)
        {
            if (request == null)
            {
                throw new ExceptionBadRequest("Debe ingresar datos válidos");
            }
            if (request.IdReserva == null && request.IdInscripcion == null)
                throw new ExceptionBadRequest("Debe enviar una reserva o inscripción");

            if (request.IdReserva != null && request.IdInscripcion != null)
                throw new ExceptionBadRequest("Solo puede enviar uno");

            if (request.MontoTotal <= 0)
            {
                throw new ExceptionBadRequest("El monto total debe ser mayor a cero");
            }
            if (request.DniCliente <= 0)
            {
                throw new ExceptionBadRequest("Ingrese un dni valido");
            }
            var cliente = await _clienteQuery.ConsultarCliente(request.DniCliente) ?? throw new ExceptionNotFound("Cliente no entrado");

            var cobro = new Domain.Entities.Cobro
            {
                IdReserva = request.IdReserva,
                IdInscripcion=request.IdInscripcion,
                DniCliente=cliente.Dni,
                MetodoPago=request.MetodoPago,
                Nombre=cliente.Nombre,
                Apellido=cliente.Apellido,
                Motivo=request.Motivo,
                MontoTotal = (double)request.MontoTotal,
                EstaCompleto = true
            };

            var cobroRegistrado = await _cobroCommand.RegistrarCobro(cobro, ct);

            return new CobroResponse
            {
                Id_Cobro = cobroRegistrado.IdCobro, 
                Id_Reserva = cobroRegistrado.IdReserva,   
                IdInscripcion= cobroRegistrado.IdInscripcion,
                clienteDni=cobroRegistrado.DniCliente,
                metodoPago=cobroRegistrado.MetodoPago,
                EstaCompleto = cobroRegistrado.EstaCompleto,
                MontoTotal = (decimal)cobroRegistrado.MontoTotal
            };
        }

        public async Task<CobroResponse> ModificarCobro(ModificarCobroRequest request, CancellationToken ct = default)
        {
            if (request == null)
            {
                throw new ExceptionBadRequest("Debe ingresar datos válidos");
            }

            if (request.MontoTotal <= 0)
            {
                throw new ExceptionBadRequest("El monto total debe ser mayor a cero");
            }

            var cobro = await _cobroQuery.ConsultarCobro(request.Id_Cobro, ct);
            if (cobro == null)
            {
                throw new ExceptionNotFound("Cobro no encontrado");
            }

            cobro.EstaCompleto = request.EstaCompleto;
            cobro.MontoTotal = (double)request.MontoTotal;

            var cobroActualizado = await _cobroCommand.ModificarCobro(cobro, ct);

            return new CobroResponse
            {
                Id_Cobro = cobroActualizado.IdCobro,
                Id_Reserva = (int)cobroActualizado.IdReserva,
                IdInscripcion= (int)cobroActualizado.IdInscripcion,
                clienteDni = cobroActualizado.DniCliente,
                metodoPago = cobroActualizado.MetodoPago,
                EstaCompleto = cobroActualizado.EstaCompleto,
                MontoTotal = (decimal)cobroActualizado.MontoTotal
            };
        }

        public async Task<CobroResponse> ConsultarCobro(int idCobro, CancellationToken ct = default)
        {
            if (idCobro <= 0)
                throw new ExceptionBadRequest("Debe ingresar un ID válido");

            var cobro = await _cobroQuery.ConsultarCobro(idCobro, ct);

            if (cobro == null)
                throw new ExceptionNotFound("Cobro no encontrado");

            return new CobroResponse
            {
                Id_Cobro = cobro.IdCobro,
                Id_Reserva = cobro.IdReserva ?? 0,   // ✔ safe
                IdInscripcion = cobro.IdInscripcion ?? 0, // ✔ safe
                clienteDni = cobro.DniCliente,
                metodoPago = cobro.MetodoPago,
                EstaCompleto = cobro.EstaCompleto,
                MontoTotal = (decimal)cobro.MontoTotal // ✔ safe
            };
        }

        public async Task<bool> EliminarCobro(EliminarCobroRequest request, CancellationToken ct = default)
        {
            if (request == null || request.Id_Cobro <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un ID válido");
            }

            var cobro = await _cobroQuery.ConsultarCobro(request.Id_Cobro, ct);
            if (cobro == null)
            {
                throw new ExceptionNotFound("Cobro no encontrado");
            }

            await _cobroCommand.EliminarCobro(cobro, ct);
            return true;
        }

        public async Task ImprimirCobro(int idCobro, CancellationToken ct = default)
        {
            if (idCobro <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un ID válido");
            }

            var cobro = await _cobroQuery.ConsultarCobro(idCobro, ct);
            if (cobro == null)
            {
                throw new ExceptionNotFound("Cobro no encontrado");
            }

            System.Diagnostics.Debug.WriteLine($"[IMPRESIÓN] Comprobante Cobro ID: {cobro.IdCobro} - Total: {cobro.MontoTotal}");
        }
    }
}
