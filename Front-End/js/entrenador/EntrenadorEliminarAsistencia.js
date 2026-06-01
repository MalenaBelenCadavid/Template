import { deleteData } from "../Global/ApiServices.js";

export async function EntrenadorEliminarAsistencia(id) {
    const endpointUrl = `Asistencia/${id}`;
    return await deleteData(endpointUrl);
}