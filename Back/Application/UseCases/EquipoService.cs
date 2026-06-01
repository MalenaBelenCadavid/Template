using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Equipos;
using Application.DTOs.Request.Equipos;
using Application.DTOs.Response.Equipos;
using Domain.Entities;
using Application.Interfaces.Cliente;
using Application.Exceptions;
using Application.Interfaces.Competencias;

namespace Application.UseCases
{
    public class EquipoService : IEquipoService
    {
        private readonly IEquipoCommand _equipoCommand;
        private readonly IEquipoQuery _equipoQuery;
        private readonly IClienteQuery _clienteQuery;
        private readonly ICompetenciaQuery _competenciaQuery;
        public EquipoService(IEquipoCommand equipoCommand, IEquipoQuery equipoQuery, IClienteQuery clienteQuery, ICompetenciaQuery competenciaQuery)
        {
            _equipoCommand = equipoCommand;
            _equipoQuery = equipoQuery;
            _clienteQuery = clienteQuery;
            _competenciaQuery = competenciaQuery;
        }
        public async Task<int> CrearEquipo(CrearEquipoRequest request, CancellationToken ct = default)
        {
            var cliente = await _clienteQuery.ConsultarCliente(request.ClienteDni) ?? throw new ExceptionNotFound("Cliente no encontrado");
            var competencia = await _competenciaQuery.ObtenerCompetenciaPorId(request.CompetenciaId) ?? throw new ExceptionNotFound("Competencia no encontrada");
            var equipo = new Equipo
            {
                Nombre = request.nombre,
                IdCompetencia=request.CompetenciaId,
                DniCliente=request.ClienteDni,
                Estado=true,
            };
            await _equipoCommand.CrearEquipo(equipo, ct);
            return equipo.IdEquipo;
        }

        public async Task ModificarEquipo(ModificarEquipoRequest request, CancellationToken ct = default)
        {
            var equipo = await _equipoQuery.ObtenerEquipoPorId(request.idEquipo, ct) ?? throw new KeyNotFoundException("Id invalido");

            equipo.Nombre = request.nombre ?? equipo.Nombre;
            equipo.Victorias = request.victorias >= 0 ? request.victorias : equipo.Victorias;
            equipo.Derrotas = request.derrotas >= 0 ? request.derrotas : equipo.Derrotas;
            equipo.IdCompetencia = request.idCompetencia;
            equipo.Estado = request.estado;

            await _equipoCommand.ModificarEquipo(equipo, ct);
        }
        public async Task EliminarEquipo(int idEquipo, CancellationToken ct = default)
        {
            var equipo = await _equipoQuery.ObtenerEquipoPorId(idEquipo, ct) ?? throw new KeyNotFoundException("Id invalido");
            await _equipoCommand.EliminarEquipo(equipo, ct);
        }
        public async Task<EquipoResponse> ObtenerEquipoPorId(int id, CancellationToken ct = default)
        {
            var equipo = await _equipoQuery.ObtenerEquipoPorId(id, ct) ?? throw new KeyNotFoundException("Id invalido");
            return new EquipoResponse
            {
                id = equipo.IdEquipo,
                nombre = equipo.Nombre,
                victorias = equipo.Victorias,
                derrotas = equipo.Derrotas,
                estado = equipo.Estado
            };

        }
        public async Task<IEnumerable<EquipoResponse>> ObtenerEquiposPorCompetencia(int competenciaId, CancellationToken ct = default)
        {
            var equipos = await _equipoQuery.ObtenerEquiposPorCompetencia(competenciaId, ct);
            if (equipos is null) return null;
            return equipos.Select(equipo => new EquipoResponse
            {
                id = equipo.IdEquipo,
                nombre = equipo.Nombre,
                victorias = equipo.Victorias,
                derrotas = equipo.Derrotas
            });
        }
        public async Task<bool> ExisteEquipo(int equipoId, CancellationToken ct = default)
        {
            return await _equipoQuery.ExisteEquipo(equipoId, ct);
        }

    }
}


