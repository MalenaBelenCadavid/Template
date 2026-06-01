using Application.DTOs.Response.Partidos;
namespace Application.DTOs.Response.Competencias;


public class FaseTorneoResponse
{
    public string NombreFase { get; set; }
    public List<PartidoResponse> Partidos { get; set; }
}
