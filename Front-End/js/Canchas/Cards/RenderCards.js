import { abrirModal } from "./Utils/HorarioModal.js";

export function RenderCards(canchas) {
  return canchas.map(c => {

    const tipo = c.tipoCancha?.nombre ?? "Sin tipo";
    const nombreCancha = c.nombre ?? "Cancha sin nombre";
    const superficie = c.tipoCancha?.superficie ?? "-";
    const precio = c.tipoCancha?.precio ?? 0;
    const capacidad = c.tipoCancha?.capacidad ?? "-";
    const duracion = c.tipoCancha?.duracion ?? 1;

    const id = c.idCancha;

    return `
      <div class="cancha-card" id="card-${id}">
        
        <div class="cancha-card-top">

          <div class="cancha-header">
            <div>
              <span class="cancha-badge">${tipo}</span>
              <h3 class="cancha-numero">${nombreCancha}</h3>
            </div>
          </div>

          <div class="cancha-meta">

            <span class="meta-pill">${superficie}</span>
            <span class="meta-pill">${capacidad} jugadores</span>
            <span class="meta-pill">${duracion} min por turno</span>

          </div>

        </div>

        <div class="cancha-footer">

          <div class="precio-wrap">
            <p class="precio-label">Precio</p>
            <span class="precio-valor">
              $${precio.toLocaleString("es-AR")}
            </span>
          </div>

          <button 
            class="btn-reservar"
            data-id="${id}"
          >
            Ver disponibilidad
          </button>

        </div>

      </div>
    `;
  }).join("");
}