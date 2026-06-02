namespace Application.Response
{
    public class DescuentoResponse
    {
        public int IdDescuento { get; set; }
        public decimal Valor { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoDescuento { get; set; }
        public bool Activo { get; set; }
    }
}
