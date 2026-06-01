

using Application.DTOs.Response.TipoCancha;

namespace Application.DTOs.Response.Cancha
{
    public class MetricasCanchaResponse
    {
        public string Nombre { get; set; }
        public TipoCanchaResponse tipoCancha { get; set; }
    }
}
