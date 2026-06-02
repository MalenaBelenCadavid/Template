
using Application.Interfaces.Reserva;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class ReservaQuery : IReservaQuery
    {
        private readonly AppDbContext _context;

        public ReservaQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reserva> ConsultarReserva(int ReservaId, CancellationToken ct = default)
        {
            return await _context.Reservas.Include(cancha=> cancha.Cancha).Include(c=>c.HorarioCancha).Include(co=>co.Cobro).FirstOrDefaultAsync(r => r.IdReserva == ReservaId,ct);
        }

        public async Task<bool>ExisteReserva(int canchaId,DateOnly fecha,CancellationToken ct = default) 
        {
            return await _context.Reservas.AnyAsync(r =>r.IdCancha == canchaId && r.Fecha == fecha,ct);
        }

        public async Task<int> IngresosPorReservas(CancellationToken ct = default)
        {
            return (int)await _context.Reservas.SumAsync(r => r.MontoTotal, ct);
        }
        public async Task<List<Cancha>> CanchasMasReservadas(CancellationToken ct = default)
        {
            var query = await _context.Reservas
                .GroupBy(r => r.IdCancha)
                .Select(g => new
                {
                    IdCancha = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .Join(_context.Canchas.Include(c => c.TipoCancha),
                    r => r.IdCancha,
                    c => c.IdCancha,
                    (r, c) => c
                )
                .ToListAsync(ct);

            return query;
        }

        public async Task<List<Reserva>> ListarPorCanchaYFecha(int idCancha, DateOnly Fecha, CancellationToken ct = default)
        {
            return await _context.Reservas.Where(res=>res.IdCancha==idCancha && res.Fecha==Fecha).ToListAsync(ct);
        }

        public async Task<List<Reserva>> ListarPorDniCliente(int dni, CancellationToken ct = default)
        {
            return await _context.Reservas.Where(r=>r.DniCliente==dni).Include(r=>r.HorarioCancha).Include(r=>r.Cancha).ToListAsync(ct);
        }

        public async Task<List<Reserva>> ListarReservas(CancellationToken ct = default)
        {
            return await _context.Reservas.ToListAsync(ct);
        }
    }
}
