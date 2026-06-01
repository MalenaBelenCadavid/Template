using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructrure.Query
{
    public class DescuentoQuery : IDescuentoQuery
    {
        private readonly AppDbContext _context;

        public DescuentoQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Descuento> GetDescuentoById(int id)
        {
            return await _context.Descuentos.FirstOrDefaultAsync(d => d.IdDescuento == id);
        }

        public async Task<List<Descuento>> GetAllDescuentos()
        {
            return await _context.Descuentos.ToListAsync();
        }

        // Retorna el descuento activo vigente para el tipo dado
        // Si no hay uno específico, busca uno General
        public async Task<Descuento> GetDescuentoActivoPorTipo(string tipoDescuento)
        {
            var ahora = DateTime.Now;
            return await _context.Descuentos
                .Where(d => d.Activo &&
                            (d.TipoDescuento == tipoDescuento || d.TipoDescuento == "General") &&
                            d.FechaInicio <= ahora &&
                            d.FechaFin >= ahora)
                .OrderByDescending(d => d.Valor)
                .FirstOrDefaultAsync();
        }
    }
}
