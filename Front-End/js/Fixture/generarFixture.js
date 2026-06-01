import {postData} from "../Global/ApiServices.js"
export async function generarFixture(competenciaId)
{
    try
    {
        return await postData(`Torneo/GenerarFixture?idTorneo=${competenciaId}`,null);
    }
    catch(error)
    {
         Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "error",
          title:
            error.Message ??
            "Error al generar fixture",
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,
          customClass: {
            popup:
              "toast-golahora toast-popup-error",
            title:
              "toast-title"
          }
        });
        throw error
    }
    
} 