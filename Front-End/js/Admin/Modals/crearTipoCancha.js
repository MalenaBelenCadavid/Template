import { InsertarTipoCancha } from "../../TipoCancha/PostTipoCancha.js";

export function crearTipoCancha() {

    const modalExistente =
        document.querySelector(".modal-overlay");

    if (modalExistente) return;

    const modal = document.createElement("div");

    modal.classList.add("modal-overlay");

    modal.innerHTML = `

        <div class="modal-content">

            <div class="modal-header">

                <h2>
                    Crear tipo de cancha
                </h2>

                <button class="modal-close">
                    ✕
                </button>

            </div>

            <form id="form-crear-tipo" class="modal-form">

                <!-- NOMBRE -->

                <div class="form-group">

                    <label>
                        Nombre
                    </label>

                    <input
                        type="text"
                        id="nombre"
                        placeholder="Fútbol 5"
                    >

                </div>

                <!-- SUPERFICIE -->

                <div class="form-group">

                    <label>
                        Superficie
                    </label>

                    <input
                        type="text"
                        id="superficie"
                        placeholder="Césped sintético"
                    >

                </div>

                <!-- CAPACIDAD -->

                <div class="form-group">

                    <label>
                        Capacidad
                    </label>

                    <input
                        type="number"
                        id="capacidad"
                        placeholder="10"
                    >

                </div>

                <!-- DURACION -->

                <div class="form-group">

                    <label>
                        Duración
                    </label>

                    <input
                        type="number"
                        id="duracion"
                        step="0.1"
                        placeholder="1"
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
                        placeholder="10000"
                    >

                </div>

                <button
                    type="submit"
                    class="btn-primary modal-btn"
                >
                    Crear tipo de cancha
                </button>

            </form>

        </div>
    `;

    document.body.appendChild(modal);

    document.body.classList.add("modal-open");

    function cerrarModal() {

        modal.remove();

        document.body.classList.remove("modal-open");
    }

    modal.querySelector(".modal-close")
        .addEventListener("click", cerrarModal);

    modal.addEventListener("click", (e) => {

        if (e.target === modal) {
            cerrarModal();
        }
    });

    modal.querySelector("#form-crear-tipo")
        .addEventListener("submit", async (e) => {

            e.preventDefault();
                const nombre=modal.querySelector("#nombre").value

                const superficie=modal.querySelector("#superficie").value

                const capacidad=Number(modal.querySelector("#capacidad").value)

                const duracion=Number(modal.querySelector("#duracion").value)

                const precio=Number(modal.querySelector("#precio").value)      

            try {

                await InsertarTipoCancha(nombre,superficie,capacidad,duracion,precio);

                Swal.fire({
                    toast: true,
                    position: "bottom-end",
                    icon: "success",
                    title: "Tipo de cancha creado correctamente",
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

            } catch (error) {

                Swal.fire({
                    toast: true,
                    position: "bottom-end",
                    icon: "error",
                    title:
                        error.message ??
                        "Error al crear el tipo de cancha",

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