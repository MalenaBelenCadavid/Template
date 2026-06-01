import { getData } from '../Global/ApiServices.js';

export async function traerDatosParaImpresion(id) {
    return await getData(`recibos/${id}/imprimir`);
}