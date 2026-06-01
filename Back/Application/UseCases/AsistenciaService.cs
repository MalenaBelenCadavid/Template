using Application.DTOs.Request.Asistencia;
using Application.DTOs.Response.Asistencia;
using Application.Exceptions;
using Application.Interfaces.Asistencia;
using Application.Interfaces.Clase;
using Application.Interfaces.Entrenamiento;
using Application.Interfaces.Incripcion;
using Domain.Entities;


namespace Application.UseCases
{
    public  class AsistenciaService : IAsistenciaService
    {
        private readonly IAsistenciaCommand _asistenciaCommand;
        private readonly IAsistenciaQuery _asistenciaQuery;
        private readonly IClaseQuery _claseQuery;
        private readonly IEntrenamientoQuery _entrenamientoQuery;
        private readonly IInscripcionQuery _inscripcionQuery;
       

        public AsistenciaService(
            IAsistenciaCommand asistenciaCommand,
            IAsistenciaQuery asistenciaQuery,
            IClaseQuery claseQuery,
            IEntrenamientoQuery entrenamientoQuery
            )
        {
            _asistenciaCommand = asistenciaCommand;
            _asistenciaQuery = asistenciaQuery;
          
        }

        public async Task<AsistenciaResponse> RegistrarAsistencia(int claseId,RegistrarAsistenciaRequest request)
        {
            var asistencia = new Asistencia
            {
                IdClase=claseId,
                DniCliente = request.DniCliente,
                Presente = request.Presente
            };

            var claseCreada = await _asistenciaCommand.RegistrarAsistencia(asistencia);

            return new AsistenciaResponse
            {
                IdAsistencia = claseCreada.IdAsistencia,
                DniCliente = claseCreada.DniCliente,
                IdClase = (int)claseCreada.IdClase,
                Presente = (bool)claseCreada.Presente
            };
        }

        public async Task<AsistenciaResponse> ModificarAsistencia(ModificarAsistenciaRequest request) {
                   
            var asistencia = await _asistenciaQuery.ConsultarAsistencia(request.IdAsistencia) ?? throw new ExceptionNotFound("Alumno no encontrado");
            if(asistencia.DniCliente != request.DniCliente) 
            {
                throw new ExceptionConflict("Al alumno no le corresponde esta asistencia");
            }
            asistencia.Presente = request.Presente;
                   
            var asistenciaActualizada = await _asistenciaCommand.ModificarAsistencia (asistencia);

            return new AsistenciaResponse
            {
                IdAsistencia = asistenciaActualizada.IdAsistencia,
                DniCliente = asistenciaActualizada.DniCliente,
                nombre = asistenciaActualizada.Cliente.Nombre,
                apellido = asistenciaActualizada.Cliente.Apellido,
                IdClase = asistenciaActualizada.IdClase,
                IdEntrenamiento = asistenciaActualizada.IdEntrenamiento,
                Presente = asistenciaActualizada.Presente
            };
        }

        public async Task<AsistenciaResponse> EliminarAsitencia(int IdAsistencia) {

            var asistencia = await _asistenciaQuery.ConsultarAsistencia(IdAsistencia);

            var asistenciaEliminada = await _asistenciaCommand.EliminarAsistencia(asistencia);

            return new AsistenciaResponse
            {
                Presente = (bool)asistencia.Presente,
                DniCliente = asistencia.DniCliente,
                IdClase = (int)asistencia.IdClase,
            };


        }

        public async Task<AsistenciaResponse> ConsultarAsistencia(int IdAsistencia) {

            var asistencia = await _asistenciaQuery.ConsultarAsistencia(IdAsistencia);


            return new AsistenciaResponse
            {
                Presente = (bool)asistencia.Presente,
                DniCliente = asistencia.DniCliente,
                IdClase = (int)asistencia.IdClase,
            };

        }

        public async Task<List<AsistenciaResponse>> ListarAsistenciaClase(int idClase)
        {
            var asistencias = await _asistenciaQuery.ListarAsistenciaClase(idClase);

            return asistencias.Select(r => new AsistenciaResponse
                {
                    IdAsistencia = r.IdAsistencia,
                    DniCliente = r.DniCliente,
                    nombre = r.Cliente.Nombre,
                    apellido=r.Cliente.Apellido,
                    IdClase = r.IdClase,
                    IdEntrenamiento=null,
                    Presente = r.Presente
                }).ToList();
        }

        public async Task<List<AsistenciaResponse>> ListarAsistenciaEntrenamiento(int idEntrenamiento)
        {
            var asistencias = await _asistenciaQuery.ListarAsistenciaEntrenamiento(idEntrenamiento);

            return asistencias.Select(r => new AsistenciaResponse
                {
                    IdAsistencia = r.IdAsistencia,
                    DniCliente = r.DniCliente,
                    nombre = r.Cliente.Nombre,
                    apellido = r.Cliente.Apellido,
                    IdClase = null,
                    IdEntrenamiento=r.IdEntrenamiento,
                    Presente = r.Presente
                }).ToList();
        }



        public async Task<AsistenciaResponse> PasarAsistencia(ModificarAsistenciaRequest request)
        {
            var asistencia = await _asistenciaQuery.ConsultarAsistencia(request.IdAsistencia);

            if (asistencia == null)
            {
                throw new ExceptionBadRequest("la Asistencia no existe");
            }
            asistencia.Presente = request.Presente;

            var AsistenciaTomada = await _asistenciaCommand.ModificarAsistencia(asistencia);

            return new AsistenciaResponse
            {
                IdAsistencia = asistencia.IdAsistencia,
                Presente = (bool)asistencia.Presente,
                DniCliente = asistencia.DniCliente,
                IdClase = (int)asistencia.IdClase,
            };

        }

       
    }
}
