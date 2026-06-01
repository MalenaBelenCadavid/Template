using Application.DTOs.Request.Inscripcion;
using Application.DTOs.Response.Inscripcion;
using Application.Exceptions;
using Application.Interfaces.Asistencia;
using Application.Interfaces.Clase;
using Application.Interfaces.Cliente;
using Application.Interfaces.Competencias;
using Application.Interfaces.Entrenamiento;
using Application.Interfaces.Equipos;
using Application.Interfaces.Incripcion;

using Domain.Entities;


namespace Application.UseCases
{
    public  class InscripcionService :IInscripcionService
    {

        private readonly IInscripcionCommand _inscripcionCommand;
        private readonly IInscripcionQuery _inscripcionQuery;
        private readonly IClaseQuery _claseQuery;
        private readonly IClienteQuery _clienteQuery;
        private readonly IEntrenamientoQuery _entrenamientoQuery;
        private readonly IAsistenciaCommand _asistenciaCommand;
        private readonly ICompetenciaQuery _competenciaQuery;
        private readonly IEntrenamientoCommand _entrenamientoCommand;
        private readonly ICompetenciaCommand _competenciaCommand;
        private readonly IClaseCommand _claseCommand;
        private readonly IEquipoQuery _equipoQuery;
        
        

        public InscripcionService(
            IInscripcionCommand inscripcionCommand,
            IInscripcionQuery inscripcionQuery,
            IClaseQuery claseQuery,
            IEntrenamientoQuery entrenamientoQuery,
            IAsistenciaCommand asistenciaCommand,
            ICompetenciaQuery competenciaQuery,
            IEntrenamientoCommand entrenamientoCommand,
            IClaseCommand claseCommand,
            ICompetenciaCommand competenciaCommand,
            IClienteQuery clienteQuery,
            IEquipoQuery equipoQuery
            )
        {
            _asistenciaCommand = asistenciaCommand;
            _inscripcionCommand = inscripcionCommand;
            _inscripcionQuery = inscripcionQuery;
            _claseQuery = claseQuery;
            _entrenamientoQuery = entrenamientoQuery;
            _competenciaQuery = competenciaQuery;
            _entrenamientoCommand = entrenamientoCommand;
            _competenciaCommand = competenciaCommand;
            _claseCommand = claseCommand;
            _clienteQuery = clienteQuery;
            _equipoQuery = equipoQuery;
        }


        public async Task<InscripcionResponse> ConsultarInscripcion(int inscripcionId) {

            var inscripcion = await _inscripcionQuery.ConsultarInscripcion(inscripcionId) ?? throw new ExceptionNotFound("Profesor no existe");

            return new InscripcionResponse
            {
                IdInscripcion = inscripcion.IdInscripcion,
                DniCliente = inscripcion.DniCliente,
                Horario = inscripcion.Horario,
                PrecioInscr = inscripcion.PrecioInscr,
                IdAct = inscripcion.IdAct,    
                IdDescuento = inscripcion.IdDescuento,
                NroAct = inscripcion.NroAct,

            };
        }


