using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Equipos
{

    public class ModificarEquipoRequest
    {
        public int idEquipo { get; set; }
        public string nombre { get; set; }
        public int victorias { get; set; }
        public int derrotas { get; set; }
        public int idCompetencia { get; set; }
        public bool estado { get; set; }
    }
}
