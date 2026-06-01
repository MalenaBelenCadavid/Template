import { getClases } from "../GetClases.js";
import { getEntrenamientos } from "../GetEntrenamientos.js";

import { RenderCardsClases } from "../Cards/Clases/RenderClasesCards.js";
import { RenderCardsEntrenamientos } from "../Cards/Entrenamientos/RenderEntrenamientosCards.js";

import { RealizarInscripcion } from "../../Inscripciones/crearInscripcion.js";

import { CrearModalRegistrarCobro } from "../../Cobros/cobroModal.js";
import { crearCobro } from "../../Cobros/crearCobro.js";

import { CrearModalEquipo } from "../../Equipo/crearModalEquipo.js";
import { InsertarEquipo } from "../../Equipo/postEquipo.js";

export async function CreateAllActivitiesCards(
  containerId = "activities-container"
) {

  const container =
    document.getElementById(containerId);

  if (!container) return;

  container.innerHTML = `
    <p style="color: rgba(255,255,255,0.4)">
      Cargando actividades...
    </p>
  `;

  try {

    const [clases, entrenamientos] =
      await Promise.all([
        getClases(),
        getEntrenamientos()
      ]);

    const clasesHTML =
      clases?.length
        ? RenderCardsClases(clases)
        : "";

    const entrenamientosHTML =
      entrenamientos?.length
        ? RenderCardsEntrenamientos(
            entrenamientos
          )
        : "";

    if (
      !clases?.length &&
      !entrenamientos?.length
    ) {

      container.innerHTML = `
        <p style="color: rgba(255,255,255,0.4)">
          No hay actividades disponibles.
        </p>
      `;

      return;
    }

    container.innerHTML = `
      ${clasesHTML}
      ${entrenamientosHTML}
    `;

    document.addEventListener(
      "click",
      async (e) => {

        const dni = Number(
          localStorage.getItem("dni")
        );

        // =========================
        // CLASE
        // =========================

        const btnClase =
          e.target.closest(
            ".btn-inscribirse"
          );

        if (btnClase) {

          const id = Number(
            btnClase.dataset.id
          );

          const clase =
            clases.find(
              c => c.idClase === id
            );

          try {

            const inscripcion =
              await RealizarInscripcion(
                dni,
                id,
                2
              );

            const modalExistente =
              document.querySelector(
                "#registrarCobroModal"
              );

            if (modalExistente) {
              modalExistente.remove();
            }

            const modalHTML =
              CrearModalRegistrarCobro(
                "Inscripción a Clase",
                clase.precio,
                dni
              );

            document.body.insertAdjacentHTML(
              "beforeend",
              modalHTML
            );

            const modal =
              document.querySelector(
                "#registrarCobroModal"
              );

            modal
              .querySelector(
                ".cerrar-modal-cobro"
              )
              .addEventListener(
                "click",
                () => {
                  modal.remove();
                }
              );

            modal
              .querySelector(
                "#formRegistrarCobro"
              )
              .addEventListener(
                "submit",
                async (e) => {

                  e.preventDefault();

                  const metodoPago =
                    modal.querySelector(
                      "#metodoPagoCobro"
                    ).value;

                  try {

                    await crearCobro(
                      null,
                      inscripcion.idInscripcion,
                      dni,
                      clase.precio,
                      metodoPago,
                      "Inscripción Clase"
                    );

                    modal.remove();

                    Swal.fire({
                      toast: true,
                      position:
                        "bottom-end",
                      icon: "success",
                      title:
                        "Clase creada correctamente",
                      showConfirmButton:
                        false,
                      timer: 2500,
                      timerProgressBar:
                        true,
                      customClass: {
                        popup:
                          "toast-golahora toast-popup-success",
                        title:
                          "toast-title"
                      }
                    });

                  } catch (error) {

                    Swal.fire({
                      toast: true,
                      position:
                        "bottom-end",
                      icon: "error",
                      title:
                        error.message ??
                        "Error al crear clase",
                      showConfirmButton:
                        false,
                      timer: 2500,
                      timerProgressBar:
                        true,
                      customClass: {
                        popup:
                          "toast-golahora toast-popup-error",
                        title:
                          "toast-title"
                      }
                    });
                  }
                }
              );

          } catch (err) {

            console.error(err);

            Swal.fire({
              toast: true,
              position: "bottom-end",
              icon: "error",
              title:
                err.message ??
                "Error al realizar inscripción",
              showConfirmButton: false,
              timer: 2500,
              timerProgressBar: true,
              customClass: {
                popup:
                  "toast-golahora toast-popup-error",
                title:
                  "toast-title"
              }
            });
          }
        }

        // =========================
        // ENTRENAMIENTO
        // =========================

        const btnEntrenamiento =
          e.target.closest(
            ".btn-inscribirse-entrenamiento"
          );

        if (btnEntrenamiento) {

          const id = Number(
            btnEntrenamiento.dataset.id
          );

          const ent =
            entrenamientos.find(
              x =>
                x.id_Entrenamiento ===
                id
            );

          try {

            const inscripcion =
              await RealizarInscripcion(
                dni,
                id,
                1
              );

            const modalExistente =
              document.querySelector(
                "#registrarCobroModal"
              );

            if (modalExistente) {
              modalExistente.remove();
            }

            const modalHTML =
              CrearModalRegistrarCobro(
                "Inscripción a Entrenamiento",
                ent.precio,
                dni
              );

            document.body.insertAdjacentHTML(
              "beforeend",
              modalHTML
            );

            const modal =
              document.querySelector(
                "#registrarCobroModal"
              );

            modal
              .querySelector(
                ".cerrar-modal-cobro"
              )
              .addEventListener(
                "click",
                () => {
                  modal.remove();
                }
              );

            modal
              .querySelector(
                "#formRegistrarCobro"
              )
              .addEventListener(
                "submit",
                async (e) => {

                  e.preventDefault();

                  const metodoPago =
                    modal.querySelector(
                      "#metodoPagoCobro"
                    ).value;

                  try {

                    await crearCobro(
                      null,
                      inscripcion.idInscripcion,
                      dni,
                      ent.precio,
                      metodoPago,
                      "Inscripción Entrenamiento"
                    );

                    modal.remove();

                    Swal.fire({
                      toast: true,
                      position:
                        "bottom-end",
                      icon: "success",
                      title:
                        "Clase creada correctamente",
                      showConfirmButton:
                        false,
                      timer: 2500,
                      timerProgressBar:
                        true,
                      customClass: {
                        popup:
                          "toast-golahora toast-popup-success",
                        title:
                          "toast-title"
                      }
                    });

                  } catch (error) {

                    Swal.fire({
                      toast: true,
                      position:
                        "bottom-end",
                      icon: "error",
                      title:
                        error.message ??
                        "Error al crear clase",
                      showConfirmButton:
                        false,
                      timer: 2500,
                      timerProgressBar:
                        true,
                      customClass: {
                        popup:
                          "toast-golahora toast-popup-error",
                        title:
                          "toast-title"
                      }
                    });
                  }
                }
              );

          } catch (err) {

            Swal.fire({
              toast: true,
              position: "bottom-end",
              icon: "error",
              title:
                err.message ??
                "Error al realizar inscripción",
              showConfirmButton: false,
              timer: 2500,
              timerProgressBar: true,
              customClass: {
                popup:
                  "toast-golahora toast-popup-error",
                title:
                  "toast-title"
              }
            });
          }
        }
      }
    );

  } catch (err) {

    container.innerHTML = `
      <p style="color: rgba(255,255,255,0.4)">
        Error al cargar actividades.
      </p>
    `;
  }
}