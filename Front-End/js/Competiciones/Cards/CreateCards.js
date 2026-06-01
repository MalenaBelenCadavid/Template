import { getCompeticiones } from "../GetCompeticiones.js";
import { RenderCompetitionCards } from "./RenderCards.js";
import { RealizarInscripcion } from "../../Inscripciones/crearInscripcion.js";
import { CrearModalEquipo } from "../../Equipo/crearModalEquipo.js";
import { CrearModalRegistrarCobro } from "../../Cobros/cobroModal.js";
import { crearCobro } from "../../Cobros/crearCobro.js";
import { InsertarEquipo } from "../../Equipo/postEquipo.js";
let competicionesData = [];

export async function CreateCompetitionCards(
  containerId = "competitions-container"
) {

  const container = document.getElementById(
    containerId
  );

  if (!container) return;

  container.innerHTML = `
    <p style="color: rgba(255,255,255,0.4)">
      Cargando competiciones...
    </p>
  `;

  try {

    competicionesData =await getCompeticiones();
    console.log(competicionesData);
    if (!competicionesData?.length) {

      container.innerHTML = `
        <p style="color: rgba(255,255,255,0.4)">
          No se encontraron competiciones.
        </p>
      `;

      return;
    }

    container.innerHTML =
      RenderCompetitionCards(
        competicionesData
      );

    // =========================
    // EVENT DELEGATION
    // =========================

    container.addEventListener(
      "click",
      async (e) => {

        const btn = e.target.closest(
          ".btn-inscribirse"
        );

        console.log(btn);

        if (!btn) return;

        try {

          const idAct = Number(
            btn.dataset.id
          );

          const nroAct = Number(
            btn.dataset.tipo
          );

          const dni = Number(
            localStorage.getItem("dni")
          );

          const comp = competicionesData.find(
              c =>
                c.competenciaId === idAct
            );

          console.log(comp);

          if (!comp) {
            throw new Error(
              "Competencia no encontrada"
            );
          }

          // =========================
          // DATOS COMPETENCIA
          // =========================
          const nombreCompetencia =comp.nombre;

          const precio = comp.precio ?? 0;

          // =========================
          // ELIMINAR MODAL COBRO
          // =========================

          const modalExistente =
            document.querySelector(
              "#registrarCobroModal"
            );

          if (modalExistente) {
            modalExistente.remove();
          }

          // =========================
          // MODAL COBRO
          // =========================

          const modalCobroHTML =
            CrearModalRegistrarCobro(
              nombreCompetencia,
              precio,
              dni
            );

          document.body.insertAdjacentHTML(
            "beforeend",
            modalCobroHTML
          );

          const modalCobro =
            document.querySelector(
              "#registrarCobroModal"
            );

          modalCobro.classList.add(
            "active"
          );

          document.body.classList.add(
            "modal-open"
          );

          // =========================
          // CERRAR MODAL COBRO
          // =========================

          const cerrarModalCobro =
            () => {

              modalCobro.remove();

              document.body.classList.remove(
                "modal-open"
              );
            };

          modalCobro
            .querySelector(
              ".cerrar-modal-cobro"
            )
            .addEventListener(
              "click",
              cerrarModalCobro
            );

          modalCobro
            .querySelector(
              ".cancelar-btn"
            )
            .addEventListener(
              "click",
              cerrarModalCobro
            );

          // =========================
          // FORM COBRO
          // =========================

          const formCobro =
            modalCobro.querySelector(
              "#formRegistrarCobro"
            );

          formCobro.addEventListener(
            "submit",
            async (ev) => {

              ev.preventDefault();

              try {

                const metodoPago =
                  modalCobro
                    .querySelector(
                      "#metodoPagoCobro"
                    )
                    .value;

                if (!metodoPago) {
                  throw new Error(
                    "Seleccione un método de pago"
                  );
                }

                cerrarModalCobro();

                // =========================
                // ELIMINAR MODAL EQUIPO
                // =========================

                const modalEquipoExistente =
                  document.querySelector(
                    "#modalEquipo"
                  );

                if (
                  modalEquipoExistente
                ) {
                  modalEquipoExistente.remove();
                }

                // =========================
                // ABRIR MODAL EQUIPO
                // =========================

                document.body.insertAdjacentHTML("beforeend",CrearModalEquipo());

                const modalEquipo =
                  document.querySelector(
                    "#modalEquipo"
                  );

                modalEquipo.classList.add(
                  "active"
                );

                document.body.classList.add(
                  "modal-open"
                );

                // =========================
                // CERRAR MODAL EQUIPO
                // =========================

                const cerrarModalEquipo =
                  () => {

                    modalEquipo.remove();

                    document.body.classList.remove(
                      "modal-open"
                    );
                  };

                modalEquipo
                  .querySelector(
                    ".cerrar-modal-equipo"
                  )
                  .addEventListener(
                    "click",
                    cerrarModalEquipo
                  );

                modalEquipo.addEventListener(
                  "click",
                  (e) => {

                    if (
                      e.target ===
                      modalEquipo
                    ) {
                      cerrarModalEquipo();
                    }
                  }
                );

                // =========================
                // FORM EQUIPO
                // =========================

                const formEquipo =
                  modalEquipo.querySelector(
                    "#formEquipo"
                  );

                formEquipo.addEventListener(
                  "submit",
                  async (e) => {

                    e.preventDefault();

                    try {

                      const nombreEquipo = modalEquipo.querySelector("#nombreEquipo").value.trim();

                      if (!nombreEquipo) {
                        throw new Error("Ingrese un nombre de equipo");
                      }

                      console.log("Equipo:",nombreEquipo);

                      // =========================
                      // CREAR EQUIPO
                      // =========================
                        const equipo= await InsertarEquipo(nombreEquipo,comp.competenciaId,dni)

                      // =========================
                      // INSCRIPCION
                      // =========================

                      var inscripcion =await RealizarInscripcion(
                        dni,
                        idAct,
                        nroAct,
                        equipo
                      );
                      console.log("INSCRIPCION",inscripcion)

                      // =========================
                      // COBRO
                      // =========================

                      await crearCobro(
                        null,
                        inscripcion.idInscripcion,
                        dni,
                        precio,
                        metodoPago,
                        "Competencia"
                      );


                      comp.cupos -= 1;

                      if (
                        comp.cupos < 0
                      ) {
                        comp.cupos = 0;
                      }

                      container.innerHTML =
                        RenderCompetitionCards(
                          competicionesData
                        );

                      cerrarModalEquipo();

                      Swal.fire({
                        toast: true,
                        position:
                          "bottom-end",
                        icon: "success",
                        title:
                          "Inscripción realizada correctamente",
                        timer: 2500,
                        showConfirmButton:
                          false,
                        customClass: {
                          popup:
                            "toast-golahora toast-popup-success",
                          title:
                            "toast-title"
                        }
                      });

                    } catch (error) {

                      console.error(
                        error
                      );

                      Swal.fire({
                        toast: true,
                        position:
                          "bottom-end",
                        icon: "error",
                        title:
                          error.message ??
                          "Error al registrar inscripción",
                        timer: 2500,
                        showConfirmButton:
                          false,
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

              } catch (error) {

                console.error(error);

                Swal.fire({
                  toast: true,
                  position:
                    "bottom-end",
                  icon: "error",
                  title:
                    error.message ??
                    "Error al registrar cobro",
                  timer: 2500,
                  showConfirmButton:
                    false,
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

        } catch (error) {

          console.error(error);
        }
      }
    );

  } catch (err) {

    console.error(err);

    container.innerHTML = `
      <p style="color: rgba(255,255,255,0.4)">
        Error al cargar las competiciones.
      </p>
    `;
  }
}