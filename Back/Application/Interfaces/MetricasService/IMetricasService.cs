
using Application.DTOs.Response.Cancha;
using Application.DTOs.Response.HorarioCancha;
using Application.UseCases;

namespace Application.Interfaces.MetricasService
{
    public interface IMetricasService
    {
        Task<int> VerTotalDeIngresosPorReserva();
        Task<int> VerTotalDeIngresosPorInscripciones();
        Task<List<MetricasCanchaResponse>> CanchasMasReservadas();
        Task<List<HorarioCanchaResponse>> HorariosMasReservados();

    }
}
