using Application.DTOs.Request.Cancha;
using Application.DTOs.Request.HorarioCancha;
using Application.DTOs.Response;
using Application.DTOs.Response.Cancha;
using Application.DTOs.Response.HorarioCancha;
using Application.Exceptions;
using Application.Interfaces.Cancha;
using Application.Interfaces.HorarioCancha;
using Application.Interfaces.Reserva;
using Application.Interfaces.TipoCancha;
using Domain.Entities;

namespace Application.UseCases
{
    public class CanchaService : ICanchaService
    {
        private readonly ICanchaCommand _canchaCommand;
        private readonly ICanchaQuery _canchaQuery;
        private readonly ITipoCanchaQuery _tipoCanchaQuery;
        private readonly IHorarioCanchaService _horarioCanchaService;
        private readonly IHorarioCanchaCommand _horarioCanchaCommand;
        private readonly IReservaQuery _reservaQuery;

        public CanchaService(ICanchaCommand canchaCommand, ICanchaQuery canchaQuery, ITipoCanchaQuery tipoCanchaQuery,
            IHorarioCanchaService horarioCanchaService,IReservaQuery reservaQuery, IHorarioCanchaCommand horarioCanchaCommand)
        {
            _canchaCommand = canchaCommand;
            _canchaQuery = canchaQuery;
            _tipoCanchaQuery = tipoCanchaQuery;
            _horarioCanchaService = horarioCanchaService;
            _reservaQuery = reservaQuery;
            _horarioCanchaCommand = horarioCanchaCommand;
        }

        public async Task<CanchaResponse> CrearCancha(CrearCanchaRequest request)
        {
            if (request.IdTipoCancha <= 0)
            {
                throw new ExceptionBadRequest("Ingrese un id válido");
            }
            if (String.IsNullOrEmpty(request.Nombre) || request.Nombre.Length>20) 
            {
                throw new ExceptionBadRequest("Ingrese un nombre valido");
            }

           var tipoCancha = await _tipoCanchaQuery.ObtenerTipoCancha(request.IdTipoCancha);

            if (tipoCancha == null)
            {
                throw new ExceptionNotFound("El tipo de cancha no fue encontrado");
            }
            if(request.Horarios == null || !request.Horarios.Any()) 
            {
                throw new ExceptionBadRequest("Tiene que ingresar un horario");
            }

            foreach (var horario in request.Horarios) 
            {
                if (horario.HoraInicio > horario.HoraFin) 
                {
                    throw new ExceptionBadRequest("El horario de inicio debe ser mayor al horario de fin");
                }
            }

            var cancha = new Cancha
            {
                TipoCanchaId = request.IdTipoCancha,
                Nombre = request.Nombre,
                Estado = true,
                Disponibilidad = new List<HorarioCancha>()
            };

            var canchaCreada = await _canchaCommand.CrearCancha(cancha);
            var slotsTotales = new List<HorarioCancha>();
            foreach (var h in request.Horarios)
            {
                var slots = GenerarSlots(h.Dia, h.HoraInicio, h.HoraFin, tipoCancha.Duracion, canchaCreada.IdCancha);

                slotsTotales.AddRange(slots);
            }
            canchaCreada.Disponibilidad = slotsTotales;
            await _canchaCommand.ModificarCancha(canchaCreada);

            return new CanchaResponse
            {
                IdCancha = canchaCreada.IdCancha,
                Nombre = canchaCreada.Nombre,

                tipoCancha = new DTOs.Response.TipoCancha.TipoCanchaResponse
                {
                    Id = canchaCreada.TipoCancha.IdTipoCancha,
                    Nombre = canchaCreada.TipoCancha.Nombre,
                    Superficie = canchaCreada.TipoCancha.Superficie,
                    Capacidad = canchaCreada.TipoCancha.Capacidad,
                    Precio = canchaCreada.TipoCancha.Precio,
                    Duracion = canchaCreada.TipoCancha.Duracion,
                },

                Disponibilidad = canchaCreada.Disponibilidad.Select(horario => new HorarioCanchaResponse 
                {
                    HorarioCanchaId=horario.Id,
                    Dia=horario.Dia,
                    HoraInicio=horario.HoraInicio, 
                    HoraFin=horario.HoraFin,
                   
                }).ToList()
            };
        }

        public async Task<CanchaResponse> ConsultarCancha(int canchaId)
        {
            if (canchaId <= 0)
            {
                throw new ExceptionBadRequest("Ingrese un número válido");
            }

            var cancha = await _canchaQuery.ConsultarCancha(canchaId);

            if (cancha == null)
            {
                throw new ExceptionNotFound("Cancha no encontrada");
            }

            return new CanchaResponse
            {
                IdCancha = cancha.IdCancha,

                tipoCancha = new DTOs.Response.TipoCancha.TipoCanchaResponse
                {
                    Id = cancha.TipoCancha.IdTipoCancha,
                    Nombre = cancha.TipoCancha.Nombre,
                    Superficie = cancha.TipoCancha.Superficie,
                    Capacidad = cancha.TipoCancha.Capacidad,
                    Precio = cancha.TipoCancha.Precio,
                    Duracion = cancha.TipoCancha.Duracion,
                },

                Disponibilidad = cancha.Disponibilidad.Select(horario => new HorarioCanchaResponse
                {HorarioCanchaId = horario.Id,
                    Dia = horario.Dia,
                    HoraInicio = horario.HoraInicio,
                    HoraFin = horario.HoraFin,
                    
                }).ToList()
            };
        }

