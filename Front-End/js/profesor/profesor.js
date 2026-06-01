import { getData } from "../Global/ApiServices.js";
import { renderProfesorRegistrar } from "./renderProfesorRegistrar.js";
import { renderProfesorConsulta } from "./renderProfesorConsulta.js";
import { renderProfesorPasarAsistencia } from "./renderProfesorPasarAsistencia.js";
import { renderProfesorAsistenciaConsultar } from "./renderProfesorAsistenciaConsulta.js";
import { ProfesorCrearAsistencia } from "./ProfesorCrearAsistencia.js";

export function profesorPanel() {

    const botones = document.querySelectorAll(".sidebar-btn");
    const secciones = document.querySelectorAll(".content-section");

    const consultarContainer = document.querySelector("#Consultar");
    const registrarContainer = document.querySelector("#Registrar");

    const dni = localStorage.getItem("dni");

    let idClase = null;

    botones.forEach((boton, index) => {

        boton.addEventListener("click", async () => {

            botones.forEach(b => b.classList.remove("active"));
            secciones.forEach(s => s.classList.add("hidden"));

            boton.classList.add("active");
            secciones[index].classList.remove("hidden");

            const text = boton.textContent.trim();

            // =========================
            // VER CLASES
            // =========================
            if (text === "Ver clases asignadas") {

                const clases = await getData(`Clase/clases/${dni}`);

                consultarContainer.innerHTML =
                    renderProfesorConsulta(clases);

                consultarContainer.onclick = async (e) => {

                    const btn = e.target.closest(".profesor-btn");
                    if (!btn) return;

                    const id = btn.dataset.id;

                    const alumnos = await getData( `Clase/inscriptos/${id}`);

                    consultarContainer.innerHTML =
                        renderProfesorAsistenciaConsultar(alumnos);
                };
            }

            // =========================
            // PASAR ASISTENCIA
            // =========================
            if (text === "Pasar asistencia") {

                const clases = await getData(`Clase/clases/${dni}`);

                registrarContainer.innerHTML =
                    renderProfesorRegistrar(clases);

               registrarContainer.onclick = async (e) => {

    // =========================
    // ABRIR CLASE
    // =========================
    const btnClase = e.target.closest(".admin-btn[data-id]");

    if (btnClase && btnClase.classList.contains("Consultar") === false
        && btnClase.classList.contains("Presente") === false
        && btnClase.classList.contains("Ausente") === false) {

        const idClase = btnClase.dataset.id;

        const alumnos = await getData(
            `Asistencia/asistencias/clase/${idClase}`
        );

        registrarContainer.innerHTML =
            renderProfesorPasarAsistencia(alumnos);

        return;
    }

    // =========================
    // PRESENTE
    // =========================
    const btnPresente = e.target.closest(".Presente");

            if (btnPresente) {

                const idAsistencia = Number(btnPresente.dataset.id);
                const dniCliente = Number(btnPresente.dataset.dni);

                await ProfesorCrearAsistencia(
                    idAsistencia,
                    dniCliente,
                    true
                );

                const card = btnPresente.closest(".admi-card");
                if (!card) return;

                // eliminar botones
                card.querySelectorAll(".admin-btn").forEach(b => b.remove());

                // eliminar estado anterior
                card.querySelector(".estado-asistencia")?.remove();

                // crear estado nuevo
                const estado = document.createElement("div");
                estado.className = "estado-asistencia presente";
                estado.textContent = "✓ Asistió";

                card.appendChild(estado);

                return;
            }
            // =========================
            // AUSENTE
            // =========================
            const btnAusente = e.target.closest(".Ausente");

            if (btnAusente) {

                const idAsistencia = Number(btnAusente.dataset.id);
                const dniCliente = Number(btnAusente.dataset.dni);

                await ProfesorCrearAsistencia(
                    idAsistencia,
                    dniCliente,
                    false
                );

                const card = btnAusente.closest(".admi-card");
                if (!card) return;

                // eliminar botones
                card.querySelectorAll(".admin-btn").forEach(b => b.remove());

                // eliminar estado anterior
                card.querySelector(".estado-asistencia")?.remove();

                // crear estado nuevo
                const estado = document.createElement("div");
                estado.className = "estado-asistencia ausente";
                estado.textContent = "✗ Ausente";

                card.appendChild(estado);

                return;
            }
        };
            }
        });
    });
}