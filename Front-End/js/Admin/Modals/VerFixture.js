import { putDataWithQueryParams } from "../../Global/ApiServices.js";

export function VerFixtures(partidos) {

  function guardarResultado(idPartido, modal) {

    const localInput = modal.querySelector(`.input-local-${idPartido}`);
    const visInput = modal.querySelector(`.input-vis-${idPartido}`);

    const golesLocal = Number(localInput.value);
    const golesVis = Number(visInput.value);

    if (golesLocal < 0 || golesVis < 0) {
      Swal.fire({
        icon: "warning",
        title: "Resultado inválido",
        text: "Los goles no pueden ser negativos",
        toast: true,
        position: "bottom-end",
        timer: 2000,
        showConfirmButton: false
      });
      return;
    }

    putDataWithQueryParams("Torneo/AgregarResultado", {
      idPartido,
      golesLocal,
      golesVis
    })
    .then(() => {

      localInput.disabled = true;
      visInput.disabled = true;

      const btn = modal.querySelector(`.btn-guardar[data-id="${idPartido}"]`);
      if (btn) {
        btn.textContent = "Guardado";
        btn.disabled = true;
      }

      Swal.fire({
        icon: "success",
        title: "Resultado guardado",
        toast: true,
        position: "bottom-end",
        timer: 1500,
        showConfirmButton: false,
        customClass: {
          popup: "toast-golahora toast-popup-success",
          title: "toast-title"
        }
      });

    })
    .catch(err => {
      console.error(err);

      Swal.fire({
        icon: "error",
        title: "Error al guardar resultado",
        toast: true,
        position: "bottom-end",
        timer: 2000,
        showConfirmButton: false,
        customClass: {
          popup: "toast-golahora toast-popup-error",
          title: "toast-title"
        }
      });
    });
  }

  // 👇 EL EVENTO SE ATAN SOLO UNA VEZ
  setTimeout(() => {
    const modal = document.getElementById("modal-fixture");
    if (!modal) return;

    modal.addEventListener("click", (e) => {
      const btn = e.target.closest(".btn-guardar");
      if (!btn) return;

      guardarResultado(Number(btn.dataset.id), modal);
    });
  }, 0);

  return `
    <div class="modal-overlay" id="modal-fixture">

      <div class="modal-content modal-fixture">

        <div class="modal-header">
          <h2>Fixture</h2>
          <button class="cerrar-modal">✕</button>
        </div>

        <div class="modal-body">

          ${
            !partidos?.length
              ? `<p style="color:white">No hay partidos</p>`
              : partidos.map(p => {

                const editable = p.estado !== "Finalizado";

                return `
                  <div class="fixture-card">

                    <div class="fixture-equipos">

                      <span class="equipo">${p.nombreLocal}</span>

                      <div class="resultado-box">

                        <input 
                          class="input-local-${p.idPartido}" 
                          type="number" 
                          min="0"
                          step="1"
                          value="${p.golesLocal ?? ""}"
                          ${!editable ? "disabled" : ""}
                        >

                        <span class="guion">-</span>

                        <input 
                          class="input-vis-${p.idPartido}" 
                          type="number" 
                          min="0"
                          step="1"
                          value="${p.golesVis ?? ""}"
                          ${!editable ? "disabled" : ""}
                        >

                      </div>

                      <span class="equipo">${p.nombreVisitante}</span>

                    </div>

                    <div class="fixture-info">
                      <span>${new Date(p.horaInicio).toLocaleString()}</span>

                      <span class="estado ${p.estado.toLowerCase()}">
                        ${p.estado}
                      </span>
                    </div>

                    ${
                      editable
                        ? `
                          <div style="display:flex; justify-content:flex-end; margin-top:10px;">
                            <button class="btn-guardar" data-id="${p.idPartido}">
                              Guardar
                            </button>
                          </div>
                        `
                        : ""
                    }

                  </div>
                `;
              }).join("")
          }

        </div>

      </div>

    </div>
  `;
}