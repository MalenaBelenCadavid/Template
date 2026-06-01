using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.DTOs.Request.Competencias;
using Application.DTOs.Response.Competencias;
//using EquipoEntidad = Domain.Entities.Equipo;


namespace Application.Interfaces.Competencias
{
    public interface ICompetenciaCommand
    {
        Task<int> CrearCompetencia(Competencia competencia, CancellationToken ct = default);
        Task ModificarCompetencia(Competencia competencia, CancellationToken ct = default);
        Task EliminarCompetencia(Competencia competencia, CancellationToken ct = default);
        Task<int> AgregarEquipo(Equipo equipo,int idcompetencia, CancellationToken ct = default);
        Task EliminarPartidos(int idComptetencia, CancellationToken ct = default);
        Task AgregarPartidos(List<Partido> fixture, CancellationToken ct = default);
        Task DecrementarCupo(int idCompetencia, CancellationToken ct = default);
    }
}