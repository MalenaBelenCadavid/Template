
namespace Domain.Entities
{
    public class Partido
    {
        public int IdPartido { get; set; }
        public int IdCompetencia { get; set; }

        public int Fecha { get; set; }

        public int IdEquipoLocal { get; set; }
        public int IdEquipoVis {  get; set; }
        public int? GolesLocal { get; set; } 
        public int? GolesVis { get; set; }

        public string Estado { get; set; }= "Programado";

        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin {  get; set; }

        public int? IdSigPartido { get; set; }

        public Partido? SigPartido { get; set; }
        public Equipo EquipoLocal { get; set; }
        public Equipo EquipoVis { get; set; }
        public Competencia Competencia { get; set; }
    }
}
