import { getCuadro } from "../Competiciones/getCuadro.js";
import { getEquipos } from "../Competiciones/getEquipos.js";
import { generarFixture }from "../Fixture/generarFixture.js";
import { generarFixtureLiga }from "../Fixture/generarFixtureLiga.js";
import { CrearModalEquipos }from "./Modals/VerEquiposModal.js";
import { VerFixtures }from "./Modals/VerFixture.js";
import { VerLiga }from "./Modals/VerLiga.js";

export function RenderCompetitionAdminCards(competiciones) {

  // =========================================
  // EVENTO VER / GENERAR COMPETENCIA
  // =========================================

  if (!window.eventCompetenciaView) {

    document.addEventListener("click", async (e) => {

      const btnFixture =
        e.target.closest(".admin-btn-fixture");

      if (!btnFixture || btnFixture.disabled)
        return;

      const competenciaId =
        Number(btnFixture.dataset.id);

      const mode =
        btnFixture.dataset.mode;

      try {

        const competencia =
          competiciones.find(
            c => c.competenciaId === competenciaId
          );

        if (!competencia) return;

        // =========================================
        // GENERAR FIXTURE
        // =========================================

        if (mode === "generate") {

          // =========================================
          // TORNEO
          // =========================================

          if (competencia.tipo === "Torneo") {

            await generarFixture(
              competenciaId
            );

          }

          // =========================================
          // LIGA
          // =========================================

          else if (competencia.tipo === "Liga") {

            await generarFixtureLiga(
              competenciaId
            );

          }

          btnFixture.dataset.mode = "view";

          btnFixture.textContent =
            competencia.tipo === "Torneo"
              ? "Ver Cuadro"
              : "Ver Liga";

          Swal.fire({
            toast: true,
            position: "bottom-end",
            icon: "success",
            title:
              competencia.tipo === "Torneo"
                ? "Cuadro generado correctamente"
                : "Fixture generado correctamente",
            showConfirmButton: false,
            timer: 2500,
            timerProgressBar: true,
            customClass: {
              popup:
                "toast-golahora toast-popup-success",
              title: "toast-title"
            }
          });

        }

        // =========================================
        // VER COMPETENCIA
        // =========================================

        else if (mode === "view") {

          // =========================================
          // ELIMINAR MODALES ANTERIORES
          // =========================================

          document
            .querySelector("#modal-fixture")
            ?.remove();

          document
            .querySelector("#modal-liga")
            ?.remove();

          // =========================================
          // TORNEO
          // =========================================

          if (competencia.tipo === "Torneo") {

            const cuadro =
              await getCuadro(
                competenciaId
              );

            const modalHTML =
              VerFixtures(cuadro);

            document.body.insertAdjacentHTML(
              "beforeend",
              modalHTML
            );

          }

          // =========================================
          // LIGA
          // =========================================

          else if (competencia.tipo === "Liga") {

            const modalHTML =
              VerLiga(competencia);

            document.body.insertAdjacentHTML(
              "beforeend",
              modalHTML
            );

          }

          // =========================================
          // OBTENER MODAL ACTUAL
          // =========================================

          const modal =
            document.querySelector(
              "#modal-fixture"
            ) ||
            document.querySelector(
              "#modal-liga"
            );

          if (!modal) return;

          // =========================================
          // CERRAR MODAL
          // =========================================

          modal.querySelector(".cerrar-modal")
            .addEventListener("click", () => {

              modal.remove();

            });

          modal.addEventListener("click", (e) => {

            if (e.target === modal) {

              modal.remove();

            }

          });

        }

      } catch (error) {

        console.error(error);

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "error",
          title:
            error?.response?.data?.Message ??
            error?.message ??
            "Error en competencia",
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,
          customClass: {
            popup:
              "toast-golahora toast-popup-error",
            title: "toast-title"
          }
        });

      }

    });

    window.eventCompetenciaView = true;
  }

  // =========================================
  // EVENTO VER EQUIPOS
  // =========================================

  if (!window.eventVerEquipos) {

    document.addEventListener("click", async (e) => {

      const btnEquipos =
        e.target.closest(".admin-btn-teams");

      if (!btnEquipos || btnEquipos.disabled)
        return;

      const competenciaId =
        Number(btnEquipos.dataset.id);

      try {

        const equipos =
          await getEquipos(competenciaId);

        // =========================================
        // ELIMINAR MODAL ANTERIOR
        // =========================================

        document
          .querySelector("#modal-equipos")
          ?.remove();

        // =========================================
        // CREAR MODAL
        // =========================================

        const modalHTML =
          CrearModalEquipos(equipos);

        document.body.insertAdjacentHTML(
          "beforeend",
          modalHTML
        );

        const modal =
          document.querySelector(
            "#modal-equipos"
          );

        if (!modal) return;

        const cerrar = () => {

          modal.remove();

        };

        modal.querySelector(
          ".cerrar-modal-equipos"
        ).addEventListener(
          "click",
          cerrar
        );

        modal.addEventListener("click", (e) => {

          if (e.target === modal) {

            cerrar();

          }

        });

      } catch (error) {

        console.error(error);

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "error",
          title:
            error?.message ??
            "Error al cargar equipos",
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,
          customClass: {
            popup:
              "toast-golahora toast-popup-error",
            title: "toast-title"
          }
        });

      }

    });

    window.eventVerEquipos = true;
  }

  // =========================================
  // SIN COMPETENCIAS
  // =========================================

  if (!competiciones?.length) {

    return `
      <p style="color: rgba(255,255,255,0.4)">
        No se encontraron competencias.
      </p>
    `;
  }

  // =========================================
  // RENDER CARDS
  // =========================================

  return `

    <div class="admin-clases-grid">

      ${competiciones.map(c => {

        const tieneEquipos =
          (c.equipos?.length ?? 0) >= 2;

        const sinEquipos =
          (c.equipos?.length ?? 0) === 0;

        const tienePartidos =
          (c.partidos?.length ?? 0) > 0;

        const puedeVerFixture =
          tienePartidos;

        const puedeGenerarFixture =
          !tienePartidos && tieneEquipos;

        return `

          <div
            class="admin-card"
            data-id="${c.competenciaId}"
          >

            <div class="admin-card-header">

              <div>

                <h3 class="admin-card-title">
                  ${c.nombre}
                </h3>

                <p class="admin-card-subtitle">
                  ${c.tipo ?? "Competencia"}
                </p>

              </div>

              <span class="admin-badge">

               Activa

              </span>

            </div>

            <div class="admin-card-info">

              <div class="admin-info-item">

                <span class="admin-info-label">
                  Cupos
                </span>

                <span class="admin-info-value">
                  ${c.cupos ?? 0}
                </span>

              </div>

              <div class="admin-info-item">

                <span class="admin-info-label">
                  Precio
                </span>

                <span class="admin-info-value">
                  $${c.precio ?? 0}
                </span>

              </div>

            </div>

            <div class="admin-card-extra">

              <div class="admin-professional-box">

                <span>
                  ${c.descripcion ??
                    "Sin descripción"}
                </span>

              </div>

            </div>

            <div class="admin-card-actions">

              <!-- EDITAR -->

              <button
                class="admin-btn admin-btn-edit"
                data-id="${c.competenciaId}"
              >
                Editar
              </button>

              <!-- FIXTURE -->

              <button
                class="admin-btn admin-btn-fixture"
                data-id="${c.competenciaId}"

                data-mode="${
                  puedeVerFixture
                    ? "view"
                    : "generate"
                }"

                ${
                  !puedeGenerarFixture &&
                  !puedeVerFixture
                    ? "disabled"
                    : ""
                }
              >

                ${
                  puedeVerFixture

                    ? (
                        c.tipo === "Torneo"
                          ? "Ver Cuadro"
                          : "Ver Liga"
                      )

                    : (
                        c.tipo === "Torneo"
                          ? "Generar Cuadro"
                          : "Generar Fixture"
                      )
                }

              </button>

              <!-- EQUIPOS -->

              <button
                class="admin-btn admin-btn-teams"
                data-id="${c.competenciaId}"
                ${sinEquipos ? "disabled" : ""}
              >

                ${
                  sinEquipos
                    ? "Sin inscriptos"
                    : "Ver equipos"
                }

              </button>

              <!-- ELIMINAR -->

              <button
                class="admin-btn admin-btn-delete"
                data-id="${c.competenciaId}"
              >
                Eliminar
              </button>

            </div>

          </div>

        `;

      }).join("")}

    </div>

  `;
}