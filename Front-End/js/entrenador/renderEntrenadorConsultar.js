export function renderEntrenadorConsultar(entrenamientos) {
  return `
    <div class="entre-entrenamientos-grid">
      ${entrenamientos.map(e => `
        <div class="entre-card">
          <div class="entre-card-header">
            <h3 class="entre-card-title">${e.nombre}</h3>
            <span class="entre-badge">Activa</span>
          </div>
          <div class="entre-card-info">
            <div class="entre-info-item">
              <span class="entre-info-label">DNI Entrenador</span>
              <span class="entre-info-value">${e.dni_Entrenador}</span>
            </div>
            <div class="entre-info-item">
              <span class="entre-info-label">Cupo</span>
              <span class="entre-info-value">${e.cupo}</span>
            </div>
            <div class="entre-info-item">
              <span class="entre-info-label">Precio</span>
              <span class="entre-info-value">$${e.precio}</span>
            </div>
          </div>
          <div class="entre-card-actions">
            <button class="admin-btn Consultar" data-id="${e.id_Entrenamiento}">
              Consultar Alumnos
            </button>
          </div>
        </div>
      `).join("")}
    </div>
  `;
}