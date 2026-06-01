export function renderAccionesCobro() {
  return `
    <h2 style="margin-bottom: 1rem; color: #fff;">
      Acciones para los cobros:
    </h2>

    <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(260px, 1fr)); gap: 1.2rem;">

      <button class="action-cobro-btn" id="btn-modificar-cobro">
        <span class="icon icon-yellow">✏️</span>
        <div>
          <strong>Modificar Cobro</strong>
        </div>
      </button>

      <button class="action-cobro-btn" id="btn-imprimir-cobro">
        <span class="icon icon-blue">🖨️</span>
        <div>
          <strong>Imprimir Ticket</strong>
        </div>
      </button>

      <button class="action-cobro-btn" id="btn-consultar-cobro">
        <span class="icon icon-purple">🔍</span>
        <div>
          <strong>Consultar Cobro</strong>
        </div>
      </button>

      <button class="action-cobro-btn" id="btn-generar-descuento">
        <span class="icon icon-red">🏷️</span>
        <div>
          <strong>Aplicar Descuento</strong>
        </div>
      </button>

    </div>
  `;
}