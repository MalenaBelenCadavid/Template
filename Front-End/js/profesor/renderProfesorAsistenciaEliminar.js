export function renderProfesorAsistenciaEliminar(asistencia) {
  // Si la API devuelve un objeto solo (no array), lo convertimos
  const lista = Array.isArray(asistencia) ? asistencia : [asistencia];

  return `
    <div class="admi-asistencia-grid">
      ${lista.map(a => `
        <div class="admi-card">
          <div class="admi-card-header">
            <div>
              <h3 class="admi-card-title">${a.dniCliente}</h3>
            </div>
          </div>
          
            <div class="admi-info-item">
          <span class="admi-info-label">Presente</span>
         <span class="admi-info-value">${a.presente ? "✅ Presente" : "❌ Ausente"}</span>
          </div>
          </div>
          <div class="entre-card-actions">
            <button class="admin-btn Eliminar" data-id="${a.idAsistencia}">
              Eliminar
            </button>
          </div>
        </div>
      `).join("")}
    </div>
  `;
}