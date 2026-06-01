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
        public TorneoService(ICompetenciaCommand competenciaCommand, ICompetenciaQuery competenciaQuery, IPartidoCommand partidoCommand, IPartidoQuery partidoQuery, IEquipoQuery equipoQuery, IEquipoCommand equipoCommand) : base(competenciaCommand, competenciaQuery)
        {
            _partidoCommand = partidoCommand;
            _partidoQuery = partidoQuery;
            _equipoQuery = equipoQuery;
            _equipoCommand = equipoCommand;
        }

        public async Task GenerarFixture(int idTorneo, CancellationToken ct = default)
        {
            var competencia = await _competenciaQuery.ObtenerCompetenciaPorId(idTorneo, ct);
            if (competencia is null) throw new KeyNotFoundException($"No se encontro competencias con id: {idTorneo}");
            if (competencia is not Torneo torneo)
                throw new InvalidOperationException($"La competencia con id: {idTorneo} no es un torneo.");
            if (competencia.Partidos.Any())
                throw new InvalidOperationException($"La competencia {competencia.Nombre} ya tiene partidos generados.");
            if (competencia.Equipos.Count() < 2)
                throw new InvalidOperationException($"La competencia {competencia.Nombre} no tiene suficientes equipos.");

            var equipos = competencia.Equipos
                .OrderBy(e => Guid.NewGuid())
                .ToList();

            int rondasTotales = (int)Math.Log2(equipos.Count);
            var partidosRondas = new List<List<Partido>>();
            int partidosPorRonda = equipos.Count / 2;
            int diasOffset = 1; 

            for (int ronda = 0; ronda < rondasTotales; ronda++)
            {
                var rondaActual = new List<Partido>();
                for (int i = 0; i < partidosPorRonda; i++)
                {
                    var partido = new Partido
                    {
                        IdCompetencia = idTorneo,
                        HoraInicio = DateTime.Now.AddDays(diasOffset),
                        HoraFin = DateTime.Now.AddDays(diasOffset).AddHours(2),
                        Estado = "Programado"
                    };
                    diasOffset += 3; 
                    rondaActual.Add(partido);
                }
                partidosRondas.Add(rondaActual);
                partidosPorRonda /= 2;
            }

            int indiceEquipo = 0;
            foreach (var partido in partidosRondas[0])
            {
                partido.IdEquipoLocal = equipos[indiceEquipo].IdEquipo;
                partido.IdEquipoVis = equipos[indiceEquipo + 1].IdEquipo;
                indiceEquipo += 2;
            }
            var todosLosPartidos = partidosRondas.SelectMany(r => r).ToList();
            await AgregarPartidos(todosLosPartidos, ct);
            Console.WriteLine("=== IDs después de guardar ===");
            foreach (var p in todosLosPartidos)
            {
                Console.WriteLine($"Partido ID: {p.IdPartido}");
            }
            bool hayActualizaciones = false;
            for (int ronda = 0; ronda < partidosRondas.Count - 1; ronda++)
            {
                var rondaActual = partidosRondas[ronda];
                var rondaSiguiente = partidosRondas[ronda + 1];

                for (int i = 0; i < rondaActual.Count; i++)
                {
                    int indiceSiguiente = i / 2;
                    rondaActual[i].IdSigPartido = rondaSiguiente[indiceSiguiente].IdPartido; 
                    hayActualizaciones = true;
                }
            }

            if (hayActualizaciones)
            {
                var partidosConSig = partidosRondas
                    .Take(partidosRondas.Count - 1)
                    .SelectMany(r => r)
                    .ToList();

                Console.WriteLine("=== LINKS A GUARDAR ===");
                foreach (var p in partidosConSig)
                {
                    Console.WriteLine($"Partido {p.IdPartido} → SigPartido: {p.IdSigPartido}");
                    await _partidoCommand.ActualizarSigPartido(p.IdPartido, p.IdSigPartido!.Value, ct);
                }
            }

        }
        public async Task ActualizarPartidos(List<Partido> partidos, CancellationToken ct = default)
        {
            foreach (var partido in partidos)
            {
                await _partidoCommand.ModificarPartido(partido, ct);
            }
        }
        public async Task AgregarPartidos(List<Partido> fixture, CancellationToken ct = default)
        {
            await _competenciaCommand.AgregarPartidos(fixture, ct);
        }

        public async Task RehacerFixture(int idTorneo, CancellationToken ct = default)
        {
            await EliminarFixture(idTorneo, ct);
            await GenerarFixture(idTorneo, ct);
        }
        public async Task EliminarFixture(int idTorneo, CancellationToken ct = default)
        {
            var competencia = await _competenciaQuery.ObtenerCompetenciaPorId(idTorneo, ct);
            if (competencia is null)
                throw new ExceptionNotFound("Competencia no encontrada");
            if (competencia is not Torneo)
                throw new ExceptionBadRequest("La competencia no es torneo");
            await _competenciaCommand.EliminarPartidos(idTorneo, ct);
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
            if (GolesLocal==GolesVis) { throw new InvalidOperationException("No se permiten empates en el torneo."); }
            await _equipoCommand.ModificarEquipo(equipoLocal, ct);
            await _equipoCommand.ModificarEquipo(equipoVis, ct);

            partido.GolesLocal = GolesLocal;
            partido.GolesVis = GolesVis;
            partido.Estado = "Finalizado";

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
            if (partido.SigPartido != null)
            {
                await _partidoCommand.ModificarPartido(partido.SigPartido, ct);
            }
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
        public async Task<ObtenerCuadroResponse> ObtenerCuadroTorneo(int idTorneo, CancellationToken ct = default)
        {
            var torneo = await _competenciaQuery.ObtenerCompetenciaPorId(idTorneo, ct);
            if (torneo is null)
                throw new KeyNotFoundException($"No se encontró torneo con id {idTorneo}");
            if (torneo is not Torneo)
                throw new InvalidOperationException($"La competencia {idTorneo} no es un torneo");

            var partidos = torneo.Partidos.ToList();

            // Partidos que nadie apunta a ellos = primera ronda
            var idsReferenciados = partidos
                .Where(p => p.IdSigPartido != null)
                .Select(p => p.IdSigPartido!.Value)
                .ToHashSet();

            var resultado = new ObtenerCuadroResponse
            {
                Fases = new List<FaseTorneoResponse>()
            };

            // Empezar desde los partidos de primera ronda
            var rondaActual = partidos
                .Where(p => !idsReferenciados.Contains(p.IdPartido))
                .ToList();

            while (rondaActual.Any())
            {
                resultado.Fases.Add(new FaseTorneoResponse
                {
                    NombreFase = ObtenerNombreFase(rondaActual.Count),
                    Partidos = rondaActual.Select(p => new PartidoResponse
                    {
                        IdPartido = p.IdPartido,
                        IdEquipoLocal = p.IdEquipoLocal,
                        NombreLocal = p.EquipoLocal?.Nombre ?? "a confirmar",
                        IdEquipoVis = p.IdEquipoVis,
                        NombreVisitante = p.EquipoVis?.Nombre ?? "a confirmar",
                        Estado = p.Estado,
                        GolesLocal = p.GolesLocal,
                        GolesVis = p.GolesVis,
                        HoraInicio = p.HoraInicio,
                        HoraFin = p.HoraFin
                    }).ToList()
                });

                // Obtener los partidos de la siguiente ronda
                var idsSiguientes = rondaActual
                    .Where(p => p.IdSigPartido != null)
                    .Select(p => p.IdSigPartido!.Value)
                    .Distinct()
                    .ToHashSet();

                rondaActual = partidos
                    .Where(p => idsSiguientes.Contains(p.IdPartido))
                    .ToList();
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