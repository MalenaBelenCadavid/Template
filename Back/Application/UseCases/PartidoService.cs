using System;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Partidos;
using Application.Interfaces.Equipos;
using Application.Interfaces.Competencias;
using Application.DTOs.Request.Partidos;
using Application.DTOs.Response.Partidos;
using System.Formats.Asn1;


namespace Application.UseCases
{


public class PartidoService : IPartidoService
{
    private readonly IPartidoCommand _IPartidoCommand;
    private readonly IPartidoQuery _IPartidoQuery;
    private readonly IEquipoQuery _IEquipoQuery;
    private readonly IEquipoCommand _equipoCommand;
    private readonly ICompetenciaQuery _ICompetenciaQuery;
    public PartidoService(IPartidoCommand partidoCommand, IPartidoQuery partidoQuery, IEquipoQuery equipoQuery, ICompetenciaQuery competenciaQuery, IEquipoCommand equipoCommand)
        {
            _IPartidoCommand = partidoCommand;
            _IPartidoQuery = partidoQuery;
            _IEquipoQuery = equipoQuery;
            _ICompetenciaQuery = competenciaQuery;
            _equipoCommand = equipoCommand;
        }
        public async Task<int> CrearPartido(AgregarPartidoRequest request, CancellationToken ct = default)
    {
        if (request.idEquipoLocal == request.idEquipoVis)
        {
            throw new ArgumentException("El equipo local y el equipo visitante no pueden ser el mismo.");
        }

        var existeEquipoLocal = await _IEquipoQuery.ExisteEquipo(request.idEquipoLocal, ct);
        var existeEquipoVisitante = await _IEquipoQuery.ExisteEquipo(request.idEquipoVis, ct);
        var existeCompetencia = await _ICompetenciaQuery.CompetenciaExiste(request.idCompetencia, ct);

        if (!existeEquipoLocal || !existeEquipoVisitante)
        {
            throw new ArgumentException("Uno o ambos equipos no existen.");
        }

        if (!existeCompetencia)
        {
            throw new ArgumentException("La competencia no existe.");
        }
        var partido = new Partido
        {
            IdCompetencia = request.idCompetencia,
            IdEquipoLocal = request.idEquipoLocal,
            IdEquipoVis = request.idEquipoVis,
            HoraInicio = request.horarioinicio,
            HoraFin = request.horariofin
        };
        await _IPartidoCommand.CrearPartido(partido, ct);
        return partido.IdPartido;
    }
    public async Task ModificarPartido(ModificarPartidoRequest request, CancellationToken ct = default)
    { 
        var partidoExistente = await _IPartidoQuery.ObtenerPartidoPorId(request.idPartido, ct);
        if (partidoExistente is null)
        {
            throw new ArgumentException("El partido no existe.");
        }
        var existeEquipoLocal = await _IEquipoQuery.ExisteEquipo(request.idEquipoLocal, ct);
        var existeEquipoVisitante = await _IEquipoQuery.ExisteEquipo(request.idEquipoVis, ct);
        if (!existeEquipoLocal || !existeEquipoVisitante)
        {
            throw new ArgumentException("Uno o ambos equipos no existen.");
        }


        var existeCompetencia = await _ICompetenciaQuery.CompetenciaExiste(request.idCompetencia, ct);

        if (!existeCompetencia)
        {
            throw new ArgumentException("La competencia no existe.");
        }

        if (request.GolesLocal > request.GolesVis) 
        {
           var equipoLoc = await _IEquipoQuery.ObtenerEquipoPorId(request.idEquipoLocal);
           var equipoVis = await _IEquipoQuery.ObtenerEquipoPorId(request.idEquipoLocal);
           equipoLoc.Victorias++;
           equipoVis.Derrotas++;
           await _equipoCommand.ModificarEquipo(equipoLoc);
           await _equipoCommand.ModificarEquipo(equipoVis);
        }

        if (request.GolesLocal < request.GolesVis)
        {
           var equipoLoc = await _IEquipoQuery.ObtenerEquipoPorId(request.idEquipoLocal);
           var equipoVis = await _IEquipoQuery.ObtenerEquipoPorId(request.idEquipoLocal);
           equipoVis.Victorias++;
           equipoLoc.Derrotas++;
           await _equipoCommand.ModificarEquipo(equipoLoc);
           await _equipoCommand.ModificarEquipo(equipoVis);
        }

        partidoExistente.IdPartido = request.idPartido;
        partidoExistente.IdCompetencia = request.idCompetencia;
        partidoExistente.IdEquipoLocal = request.idEquipoLocal;
        partidoExistente.GolesLocal= request.GolesLocal;
        partidoExistente.GolesVis = request.GolesVis;
        partidoExistente.IdEquipoVis = request.idEquipoVis;
        partidoExistente.HoraInicio = request.horarioinicio;
        partidoExistente.HoraFin = request.horariofin;

        await _IPartidoCommand.ModificarPartido(partidoExistente, ct);
    }
    public async Task EliminarPartido(int idPartido, CancellationToken ct = default)
    {
       var partido = await _IPartidoQuery.ObtenerPartidoPorId(idPartido, ct);
        if (partido is null)
        {
            throw new ArgumentException("El partido no existe.");
        }
        await _IPartidoCommand.EliminarPartido(partido, ct);
    }
    public async Task<PartidoResponse> ObtenerPartidoPorId(int id, CancellationToken ct = default)
    {
        var partido = await _IPartidoQuery.ObtenerPartidoPorId(id, ct);
        if (partido is null)
        {
            throw new ArgumentException("El partido no existe.");
        }
        return new PartidoResponse
        {
            IdPartido = partido.IdPartido,
            IdCompetencia = partido.IdCompetencia,
            IdEquipoLocal = partido.IdEquipoLocal,
            IdEquipoVis = partido.IdEquipoVis,
            GolesLocal = partido.GolesLocal,
            GolesVis = partido.GolesVis,
            HoraInicio = partido.HoraInicio,
            HoraFin = partido.HoraFin
        };
    }
    public async Task<IEnumerable<PartidoResponse>> ObtenerPartidoPorEquipo(int equipoId, CancellationToken ct = default)
    {
        var existequipo = await _IEquipoQuery.ExisteEquipo(equipoId, ct);
        if (!existequipo)
        {
            throw new ArgumentException("El equipo no existe.");
        }
        var partidos = await _IPartidoQuery.ObtenerPartidoPorEquipo(equipoId, ct);
        return partidos.Select(partido => new PartidoResponse
        {
            IdPartido = partido.IdPartido,
            IdCompetencia = partido.IdCompetencia,
            IdEquipoLocal = partido.IdEquipoLocal,
            IdEquipoVis = partido.IdEquipoVis,
            GolesLocal = partido.GolesLocal,
            GolesVis = partido.GolesVis,
            HoraInicio = partido.HoraInicio,
            HoraFin = partido.HoraFin
        });
    }
}
}