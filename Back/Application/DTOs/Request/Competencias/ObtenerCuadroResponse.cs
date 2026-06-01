using System;
using System.Collections.Generic;

namespace Application.DTOs.Response.Competencias;

public class ObtenerCuadroResponse
{
    public List<FaseTorneoResponse> Fases { get; set; }
}
