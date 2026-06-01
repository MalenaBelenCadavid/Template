

namespace Application.DTOs.Response.Partidos
{
    public class PartidoResponse
    {
            public int IdPartido { get; set; }
            public int IdCompetencia { get; set; }
            public int? IdEquipoLocal { get; set; }
            public string NombreLocal { get; set; }
            public int? IdEquipoVis { get; set; }
            public string NombreVisitante { get; set; }
            public int? GolesLocal { get; set; }
            public int? GolesVis { get; set; }
            public DateTime HoraInicio { get; set; }
            public DateTime HoraFin { get; set; }
            public string Estado { get; set; } 
    }
}
