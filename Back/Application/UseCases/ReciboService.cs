using Application.DTOs.Request.Recibo;
using Application.DTOs.Response.Recibo;
using Application.Exceptions;
using Application.Interfaces.Cobro;
using Application.Interfaces.Recibo;
using Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Services
{
    public class ReciboService : IReciboService
    {
        private readonly IReciboCommand _command;
        private readonly IReciboQuery _query;
        private readonly ICobroQuery _cobroQuery;

        public ReciboService(IReciboCommand command, IReciboQuery query,ICobroQuery cobroQuery)
        {
            _command = command;
            _query = query;
            _cobroQuery = cobroQuery;
        }

        public async Task<ReciboResponse> RegistrarRecibo(
    RegistrarReciboRequest request,
    CancellationToken ct = default)
        {
            if (request == null)
                throw new ExceptionBadRequest("Debe ingresar datos");

            if (request.IdCobro <= 0)
                throw new ExceptionBadRequest("Id de cobro inválido");

            var cobro = await _cobroQuery.ConsultarCobro(request.IdCobro, ct);

            if (cobro == null)
                throw new ExceptionNotFound("Cobro no encontrado");

            // =========================
            // VALIDACIÓN DE NEGOCIO
            // =========================
            if (!cobro.IdReserva.HasValue)
                throw new ExceptionBadRequest(
                    "El cobro no tiene una reserva asociada y no se puede generar el recibo"
                );

            // =========================
            // CREAR RECIBO
            // =========================
            var recibo = new Recibo
            {
                IdCobro = cobro.IdCobro,
                IdReserva = cobro.IdReserva.Value,
                MontoTotal = cobro.MontoTotal,
                FechaEmision = DateTime.Now
            };

            var resultado = await _command.RegistrarRecibo(recibo, ct);

            // =========================
            // RESPONSE
            // =========================
            return new ReciboResponse
            {
                Id_Recibo = resultado.IdRecibo,
                Id_Cobro = resultado.IdCobro,
                Id_Reserva = resultado.IdReserva,
                MontoTotal = resultado.MontoTotal,
                FechaEmision = resultado.FechaEmision
            };
        }

        public async Task<ReciboResponse> ModificarRecibo(ModificarReciboRequest request, CancellationToken ct = default)
        {
            var reciboExistente = await _query.ConsultarRecibo(request.IdRecibo, ct);
            if (reciboExistente == null)
            {
                throw new Exception($"No se encontró el recibo con ID {request.IdRecibo}");
            }

            reciboExistente.IdCobro = request.IdCobro;
            reciboExistente.IdReserva = request.IdReserva;
            reciboExistente.MontoTotal = request.MontoTotal;
            reciboExistente.FechaEmision = request.FechaEmision;

            var resultado = await _command.ModificarRecibo(reciboExistente, ct);

            return new ReciboResponse
            {
                Id_Recibo = resultado.IdRecibo,
                Id_Cobro = resultado.IdCobro,
                Id_Reserva = resultado.IdReserva,
                MontoTotal = resultado.MontoTotal,
                FechaEmision = resultado.FechaEmision
            };
        }

        public async Task<ReciboResponse> ConsultarRecibo(int idRecibo, CancellationToken ct = default)
        {
            var recibo = await _query.ConsultarRecibo(idRecibo, ct);
            if (recibo == null)
            {
                throw new ExceptionNotFound($"No se encontró el recibo con ID {idRecibo}");
            }
            var cobro = await _cobroQuery.ConsultarCobro(recibo.IdCobro);


            return new ReciboResponse
            {
                Id_Recibo = recibo.IdRecibo,
                Id_Cobro = recibo.IdCobro,
                Id_Reserva = (int)cobro.IdReserva,
                MontoTotal = cobro.MontoTotal,
                FechaEmision = recibo.FechaEmision
            };
        }

        public async Task<ReciboResponse> ImprimirRecibo(int idRecibo, CancellationToken ct = default)
        {
            // El diagrama pide Imprimir, usamos la consulta específica para traer el modelo listo para salida
            var recibo = await _query.ImprimirRecibo(idRecibo, ct);
            if (recibo == null)
            {
                throw new Exception($"No se pudo generar la impresión. No existe el recibo con ID {idRecibo}");
            }

            return new ReciboResponse
            {
                Id_Recibo = recibo.IdRecibo,
                Id_Cobro = recibo.IdCobro,
                Id_Reserva = recibo.IdReserva,
                MontoTotal =(double) recibo.MontoTotal,
                FechaEmision = recibo.FechaEmision
            };
        }

        public async Task<bool> EliminarRecibo(EliminarReciboRequest request, CancellationToken ct = default)
        {
            var recibo = await _query.ConsultarRecibo(request.Id_Recibo, ct);
            if (recibo == null)
            {
                throw new Exception($"No se encontró el recibo a eliminar con ID {request.Id_Recibo}");
            }

            await _command.EliminarRecibo(recibo, ct);
            return true;
        }
    }
}