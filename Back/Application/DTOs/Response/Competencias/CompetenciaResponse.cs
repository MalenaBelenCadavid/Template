
using Application.DTOs.Response.Equipos;
using Application.DTOs.Response.Partidos;


namespace Application.DTOs.Response.Competencias
{

    public class CompetenciaResponse
    {
        public int competenciaId {  get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cupos { get; set; }
        public double Precio { get; set; }
        public ICollection<EquipoResponse> Equipos { get; set; }
        public ICollection<PartidoResponse> Partidos { get; set; }
    }
}
