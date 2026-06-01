using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.DTOs.Request.Competencias;
using Application.DTOs.Response.Competencias;
using Application.DTOs.Response.Equipos;
using Application.DTOs.Response.Partidos;

namespace Application.Interfaces.Competencias
{
    public interface ICompetenciaService
    {
        Task<int> CrearCompetencia(CrearCompetenciaRequest competencia, CancellationToken ct = default);
        Task ModificarCompetencia(int idCompetencia,ModificarCompetenciaRequest competencia, CancellationToken ct = default);
        Task<int> AgregarEquipo(AgregarEquipoRequest request,int idCompetencia, CancellationToken ct = default);
        Task EliminarCompetencia(int idCompetencia, CancellationToken ct = default);
        Task EliminarPartidos(int idCompetencia, CancellationToken ct = default);

        Task<CompetenciaResponse?> ObtenerCompetenciaPorId(int id, CancellationToken ct = default);
        Task<IEnumerable<CompetenciaResponse?>> ObtenerTodasLasCompetencias(CancellationToken ct = default);
        Task<IEnumerable<EquipoResponse?>> ObtenerEquipos(int idcompetencia, CancellationToken ct = default);
        Task<IEnumerable<PartidoResponseCompetencia?>> ObtenerPartidos(int id, CancellationToken ct = default);


        Task<bool> CompetenciaExiste(int idcompetencia, CancellationToken ct = default);
      
    }

}
