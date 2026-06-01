export function renderProfesorEliminar(clases) {
  return `
    <div class="entre-clases-grid">

      ${clases.map(c => `
        <div class="entre-card">

          <div class="entre-card-header">
            <div>
              <h3 class="entre-card-title">${c.nombre}</h3>
              
            </div>

            <span class="entre-badge">Activa</span>
          </div>

          <div class="entre-card-info">

            <div class="entre-info-item">
              <span class="entre-info-label">Precio</span>
              <span class="entre-info-value">${c.cupo}</span>
            </div>

            <div class="entre-info-item">
              <span class="entre-info-label">DNI Profesor</span>
              <span class="entre-info-value">${c.dniProfesor}</span>
            </div>

            <div class="entre-info-item">
              <span class="entre-info-label">Precio</span>
              <span class="entre-info-value">$${c.precio}</span>
            </div>

          </div>

          <div class="entre-card-actions">

            <button class="admin-btn EliminarAsistencias" data-id="${c.idClase}">
            Eliminar Asistencias
            </button>


          </div>

        </div>
      `).join("")}

    </div>
  `;
}