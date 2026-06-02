import { getData } from "../Global/ApiServices.js";
export async function getCuadro(idTorneo){

   return await getData(
      `Torneo/ObtenerCuadro?idTorneo=${idTorneo}`
   );

}