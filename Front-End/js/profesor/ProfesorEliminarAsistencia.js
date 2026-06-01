import { deleteData } from "../Global/ApiServices.js";

export async function ProfesorEliminarAsistencia(id) {
    const endpointUrl = `Asistencia/${id}`;
    return await deleteData(endpointUrl);
}