        public async Task<InscripcionResponse> AgregarInscripcion(AgregarInscripcionRequest request)
        {

            if (request == null)
            {
                throw new ExceptionBadRequest("Debe ingresar datos");
            }

            if (request.DniCliente <= 0)
            {
                throw new ExceptionBadRequest("Ingrese valor valido");
            }
            if (request.IdAct <= 0)
            {
                throw new ExceptionBadRequest("Ingrese valor valido");
            }
            if (request.NroAct <= 0)
            {
                throw new ExceptionBadRequest("Ingrese valor valido");
            }

            var cliente = await _clienteQuery.ConsultarCliente(request.DniCliente) ?? throw new ExceptionNotFound("Cliente no encontrado");

            double precioInscr;
            int cupoAct;

            switch (request.NroAct)
            {
                case 1: // Entrenamiento
                    var entrenamiento = await _entrenamientoQuery.ConsultarEntrenamiento(request.IdAct);
                    if (entrenamiento == null) 
                    {
                        throw new ExceptionNotFound("No se encontró el entrenamiento");
                    }
                    precioInscr = entrenamiento.Precio;
                    cupoAct = entrenamiento.Cupo;
                    var asistenciaEntrenamiento= new Asistencia
                    {
                        DniCliente = request.DniCliente,
                        IdClase = null,
                        IdEntrenamiento = entrenamiento.IdEntrenamiento,
                        Presente = null
                    };
                    await _asistenciaCommand.RegistrarAsistencia(asistenciaEntrenamiento);
                    break;

                case 2: // Clase
                    var clase = await _claseQuery.ConsultarClase(request.IdAct);
                    if (clase == null)
                    {
                        throw new ExceptionNotFound("No se encontró la clase");
                    }

                    var asistenciaClase = new Asistencia
                    {
                      DniCliente = request.DniCliente,
                      IdClase=clase.IdClase,
                      IdEntrenamiento=null,
                      Presente=null
                    };
                    precioInscr = clase.Precio;
                    cupoAct=clase.Cupo;
                    await _asistenciaCommand.RegistrarAsistencia(asistenciaClase);
                    break;

                case 3: // Competencia
                    var competencia = await _competenciaQuery.ObtenerCompetenciaPorId(request.IdAct);
                    if (competencia == null)
                    {
                        throw new ExceptionNotFound("No se encontró la competencia");
                    }
                    precioInscr = competencia.Precio;
                    cupoAct = competencia.Cupos;
                    break;

                default:
                    throw new ExceptionBadRequest("Tipo de actividad inválido");
            }                  
                                

            if (cupoAct == 0) {
                throw new ExceptionConflict("No hay mas cupo,elija otra actividad");
            }
            switch (request.NroAct)
            {
                case 1: // Entrenamiento
                    await _entrenamientoCommand.DecrementarCupo(request.IdAct);
                    break;

                case 2: // Clase
                    await _claseCommand.DecrementarCupo(request.IdAct);
                    break;

                case 3: // Competencia
                    await _competenciaCommand.DecrementarCupo(request.IdAct);
                    break;

                default:
                    throw new ExceptionBadRequest("Tipo de actividad inválido");
            }


            var inscripcion = new Inscripcion
            {
                DniCliente = request.DniCliente,
                Horario = DateTime.Now, 
                PrecioInscr = precioInscr,
                IdAct = request.IdAct,
                NroAct = request.NroAct //1 - ENTRENAMIENTO, 2 - CLASE, 3 - COMPETICIÓN
            };
                           

            var InscripcionCreada = await _inscripcionCommand.AgregarInscripcion(inscripcion);


            return new InscripcionResponse
            {
                IdInscripcion = InscripcionCreada.IdInscripcion,
                DniCliente = InscripcionCreada.DniCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Horario = InscripcionCreada.Horario, // o request.Horario si lo trae
                PrecioInscr = InscripcionCreada.PrecioInscr,
                IdAct = InscripcionCreada.IdAct,
                IdDescuento = 2,
                NroAct =InscripcionCreada.NroAct
            };


        }


        public async Task<InscripcionResponse> EliminarInscripcion(int inscripcionId) {

            if (inscripcionId <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un id valido");
            }

            var inscripcion = await _inscripcionQuery.ConsultarInscripcion(inscripcionId);

            if (inscripcion == null)
            {
                throw new ExceptionNotFound("Inscripcion no encontrada");
            }

            var inscripcionEliminada = await _inscripcionCommand.EliminarInscripcion(inscripcion);

            return new InscripcionResponse
            {
                IdInscripcion = inscripcionEliminada.IdInscripcion,

                IdAct = inscripcionEliminada.IdAct,

                DniCliente = inscripcionEliminada.DniCliente,

                Horario = inscripcionEliminada.Horario,

                PrecioInscr = inscripcionEliminada.PrecioInscr,

                NroAct= inscripcionEliminada.NroAct,

                IdDescuento= inscripcionEliminada.IdDescuento,




            };

        }

        public async Task<List<InscripcionResponse>> ListaDeInscriptos() {

            var inscripciones = await _inscripcionQuery.ListaDeInscriptos();


            return inscripciones.Select(inscripcion => new InscripcionResponse
            {



                IdInscripcion = inscripcion.IdInscripcion,
                DniCliente = inscripcion.DniCliente,
                Horario = inscripcion.Horario,
                PrecioInscr = inscripcion.PrecioInscr,
                NroAct = inscripcion.NroAct,
                IdAct = inscripcion.IdAct,
                IdDescuento = inscripcion.IdDescuento

            }).ToList();


        }




   


        public async Task<int> ContadorInscripcion(int idActividad, int nroActividad)
        {
            // Traer todas las inscripciones
            var inscripciones = await _inscripcionQuery.ListaDeInscriptos();

            // Filtrar por actividad y número de actividad
            var filtradas = inscripciones
                            .Where(i => i.IdAct == idActividad && i.NroAct == nroActividad)
                            .ToList();

            

            // Devolver el número de inscriptos
            return filtradas.Count;
        }

        public async Task<int> CuposEnNumeroActividad(int idActividad, int nroActividad)
        {
            int cuposTotales = 0;

            switch (idActividad)
            {
                case 1: // Entrenamiento
                    var entrenamiento = await _entrenamientoQuery.ConsultarEntrenamiento(nroActividad);
                    cuposTotales = entrenamiento.Cupo;
                    break;

                case 2: // Clase
                    var clase = await _claseQuery.ConsultarClase(nroActividad);
                    cuposTotales = clase.Cupo;
                    break;

                default:
                    throw new ExceptionBadRequest("Actividad no válida");
            }

            // Contar inscriptos filtrados por actividad y número
            int inscriptos = await _inscripcionQuery.ContadorInscripcion(idActividad, nroActividad);//consulta numeros de inscriptos de la clase/entrenamiento

            // Restar cupos totales - inscriptos
            return cuposTotales - inscriptos;
        }


        



        }









    }

