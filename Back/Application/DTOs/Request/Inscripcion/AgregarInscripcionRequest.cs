

namespace Application.DTOs.Request.Inscripcion
{
    public class AgregarInscripcionRequest
    {
        public int DniCliente { get; set; }
        public int IdAct { get; set; }
        public int NroAct { get; set; } //1- Entrenamiento,2- Clase, 3- Competición, etc
        public int? IdEquipo { get; set; }
    

    }
}
