using Application.Interfaces.Recibo;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class ReciboQuery : IReciboQuery
    {
        private readonly AppDbContext _context;

        public ReciboQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Recibo> ConsultarRecibo(int idRecibo, CancellationToken ct = default)
        {
            return await _context.Recibos.FirstOrDefaultAsync(r => r.IdRecibo == idRecibo, ct);
        }

        public async Task<Recibo> ImprimirRecibo(int idRecibo, CancellationToken ct = default)
        {
            // Traemos el recibo. Si más adelante necesitan incluir datos de las relaciones para el reporte, 
            // se puede meter un .Include(r => r.Cobro) o .Include(r => r.Reserva) acá.
            return await _context.Set<Recibo>()
                .FirstOrDefaultAsync(r => r.IdRecibo == idRecibo, ct);
        }
    }
}