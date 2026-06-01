using Application.Interfaces.Clase;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Query
{
    public class ClaseQuery : IClaseQuery
    {

        private readonly AppDbContext _context;

        public ClaseQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Domain.Entities.Clase> ConsultarClase(int Clase, CancellationToken ct = default) {

            return await _context.Clases.Include(c=>c.Inscripto).ThenInclude(i => i.cliente).FirstOrDefaultAsync(c => c.IdClase == Clase,ct);
        }
        public async Task<List<Domain.Entities.Inscripcion>> MostrarInscriptos(int claseId,CancellationToken ct = default) {

            return await _context.Inscripciones.Include(i=>i.cliente).Where(i=>i.IdAct==claseId && i.NroAct==2).ToListAsync(ct);
        }

        public async Task<Domain.Entities.Inscripcion> DevolverInscripto(int InscripcionId, CancellationToken ct = default) {
            return await _context.Inscripciones.FirstOrDefaultAsync(i => i.IdInscripcion == InscripcionId,ct);
        }

        public async Task<List<Clase>> ListarClases(CancellationToken ct = default)
        {
            return await _context.Clases.Include(c=>c.Profesor).ToListAsync(ct);
        }

        public async Task<List<Clase>> VerClasesPorProfesor(int profesorDni, CancellationToken ct = default)
        {
            return await _context.Clases.Include(c => c.Profesor).Where(c => c.DniProfesor == profesorDni).ToListAsync(ct);
        }
    }
}
