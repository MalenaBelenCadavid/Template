using Application.DTOs.Request.Asistencia;
using Application.DTOs.Request.Clase;
using Application.DTOs.Response.Asistencia;
using Application.DTOs.Response.Clase;
using Application.DTOs.Response.Inscripcion;
using Application.Exceptions;
using Application.Interfaces.Asistencia;
using Application.Interfaces.Clase;
using Application.Interfaces.Cliente;
using Application.Interfaces.Entrenamiento;
using Application.Interfaces.Incripcion;
using Application.Interfaces.Profesionales;
using Domain.Entities;


namespace Application.UseCases
{
    public  class ClaseService : IClaseService
    {

        private readonly IClaseCommand _claseCommand;
        private readonly IClaseQuery _claseQuery;       
        private readonly IInscripcionQuery _inscripcionQuery;
        private readonly IAsistenciaCommand _asistenciaCommand;
        private readonly IAsistenciaQuery _asistenciaQuery;
        private readonly IProfesionalQuery _profesionalQuery;
        private readonly IClienteQuery _clienteQuery;

        public ClaseService(
            IClaseCommand claseCommand,
            IClaseQuery claseQuery,            
            IInscripcionQuery inscripcionQuery,
            IAsistenciaCommand asistenciaCommand,
            IAsistenciaQuery asistenciaQuery,
            IProfesionalQuery profesionalQuery,
            IClienteQuery clienteQuery
            )
        {
            _asistenciaQuery = asistenciaQuery;
            _inscripcionQuery = inscripcionQuery;
            _claseCommand = claseCommand;
            _claseQuery = claseQuery;
            _asistenciaCommand = asistenciaCommand;
            _profesionalQuery = profesionalQuery;
            _clienteQuery = clienteQuery;

        }


        public async Task<ClaseResponse> ProgramarClase(ProgramarClasesRequest request)
        {
            if (request == null)
            {
                throw new ExceptionBadRequest("Debe ingresar datos");
            }
            if (String.IsNullOrEmpty(request.Nombre)) 
            {
                throw new ExceptionBadRequest("Ingrese un nombre");
            }

            if (request.Cupo <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un cupo valido");
            }

            if (request.Precio <= 0)
            {
                throw new ExceptionBadRequest("El precio es invalido");

            }
            if (request.Dia == DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ExceptionBadRequest("Ingrese una fecha valida");
            }

            var profesor = await _profesionalQuery.ObtenerProfesorPorId(request.DniProfesor);
            if (profesor == null) 
            {
                throw new ExceptionNotFound("El profesor buscado no existe");
            }

            var clase = new Clase
            {
                DniProfesor = request.DniProfesor,
                Nombre=request.Nombre,
                Cupo = request.Cupo,
                Dia = request.Dia,
                Horario=request.Hora,
                Precio = request.Precio,
                IdActividad = 2,
                Profesor=profesor
            };

            var claseProgramada = await _claseCommand.ProgramarClase(clase);

            return new ClaseResponse
            {
                Nombre= claseProgramada.Nombre,
                DniProfesor = claseProgramada.DniProfesor,
                Cupo = claseProgramada.Cupo,
                Fecha=claseProgramada.Dia,
                Hora=claseProgramada.Horario,
                IdClase = claseProgramada.IdClase,
                Precio = claseProgramada.Precio,                
            };

        }

