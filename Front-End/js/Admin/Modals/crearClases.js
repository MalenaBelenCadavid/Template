import { postData } from "../../Global/ApiServices.js";
import { getProfesionales } from "../../Profesionales/getProfesionales.js";

export async function crearClase() {

  const modalExistente = document.querySelector(".modal-overlay");

  if (modalExistente) return;

  const profesores = await getProfesionales("Profesor");

  const modal = document.createElement("div");

  modal.classList.add("modal-overlay");

  modal.innerHTML = `
  
    <div class="modal-content">

      <div class="modal-header">

        <h2>
          Crear clase
        </h2>

        <button 
          class="btn-cancel"
          id="close-modal"
        >
          X
        </button>

      </div>

      <div class="modal-body">

        <!-- NOMBRE -->
        <div class="form-group">

          <label>
            Nombre
          </label>

          <input 
            type="text" 
            id="nombre" 
            placeholder="Ej: Clase de funcional"
          >

        </div>

        <!-- PROFESOR -->
        <div class="form-group">

          <label>
            Profesor
          </label>

          <select id="dni">

            <option value="">
              Seleccionar profesor
            </option>

            ${profesores.map(p => `
              <option value="${p.dni}">
                ${p.nombre} ${p.apellido ?? ""}
              </option>
            `).join("")}

          </select>

        </div>

        <!-- CUPO -->
        <div class="form-group">

          <label>
            Cupo
          </label>

          <input 
            type="number" 
            id="cupo" 
            placeholder="Ej: 15"
          >

        </div>

        <!-- FECHA -->
        <div class="form-group">

          <label>
            Fecha
          </label>

          <input 
            type="date" 
            id="dia"
          >

        </div>

        <!-- HORA -->
        <div class="form-group">

          <label>
            Hora
          </label>

          <input 
            type="time" 
            id="hora"
          >

        </div>

        <!-- PRECIO -->
        <div class="form-group">

          <label>
            Precio
          </label>

          <input 
            type="number" 
            id="precio" 
            placeholder="Ej: 3500"
          >

        </div>

        <button 
          class="btn-save"
          id="btn-guardar"
        >
          Guardar
        </button>

      </div>

    </div>
  `;

  document.body.appendChild(modal);

  // =========================
  // BLOQUEAR FECHAS PASADAS
  // =========================

  const inputFecha = modal.querySelector("#dia");

  const hoy = new Date();

  const year = hoy.getFullYear();

  const month = String(
    hoy.getMonth() + 1
  ).padStart(2, "0");

  const day = String(
    hoy.getDate()
  ).padStart(2, "0");

  inputFecha.min = `${year}-${month}-${day}`;

  // =========================
  // CERRAR MODAL
  // =========================

  modal.querySelector("#close-modal")
    .addEventListener("click", () => {

      modal.remove();
    });

  // =========================
  // GUARDAR
  // =========================

  modal.querySelector("#btn-guardar")
    .addEventListener("click", async () => {

      try {

        const nombre =
          modal.querySelector("#nombre").value.trim();

        const dniProfesor = Number(
          modal.querySelector("#dni").value
        );

        const cupo = Number(
          modal.querySelector("#cupo").value
        );

        const precio = Number(
          modal.querySelector("#precio").value
        );

        const fechaSeleccionada =
          modal.querySelector("#dia").value;

        const horaSeleccionada =
          modal.querySelector("#hora").value;

        // =========================
        // VALIDACIONES
        // =========================

        if (!nombre) {

          throw new Error(
            "Ingrese un nombre"
          );
        }

        if (!dniProfesor) {

          throw new Error(
            "Seleccione un profesor"
          );
        }

        if (!cupo || cupo <= 0) {

          throw new Error(
            "Ingrese un cupo válido"
          );
        }

        if (!precio || precio <= 0) {

          throw new Error(
            "Ingrese un precio válido"
          );
        }

        if (!fechaSeleccionada) {

          throw new Error(
            "Seleccione una fecha"
          );
        }

        if (!horaSeleccionada) {

          throw new Error(
            "Seleccione una hora"
          );
        }

        // =========================
        // VALIDAR FECHA
        // =========================

        const fechaIngresada =
          new Date(fechaSeleccionada);

        hoy.setHours(0, 0, 0, 0);

        if (fechaIngresada < hoy) {

          throw new Error(
            "La fecha no puede ser anterior a hoy"
          );
        }

        // =========================
        // PAYLOAD
        // =========================

        const payload = {

          nombre: nombre,

          cupo: cupo,

          dia: fechaSeleccionada,

          hora: horaSeleccionada + ":00",

          dniProfesor: dniProfesor,

          precio: precio
        };

        console.log(payload);

        // =========================
        // POST
        // =========================

        await postData(
          "Clase",
          payload
        );

        modal.remove();

        // =========================
        // SUCCESS
        // =========================

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "success",
          title: "Clase creada correctamente",
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,
          customClass: {
            popup:
              "toast-golahora toast-popup-success",
            title:
              "toast-title"
          }
        });

        setTimeout(() => {

          window.location.reload();

        }, 2500);

      } catch (error) {

        console.error(error);

        // =========================
        // ERROR
        // =========================

        Swal.fire({
          toast: true,
          position: "bottom-end",
          icon: "error",
          title:
            error.message ??
            "Error al crear clase",
          showConfirmButton: false,
          timer: 2500,
          timerProgressBar: true,
          customClass: {
            popup:
              "toast-golahora toast-popup-error",
            title:
              "toast-title"
          }
        });
      }
    });
}