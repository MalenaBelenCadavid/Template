import { getEquipos } from "../Competiciones/getEquipos.js";
import { generarFixture } from "../Fixture/generarFixture.js";
import { CrearModalEquipos } from "./Modals/VerEquiposModal.js";
import { VerFixtures } from "./Modals/VerFixture.js";

export function RenderCompetitionAdminCards(competiciones) {


  if (!window.eventGenerarFixture) {

    document.addEventListener("click", async (e) => {

      const btnFixture = e.target.closest(".admin-btn-fixture");
      if (!btnFixture || btnFixture.disabled) return;

      const competenciaId = Number(btnFixture.dataset.id);
      const mode = btnFixture.dataset.mode;

      try {

        if (mode === "generate") {

          await generarFixture(competenciaId);

          btnFixture.disabled = true;
          btnFixture.textContent = "Fixture generado";
          btnFixture.dataset.mode = "view";

          Swal.fire({
            toast: true,
            position: "bottom-end",
            icon: "success",
            title: "Fixture generado correctamente",
            showConfirmButton: false,
            timer: 2500,
            timerProgressBar: true,
            customClass: {
              popup: "toast-golahora toast-popup-success",
              title: "toast-title"
            }
          });

        }

   
        else if (mode === "view") {

          const competencia = competiciones.find(
            c => c.competenciaId === competenciaId
          );

          const partidos = competencia?.partidos ?? [];

          const modalExistente = document.querySelector("#modal-fixture");
          if (modalExistente) modalExistente.remove();

          const modalHTML = VerFixtures(partidos);

          document.body.insertAdjacentHTML("beforeend", modalHTML);

          const modal = document.querySelector("#modal-fixture");

          modal.querySelector(".cerrar-modal")
            .addEventListener("click", () => modal.remove());

          modal.addEventListener("click", (e) => {
            if (e.target === modal) modal.remove();
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
            "Error en fixture",
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,
          customClass: {
            popup: "toast-golahora toast-popup-error",
            title: "toast-title"
          }
        });
      }
    });

    window.eventGenerarFixture = true;
  }

  if (!window.eventVerEquipos) {

    document.addEventListener("click", async (e) => {

      const btnEquipos = e.target.closest(".admin-btn-teams");
      if (!btnEquipos || btnEquipos.disabled) return;

      const competenciaId = Number(btnEquipos.dataset.id);

      try {

        const equipos = await getEquipos(competenciaId);

        const modalExistente = document.querySelector("#modal-equipos");
        if (modalExistente) modalExistente.remove();

        const modalHTML = CrearModalEquipos(equipos);

        document.body.insertAdjacentHTML("beforeend", modalHTML);

        const modal = document.querySelector("#modal-equipos");

        const cerrar = () => modal.remove();

        modal.querySelector(".cerrar-modal-equipos")
          .addEventListener("click", cerrar);

        modal.addEventListener("click", (e) => {
          if (e.target === modal) cerrar();
        });

      } catch (error) {

        console.error(error);

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "error",
          title: error?.message ?? "Error al cargar equipos",
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,
          customClass: {
            popup: "toast-golahora toast-popup-error",
            title: "toast-title"
          }
        });
      }
    });

    window.eventVerEquipos = true;
  }


  if (!competiciones?.length) {

    return `
      <p style="color: rgba(255,255,255,0.4)">
        No se encontraron competencias.
      </p>
    `;
  }

  return `
    <div class="admin-clases-grid">

      ${competiciones.map(c => {

        const tieneEquipos = (c.equipos?.length ?? 0) >= 2;
        const sinEquipos = (c.equipos?.length ?? 0) === 0;
        const tienePartidos = Array.isArray(c.partidos) && c.partidos.length > 0;

        const puedeGenerarFixture = c.fixtureGenerado === false && tieneEquipos;

        return `
          <div class="admin-card" data-id="${c.competenciaId}">

            <div class="admin-card-header">

              <div>
                <h3 class="admin-card-title">${c.nombre}</h3>
                <p class="admin-card-subtitle">${c.tipo ?? "Competencia"}</p>
              </div>

              <span class="admin-badge">
                ${c.estado ? "Activa" : "Inactiva"}
              </span>

            </div>

            <div class="admin-card-info">

              <div class="admin-info-item">
                <span class="admin-info-label">Cupos</span>
                <span class="admin-info-value">${c.cupos ?? 0}</span>
              </div>

              <div class="admin-info-item">
                <span class="admin-info-label">Precio</span>
                <span class="admin-info-value">$${c.precio ?? 0}</span>
              </div>

            </div>

            <div class="admin-card-extra">

              <div class="admin-professional-box">
                <span>${c.descripcion ?? "Sin descripción"}</span>
              </div>

            </div>

            <div class="admin-card-actions">

              <button class="admin-btn admin-btn-edit" data-id="${c.competenciaId}">
                Editar
              </button>

              <button 
                class="admin-btn admin-btn-fixture"
                data-id="${c.competenciaId}"
                data-mode="${puedeGenerarFixture ? "generate" : "view"}"
              >
                ${puedeGenerarFixture ? "Generar Fixture" : "Ver fixture"}
              </button>

              <button 
                class="admin-btn admin-btn-teams"
                data-id="${c.competenciaId}"
                ${sinEquipos ? "disabled" : ""}
              >
                ${sinEquipos ? "Sin inscriptos" : "Ver equipos"}
              </button>

              <button class="admin-btn admin-btn-delete" data-id="${c.competenciaId}">
                Eliminar
              </button>

            </div>

          </div>
        `;
      }).join("")}

    </div>
  `;
}