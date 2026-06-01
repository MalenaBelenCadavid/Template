using Application.DTOs.Response.Asistencia;
using Application.DTOs.Response.Inscripcion;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Incripcion
{
    public interface IInscripcionQuery
    {
        Task<Inscripcion> ConsultarInscripcion(int inscripcionId , CancellationToken ct = default);
        Task<List<Inscripcion>> ListaDeInscriptos(CancellationToken ct = default);

        Task<int> ContadorInscripcion(int idActividad, int nroActividad);

        Task<int> CuposEnNumeroActividad(int idActividad, int nroActividad);
        Task<bool> EstaIncripto(int idActividad, int dni,CancellationToken ct = default);
        Task<int> IngresosPorInscripciones(CancellationToken ct = default);




    }
}
