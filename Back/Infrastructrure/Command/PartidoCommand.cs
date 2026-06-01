using Application.Interfaces.Partidos;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{

    public class PartidoCommand : IPartidoCommand
    {
        private readonly AppDbContext _context;
        public PartidoCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CrearPartido(Partido partido, CancellationToken ct = default)
        {
            _context.Partidos.Add(partido);
            await _context.SaveChangesAsync(ct);
            return partido.IdPartido;
        }
        public async Task ModificarPartido(Partido partido, CancellationToken ct = default)
        {
            await _context.Partidos
        .Where(p => p.IdPartido == partido.IdPartido)
        .ExecuteUpdateAsync(s => s
            .SetProperty(p => p.IdSigPartido, partido.IdSigPartido)
            .SetProperty(p => p.IdEquipoLocal, partido.IdEquipoLocal)
            .SetProperty(p => p.IdEquipoVis, partido.IdEquipoVis)
            .SetProperty(p => p.GolesLocal, partido.GolesLocal)
            .SetProperty(p => p.GolesVis, partido.GolesVis)
            .SetProperty(p => p.Estado, partido.Estado)
            .SetProperty(p => p.HoraInicio, partido.HoraInicio)
            .SetProperty(p => p.HoraFin, partido.HoraFin),
        ct);
        }
        public async Task EliminarPartido(Partido partido, CancellationToken ct = default)
        {
            _context.Partidos.Remove(partido);
            await _context.SaveChangesAsync(ct);
        }
        public async Task<IEnumerable<Partido>> AgregarPartidos(List<Partido> fixture, CancellationToken ct = default)
        {
            _context.Partidos.AddRange(fixture);
            await _context.SaveChangesAsync(ct);
            return fixture;
        }
        public async Task ActualizarSigPartido(int idPartido, int idSigPartido, CancellationToken ct = default)
        {
            await _context.Partidos
                .Where(p => p.IdPartido == idPartido)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.IdSigPartido, idSigPartido), ct);
        }
    }
}
