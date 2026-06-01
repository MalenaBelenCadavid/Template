
namespace Application.Interfaces.Reserva
{
    public interface IReservaQuery
    {
        Task<Domain.Entities.Reserva> ConsultarReserva(int ReservaId, CancellationToken ct = default);
        Task<List<Domain.Entities.Reserva>> ListarReservas(CancellationToken ct = default);
        Task<bool> ExisteReserva(int canchaId, DateOnly Fecha, CancellationToken ct = default);
        Task<List<Domain.Entities.Reserva>> ListarPorCanchaYFecha(int idCancha, DateOnly Fecha, CancellationToken ct = default);

        Task<List<Domain.Entities.Reserva>> ListarPorDniCliente(int dni, CancellationToken ct = default);
        Task<List<Domain.Entities.Cancha>> CanchasMasReservadas(CancellationToken ct = default);
        Task<int> IngresosPorReservas (CancellationToken ct = default);

    }
}
