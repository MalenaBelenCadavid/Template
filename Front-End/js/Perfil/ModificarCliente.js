import { putData } from "../Global/ApiServices.js";

export async function ModificarCliente(cliente) {

    return await putData("Clientes", cliente);

}