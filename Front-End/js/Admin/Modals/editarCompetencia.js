import { putData } from "../../Global/ApiServices.js";

export async function editarCompetencia(competencia) {
    console.log(competencia)
    const modal = document.createElement("div");
    modal.classList.add("modal-overlay");

    modal.innerHTML = `
      <div class="modal-card">

        <h2>Editar competencia</h2>

        <div class="form-group">
          <label>Nombre</label>
          <input type="text" id="nombre" value="${competencia.nombre ?? ""}" />
        </div>

        <div class="form-group">
          <label>Descripción</label>
          <textarea id="descripcion">${competencia.descripcion ?? ""}</textarea>
        </div>

        <div class="form-group">
          <label>Cupos</label>
          <input type="number" id="cupos" value="${competencia.cupos ?? 0}" />
        </div>

        <div class="form-group">
          <label>Precio</label>
          <input type="number" id="precio" value="${competencia.precio ?? 0}" />
        </div>

        <div class="modal-actions">

          <button type="button" class="btn-cancelar">
            Cancelar
          </button>

          <button type="button" class="btn-guardar">
            Guardar
          </button>

        </div>

      </div>
    `;

    document.body.appendChild(modal);

    // =========================
    // CERRAR MODAL
    // =========================
    modal.querySelector(".btn-cancelar").addEventListener("click", () => {
        modal.remove();
    });

    modal.addEventListener("click", (e) => {
        if (e.target === modal) {
            modal.remove();
        }
    });

    // =========================
    // GUARDAR
    // =========================
    modal.querySelector(".btn-guardar").addEventListener("click", async () => {

        const body = {
            nombre: modal.querySelector("#nombre").value || null,
            descripcion: modal.querySelector("#descripcion").value || null,
            cupos: Number(modal.querySelector("#cupos").value),
            precio: Number(modal.querySelector("#precio").value)
        };

        try {

            await putData(`Competencia/${competencia.competenciaId}`, body);
            console.log("URL:", `Competencia/${competencia.idCompetencia}`);
            Swal.fire({
                toast: true,
                position: "bottom-end",
                icon: "success",
                title: "Competencia actualizada con éxito",
                showConfirmButton: false,
                timer: 2500,
                timerProgressBar: true,
                customClass: {
                    popup: "toast-golahora toast-popup-success",
                    title: "toast-title"
                }
            });

            setTimeout(() => {
                location.reload();
            }, 2500);

            modal.remove();

        } catch (err) {

            Swal.fire({
                toast: true,
                position: "bottom-end",
                icon: "error",
                title: err.message ?? "Error al actualizar competencia",
                showConfirmButton: false,
                timer: 2500,
                timerProgressBar: true,
                customClass: {
                    popup: "toast-golahora toast-popup-error",
                    title: "toast-title"
                }
            });
        }
    });
}