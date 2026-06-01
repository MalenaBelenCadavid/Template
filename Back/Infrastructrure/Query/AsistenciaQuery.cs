using Application.Interfaces.Asistencia;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Query
{
    public  class AsistenciaQuery : IAsistenciaQuery
    {
        private readonly AppDbContext _context;

        public AsistenciaQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Asistencia> ConsultarAsistencia(int idAsistencia) {
            return await _context.Asistencias.Include(a=>a.Cliente).FirstOrDefaultAsync(e => e.IdAsistencia == idAsistencia);
        }

        public async Task<List<Domain.Entities.Asistencia>> ListarAsistenciaClase(int idClase,CancellationToken ct = default) {
            return await _context.Asistencias.Include(a=>a.Cliente).Where(a => a.IdClase == idClase).ToListAsync(ct);
        }
        public async Task<List<Domain.Entities.Asistencia>> ListarAsistenciaEntrenamiento(int idEntrenamiento, CancellationToken ct = default)
        {
            return await _context.Asistencias.Include(a => a.Cliente).Where(a => a.IdEntrenamiento == idEntrenamiento).ToListAsync(ct);
        }

        public async Task<List<Domain.Entities.Asistencia>> ConsultarAsistenciaporDNI(int DniCliente) {
            return await _context.Asistencias.Where(a => a.DniCliente == DniCliente).ToListAsync();
        }
    }
}
