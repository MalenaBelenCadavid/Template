export function renderProfesorConsulta(clases) {

  return `

    <div class="profesor-clases-grid">

      ${clases.map(c => `

        <div class="profesor-card">

          <!-- HEADER -->
          <div class="profesor-card-header">

            <h3 class="profesor-card-title">
              ${c.nombre}
            </h3>

            <span class="profesor-badge">
              Activa
            </span>

          </div>

          <!-- INFO -->
          <div class="profesor-card-info">

            <div class="profesor-info-item">

              <span class="profesor-info-label">
                Cupo
              </span>

              <span class="profesor-info-value">
                ${c.cupo}
              </span>

            </div>

            <div class="profesor-info-item">

              <span class="profesor-info-label">
                DNI Profesor
              </span>

              <span class="profesor-info-value">
                ${c.dniProfesor}
              </span>

            </div>

            <div class="profesor-info-item">

              <span class="profesor-info-label">
                Precio
              </span>

              <span class="profesor-info-value">
                $${c.precio}
              </span>

            </div>

          </div>

          <!-- ACTIONS -->
          <div class="profesor-card-actions">

            <button
              class="profesor-btn"
              data-id="${c.idClase}"
            >
              Consultar Alumnos
            </button>

          </div>

        </div>

      `).join("")}

    </div>

  `;
}