import { postData } from "../../Global/ApiServices.js";
import { RenderProfesionalCards } from "../../Profesionales/Cards/renderProfesionalCards.js";

export function abrirModalCrearProfesional(tipo) {

  const modalExistente = document.querySelector(".modal-overlay");

  if (modalExistente) return;

  const modal = document.createElement("div");

  modal.classList.add("modal-overlay");

  modal.innerHTML = `

    <div class="modal-content modal-profesional">

      <div class="modal-header">

        <div>



          <h2 class="modal-title">
            Crear ${tipo}
          </h2>

          <p class="modal-subtitle">
            Completá los datos del profesional
          </p>

        </div>

        <button class="modal-close">
          ✕
        </button>

      </div>

      <form class="modal-form" id="form-crear-profesional">

        <div class="form-grid">

          <div class="form-group">
            <label>DNI</label>

            <input 
              type="number"
              id="dni"
              class="modal-input"
              placeholder="Ej: 42123456"
              required
            >
          </div>

          <div class="form-group">
            <label>Nombre</label>

            <input 
              type="text"
              id="nombre"
              class="modal-input"
              placeholder="Ej: Carlos"
              required
            >
          </div>

          <div class="form-group">
            <label>Apellido</label>

            <input 
              type="text"
              id="apellido"
              class="modal-input"
              placeholder="Ej: García"
              required
            >
          </div>

          <div class="form-group">
            <label>Correo</label>

            <input 
              type="email"
              id="correo"
              class="modal-input"
              placeholder="correo@email.com"
              required
            >
          </div>

          <div class="form-group">
            <label>Contraseña</label>

            <input 
              type="password"
              id="password"
              class="modal-input"
              placeholder="********"
              required
            >
          </div>

          <div class="form-group">
            <label>Fecha de nacimiento</label>

            <input 
              type="date"
              id="fechaNac"
              class="modal-input"
              required
            >
          </div>

          <div class="form-group">
            <label>Localidad</label>

            <input 
              type="text"
              id="localidad"
              class="modal-input"
              placeholder="Ej: Berazategui"
            >
          </div>

          <div class="form-group">
            <label>País</label>

            <input 
              type="text"
              id="pais"
              class="modal-input"
              placeholder="Ej: Argentina"
            >
          </div>

        </div>

        <div class="form-group full-width">

          <label>Certificado</label>

          <textarea
            id="certificado"
            class="modal-input modal-textarea"
            placeholder="Detalle del certificado..."
          ></textarea>

        </div>

        <div class="modal-actions">

          <button 
            type="submit"
            class="btn-save"
          >
            Guardar ${tipo}
          </button>

        </div>

      </form>

    </div>
  `;

  document.body.appendChild(modal);

  function cerrarModal() {

    modal.style.opacity = "0";

    setTimeout(() => {
      modal.remove();
    }, 200);
  }

  modal.querySelector(".modal-close")
    .addEventListener("click", cerrarModal);


  modal.querySelector("#form-crear-profesional")
    .addEventListener("submit", async (e) => {

      e.preventDefault();

      try {

        const body = {

          dni: Number(
            document.querySelector("#dni").value
          ),

          nombre:
            document.querySelector("#nombre").value,

          apellido:
            document.querySelector("#apellido").value,

          correo:
            document.querySelector("#correo").value,

          password:
            document.querySelector("#password").value,

          localidad:
            document.querySelector("#localidad").value,

          pais:
            document.querySelector("#pais").value,

          fechaNac:
            document.querySelector("#fechaNac").value,

          certificado:
            document.querySelector("#certificado").value
        };

        const endpoint =
          tipo === "Profesor"
            ? "Profesionales/profesores"
            : "Profesionales/entrenadores";

        const profesionalCreado =
          await postData(endpoint, body);

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "success",
          title: `${tipo} creado correctamente`,
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,
          customClass: {
            popup: "toast-golahora toast-popup-success",
            title: "toast-title"
          }
        });

        const list =
          document.querySelector("#profesionales-list");

        const cardsGrid =
          list.querySelector(".profesionales-grid");

        if (cardsGrid) {

          cardsGrid.insertAdjacentHTML(
            "beforeend",

            RenderProfesionalCards(
              [profesionalCreado],
              tipo

            ).replace(
              '<div class="profesionales-grid">',
              ""
            ).replace("</div>", "")
          );
        }

        cerrarModal();

      } catch (error) {

        console.error(error);

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "error",
          title:
            error.message ??
            "Error al crear profesional",
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