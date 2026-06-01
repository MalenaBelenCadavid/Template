import { putData } from '../Global/ApiServices.js';

export async function actualizarRecibo(id, nuevosDatos) {
    // endpoint: "recibos/1"
    return await putData(`recibos/${id}`, nuevosDatos);
}