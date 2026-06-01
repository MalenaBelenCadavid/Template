
using Application.DTOs.Response.Cancha;
using Application.DTOs.Response.HorarioCancha;
using Application.Interfaces.HorarioCancha;
using Application.Interfaces.Incripcion;
using Application.Interfaces.MetricasService;
using Application.Interfaces.Reserva;

namespace Application.UseCases
{
    public class MetricasService : IMetricasService
    {
        private readonly IReservaQuery _reservaQuery;
        private readonly IInscripcionQuery _inscripcionQuery;
        private readonly IHorarioCanchaQuery _horarioCanchaQuery;

        public MetricasService(IReservaQuery reservaQuery, IInscripcionQuery inscripcionQuery, IHorarioCanchaQuery horarioCanchaQuery)
        {
            _reservaQuery = reservaQuery;
            _inscripcionQuery = inscripcionQuery;
            _horarioCanchaQuery = horarioCanchaQuery;
        }

        public async Task<List<MetricasCanchaResponse>> CanchasMasReservadas()
        {
            var result = await _reservaQuery.CanchasMasReservadas();
            return result.Select(c => new MetricasCanchaResponse
            {
                Nombre = c.Nombre,
                tipoCancha=new DTOs.Response.TipoCancha.TipoCanchaResponse 
                {
                    Id=c.TipoCancha.IdTipoCancha,
                    Nombre=c.TipoCancha.Nombre,
                    Superficie=c.TipoCancha.Superficie,
                    Capacidad = c.TipoCancha.Capacidad,
                    Precio = c.TipoCancha.Precio,
                    Duracion = c.TipoCancha.Duracion
                }          
            }).ToList();
        }

        public Task<List<HorarioCanchaResponse>> HorariosMasReservados()
        {
            throw new NotImplementedException();
        }

        public async Task<int> VerTotalDeIngresosPorInscripciones()
        {
            return await _inscripcionQuery.IngresosPorInscripciones();
        }

        public async Task<int> VerTotalDeIngresosPorReserva()
        {
            return await _reservaQuery.IngresosPorReservas();
        }
    }
}
