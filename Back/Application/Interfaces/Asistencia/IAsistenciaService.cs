using Application.DTOs.Request.Asistencia;
using Application.DTOs.Response.Asistencia;

namespace Application.Interfaces.Asistencia
{
    public interface IAsistenciaService
    {
     Task<AsistenciaResponse> RegistrarAsistencia(int claseId,RegistrarAsistenciaRequest request);
     Task<AsistenciaResponse> ModificarAsistencia(ModificarAsistenciaRequest request);
     Task<AsistenciaResponse> EliminarAsitencia(int IdAsistencia); // también faltaba el parámetro
     Task<AsistenciaResponse> ConsultarAsistencia(int IdAsistencia); // también faltaba
     Task<List<AsistenciaResponse>> ListarAsistenciaClase(int idClase);
     Task<List<AsistenciaResponse>> ListarAsistenciaEntrenamiento(int idEntrenamiento);
     Task<AsistenciaResponse> PasarAsistencia(ModificarAsistenciaRequest request);

        
    }
}
