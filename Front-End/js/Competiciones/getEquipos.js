import { getData } from "../Global/ApiServices.js";

export async function getEquipos(competicionId) {
    var competencia = await getData(`Competencia/${competicionId}`);
    return competencia?.equipos ?? [];
}