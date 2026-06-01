export function CrearModalEquipo() {

  return `
  
    <div class="modal-overlay active" id="modalEquipo">

      <div class="modal-content">

        <div class="modal-header">

          <button class="cerrar-modal-equipo">
            X
          </button>

          <span class="modal-badge">
            Competencia
          </span>

          <h2 class="modal-title">
            Registrar Equipo
          </h2>

        </div>

        <form id="formEquipo" class="modal-body">

          <input
            type="text"
            id="nombreEquipo"
            placeholder="Nombre del equipo"
            required
          />

          <button
            type="submit"
            class="btn-crear-equipo"
          >
            Crear equipo
          </button>

        </form>

      </div>

    </div>
  `;
}