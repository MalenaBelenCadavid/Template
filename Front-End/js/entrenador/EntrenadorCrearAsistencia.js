import { putData } from "../Global/ApiServices.js";

export async function EntrenadorCrearAsistencia(idAsistencia,dniCliente, presente) {
    const endpointUrl = `Asistencia`;

    const body = {
        idAsistencia:idAsistencia,
        dniCliente: dniCliente,
        presente: presente
    };

    return await putData(endpointUrl, body);
}