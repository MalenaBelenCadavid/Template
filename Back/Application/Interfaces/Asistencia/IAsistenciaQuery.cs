

using Application.DTOs.Response.Asistencia;

namespace Application.Interfaces.Asistencia
{
    public interface IAsistenciaQuery
    {

         Task<Domain.Entities.Asistencia> ConsultarAsistencia(int idAsistencia);
         Task<List<Domain.Entities.Asistencia>> ListarAsistenciaClase(int idClase,CancellationToken ct = default);
        Task<List<Domain.Entities.Asistencia>> ListarAsistenciaEntrenamiento(int idEntrenamiento, CancellationToken ct = default);


    }
}