        public async Task<ClaseResponse> ModificarClase(int claseId, ModificarClaseRequest request) {
            if (request == null)
            {
                throw new ExceptionBadRequest("Debe ingresar datos");
            }

            if (request.Cupo <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un cupo valido");
            }

            if (request.Precio <= 0)
            {
                throw new ExceptionBadRequest("El precio es invalido");

            }

            var clase = await _claseQuery.ConsultarClase(claseId);

            if (clase == null)
            {
                throw new ExceptionNotFound("Clase no encontrada");
            }
            if (request.Fecha == DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ExceptionBadRequest("Ingrese una fecha valida");
            }
            clase.Nombre = request.Nombre ?? clase.Nombre;
            clase.DniProfesor = (int)request.DniProfesor;
            clase.Precio = (int)request.Precio;
            clase.Cupo = (int)request.Cupo;
            clase.Dia = (DateOnly)request.Fecha;
            clase.Horario = (TimeSpan)request.Hora;
            
            var ClaseModificada = await _claseCommand.ModificarClase(clase);

            return new ClaseResponse
            {
                Nombre= clase.Nombre,
                IdClase= clase.IdClase,
                DniProfesor = ClaseModificada.DniProfesor,
                Precio = ClaseModificada.Precio,
                Cupo = ClaseModificada.Cupo,
                Fecha = ClaseModificada.Dia,
                Hora = ClaseModificada.Horario, 
              
            };
        }

        public async Task<ClaseResponse> ConsultarClase(int claseId) {

            Clase clase = await _claseQuery.ConsultarClase( claseId ) ?? throw new ExceptionNotFound("La clase no existe");

             return new ClaseResponse
            {
                Nombre= clase.Nombre,
                DniProfesor = clase.DniProfesor,
                IdClase = clase.IdClase,
                Cupo = clase.Cupo,
                Fecha=clase.Dia,
                Hora = clase.Horario,
                Precio = clase.Precio,
            
            };
     }     
            
        public async Task<ClaseResponse> EliminarClase(int claseId) {


            if (claseId <= 0)
            {
                throw new ExceptionBadRequest("Debe ingresar un id valido");
            }

            var clase = await _claseQuery.ConsultarClase(claseId);

            if (clase == null)
            {
                throw new ExceptionNotFound("Clase no encontrada");
            }

            var claseEliminada = await _claseCommand.EliminarClase(clase);

            return new ClaseResponse
            { 
                Nombre = claseEliminada.Nombre,
                IdClase = claseEliminada.IdClase,
                Cupo = claseEliminada.Cupo,
                Fecha = claseEliminada.Dia,
                Hora = claseEliminada.Horario,
                DniProfesor = claseEliminada.DniProfesor,
                Precio= claseEliminada.Precio
              };


        }
        public async Task<List<InscripcionResponse>> ListarInscripctos() {

            var inscripciones = await _inscripcionQuery.ListaDeInscriptos();


            return inscripciones.Select(inscripcion => new InscripcionResponse
            {                              
                    IdInscripcion = inscripcion.IdInscripcion,
                    DniCliente= inscripcion.DniCliente,
                    Horario= inscripcion.Horario,
                    PrecioInscr = inscripcion.PrecioInscr,
                    NroAct= inscripcion.NroAct,
                    IdAct=inscripcion.IdAct,                    
                    IdDescuento=inscripcion.IdDescuento                            
            }).ToList();
        }
        public async Task<InscripcionResponse> DevolverInscripto(int InscripcionId)
        {

            var inscripcion = await _inscripcionQuery.ConsultarInscripcion(InscripcionId);


            return new InscripcionResponse
            {
                IdInscripcion = inscripcion.IdInscripcion,
                DniCliente = inscripcion.DniCliente,
                Nombre = inscripcion.cliente.Nombre,
                Apellido=inscripcion.cliente.Apellido,
                Horario = inscripcion.Horario,
                PrecioInscr = inscripcion.PrecioInscr,
                NroAct = inscripcion.NroAct,
                IdAct = inscripcion.IdAct,                
                IdDescuento = inscripcion.IdDescuento
            };



        }
        public async Task<int> ContarCuposLibres(int idActividad, int IdClase)
        {
            // 1. Traer el entrenamiento
            var clase= await _claseQuery.ConsultarClase(IdClase)
                                  ?? throw new ExceptionNotFound("Clase no encontrada");

            // 2. Cupos totales
            int cuposTotales = clase.Cupo;

            // 3. Contar inscriptos (consulta a Inscripción)
            int cuposOcupados = await _inscripcionQuery.ContadorInscripcion(idActividad, IdClase);

            // 4. Restar
            int cuposLibres = cuposTotales - cuposOcupados;

            // 5. Evitar negativos
            return cuposLibres;
        }
             
