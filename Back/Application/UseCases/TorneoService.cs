using Application.Interfaces.Competencias;
using Domain.Entities;
using Application.DTOs.Request.Competencias;
using Application.DTOs.Response.Competencias;
using Application.DTOs.Response.Partidos;
using Application.DTOs.Response.Equipos;
using Application.Interfaces.Equipos;
using Application.Interfaces.Partidos;
using Application.Exceptions;
using Application.Interfaces.Equipos;


namespace Application.UseCases
{

    public class TorneoService : CompetenciaService, ITorneoService
    {
        private readonly IPartidoCommand _partidoCommand;
        private readonly IPartidoQuery _partidoQuery;
        private readonly IEquipoQuery _equipoQuery;
        private readonly IEquipoCommand _equipoCommand;
        public TorneoService(ICompetenciaCommand competenciaCommand, ICompetenciaQuery competenciaQuery, IPartidoCommand partidoCommand,IPartidoQuery partidoQuery,IEquipoQuery equipoQuery, IEquipoCommand equipoCommand) : base(competenciaCommand, competenciaQuery)
        {
            _partidoCommand = partidoCommand;
            _partidoQuery = partidoQuery;
            _equipoQuery = equipoQuery;
            _equipoCommand = equipoCommand;
        }

        public async Task GenerarFixture(int idTorneo, CancellationToken ct = default)
        {
            var competencia = await _competenciaQuery.ObtenerCompetenciaPorId(idTorneo, ct);

            if (competencia is null)
                throw new ExceptionNotFound("Competencia no encontrada");

            if (competencia is not Torneo)
                throw new ExceptionBadRequest("El torneo no existe");

            if (competencia.Partidos?.Any() == true)
                throw new ExceptionConflict("El torneo ya tiene fixture generado");

            var equipos = competencia.Equipos?.ToList();

            if (equipos == null || equipos.Count < 2)
                throw new ExceptionConflict("No hay suficientes equipos");

            equipos = equipos.OrderBy(_ => Guid.NewGuid()).ToList();

            var partidos = new List<Partido>();

            for (int i = 0; i < equipos.Count / 2; i++)
            {
                partidos.Add(new Partido
                {
                    IdCompetencia = idTorneo,
                    IdEquipoLocal = equipos[i * 2].IdEquipo,
                    IdEquipoVis = equipos[i * 2 + 1].IdEquipo,
                    HoraInicio = DateTime.Now.AddDays(i),
                    HoraFin = DateTime.Now.AddDays(i).AddHours(2),
                    Estado = "Programado"
                });
            }

            await _partidoCommand.AgregarPartidos(partidos, ct);
        }
        public async Task AgregarPartidos(List<Partido> fixture, CancellationToken ct = default)
        {
            await _competenciaCommand.AgregarPartidos(fixture, ct);
        }

