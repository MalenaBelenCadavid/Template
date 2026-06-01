import { postData } from "../Global/ApiServices.js";


export async function RealizarInscripcion(dniCliente, idAct, nroAct,idEquipo) {
    const endpointUrl = "Inscripcion";
    const body = {
        dniCliente,
        idAct,
        nroAct,
        idEquipo
    };
   return await postData(endpointUrl, body);
        
}