using Domain.Entities;
using Application.DTOs.Request.Competencias;
using Application.DTOs.Response.Competencias;
using Application.DTOs.Response.Equipos;
using Application.DTOs.Response.Partidos;


namespace Application.Interfaces.Competencias
{
    public interface ILigaService : ICompetenciaService
    {
        Task GenerarFixture(int idLiga, CancellationToken ct = default);
        Task AgregarPartidos(List<Partido> fixture, CancellationToken ct = default);
        Task CargarResultado(int IdPartido, int GolesLocal, int GolesVis, CancellationToken ct = default);
        Task<IEnumerable<TablaLigaResponse?>> ObtenerTabla(int idLiga, CancellationToken ct = default);
        Task EliminarFixture(int idLiga, CancellationToken ct = default);
        Task RehacerFixture(int idLiga, CancellationToken ct = default);
    }

}

