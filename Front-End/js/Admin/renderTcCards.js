export function RenderTcCards(tipos)
{
  if (!Array.isArray(tipos) || tipos.length === 0) {
    return `
      <p style="color: rgba(255,255,255,0.4)">
        No hay tipos de cancha disponibles.
      </p>
    `;
  }

  return `
  
    <div class="admin-clases-grid">

      ${tipos.map(tipo => `

        <div class="admin-card" data-id="${tipo.id}">

          <div class="admin-card-header">

            <div>

              <h3 class="admin-card-title">
                ${tipo.nombre}
              </h3>

              <p class="admin-card-subtitle">
                ${tipo.superficie}
              </p>

            </div>

            <span class="admin-badge">
              ID ${tipo.id}
            </span>

          </div>

          <div class="admin-card-info">

            <div class="admin-info-item">

              <span class="admin-info-label">
                Capacidad
              </span>

              <span class="admin-info-value">
                ${tipo.capacidad} jugadores
              </span>

            </div>

            <div class="admin-info-item">

              <span class="admin-info-label">
                Duración
              </span>

              <span class="admin-info-value">
                ${tipo.duracion} mins
              </span>

            </div>

          </div>

          <div class="admin-card-extra">

            <div class="admin-professional-box">

              <span>
                Precio por turno
              </span>

              <span>
                $${tipo.precio}
              </span>

            </div>

          </div>

          <div class="admin-card-actions">

            <button class="admin-btn admin-btn-edit" data-id="${tipo.id}">
              Editar
            </button>

            <button 
              class="admin-btn admin-btn-delete"
              data-id="${tipo.id}"
            >
              Eliminar
            </button>

          </div>

        </div>

      `).join("")}

    </div>

  `;
}