

namespace Application.DTOs.Request.Equipos
{

    public class CrearEquipoRequest
    {
        public string nombre { get; set; }
        public int CompetenciaId { get; set; }
        public int ClienteDni { get; set; }
    }
}
