
namespace Domain.Entities
{
    public class Clase
    {
        public int IdClase { get; set; }
        public string Nombre { get; set; }
        public int Cupo { get; set; }
        public DateOnly Dia { get; set; }
        public TimeSpan Horario { get; set; }
        public IEnumerable<Inscripcion> Inscripto { get; set; }
        public int DniProfesor {  get; set; }
        public double Precio { get; set; }
        public int IdActividad { get; set; }

        //Relación
        public Profesor Profesor { get; set; }
        public IEnumerable<Asistencia>? Asistencias { get; set; }

        
    }
}
