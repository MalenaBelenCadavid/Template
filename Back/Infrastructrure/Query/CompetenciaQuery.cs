using Application.Interfaces.Competencias;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{


    public class CompetenciaQuery: ICompetenciaQuery
    {
        private readonly AppDbContext _context;
        public CompetenciaQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Competencia?> ObtenerCompetenciaPorId(int id, CancellationToken cancellationToken=default)
        {
            return await _context.Competencias.Include(c => c.Equipos).Include(c => c.Partidos).FirstOrDefaultAsync(c => c.IdCompetencia == id, cancellationToken);
        }

        public async Task<IEnumerable<Competencia>> ObtenerTodasLasCompetencias(CancellationToken cancellationToken = default)
        {
            return await _context.Competencias
                .Include(c => c.Equipos)
                .Include(c => c.Partidos)
                    .ThenInclude(p => p.EquipoLocal)
                .Include(c => c.Partidos)
                    .ThenInclude(p => p.EquipoVis)
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Equipo>> ObtenerEquipos(int id, CancellationToken cancellationToken = default)
        {
            var competencia = await _context.Competencias.Include(c => c.Equipos).FirstOrDefaultAsync(c => c.IdCompetencia == id, cancellationToken);
            if (competencia == null)
                return new List<Equipo>();
            return competencia.Equipos;
        }
        public async Task<IEnumerable<Partido>> ObtenerPartidos(int id, CancellationToken cancellationToken){
            var competencia = await _context.Competencias.Include(c => c.Partidos).ThenInclude(p => p.EquipoLocal)
        .Include(c => c.Partidos)
          .ThenInclude(p => p.EquipoVis)
      .FirstOrDefaultAsync(c => c.IdCompetencia == id,cancellationToken);

            if (competencia == null)
                return new List<Partido>();

            return competencia.Partidos;
        }
        public async Task<bool> CompetenciaExiste(int idcompetencia, CancellationToken ct = default)
        {
            return await _context.Competencias.AnyAsync(c => c.IdCompetencia == idcompetencia, ct);
        }

    }

}