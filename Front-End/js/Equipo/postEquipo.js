import { postData } from "../Global/ApiServices.js";

export async function InsertarEquipo(nombre,idCompetencia,dni) {

    const body = 
    {
        nombre:nombre,
        competenciaId:idCompetencia,
        clienteDni:dni
    };

    return await postData("Equipo",body);
    
}