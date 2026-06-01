export function renderProfesorPasarAsistencia(alumnos) {

  function renderEstado(presente) {

    if (presente === true) {
      return `
        <div class="estado-asistencia presente">
          ✓ Asistió
        </div>
      `;
    }

    if (presente === false) {
      return `
        <div class="estado-asistencia ausente">
          ✗ Ausente
        </div>
      `;
    }

    return "";
  }

  function renderBotones(alum) {

    const yaRespondio =
      alum.presente === true || alum.presente === false;

    if (yaRespondio) return "";

    return `
      <div class="entre-card-actions">

        <button 
          class="admin-btn Presente"
          data-id="${alum.idAsistencia}"
          data-dni="${alum.dniCliente}"
        >
          Presente
        </button>

        <button 
          class="admin-btn Ausente"
          data-id="${alum.idAsistencia}"
          data-dni="${alum.dniCliente}"
        >
          Ausente
        </button>

      </div>
    `;
  }

  return `
    <div class="admi-asistencia-grid">

      ${alumnos.map(alum => `
        <div class="admi-card">

          <div class="admi-card-header">
            <h3 class="admi-card-title">
              ${alum.nombre} ${alum.apellido ?? ""}
            </h3>
          </div>

          <div class="admi-card-info">
            <div class="admi-info-item">
              <span class="admi-info-label">DNI cliente</span>
              <span class="admi-info-value">
                ${alum.dniCliente}
              </span>
            </div>
          </div>

          ${renderEstado(alum.presente)}

          ${renderBotones(alum)}

        </div>
      `).join("")}

    </div>
  `;
}