

namespace Domain.Entities
{
    public class Cliente:Usuario
    {
        public IEnumerable<Reserva> Reservas {  get; set; }
        public IEnumerable<Inscripcion> Inscripciones { get; set; }
        public IEnumerable<Cobro> Cobros { get; set; }
        public bool EsSocio {  get; set; }

        public IEnumerable<Equipo>? Equipos { get; set; }
        public IEnumerable<Asistencia>? Asistencias { get; set; }
    }
}
