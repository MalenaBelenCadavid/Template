using Application.Interfaces.Partidos;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class PartidoQuery : IPartidoQuery
    {
        private readonly AppDbContext _context;
        public PartidoQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Partido> ObtenerPartidoPorId(int id, CancellationToken ct = default)
        {
            return await _context.Partidos.Include(p => p.EquipoLocal).Include(p => p.EquipoVis).FirstOrDefaultAsync(p => p.IdPartido == id, ct);
        }
        public async Task<IEnumerable<Partido>> ObtenerPartidoPorEquipo(int equipoId, CancellationToken ct = default)
        {
            return await _context.Partidos.Where(p => p.IdEquipoLocal == equipoId ||
                            p.IdEquipoVis == equipoId)
                .ToListAsync(ct);
        }


    }
}


