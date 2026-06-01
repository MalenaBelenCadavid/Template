using Domain.Entities;
using Application.DTOs.Request.Competencias;
using Application.DTOs.Response.Competencias;
using Application.Interfaces.Competencias;

namespace Application.Interfaces.Competencias
{
    public interface ITorneoService: ICompetenciaService
    {
          Task GenerarFixture(int idTorneo, CancellationToken ct = default);
          Task AgregarPartidos(List<Partido> fixture, CancellationToken ct = default);
          Task CargarResultado(int IdPartido, int GolesLocal, int GolesVis, CancellationToken ct = default);
          Task DescalificarEquipo(int idEquipo, CancellationToken ct = default);
        Task<ObtenerCuadroResponse> ObtenerCuadroTorneo(int idTorneo, CancellationToken ct = default);
        Task EliminarFixture(int idTorneo, CancellationToken ct = default);
            Task RehacerFixture(int idTorneo, CancellationToken ct = default);

    }

}

