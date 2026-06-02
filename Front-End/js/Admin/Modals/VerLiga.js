import { putDataWithQueryParams } from "../../Global/ApiServices.js";

export function VerLiga(liga, tabla = []) {

  async function guardarResultado(idPartido, modal) {

    const localInput =
      modal.querySelector(`.input-local-${idPartido}`);

    const visInput =
      modal.querySelector(`.input-vis-${idPartido}`);

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

    try {

      await putDataWithQueryParams(
        "Liga/AgregarResultado",
        {
          idPartido,
          golesLocal,
          golesVis
        }
      );

      localInput.disabled = true;
      visInput.disabled = true;

      const btn = modal.querySelector(
        `.btn-guardar[data-id="${idPartido}"]`
      );

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

    } catch (err) {

      console.error(err);

      Swal.fire({
        icon: "error",
        title:
          err?.response?.data?.Message ??
          "Error al guardar resultado",
        toast: true,
        position: "bottom-end",
        timer: 2000,
        showConfirmButton: false,
        customClass: {
          popup: "toast-golahora toast-popup-error",
          title: "toast-title"
        }
      });

    }
  }

  // =========================================
  // EVENTO GUARDAR RESULTADO
  // =========================================

  setTimeout(() => {

    const modal =
      document.getElementById("modal-liga");

    if (!modal) return;

    modal.addEventListener("click", async (e) => {

      const btn =
        e.target.closest(".btn-guardar");

      if (!btn) return;

      await guardarResultado(
        Number(btn.dataset.id),
        modal
      );

    });

  }, 0);

  // =========================================
  // AGRUPAR PARTIDOS POR FECHA
  // =========================================

  const partidosPorFecha = {};

  liga.partidos?.forEach((p, index) => {

    const fecha =
      Math.floor(index / (liga.equipos.length / 2)) + 1;

    if (!partidosPorFecha[fecha]) {
      partidosPorFecha[fecha] = [];
    }

    partidosPorFecha[fecha].push(p);

  });

  return `

    <div class="modal-overlay" id="modal-liga">

      <div class="modal-content modal-liga">

        <div class="modal-header">

          <h2>
            ${liga.nombre}
          </h2>

          <button class="cerrar-modal">
            ✕
          </button>

        </div>

        <div class="modal-body">

          <!-- ================================= -->
          <!-- TABLA -->
          <!-- ================================= -->

          <div class="liga-tabla-container">

            <h3 class="liga-section-title">
              Tabla de posiciones
            </h3>

            <table class="tabla-liga">

              <thead>

                <tr>
                  <th>#</th>
                  <th>Equipo</th>
                  <th>Pts</th>
                  <th>PJ</th>
                  <th>PG</th>
                  <th>PE</th>
                  <th>PP</th>
                  <th>GF</th>
                  <th>GC</th>
                  <th>DG</th>
                </tr>

              </thead>

              <tbody>

                ${
                  !tabla.length

                    ? `
                      <tr>
                        <td colspan="10">
                          No hay tabla disponible
                        </td>
                      </tr>
                    `

                    : tabla.map((e, index) => `

                      <tr>

                        <td>
                          ${index + 1}
                        </td>

                        <td>
                          ${e.equipo}
                        </td>

                        <td>
                          <strong>
                            ${e.puntos}
                          </strong>
                        </td>

                        <td>${e.pj}</td>
                        <td>${e.pg}</td>
                        <td>${e.pe}</td>
                        <td>${e.pp}</td>
                        <td>${e.gf}</td>
                        <td>${e.gc}</td>
                        <td>${e.dg}</td>

                      </tr>

                    `).join("")
                }

              </tbody>

            </table>

          </div>

          <!-- ================================= -->
          <!-- FIXTURE -->
          <!-- ================================= -->

          <div class="liga-fixture-container">

            <h3 class="liga-section-title">
              Fixture
            </h3>

            ${
              Object.keys(partidosPorFecha).length === 0

                ? `
                  <p style="color:white">
                    No hay partidos generados
                  </p>
                `

                : Object.entries(partidosPorFecha)
                  .map(([fecha, partidos]) => `

                    <div class="fecha-bloque">

                      <h3 class="fecha-title">
                        Fecha ${fecha}
                      </h3>

                      ${partidos.map(p => {

                        const editable =
                          p.estado !== "Finalizado";

                        return `

                          <div class="fixture-card">

                            <div class="fixture-equipos">

                              <div class="equipo-row">

                                <span class="equipo">
                                  ${p.nombreLocal}
                                </span>

                                <input
                                  class="input-local-${p.idPartido}"
                                  type="number"
                                  min="0"
                                  step="1"
                                  value="${p.golesLocal ?? ""}"
                                  ${!editable ? "disabled" : ""}
                                >

                              </div>

                              <div class="equipo-row">

                                <span class="equipo">
                                  ${p.nombreVisitante}
                                </span>

                                <input
                                  class="input-vis-${p.idPartido}"
                                  type="number"
                                  min="0"
                                  step="1"
                                  value="${p.golesVis ?? ""}"
                                  ${!editable ? "disabled" : ""}
                                >

                              </div>

                            </div>

                            <div class="fixture-info">

                              <span>
                                ${new Date(
                                  p.horaInicio
                                ).toLocaleString()}
                              </span>

                              <span class="
                                estado
                                ${p.estado.toLowerCase()}
                              ">
                                ${p.estado}
                              </span>

                            </div>

                            ${
                              editable
                                ? `
                                  <div class="fixture-actions">

                                    <button
                                      class="btn-guardar"
                                      data-id="${p.idPartido}"
                                    >
                                      Guardar
                                    </button>

                                  </div>
                                `
                                : ""
                            }

                          </div>

                        `;

                      }).join("")}

                    </div>

                  `).join("")
            }

          </div>

        </div>

      </div>

    </div>

  `;
}