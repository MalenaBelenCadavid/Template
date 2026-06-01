export function renderAccionesRecibo() {
  return `
    <h2 class="section-title">Acciones para los recibos:</h2>

    <div class="recibo-grid">

      <button class="action-recibo-btn" id="btn-generar-recibo">
        <span class="icon icon-green">🧾</span>
        <div>
          <strong>Generar Recibo</strong>
        </div>
      </button>

      <button class="action-recibo-btn" id="btn-modificar-recibo">
        <span class="icon icon-yellow">✏️</span>
        <div>
          <strong>Modificar Recibo</strong>
        </div>
      </button>

      <button class="action-recibo-btn" id="btn-consultar-recibo">
        <span class="icon icon-purple">🔍</span>
        <div>
          <strong>Consultar Recibo</strong>
        </div>
      </button>

      <button class="action-recibo-btn" id="btn-imprimir-recibo">
        <span class="icon icon-blue">🖨️</span>
        <div>
          <strong>Imprimir Copia</strong>
        </div>
      </button>

    </div>
  `;
}