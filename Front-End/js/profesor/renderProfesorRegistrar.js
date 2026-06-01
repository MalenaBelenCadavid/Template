export function renderProfesorRegistrar(clases) {
  return `
    <div class="entre-clases-grid">

      ${clases.map(c => `
        <div class="entre-card">

          <div class="entre-card-header">

            <h3 class="entre-card-title">
              ${c.nombre}
            </h3>

            <span class="entre-badge">
              Activa
            </span>

          </div>

          <div class="entre-card-info">

            <div class="entre-info-item">
              <span class="entre-info-label">Cupo</span>
              <span class="entre-info-value">${c.cupo}</span>
            </div>

            <div class="entre-info-item">
              <span class="entre-info-label">Profesor</span>
              <span class="entre-info-value">${c.dniProfesor}</span>
            </div>

            <div class="entre-info-item">
              <span class="entre-info-label">Precio</span>
              <span class="entre-info-value">$${c.precio}</span>
            </div>

          </div>

          <div class="entre-card-actions">

            <button class="admin-btn" data-id="${c.idClase}">
              Pasar Asistencia
            </button>

          </div>

        </div>
      `).join("")}

    </div>
  `;
}