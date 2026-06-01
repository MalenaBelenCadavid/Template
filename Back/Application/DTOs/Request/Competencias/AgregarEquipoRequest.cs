using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Competencias
{
    public class AgregarEquipoRequest
    {
        public string nombre { get; set; }
        public bool estado { get; set; } = true;
    }

}
