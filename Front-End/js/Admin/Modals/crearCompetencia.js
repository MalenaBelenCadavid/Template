import { crearCompeticion } from "../../Competiciones/postCompeticiones.js"; 

export function crearCompetencia() {

  const modalExistente = document.querySelector(".modal-overlay");

  if (modalExistente) return;

  const modal = document.createElement("div");

  modal.classList.add("modal-overlay");

  modal.innerHTML = `

    <div class="modal-content">

      <div class="modal-header">

        <h2 class="modal-title">
          Crear competencia
        </h2>

        <button class="modal-close">
          ✕
        </button>

      </div>

      <form id="form-crear-competencia" class="modal-form">

        <div class="form-group">

          <label>
            Tipo
          </label>

          <input
            type="text"
            id="tipoCompetencia"
            class="modal-input"
            placeholder="Liga / Torneo"
          >

        </div>

        <div class="form-group">

          <label>
            Nombre
          </label>

          <input
            type="text"
            id="nombreCompetencia"
            class="modal-input"
            placeholder="Copa GolAhora"
          >

        </div>

        <div class="form-group">

          <label>
            Descripción
          </label>

          <textarea
            id="descripcionCompetencia"
            class="modal-input"
            placeholder="Descripción de la competencia"
            rows="4"
          ></textarea>

        </div>

        <div class="form-group">

          <label>
            Cupos
          </label>

          <input
            type="number"
            id="cuposCompetencia"
            class="modal-input"
            placeholder="16"
          >

        </div>

        <div class="form-group">

          <label>
            Precio
          </label>

          <input
            type="number"
            id="precioCompetencia"
            class="modal-input"
            placeholder="5000"
          >

        </div>

        <div class="modal-actions">

         

          <button
            type="submit"
            class="btn-save"
          >
            Crear competencia
          </button>

        </div>

      </form>

    </div>

  `;

  document.body.appendChild(modal);

  document.body.classList.add("modal-open");

  function cerrarModal() {

    modal.remove();

    document.body.classList.remove("modal-open");

  }

  modal
    .querySelector(".modal-close")
    .addEventListener("click", cerrarModal);



  modal.addEventListener("click", (e) => {

    if (e.target === modal) {

      cerrarModal();

    }

  });

  modal
    .querySelector("#form-crear-competencia")
    .addEventListener("submit", async (e) => {

      e.preventDefault();

      try {

        

         const tipo = modal.querySelector("#tipoCompetencia").value
         const nombre = modal.querySelector("#nombreCompetencia").value
         const descripcion = modal.querySelector("#descripcionCompetencia").value
         const cupos = Number(modal.querySelector("#cuposCompetencia").value)
         const precio = Number(modal.querySelector("#precioCompetencia").value)

        

        await crearCompeticion(tipo,nombre,descripcion,cupos,precio);

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "success",
          title: "Competencia creada correctamente",
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,

          customClass: {
            popup: "toast-golahora toast-popup-success",
            title: "toast-title"
          }
        });

        cerrarModal();

        setTimeout(() => {

          location.reload();

        }, 2500);

      } catch (error) {

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "error",
          title: error.message ?? "Error al crear competencia",
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