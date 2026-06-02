export function RenderCompetitionCards(competiciones) {

  // =========================================
  // FILTRAR SOLO COMPETENCIAS CON CUPOS
  // =========================================

  const competicionesDisponibles =
    competiciones.filter(
      c => (c.cupos ?? 0) > 0
    );

  // =========================================
  // SIN COMPETENCIAS
  // =========================================

  if (!competicionesDisponibles.length) {

    return `
      <p style="color: rgba(255,255,255,0.4)">
        No hay competiciones disponibles.
      </p>
    `;
  }

  // =========================================
  // RENDER
  // =========================================

  return competicionesDisponibles.map(c => `

    <div class="card">

      <div class="card-top">

        <span class="badge">
          ${c.tipo ?? "Competición"}
        </span>

        <h3>${c.nombre}</h3>

        <p class="info">
          ${c.descripcion}
        </p>

        <p class="cupos">
          Cupos: ${c.cupos}
        </p>

        <div class="precio">
          $${c.precio}
        </div>

      </div>

      <div class="card-footer">

        <button
          class="btn-inscribirse"
          data-id="${c.competenciaId}"
          data-tipo="3"
        >
          Inscribirme
        </button>

      </div>

    </div>

  `).join("");
}