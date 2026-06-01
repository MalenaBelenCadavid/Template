import { getData } from "../Global/ApiServices.js";

export async function getInscriptos (tipo,actividadId) {

    const endpointUrl = `${tipo}/inscriptos/${actividadId}`;   
    return await getData(endpointUrl);
}