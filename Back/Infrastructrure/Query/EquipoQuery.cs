using Application.Interfaces.Equipos;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class EquipoQuery : IEquipoQuery
    {
        private readonly AppDbContext _context;
        public EquipoQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Equipo> ObtenerEquipoPorId(int id, CancellationToken ct = default)
        {
            return await _context.Equipos.FirstOrDefaultAsync(e => e.IdEquipo == id, ct);
        }
        public async Task<IEnumerable<Equipo>> ObtenerEquiposPorCompetencia(int competenciaId, CancellationToken ct = default)
        {
            return await _context.Equipos.Where(e => e.IdCompetencia == competenciaId).ToListAsync(ct);
        }
        public async Task<bool> ExisteEquipo(int equipoId, CancellationToken ct = default)
        {
            return await _context.Equipos.AnyAsync(e => e.IdEquipo == equipoId, ct);
        }
    }
}


