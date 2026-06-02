import { postData } from "../Global/ApiServices.js";

export async function generarFixtureLiga(idLiga) {

  try {

    return await postData(
      `Liga/GenerarFixture?idLiga=${idLiga}`,
      null
    );

  } catch (error) {

    Swal.fire({
      toast: true,
      position: "bottom-end",
      icon: "error",
      title:
        error?.response?.data?.Message ??
        "Error al generar fixture de liga",
      showConfirmButton: false,
      timer: 2500
    });

    throw error;
  }
}