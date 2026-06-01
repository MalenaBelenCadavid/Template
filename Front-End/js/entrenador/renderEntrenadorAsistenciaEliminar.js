export function renderEntrenadorAsistenciaEliminar(asistencia) {
  return `
    <div class="admi-asistencia-grid">

      ${asistencia.map (a=> `
        <div class="admi-card">

          <div class="admi-card-header">
            <div>
              <h3 class="admi-card-title">${a.DniCliente}</h3>
              
            </div>

            
          </div>

          <div class="admi-card-info">
            
          
            <div class="admi-info-item">
              <span class="admi-info-label">DNI cliente</span>
              <span class="admi-info-value">${a.IdEntrenamiento}</span>
            </div>

            

          </div>

          <div class="entre-card-actions">

            <button class="admin-btn Eliminar" data-id="${a.IdAsistencia}">
              Eliminar
            </button>
            

          </div>

        </div>
      `).join("")}

    </div>
  `;
}