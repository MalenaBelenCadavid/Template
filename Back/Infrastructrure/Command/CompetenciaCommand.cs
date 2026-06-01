using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Competencias;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{
    public class CompetenciaCommand : ICompetenciaCommand
    {
        private readonly AppDbContext _context;

        public CompetenciaCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearCompetencia(Competencia competencia, CancellationToken ct = default)
        {
            _context.Competencias.Add(competencia);
            await _context.SaveChangesAsync(ct);
            return competencia.IdCompetencia;
        }
        public async Task ModificarCompetencia(Competencia competencia, CancellationToken ct = default)
        {
            _context.Competencias.Update(competencia);
            await _context.SaveChangesAsync(ct);
        }
        public async Task EliminarCompetencia(Competencia competencia, CancellationToken ct = default)
        {
            _context.Competencias.Remove(competencia);
            await _context.SaveChangesAsync(ct);
        }
        public async Task<int> AgregarEquipo(Equipo equipo, int idcompetencia, CancellationToken ct = default)
        {
            var competencia = await _context.Competencias
         .AnyAsync(c => c.IdCompetencia == idcompetencia, ct);

            if (!competencia)
                throw new ExceptionNotFound("Competencia no encontrada");

            
            await _context.Equipos.AddAsync(equipo, ct);
            await _context.SaveChangesAsync(ct);
            return equipo.IdEquipo;
        }
        public async Task AgregarPartidos(List<Partido> fixture, CancellationToken ct = default)
        {
            await _context.Partidos.AddRangeAsync(fixture, ct);
            await _context.SaveChangesAsync(ct);
        }
        public async Task DecrementarCupo(int idCompetencia, CancellationToken ct = default)
        {
            var competencia = await _context.Competencias
                .FirstOrDefaultAsync(c => c.IdCompetencia == idCompetencia, ct);
            if (competencia is null)
                throw new ExceptionNotFound("Competencia no encontrada");
            competencia.Cupos--;
            _context.Competencias.Update(competencia);
            await _context.SaveChangesAsync(ct);
        }
    }
}


