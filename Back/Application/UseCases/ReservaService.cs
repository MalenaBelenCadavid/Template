using Application.DTOs.Request.Reserva;
using Application.DTOs.Response.Reserva;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Cancha;
using Application.Interfaces.Cliente;
using Application.Interfaces.Cobro;
using Application.Interfaces.HorarioCancha;
using Application.Interfaces.Reserva;
using Application.Interfaces.TipoCancha;
using Domain.Entities;

namespace Application.UseCases
{
    public class ReservaService : IReservaServices
    {
        private readonly IReservaCommand _reservaCommand;
        private readonly IReservaQuery _reservaQuery;
        private readonly ICanchaQuery _canchaQuery;
        private readonly IHorarioCanchaQuery _horarioCanchaQuery;
        private readonly IHorarioCanchaCommand _horarioCanchaCommand;
        private readonly ICobroQuery _cobroQuery;
        private readonly ICobroCommand _cobroCommand;
        private readonly IDescuentoQuery _descuentoQuery;
        private readonly IClienteQuery _clienteQuery;
        private readonly ITipoCanchaQuery _tipoCancha;
        public ReservaService(
            IReservaCommand reservaCommand,
            IReservaQuery reservaQuery,
            ICanchaQuery canchaQuery,
            IHorarioCanchaQuery horarioCanchaQuery,
            IHorarioCanchaCommand horarioCanchaCommand,
            ICobroCommand cobroCommand,
            ICobroQuery cobroQuery,
            IDescuentoQuery descuentoQuery,
            IClienteQuery clienteQuery,
            ITipoCanchaQuery tipoCanchaQuery)
        {
            _reservaCommand = reservaCommand;
            _reservaQuery = reservaQuery;
            _canchaQuery = canchaQuery;
            _horarioCanchaQuery= horarioCanchaQuery;
            _horarioCanchaCommand = horarioCanchaCommand;
            _cobroCommand = cobroCommand;
            _cobroQuery= cobroQuery;
            _descuentoQuery = descuentoQuery;
            _clienteQuery= clienteQuery;
            _tipoCancha = tipoCanchaQuery;
        }

        public async Task<ReservaResponse> CrearReserva(CrearReservaRequest request)
        {
            if (request == null)
            {
                throw new ExceptionBadRequest("Debe ingresar datos");
            }

            if (request.DniCliente <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un DNI valido");
            }

            if (request.IdCancha <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar una cancha valida");
            }

            var cancha = await _canchaQuery.ConsultarCancha(request.IdCancha) ?? throw new ExceptionNotFound("La cancha no existe");

            if (request.IdCanchaHorario <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un horario valido");
            }

            var cliente = await _clienteQuery.ConsultarCliente(request.DniCliente) ?? throw new ExceptionNotFound("Cliente no encontrado");

            var descuento = await _descuentoQuery.GetDescuentoActivoPorTipo("Reserva");
            if (descuento == null)
                descuento = await _descuentoQuery.GetDescuentoActivoPorTipo("General");
            var horarioCancha = await _horarioCanchaQuery.ConsultarHorarioCancha(request.IdCanchaHorario);
            if (horarioCancha == null)
            {
                throw new ExceptionBadRequest("Debe un horario valido");
            }

            if (horarioCancha.IdCancha != request.IdCancha)
            {
                throw new ExceptionBadRequest("El horario solicitado no pertenece a la cancha");
            }

            if (await _reservaQuery.ExisteReserva(request.IdCanchaHorario, request.Fecha))
            {
                throw new ExceptionConflict("El día que intenta reservar ya fue reservado");
            }
            var tipoCancha = await _tipoCancha.ObtenerTipoCancha(cancha.TipoCanchaId);
            decimal precioFinal = tipoCancha.Precio;



            Console.WriteLine("Descuento: " + (descuento?.Valor ?? 0));
            if (cliente.EsSocio && descuento != null) 
            {
                decimal porcentaje = descuento.Valor / 100m;
                precioFinal = precioFinal - (precioFinal * porcentaje);
            }

            var reserva = new Reserva
            {
                DniCliente = request.DniCliente,
                IdCancha = request.IdCancha,
                IdCanchaHorario = request.IdCanchaHorario,
                MontoTotal =(int)precioFinal,
                Fecha=request.Fecha,
                EsValida = true,
                Cancha = cancha
            };

            var reservaCreada = await _reservaCommand.CrearReserva(reserva);
            return new ReservaResponse
            {
                ReservaId = reservaCreada.IdReserva,
                DniCliente = reservaCreada.DniCliente,
                ReservaHorarioCanchaResponse = new DTOs.Response.HorarioCancha.ReservaHorarioCanchaResponse
                {
                    IdCanchaHorario= horarioCancha.Id,
                    Fecha = reserva.Fecha,
                    HoraInicio= horarioCancha.HoraInicio,
                    HoraFin= horarioCancha.HoraFin,
                },
                Total = reservaCreada.MontoTotal,
                NombreCancha=reservaCreada.Cancha.Nombre,
                esValida=true
            };
        }

