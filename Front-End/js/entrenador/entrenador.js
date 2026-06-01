import { getData } from "../Global/ApiServices.js";
import { renderEntrenadorRegistrar } from "./renderEntrenadorRegistrar.js";
import { renderEntrenadorConsultar } from "./renderEntrenadorConsultar.js";
import { renderEntrenadorEliminar } from "./renderEntrenadorEliminar.js";
import { renderEntrenadorPasarAsistencia } from "./renderEntrenadorPasarAsistencia.js";
import { renderEntrenadorAsistenciaConsultar } from "./renderEntrenadorAsistenciaConsultar.js";
import { EntrenadorCrearAsistencia } from "./EntrenadorCrearAsistencia.js";

export function entrenadorPanel() {

    const botones = document.querySelectorAll(".sidebar-btn");
    const secciones = document.querySelectorAll(".content-section");

    const consultarContainer = document.querySelector("#Consultar");
    const registrarContainer = document.querySelector("#Registrar");
    const eliminarContainer = document.querySelector("#Eliminar");

    let entrenamientoActivoId = null;

    // =========================
    // CONSULTAR
    // =========================
    consultarContainer.onclick = async (e) => {

        const btn = e.target.closest(".Consultar");
        if (!btn) return;

        const id = btn.dataset.id;

        const inscriptos = await getData(
            `Entrenamiento/inscriptos/${id}`
        );

        consultarContainer.innerHTML =
            renderEntrenadorAsistenciaConsultar(inscriptos);
    };

    // =========================
    // REGISTRAR
    // =========================
    registrarContainer.onclick = async (e) => {

        // =========================
        // ABRIR ENTRENAMIENTO
        // =========================
        const btnClase = e.target.closest(".Consultar");

        if (btnClase) {

            entrenamientoActivoId = btnClase.dataset.id;

            const alumnos = await getData(
                `Asistencia/asistencias/entrenamiento/${entrenamientoActivoId}`
            );

            registrarContainer.innerHTML =
                renderEntrenadorPasarAsistencia(alumnos);

            return;
        }

        // =========================
        // PRESENTE
        // =========================
        const btnPresente = e.target.closest(".Presente");

        if (btnPresente) {

            console.log("CLICK PRESENTE");

            const idAsistencia = Number(btnPresente.dataset.id);
            const dniCliente = btnPresente.dataset.dni;

            await EntrenadorCrearAsistencia(
                idAsistencia,
                dniCliente,
                true
            );

            await refrescarEntrenamiento();
            return;
        }

        // =========================
        // AUSENTE
        // =========================
        const btnAusente = e.target.closest(".Ausente");

        if (btnAusente) {

            console.log("CLICK AUSENTE");

            const idAsistencia = Number(btnAusente.dataset.id);
            const dniCliente = btnAusente.dataset.dni;

            await EntrenadorCrearAsistencia(
                idAsistencia,
                dniCliente,
                false
            );

            await refrescarEntrenamiento();
            return;
        }
    };

    // =========================
    // REFRESH CENTRALIZADO
    // =========================
    async function refrescarEntrenamiento() {

        const alumnos = await getData(
            `Asistencia/asistencias/entrenamiento/${entrenamientoActivoId}`
        );

        registrarContainer.innerHTML =
            renderEntrenadorPasarAsistencia(alumnos);
    }

    // =========================
    // SIDEBAR
    // =========================
    botones.forEach((boton, index) => {

        boton.addEventListener("click", async () => {

            botones.forEach(b => b.classList.remove("active"));
            secciones.forEach(s => s.classList.add("hidden"));

            boton.classList.add("active");
            secciones[index].classList.remove("hidden");

            const seccionAct = boton.textContent.trim();
            const dni = localStorage.getItem("dni");

            if (!dni) {
                console.error("DNI no encontrado en localStorage");
                return;
            }

            // =========================
            // CONSULTAR
            // =========================
            if (seccionAct === "Ver entrenamientos asignados") {

                const entrenamientos = await getData(
                    `Entrenamiento/entrenamientos/${dni}`
                );

                consultarContainer.innerHTML =
                    renderEntrenadorConsultar(entrenamientos);
            }

            // =========================
            // REGISTRAR
            // =========================
            if (seccionAct === "Pasar asistencia") {

                const entrenamientos = await getData(
                    `Entrenamiento/entrenamientos/${dni}`
                );

                registrarContainer.innerHTML =
                    renderEntrenadorRegistrar(entrenamientos);
            }
        });
    });
}