        public async Task CargarResultado(int IdPartido, int GolesLocal, int GolesVis, CancellationToken ct = default)
        {
            var partido = await _partidoQuery.ObtenerPartidoPorId(IdPartido, ct);

            if (partido is null)
                throw new ExceptionNotFound($"No se encontro partido con id: {IdPartido}");

            if (partido.IdEquipoLocal == 0 || partido.IdEquipoVis == 0)
                throw new ExceptionConflict("El partido no tiene equipos asignados");

            var equipoLocal = await _equipoQuery.ObtenerEquipoPorId(partido.IdEquipoLocal.Value, ct);
            var equipoVis = await _equipoQuery.ObtenerEquipoPorId(partido.IdEquipoVis.Value, ct);

            if (equipoLocal is null || equipoVis is null)
                throw new ExceptionConflict("Uno o ambos equipos del partido no existen");

            if (GolesLocal > GolesVis)
            {
                equipoLocal.Victorias++;
                equipoVis.Derrotas++;
                equipoVis.Estado = false;
            }
            else if (GolesLocal < GolesVis)
            {
                equipoVis.Victorias++;
                equipoLocal.Derrotas++;
                equipoLocal.Estado = false;
            }

            await _equipoCommand.ModificarEquipo(equipoLocal, ct);
            await _equipoCommand.ModificarEquipo(equipoVis, ct);

            partido.GolesLocal = GolesLocal;
            partido.GolesVis = GolesVis;
            partido.Estado = "Finalizado";
            if (GolesLocal==GolesVis) { throw new InvalidOperationException("No se permiten empates en el torneo."); }
            if (partido.SigPartido != null && partido.SigPartido.Estado == "Programado")
            {
                if (GolesLocal > GolesVis)
                {
                    if (partido.SigPartido.IdEquipoLocal==null)
                    {
                        partido.SigPartido.IdEquipoLocal = partido.IdEquipoLocal;
                        await DescalificarEquipo(partido.IdEquipoVis.Value, ct);
                    }
                    else
                    {
                        partido.SigPartido.IdEquipoVis = partido.IdEquipoLocal;
                        await DescalificarEquipo(partido.IdEquipoVis.Value, ct);
                    }

                }
                else
                {
                    if (partido.SigPartido.IdEquipoLocal is null)
                    {
                        partido.SigPartido.IdEquipoLocal = partido.IdEquipoVis ?? partido.SigPartido.IdEquipoVis;
                        await DescalificarEquipo(partido.IdEquipoLocal.Value, ct);
                    }
                    else
                    {
                        partido.SigPartido.IdEquipoVis = partido.IdEquipoVis ?? partido.SigPartido.IdEquipoVis;
                        await DescalificarEquipo(partido.IdEquipoLocal.Value, ct);
                    }

                }
            }

            await _partidoCommand.ModificarPartido(partido, ct);
            await _partidoCommand.ModificarPartido(partido.SigPartido, ct);
        }
        public async Task DescalificarEquipo(int idEquipo, CancellationToken ct = default)
        {
            var equipo = await _equipoQuery.ObtenerEquipoPorId(idEquipo, ct);
            if (equipo is null) throw new KeyNotFoundException($"No se encontro equipo con id: {idEquipo}");
            if (equipo.Estado)
            {
                equipo.Estado = false;
                await _equipoCommand.ModificarEquipo(equipo, ct);
            }
        }
        public async Task<ObtenerCuadroResponse> ObtenerCuadroTorneo(
            int idTorneo,
            CancellationToken ct = default)
        {
            var torneo = await _competenciaQuery
                .ObtenerCompetenciaPorId(idTorneo, ct);

            if (torneo is null)
                throw new KeyNotFoundException(
                    $"No se encontró torneo con id {idTorneo}");

            if (torneo is not Torneo)
                throw new InvalidOperationException(
                    $"La competencia {idTorneo} no es un torneo");

            var partidos = torneo.Partidos.ToList();

            var resultado = new ObtenerCuadroResponse
            {
                Fases = new List<FaseTorneoResponse>()
            };

            int cantidadPartidos = torneo.Cupos / 2;

            while (cantidadPartidos > 0)
            {
                var ronda = partidos
                    .Take(cantidadPartidos)
                    .ToList();

                resultado.Fases.Add(new FaseTorneoResponse
                {
                    NombreFase = ObtenerNombreFase(cantidadPartidos),

                    Partidos = ronda.Select(p => new PartidoResponse
                    {
                        IdPartido = p.IdPartido,
                        IdEquipoLocal = p.IdEquipoLocal,
                        IdEquipoVis = p.IdEquipoVis,
                        Estado = p.Estado,
                        GolesLocal = p.GolesLocal,
                        GolesVis = p.GolesVis,
                        HoraInicio = p.HoraInicio,
                        HoraFin = p.HoraFin
                    }).ToList()
                });

                partidos = partidos
                    .Skip(cantidadPartidos)
                    .ToList();

                cantidadPartidos /= 2;
            }

            return resultado;
        }

        private string ObtenerNombreFase(int cantidadPartidos)
        {
            return cantidadPartidos switch
            {
                1 => "Final",
                2 => "Semifinal",
                4 => "Cuartos de Final",
                8 => "Octavos de Final",
                16 => "Dieciseisavos de Final",
                _ => $"Ronda de {cantidadPartidos * 2}"
            };
        }
    }
    }