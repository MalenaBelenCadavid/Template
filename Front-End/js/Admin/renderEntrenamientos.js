import { CrearModalInscriptos } from "../Admin/Modals/VerInscriptos.js";
import { getInscriptos } from "../Inscripciones/getInscriptos.js";

export function RenderAdminEntrenamientoCards(entrenamientos) {

  if (!Array.isArray(entrenamientos) || entrenamientos.length === 0) {

    return `
      <p style="color: rgba(255,255,255,0.4)">
        No hay entrenamientos disponibles.
      </p>
    `;
  }

  // =========================
  // EVENT GLOBAL DETALLES
  // =========================

  if (!window.eventDetalleEntrenamiento) {

    document.addEventListener("click", async (e) => {

      const btnDetalles = e.target.closest(
        ".admin-btn-detalles"
      );

      if (!btnDetalles) return;

      const card = btnDetalles.closest(".admin-card");

      if (!card) return;

      const id = Number(
        card.dataset.id
      );

      const entrenamiento =
        entrenamientos.find(
          x => x.id_Entrenamiento === id
        );

      if (!entrenamiento) return;

      try {

        // =========================
        // TRAER INSCRIPTOS
        // =========================

        const inscriptos =
          await getInscriptos(
            "Entrenamiento",
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
            entrenamiento.nombre
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

    window.eventDetalleEntrenamiento = true;
  }

  return `
    <div class="admin-entrenamientos-grid">

      ${entrenamientos.map(e => `

        <div 
          class="admin-card"
          data-id="${e.id_Entrenamiento}"
        >

          <div class="admin-card-header">

            <div>

              <h3 class="admin-card-title">
                ${e.nombre}
              </h3>

              <p class="admin-card-subtitle">
                Profesor:
                ${e.profesional?.nombre ?? ""}
                ${e.profesional?.apellido ?? ""}
              </p>

            </div>

            <span class="admin-badge">
              Activo
            </span>

          </div>

          <div class="admin-card-info">

            <div class="admin-info-item">

              <span class="admin-info-label">
                Cupo
              </span>

              <span class="admin-info-value">
                ${e.cupo}
              </span>

            </div>

            <div class="admin-info-item">

              <span class="admin-info-label">
                Precio
              </span>

              <span class="admin-info-value">
                $${e.precio}
              </span>

            </div>

          </div>

          <div class="admin-card-extra">

            <div class="admin-professional-box">

              <span>
                ${e.profesional?.correo ?? ""}
              </span>

              <span>
                ${e.profesional?.localidad ?? ""}
                -
                ${e.profesional?.pais ?? ""}
              </span>

            </div>

          </div>

          <div class="admin-card-actions">

            <button class="admin-btn admin-btn-detalles">
              Ver Detalles
            </button>

            <button 
              class="admin-btn admin-btn-delete"
              data-id="${e.id_Entrenamiento}"
            >
              Eliminar
            </button>

          </div>

        </div>

      `).join("")}

    </div>
  `;
}