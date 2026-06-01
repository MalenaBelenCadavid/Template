import {putData} from "../Global/ApiServices.js";

export async function ProfesorCrearAsistencia(IdAsistencia,dniCliente,presente) {
    const endpointUrl = `Asistencia`;

    const body = {
        idAsistencia: IdAsistencia,
        dniCliente: dniCliente,
        presente: presente
    };

    return await putData(endpointUrl, body);
}
