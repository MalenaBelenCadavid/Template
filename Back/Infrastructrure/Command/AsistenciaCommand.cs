using Application.DTOs.Request.Asistencia;

using Application.Interfaces.Asistencia;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Command
{
    public class AsistenciaCommand : IAsistenciaCommand
    {
        private readonly AppDbContext _context;

        public AsistenciaCommand(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Domain.Entities.Asistencia> EliminarAsistencia(Domain.Entities.Asistencia Asistencia, CancellationToken ct = default) {


            _context.Remove(Asistencia);
            await _context.SaveChangesAsync(ct);
            return Asistencia;

        }
        public async Task<Domain.Entities.Asistencia> ModificarAsistencia(Domain.Entities.Asistencia Asistencia, CancellationToken ct = default) {


            _context.Update(Asistencia);
            await _context.SaveChangesAsync(ct);
            return Asistencia;
        }

        public Task<Asistencia> PasarAsistencia(ModificarAsistenciaRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Entities.Asistencia> RegistrarAsistencia(Domain.Entities.Asistencia Asistencia, CancellationToken ct = default) {

            _context.AddAsync(Asistencia);
            await _context.SaveChangesAsync(ct);
            return Asistencia;
        }

        

    }
}
