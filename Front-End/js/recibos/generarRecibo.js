import { postData } from '../Global/ApiServices.js';

export async function registrarRecibo(datos) {
    // Ejemplo: datos = { concepto: "...", monto: 100 }
    return await postData("recibos", datos);
}