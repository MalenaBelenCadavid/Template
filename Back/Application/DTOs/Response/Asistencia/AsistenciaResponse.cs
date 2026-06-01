

namespace Application.DTOs.Response.Asistencia
{
    public class AsistenciaResponse
    {
        public int IdAsistencia { get; set; }
        public int DniCliente { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int? IdClase { get; set; }
        public int? IdEntrenamiento { get; set; }
        public bool? Presente { get; set; }
    }
}
