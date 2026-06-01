using Application.Interfaces.Competencias;
using Domain.Entities;
using Application.DTOs.Request.Competencias;
using Application.DTOs.Response.Competencias;

using Application.Interfaces.Partidos;

namespace Application.UseCases
{

    public class LigaService: CompetenciaService,ILigaService 
    {
        private readonly IPartidoQuery _partidoQuery;
        private readonly IPartidoCommand _partidoCommand;
        public LigaService(ICompetenciaCommand competenciaCommand, ICompetenciaQuery competenciaQuery, IPartidoQuery partidoQuery, IPartidoCommand partidoCommand):base(competenciaCommand, competenciaQuery)
        {
            _partidoQuery = partidoQuery;
            _partidoCommand = partidoCommand;
        }

        public async Task GenerarFixture(int idLiga, CancellationToken ct = default)
        {
            var competencia = await _competenciaQuery.ObtenerCompetenciaPorId(idLiga, ct);
           
            if (competencia is null) throw new KeyNotFoundException($"No se encontro competencias con id: {idLiga}");
            if(competencia is not Liga liga)
            { throw new InvalidOperationException($"La competencia con id: {idLiga} no es una liga."); }
            if (competencia.Partidos.Any())
            {
                throw new InvalidOperationException($"La competencia {competencia.Nombre} ya tiene partidos generados.");
            }
            if (competencia.Equipos.Count() < 2)
            {
                throw new InvalidOperationException($"La competencia {competencia.Nombre} no tiene suficientes equipos para generar un fixture.");
            }
            var partidos = new List<Partido>();
            var equipos = competencia.Equipos.ToList();
            for (int i = 0; i < competencia.Equipos.Count(); i++)
            {
                for (int j = i + 1; j < competencia.Equipos.Count(); j++)
                {
                    partidos.Add(new Partido
                    {
                        IdCompetencia = idLiga,
                        IdEquipoLocal = equipos[i].IdEquipo,
                        IdEquipoVis = equipos[j].IdEquipo,
                        GolesLocal = 0,
                        GolesVis = 0,
                        HoraInicio = DateTime.Now.AddDays(partidos.Count()),
                        HoraFin = DateTime.Now.AddDays(partidos.Count()).AddHours(2)
                    });
                }
            }

            await AgregarPartidos(partidos, ct);
        }
        public async Task AgregarPartidos(List<Partido> fixture, CancellationToken ct = default)
        {
            await _competenciaCommand.AgregarPartidos(fixture, ct);
        }
        public async Task CargarResultado(int IdPartido, int GolesLocal, int GolesVis, CancellationToken ct = default)
        {
            var partido = await _partidoQuery.ObtenerPartidoPorId(IdPartido, ct);
            if (partido is null) throw new KeyNotFoundException($"No se encontro partido con id: {IdPartido}");

            partido.GolesLocal = GolesLocal;
            partido.GolesVis = GolesVis;
            partido.Estado = "Finalizado";

            await _partidoCommand.ModificarPartido(partido, ct);
        }

        public async Task<IEnumerable<TablaLigaResponse?>> ObtenerTabla(int idLiga, CancellationToken ct = default)
        {
            var competencia = await _competenciaQuery.ObtenerCompetenciaPorId(idLiga, ct);
            if (competencia is null) { throw new KeyNotFoundException($"No se encontro competencias con id: {idLiga}"); }
            var partidos = competencia.Partidos.Where(partido => partido.Estado == "Finalizado").ToList();

            var equipos =  competencia.Equipos.Select(equipo => new TablaLigaResponse { IdEquipo = equipo.IdEquipo,
                Equipo = equipo.Nombre,
            }).ToList();

            foreach (var partido in partidos)
            { 
                var equipoLocal= equipos.First(e => e.IdEquipo == partido.IdEquipoLocal);
                var equipoVis= equipos.First(e => e.IdEquipo == partido.IdEquipoVis);

                equipoLocal.PJ++;
                equipoVis.PJ++;

                equipoLocal.GF += partido.GolesLocal ?? 0;
                equipoLocal.GC += partido.GolesVis ?? 0;

                equipoVis.GF += partido.GolesVis ?? 0;
                equipoVis.GC += partido.GolesLocal ?? 0;

                if (partido.GolesLocal > partido.GolesVis)
                {
                    equipoLocal.PG++;
                    equipoVis.PP++;

                    equipoLocal.Puntos += 3;
                }
                else if (partido.GolesLocal < partido.GolesVis)
                {
                    equipoVis.PG++;
                    equipoLocal.PP++;
                    equipoVis.Puntos += 3;
                }
                else
                {
                    equipoLocal.PE++;
                    equipoVis.PE++;
                    equipoLocal.Puntos += 1;
                    equipoVis.Puntos += 1;
                }

                
            }
            return equipos.OrderByDescending(e => e.Puntos)
                    .ThenByDescending(e => e.GF - e.GC)
                    .ThenByDescending(e => e.GF)
                    .ThenBy(e => e.Equipo)
                    .ToList();

        }
        public async Task EliminarFixture(int idLiga, CancellationToken ct = default)
        {
            var competencia = await _competenciaQuery.ObtenerCompetenciaPorId(idLiga, ct);
            if (competencia is null) throw new KeyNotFoundException($"No se encontro competencias con id: {idLiga}");
            if (competencia.Partidos.Count() == 0)
            {
                throw new InvalidOperationException($"La competencia {competencia.Nombre} no tiene partidos para eliminar.");
            }
            await _competenciaCommand.EliminarPartidos(int idLiga, ct);

        }
        public async Task RehacerFixture(int idLiga, CancellationToken ct = default)
        {
            await EliminarFixture(idLiga, ct);
            await GenerarFixture(idLiga, ct);
        }
}