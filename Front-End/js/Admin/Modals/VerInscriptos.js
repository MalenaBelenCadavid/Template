export function CrearModalInscriptos(inscriptos,titulo) {

  return `
  
    <div 
      class="modal-overlay"
      id="modal-inscriptos"
    >

      <div class="modal-content modal-inscriptos">

        <div class="modal-header">

          <div>

            <h2>
              ${titulo}
            </h2>

            <p>
              Lista de inscriptos
            </p>

          </div>

          <button class="cerrar-modal-inscriptos">
            ✕
          </button>

        </div>

        <div class="modal-body">

          ${
            !inscriptos?.length
            ? `
              <p class="sin-inscriptos">
                No hay inscriptos.
              </p>
            `
            : `
              <div class="inscriptos-grid">

                ${inscriptos.map(i => `

                  <div class="inscripto-card">

                    <div class="inscripto-top">

                      <h3>
                        ${i.nombre}
                        ${i.apellido}
                      </h3>

                      <span>
                        DNI:
                        ${i.dniCliente}
                      </span>

                    </div>

                    <div class="inscripto-info">

                      <span>
                        Precio:
                        $${i.precioInscr}
                      </span>

                      <span>
                        Fecha:
                        ${new Date(
                          i.horario
                        ).toLocaleString("es-AR")}
                      </span>

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