        public async Task<ReservaResponse> ConsultarReserva(int reservaId)
        {
            if (reservaId <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un id valido");
            }

            var reserva = await _reservaQuery.ConsultarReserva(reservaId);

            if (reserva == null)
            {
                throw new ExceptionNotFound("Reserva no encontrada");
            }

            return new ReservaResponse
            {
                ReservaId = reserva.IdReserva,
                DniCliente = reserva.DniCliente,
                ReservaHorarioCanchaResponse = new DTOs.Response.HorarioCancha.ReservaHorarioCanchaResponse
                {
                    IdCanchaHorario = reserva.HorarioCancha.Id,
                    Fecha = reserva.Fecha,
                    HoraInicio = reserva.HorarioCancha.HoraInicio,
                    HoraFin = reserva.HorarioCancha.HoraFin,
                },
                Total = reserva.MontoTotal,
                NombreCancha = reserva.Cancha.Nombre,
                esValida=reserva.EsValida
            };
        }

        public async Task<List<ReservaResponse>> ListarReservas()
        {
            var reservas = await _reservaQuery.ListarReservas();

            return reservas.Select(r => new ReservaResponse
            {
                ReservaId = r.IdReserva,
                DniCliente = r.DniCliente,
                ReservaHorarioCanchaResponse = new DTOs.Response.HorarioCancha.ReservaHorarioCanchaResponse
                {
                    IdCanchaHorario = r.HorarioCancha.Id,
                    Fecha = r.Fecha,
                    HoraInicio = r.HorarioCancha.HoraInicio,
                    HoraFin = r.HorarioCancha.HoraFin,
                },
                Total = r.MontoTotal,
                NombreCancha = r.Cancha.Nombre,
                esValida=r.EsValida


            }).ToList();
        }

        public async Task<ReservaResponse> ModificarReserva(ActualizarReservaRequest request)
        {
            if (request == null)
            {
                throw new ExceptionBadRequest("Debe ingresar datos");
            }

           
            var reserva = await _reservaQuery.ConsultarReserva(request.ReservaId);

            if (reserva == null)
            {
                throw new ExceptionNotFound("Reserva no encontrada");
            }

            if (request.IdHorarioCancha==null)
            {
                throw new ExceptionNotFound("Reserva no encontrada");
            }

            var horarioCancha = await _horarioCanchaQuery.ConsultarHorarioCancha(request.IdHorarioCancha);

            reserva.IdCanchaHorario = request.IdHorarioCancha;

            var reservaActualizada = await _reservaCommand.ModificarReserva(reserva);

            return new ReservaResponse
            {
                ReservaId = reservaActualizada.IdReserva,
                DniCliente = reservaActualizada.DniCliente,
                ReservaHorarioCanchaResponse = new DTOs.Response.HorarioCancha.ReservaHorarioCanchaResponse
                {
                    IdCanchaHorario = horarioCancha.Id,
                    Fecha = reservaActualizada.Fecha,
                    HoraInicio = horarioCancha.HoraInicio,
                    HoraFin = horarioCancha.HoraFin,
                },
                Total = reservaActualizada.MontoTotal,
                NombreCancha = reservaActualizada.Cancha.Nombre,
                esValida= reservaActualizada.EsValida,


            };
        }

        public async Task<ReservaResponse> EliminarReserva(int reservaId)
        {
            if (reservaId <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un id valido");
            }

            var reserva = await _reservaQuery.ConsultarReserva(reservaId);


            if (reserva == null)
            {
                throw new ExceptionNotFound("Reserva no encontrada");
            }

            var cobro = await _cobroQuery.ConsultarCobro(reserva.Cobro.IdCobro);

            

            if (cobro == null)
            {
                throw new ExceptionNotFound("Cobro no encontrado");
            }

            await _cobroCommand.EliminarCobro(cobro);

            var response = new ReservaResponse
            {
                ReservaId = reserva.IdReserva,
                DniCliente = reserva.DniCliente,
                ReservaHorarioCanchaResponse = new DTOs.Response.HorarioCancha.ReservaHorarioCanchaResponse
                {
                    IdCanchaHorario = reserva.HorarioCancha.Id,
                    Fecha = reserva.Fecha,
                    HoraInicio = reserva.HorarioCancha.HoraInicio,
                    HoraFin = reserva.HorarioCancha.HoraFin,
                },
                Total = reserva.MontoTotal,
                NombreCancha = reserva.Cancha.Nombre,
                esValida = reserva.EsValida
            };


            var reservaEliminada = await _reservaCommand.EliminarReserva(reserva);

            return response;
        }

        public async Task<List<ReservaResponse>> ListarReservasPorDni(int dni)
        {
            var reservas = await _reservaQuery.ListarPorDniCliente(dni);
            return reservas.Select(r => new ReservaResponse
            {

                ReservaId = r.IdReserva,
                DniCliente = r.DniCliente,
                ReservaHorarioCanchaResponse = new DTOs.Response.HorarioCancha.ReservaHorarioCanchaResponse
                {
                    IdCanchaHorario = r.HorarioCancha.Id,
                    Fecha = r.Fecha,
                    HoraInicio = r.HorarioCancha.HoraInicio,
                    HoraFin = r.HorarioCancha.HoraFin,
                },
                Total = r.MontoTotal,
                NombreCancha = r.Cancha.Nombre,
                esValida = r.EsValida

            }).ToList();
        }
    }
}