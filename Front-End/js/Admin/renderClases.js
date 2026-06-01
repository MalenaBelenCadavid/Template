import { CrearModalInscriptos } from "../Admin/Modals/VerInscriptos.js";
import { getInscriptos } from "../Inscripciones/getInscriptos.js";

export function RenderAdminClasesCards(clases) {

  if (!Array.isArray(clases) || clases.length === 0) {

    return `
      <p style="color: rgba(255,255,255,0.4)">
        No hay clases disponibles.
      </p>
    `;
  }

  // =========================
  // EVENT GLOBAL DETALLES
  // =========================

  if (!window.eventDetalleClase) {

    document.addEventListener("click", async (e) => {

      const btnDetalles = e.target.closest(
        ".admin-btn-detalles"
      );

      if (!btnDetalles) return;

      const card = btnDetalles.closest(".admin-card");

      if (!card) return;

      // =========================
      // VALIDAR QUE SEA CLASE
      // =========================

      if (!card.dataset.id) return;

      const id = Number(
        card.dataset.id
      );

      const clase =
        clases.find(
          x => x.idClase === id
        );

      if (!clase) return;

      try {

        // =========================
        // TRAER INSCRIPTOS
        // =========================

        const inscriptos =
          await getInscriptos(
            "Clase",
            id
          );

        // =========================
        // ELIMINAR MODAL EXISTENTE
        // =========================

        const modalExistente =
          document.querySelector(
            "#modal-inscriptos"
          );

        if (modalExistente) {
          modalExistente.remove();
        }

        // =========================
        // CREAR MODAL
        // =========================

        const modalHTML =
          CrearModalInscriptos(
            inscriptos ?? [],
            clase.nombre
          );

        document.body.insertAdjacentHTML(
          "beforeend",
          modalHTML
        );

        const modal =
          document.querySelector(
            "#modal-inscriptos"
          );

        // =========================
        // CERRAR MODAL
        // =========================

        const cerrarModal = () => {
          modal.remove();
        };

        modal
          .querySelector(
            ".cerrar-modal-inscriptos"
          )
          .addEventListener(
            "click",
            cerrarModal
          );

        modal.addEventListener(
          "click",
          (e) => {

            if (e.target === modal) {
              cerrarModal();
            }
          }
        );

      } catch (error) {

        console.error(error);

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "error",
          title:
            error.message ??
            "Error al obtener inscriptos",
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
    });

    window.eventDetalleClase = true;
  }

  return `
    <div class="admin-clases-grid">

      ${clases.map(c => `

        <div 
          class="admin-card"
          data-id="${c.idClase}"
        >

          <div class="admin-card-header">

            <div>

              <h3 class="admin-card-title">
                ${c.nombre}
              </h3>

              <p class="admin-card-subtitle">
                Profesor:
                ${c.profesional.nombre}
                ${c.profesional.apellido}
              </p>

            </div>

            <span class="admin-badge">
              Activa
            </span>

          </div>

          <div class="admin-card-info">

            <div class="admin-info-item">

              <span class="admin-info-label">
                Cupo
              </span>

              <span class="admin-info-value">
                ${c.cupo}
              </span>

            </div>

            <div class="admin-info-item">

              <span class="admin-info-label">
                Precio
              </span>

              <span class="admin-info-value">
                $${c.precio}
              </span>

            </div>

          </div>

          <div class="admin-card-extra">

            <div class="admin-professional-box">

              <span>
                ${c.profesional.correo}
                |
              </span>

              <span>
                ${c.profesional.localidad}
                -
                ${c.profesional.pais}
              </span>

            </div>

          </div>

          <div class="admin-card-actions">

            <button class="admin-btn admin-btn-detalles">
              Ver Detalles
            </button>

            <button 
              class="admin-btn admin-btn-delete"
              data-id="${c.idClase}"
            >
              Eliminar
            </button>

          </div>

        </div>

      `).join("")}

    </div>
  `;
}