        public async Task<CanchaResponse> ModificarCancha(int canchaId, ActualizarCanchaRequest request)
        {
            if (canchaId <= 0)
            {
                throw new ExceptionBadRequest("Ingrese un valor válido");
            }

            var cancha = await _canchaQuery.ConsultarCancha(canchaId);

            if (cancha == null)
            {
                throw new ExceptionNotFound("Cancha no encontrada");
            }

            if (request.TipoCanchaId != null && request.TipoCanchaId != cancha.TipoCanchaId)
            {

                var tipoCancha = await _tipoCanchaQuery.ObtenerTipoCancha((int)request.TipoCanchaId);

                var horariosAgrupados = cancha.Disponibilidad
                    .GroupBy(h => h.Dia)
                    .Select(g => new
                    {
                        Dia = g.Key,

                        // hora mas temprana
                        HoraInicio = g.Min(x => x.HoraInicio),

                        // hora mas tardia
                        HoraFin = g.Max(x => x.HoraFin)
                    })
                    .ToList();



                var nuevosHorarios = new List<HorarioCancha>();

                foreach (var horario in horariosAgrupados)
                {
                    var slots = GenerarSlots(
                        horario.Dia,
                        horario.HoraInicio,
                        horario.HoraFin,
                        tipoCancha.Duracion,
                        cancha.IdCancha
                    );

                    nuevosHorarios.AddRange(slots);
                }

                await _horarioCanchaCommand.EliminarHorario(cancha.Disponibilidad);

                cancha.Disponibilidad.Clear();

                foreach (var slot in nuevosHorarios)
                {
                    cancha.Disponibilidad.Add(slot);
                }

                // actualizar tipo cancha
                cancha.TipoCanchaId = (int)request.TipoCanchaId;
                cancha.TipoCancha = tipoCancha;
            }

            if (request.horarios != null)
            {
                foreach (var horarioReq in request.horarios)
                {
                    var aEliminar = cancha.Disponibilidad.Where(h => h.Dia == horarioReq.Dia).ToList();

                    foreach (var h in aEliminar)
                    {
                        cancha.Disponibilidad.Remove(h);
                    }

                    var nuevosSlots = GenerarSlots(
                        horarioReq.Dia,
                        horarioReq.HoraInicio,
                        horarioReq.HoraFin,
                        cancha.TipoCancha.Duracion,
                        cancha.IdCancha
                    );

                    cancha.Disponibilidad.AddRange(nuevosSlots);
                }
            }

            cancha.Nombre = request.Nombre ?? cancha.Nombre;
            cancha.TipoCanchaId = request.TipoCanchaId ?? cancha.TipoCanchaId;



            var canchaAct = await _canchaCommand.ModificarCancha(cancha);

            return new CanchaResponse
            {
                IdCancha = canchaAct.IdCancha,
                Nombre = canchaAct.Nombre,
                tipoCancha = new DTOs.Response.TipoCancha.TipoCanchaResponse
                {
                    Id = canchaAct.TipoCancha.IdTipoCancha,
                    Nombre = canchaAct.TipoCancha.Nombre,
                    Superficie = canchaAct.TipoCancha.Superficie,
                    Capacidad = canchaAct.TipoCancha.Capacidad,
                    Precio = canchaAct.TipoCancha.Precio,
                    Duracion = canchaAct.TipoCancha.Duracion,
                },

                Disponibilidad = cancha.Disponibilidad.Select(horario => new HorarioCanchaResponse
                {
                    HorarioCanchaId = horario.Id,
                    Dia = horario.Dia,
                    HoraInicio = horario.HoraInicio,
                    HoraFin = horario.HoraFin,

                }).ToList(),
            };
        }

        public async Task<CanchaResponse> EliminarCancha(int canchaId)
        {
            if (canchaId <= 0)
            {
                throw new ExceptionBadRequest("Ingrese un valor válido");
            }

            var cancha = await _canchaQuery.ConsultarCancha(canchaId);

            if (cancha == null)
            {
                throw new ExceptionNotFound("Cancha no encontrada");
            }

            await _canchaCommand.EliminarCancha(cancha);

            return new CanchaResponse
            {
                IdCancha = cancha.IdCancha,

                tipoCancha = new DTOs.Response.TipoCancha.TipoCanchaResponse
                {
                    Id = cancha.TipoCancha.IdTipoCancha,
                    Nombre = cancha.TipoCancha.Nombre,
                    Superficie = cancha.TipoCancha.Superficie,
                    Capacidad = cancha.TipoCancha.Capacidad,
                    Precio = cancha.TipoCancha.Precio,
                    Duracion = cancha.TipoCancha.Duracion,
                },

                Disponibilidad = cancha.Disponibilidad.Select(horario => new HorarioCanchaResponse
                {HorarioCanchaId = horario.Id,
                    Dia = horario.Dia,
                    HoraInicio = horario.HoraInicio,
                    HoraFin = horario.HoraFin,
                   
                }).ToList(),
            };
        }

