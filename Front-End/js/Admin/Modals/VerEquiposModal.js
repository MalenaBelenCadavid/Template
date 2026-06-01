export function CrearModalEquipos(equipos) {

  return `
    <div class="modal-overlay" id="modal-equipos">

      <div class="modal-content modal-equipos">

        <div class="modal-header">

          <div>
            <h2>Equipos inscriptos</h2>
            <p class="modal-subtitle">
              Equipos registrados en la competencia
            </p>
          </div>

          <button class="modal-close cerrar-modal-equipos">✕</button>

        </div>

        <div class="modal-body">

          ${
            !equipos?.length
              ? `
                <p class="sin-inscriptos">
                  No hay equipos inscriptos.
                </p>
              `
              : `
                <div class="equipos-grid">

                  ${equipos.map(e => `
                    <div class="equipo-card">

                      <!-- HEADER -->
                      <div class="equipo-header">
                        <h3>${e.nombre}</h3>
                      </div>

                      <!-- ESTADO (SEPARADO) -->
                      <div class="equipo-estado">
                        <span class="estado-label">Estado:</span>

                        <span class="estado ${e.estado ? "calificado" : "descalificado"}">
                          ${e.estado ? "Calificado" : "Descalificado"}
                        </span>
                      </div>

                      <hr class="divider" />

                      <!-- RESULTADOS -->
                      <div class="equipo-stats">

                        <div class="stat">
                          <span class="label">Victorias</span>
                          <span class="value win">${e.victorias}</span>
                        </div>

                        <div class="stat">
                          <span class="label">Derrotas</span>
                          <span class="value lose">${e.derrotas}</span>
                        </div>

                      </div>

                    </div>
                  `).join("")}

                </div>
              `
          }

        </div>

      </div>

    </div>
  `;
}