        public async Task<List<FullClaseResponse>> ListarClases()
        {
            var clases = await _claseQuery.ListarClases();

            return clases.Select(c => new FullClaseResponse
            {
                Nombre=c.Nombre,
                IdClase = c.IdClase,

                Cupo = c.Cupo,
                Fecha = c.Dia,
                Hora = c.Horario,
                Profesional = new DTOs.Response.Profesional.ProfesionalResponse
                {
                    Dni = c.Profesor.Dni,
                    Nombre = c.Profesor.Nombre,
                    Apellido = c.Profesor.Apellido,
                    Localidad = c.Profesor.Localidad,
                    Pais = c.Profesor.Pais,
                    Correo = c.Profesor.Correo,
                    Estado = c.Profesor.Estado,
                    Certificado = c.Profesor.Certificado,
                    EstaCertificado = c.Profesor.EstaCertificado
                },

                Precio = c.Precio,

                NroActividad = c.IdActividad

            }).ToList();
        }

        public async Task<List<AsistenciaResponse>> RegistrarAsistencia(int claseId,List<RegistrarAsistenciaRequest> requests)
        {
            var clase = await _claseQuery.ConsultarClase(claseId) ?? throw new ExceptionNotFound("La clase no fue encontrada");

            var respuestas = new List<AsistenciaResponse>();

            foreach (var request in requests)
            {
                var cliente = await _clienteQuery.ConsultarCliente(request.DniCliente) ?? throw new ExceptionNotFound($"Cliente {request.DniCliente} no encontrado");

                var estaInscripto = await _inscripcionQuery.EstaIncripto(claseId, cliente.Dni);

                if (!estaInscripto)
                {
                 throw new ExceptionConflict($"El cliente {cliente.Dni} no está inscripto en esta clase");
                }

                var asistencia = new Asistencia
                {
                    DniCliente = request.DniCliente,
                    IdClase = claseId,
                    Presente = request.Presente,
                };

                var asistenciaCreada = await _asistenciaCommand.RegistrarAsistencia(asistencia);

                respuestas.Add(new AsistenciaResponse
                {
                    IdAsistencia = asistenciaCreada.IdAsistencia,
                    DniCliente = asistenciaCreada.DniCliente,
                    IdClase = (int)asistenciaCreada.IdClase,
                    Presente = (bool)asistenciaCreada.Presente,
                });
            }

            return respuestas;
        }

        public async Task<List<InscripcionResponse>> VerInscriptos(int claseId)
        {
            var clase = await _claseQuery.ConsultarClase(claseId); 
                
            var inscriptos = await _claseQuery.MostrarInscriptos(claseId);

            return inscriptos.Select(inscripto => new InscripcionResponse
            {

                IdInscripcion = inscripto.IdInscripcion,
                DniCliente = inscripto.DniCliente,
                Nombre=inscripto.cliente.Nombre,
                Apellido=inscripto.cliente.Apellido,
                Horario = inscripto.Horario,
                PrecioInscr = inscripto.PrecioInscr,
                NroAct = inscripto.NroAct,
                IdAct = inscripto.IdAct,
                IdDescuento = inscripto.IdDescuento
            }).ToList();
        }

        public async Task<List<ClaseResponse>> VerClasesPorProfesorDni(int ProfesorDni)
        {
            var profesor = await _profesionalQuery.ObtenerProfesorPorId(ProfesorDni) ?? throw new ExceptionNotFound("El profesor buscado no fue encontrado");
            var clases = await _claseQuery.VerClasesPorProfesor(ProfesorDni);
            return clases.Select(c => new ClaseResponse 
            {
               Nombre = c.Nombre,
               IdClase = c.IdClase,
               Fecha = c.Dia,
               Hora = c.Horario,
               Cupo = c.Cupo,          
               Precio = c.Precio,
               DniProfesor=c.Profesor.Dni,
            }).ToList();
        }
    }
}
