using Application.Interfaces.Incripcion;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Query
{
    public class InscripcionQuery : IInscripcionQuery
    {
        private readonly AppDbContext _context;

        public InscripcionQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Domain.Entities.Inscripcion> ConsultarInscripcion(int inscripcionId, CancellationToken ct = default)
        {
            return await _context.Inscripciones.Include(i=>i.cliente).FirstOrDefaultAsync(i => i.IdInscripcion == inscripcionId, ct); 
        }

        public async Task<List<Domain.Entities.Inscripcion>> ListaDeInscriptos(CancellationToken ct = default)
        {
            return await _context.Inscripciones.ToListAsync(); 
        }

        public async Task<int> ContadorInscripcion(int idActividad, int nroActividad)
        {
            return await _context.Inscripciones.Where(i => i.IdAct == idActividad && i.NroAct == nroActividad).CountAsync();
        }



        public async Task<int> CuposEnNumeroActividad(int IdAct, int NroAct)
        {
            return await _context.Inscripciones
                .Where(c => c.IdAct == IdAct && c.NroAct == NroAct)
                .CountAsync();
        }

        public async Task<bool> EstaIncripto(int idActividad, int dni, CancellationToken ct = default)
        {
            return await _context.Inscripciones.AnyAsync(i => i.IdAct == idActividad && i.DniCliente == dni,ct);
        }

        public async Task<int> IngresosPorInscripciones(CancellationToken ct = default)
        {
            return (int)await _context.Inscripciones.SumAsync(i => i.PrecioInscr, ct);
        }
    }
}
