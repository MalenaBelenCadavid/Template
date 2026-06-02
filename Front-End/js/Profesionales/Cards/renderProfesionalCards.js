export function RenderProfesionalCards(profesionales, tipo) {

  if (!profesionales?.length) {
    return `
      <p style="color: rgba(255,255,255,0.4)">
        No se encontraron profesionales.
      </p>
    `;
  }

  return `
    <div class="admin-clases-grid">

      ${profesionales.map(p => `

        <div class="admin-card" data-id="${p.dni}">

          <div class="admin-card-header">

            <div>

              <h3 class="admin-card-title">
                ${p.nombre} ${p.apellido ?? ""}
              </h3>

              <p class="admin-card-subtitle">
                ${tipo}
              </p>

            </div>

            <span class="admin-badge">
              ${p.estado ? "Activo" : "Inactivo"}
            </span>

          </div>

          <div class="admin-card-info">

            <div class="admin-info-item">

              <span class="admin-info-label">
                DNI
              </span>

              <span class="admin-info-value">
                ${p.dni}
              </span>

            </div>

            <div class="admin-info-item">

              <span class="admin-info-label">
                Certificado
              </span>

              <span class="admin-info-value">
                Si
              </span>

            </div>

          </div>

          <div class="admin-card-extra">

            <div class="admin-professional-box">

              <span>
                ${p.correo ?? "-"}
              </span>

              <span>
                ${p.localidad ?? "-"} - ${p.pais ?? "-"}
              </span>

            </div>

          </div>

          <div class="admin-card-actions">

            

            <button 
              class="admin-btn admin-btn-delete"
              data-id="${p.dni}"
            >
              Eliminar
            </button>

          </div>

        </div>

      `).join("")}

    </div>
  `;
}