        public async Task<List<CanchaResponse>> ListarCanchas()
        {
            var canchas = await _canchaQuery.ListarCanchas();

            return canchas.Select(cancha => new CanchaResponse
            {
                IdCancha = cancha.IdCancha,
                Nombre=cancha.Nombre,

                tipoCancha = new DTOs.Response.TipoCancha.TipoCanchaResponse
                {
                    Id = cancha.TipoCancha.IdTipoCancha,
                    Nombre = cancha.TipoCancha.Nombre,
                    Superficie = cancha.TipoCancha.Superficie,
                    Capacidad = cancha.TipoCancha.Capacidad,
                    Precio = cancha.TipoCancha.Precio,
                    Duracion = cancha.TipoCancha.Duracion,
                },

                Disponibilidad = cancha.Disponibilidad.Select(horario => new HorarioCanchaResponse
                {HorarioCanchaId = horario.Id,
                    Dia = horario.Dia,
                    HoraInicio = horario.HoraInicio,
                    HoraFin = horario.HoraFin,
                }).ToList(),

            }).ToList();
        }

        public async Task<CanchaResponse> CambiarEstado(int canchaId)
        {
            if (canchaId <= 0)
            {
                throw new ExceptionBadRequest("Ingrese un valor válido");
            }

            var cancha = await _canchaQuery.ConsultarCancha(canchaId);

            if (cancha == null)
            {
                throw new ExceptionNotFound("Cancha no encontrada");
            }

            cancha.Estado = false;

            await _canchaCommand.CambiarEstado(cancha);

            return new CanchaResponse
            {
                IdCancha = cancha.IdCancha,

                tipoCancha = new DTOs.Response.TipoCancha.TipoCanchaResponse
                {
                    Id = cancha.TipoCancha.IdTipoCancha,
                    Nombre = cancha.TipoCancha.Nombre,
                    Superficie = cancha.TipoCancha.Superficie,
                    Capacidad = cancha.TipoCancha.Capacidad,
                    Precio = cancha.TipoCancha.Precio,
                    Duracion = cancha.TipoCancha.Duracion,
                },

                Disponibilidad = cancha.Disponibilidad.Select(horario => new HorarioCanchaResponse
                {
                    HorarioCanchaId = horario.Id,
                    Dia = horario.Dia,
                    HoraInicio = horario.HoraInicio,
                    HoraFin = horario.HoraFin,
                }).ToList(),
            };
        }

        public async Task<List<HorarioCanchaResponse>> VerDisponibilidad(int CanchaId, DateOnly fecha)
        {
            if (CanchaId <= 0)
            {
                throw new ExceptionBadRequest("Ingrese una cancha valida");
            }

            var cancha = await _canchaQuery.ConsultarCancha(CanchaId);
            if (cancha == null)
            {
                throw new ExceptionNotFound("La cancha ingresada no fue encontrada");
            }

            var diaSeleccionado = fecha.DayOfWeek;

            var horarios = await _canchaQuery.VerDisponibilidad(cancha.IdCancha);

            var horariosDelDia = horarios
                .Where(h => h.Dia == diaSeleccionado)
                .ToList();

            var reservas = await _reservaQuery.ListarPorCanchaYFecha(cancha.IdCancha, fecha);

            var ocupados = reservas.Select(r => r.IdCanchaHorario).ToHashSet();

            var resultado = horariosDelDia.Select(h => new HorarioCanchaResponse
            {
                HorarioCanchaId=h.Id,
                Dia = h.Dia,
                HoraInicio = h.HoraInicio,
                HoraFin = h.HoraFin,
                Disponible = !ocupados.Contains(h.Id)
            }).ToList();

            return resultado;
        }


        private List<HorarioCancha> GenerarSlots(DayOfWeek dia,TimeSpan inicio,TimeSpan fin,int duracionMinutos,int canchaId)
        {
            var slots = new List<HorarioCancha>();

            var duracion = TimeSpan.FromMinutes(duracionMinutos);
            var actual = inicio;

            while (actual + duracion <= fin)
            {
                slots.Add(new HorarioCancha
                {
                    IdCancha = canchaId,
                    Dia = dia,
                    HoraInicio = actual,
                    HoraFin = actual + duracion,
                });

                actual = actual + duracion;
            }

            return slots;
        }

        
    }
}