namespace Domain.Entities
{
    public class Entrenamiento
    {
        public string Nombre { get; set; }
        public int IdEntrenamiento { get; set; }
        public IEnumerable<Inscripcion>Inscriptos { get; set; }
        public int DniEntrenador { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeSpan Horario { get; set; }
        public double Precio { get; set; }
        public int Cupo { get; set; }
        public int IdActividad { get; set; }

        //Relación
        public Entrenador Entrenador { get; set; }
        public IEnumerable<Asistencia>? Asistencias { get; set; }
    }
}
