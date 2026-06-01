export function renderProfesorAsistenciaConsultar(inscriptos) {
  return `
    <div class="admi-asistencia-grid">

      ${inscriptos.map (i=> `
        <div class="admi-card">

          <div class="admi-card-header">
            <div>
              <h3 class="admi-card-title">${i.nombre}</h3>
              
            </div>

            
          </div>

          <div class="admi-card-info">
            
          
            <div class="admi-info-item">
              <span class="admi-info-label">DNI cliente</span>
              <span class="admi-info-value">${i.dniCliente}</span>
            </div>

            

          </div>

          

        </div>
      `).join("")}

    </div>
  `;
}