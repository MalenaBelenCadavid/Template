import { getData } from "../Global/ApiServices.js";

export async function verPartidos(competenciaId) {
    var competencia = await getData(`Competencia/${competicionId}`);
    return competencia?.partidos ?? [];   
}