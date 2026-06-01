using Application.Interfaces.Partidos;
using Domain.Entities;
using Infrastructure.Persistence;


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
            _context.Partidos.Update(partido);
            await _context.SaveChangesAsync(ct);
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
    }
}
