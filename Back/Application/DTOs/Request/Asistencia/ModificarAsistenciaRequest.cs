


namespace Application.DTOs.Request.Asistencia
{
    public class ModificarAsistenciaRequest
    {
        public int IdAsistencia { get; set; }
        public int DniCliente { get; set; }
        public bool Presente { get; set; }
    }
}
