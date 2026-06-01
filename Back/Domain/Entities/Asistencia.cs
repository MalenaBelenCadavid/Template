
namespace Domain.Entities
{
    public class Asistencia
    {
        public int IdAsistencia { get; set; }
        public int DniCliente { get; set; }
        public int? IdClase { get; set; }
        public int? IdEntrenamiento { get; set; }
        public bool? Presente { get; set; } // True = Presente, False = Ausente

        //Relación
        public Cliente Cliente { get; set; }
        public Clase? Clase { get; set; }
        public Entrenamiento? Entrenamiento{ get; set; }
    }
}
