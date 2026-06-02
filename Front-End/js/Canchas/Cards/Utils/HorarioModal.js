import { getHorarios } from "./getHorarios.js";
import { CrearReserva } from "../../../Reserva/PostReserva.js";
import { CrearModalRegistrarCobro } from "../../../Cobros/cobroModal.js";
import { crearCobro } from "../../../Cobros/crearCobro.js";

const diasSemana = {
  1: "Lunes",
  2: "Martes",
  3: "Miércoles",
  4: "Jueves",
  5: "Viernes",
  6: "Sábado",
  0: "Domingo"
};

function obtenerFechaDeDia(diaSemana) {
  const hoy = new Date();
  const actual = hoy.getDay();

  let diff = diaSemana - actual;
  if (diff < 0) diff += 7;

  const fecha = new Date(hoy);
  fecha.setHours(0, 0, 0, 0);
  fecha.setDate(hoy.getDate() + diff);

  const year = fecha.getFullYear();
  const month = String(fecha.getMonth() + 1).padStart(2, "0");
  const day = String(fecha.getDate()).padStart(2, "0");

  return `${year}-${month}-${day}`;
}

function formatearFechaCompleta(fechaString) {
  const [year, month, day] = fechaString.split("-");
  const fecha = new Date(year, month - 1, day);

  const nombreDia = diasSemana[fecha.getDay()];
  return `${nombreDia} (${day}/${month}/${year})`;
}

export function abrirModal(cancha) {
  const modal = document.querySelector("#modal");

  const diasDisponibles = [
    ...new Set(cancha.disponibilidad.map(d => d.dia))
  ];

  modal.innerHTML = `
    <div class="modal-content">

      <button class="cerrar-modal">X</button>

      <h2>${cancha.nombre}</h2>
      <p>${cancha.tipoCancha.nombre}</p>

      <div class="dias-container">
        ${diasDisponibles.map(dia => `
          <button class="dia-btn" data-dia="${dia}">
            ${formatearFechaCompleta(obtenerFechaDeDia(dia))}
          </button>
        `).join("")}
      </div>

      <div class="horarios-container">
        <p>Seleccione un día</p>
      </div>

    </div>
  `;

  modal.classList.add("active");
  document.body.classList.add("modal-open");

  const horariosContainer = modal.querySelector(".horarios-container");
  const botonesDias = modal.querySelectorAll(".dia-btn");

  let fechaSeleccionadaGlobal = null;

  botonesDias.forEach(btn => {
    btn.addEventListener("click", async () => {

      botonesDias.forEach(b => b.classList.remove("active"));
      btn.classList.add("active");

      const dia = Number(btn.dataset.dia);
      const fecha = obtenerFechaDeDia(dia);
      fechaSeleccionadaGlobal = fecha;

      horariosContainer.innerHTML = "<p>Cargando horarios...</p>";

      try {
        const horarios = await getHorarios(cancha.idCancha, fecha);

        if (!horarios || horarios.length === 0) {
          horariosContainer.innerHTML = "<p>No hay horarios disponibles</p>";
          return;
        }

        const ahora = new Date();
        const fechaBase = new Date(fecha + "T00:00:00");

        const esHoy =
          ahora.getFullYear() === fechaBase.getFullYear() &&
          ahora.getMonth() === fechaBase.getMonth() &&
          ahora.getDate() === fechaBase.getDate();

        const horariosFiltrados = horarios
          .filter(h => h.disponible)
          .filter(h => {
            if (!esHoy) return true;

            const [hora, minuto] = h.horaInicio.split(":").map(Number);

            const fechaHora = new Date(fechaBase);
            fechaHora.setHours(hora, minuto, 0, 0);

            return fechaHora > ahora;
          });

        horariosContainer.innerHTML = horariosFiltrados.length > 0
          ? horariosFiltrados.map(h => `
              <button class="horario-btn" data-id="${h.horarioCanchaId}">
                ${h.horaInicio.slice(0,5)} - ${h.horaFin.slice(0,5)}
              </button>
            `).join("")
          : "<p>No hay horarios futuros disponibles</p>";

        const horariosBtn = horariosContainer.querySelectorAll(".horario-btn");

        horariosBtn.forEach(btnHorario => {
          btnHorario.addEventListener("click", () => {

            horariosBtn.forEach(b => b.classList.remove("active"));
            btnHorario.classList.add("active");

            const anterior = horariosContainer.querySelector(".reservar-btn");
            if (anterior) anterior.remove();

            const reservarBtn = document.createElement("button");
            reservarBtn.classList.add("reservar-btn");
            reservarBtn.textContent = "Realizar reserva";

            horariosContainer.appendChild(reservarBtn);

            reservarBtn.addEventListener("click", async () => {

              const modalExistente = document.querySelector("#registrarCobroModal");
              if (modalExistente) modalExistente.remove();

              const dni = Number(localStorage.getItem("dni"));

              const modalCobroHTML = CrearModalRegistrarCobro(
                cancha.nombre,
                cancha.tipoCancha.precio,
                dni
              );

              document.body.insertAdjacentHTML("beforeend", modalCobroHTML);

              const modalCobro = document.querySelector("#registrarCobroModal");
              modalCobro.classList.add("active");
              document.body.classList.add("modal-open");

              modalCobro.querySelector(".cerrar-modal-cobro").addEventListener("click", () => {
                modalCobro.remove();
                document.body.classList.remove("modal-open");
              });

              const formCobro = modalCobro.querySelector("#formRegistrarCobro");

              formCobro.addEventListener("submit", async (e) => {
                e.preventDefault();

                try {
                  const dni = Number(localStorage.getItem("dni"));
                  const horarioCanchaId = Number(btnHorario.dataset.id);
                  const metodoPago = modalCobro.querySelector("#metodoPagoCobro").value;

                  const reserva = await CrearReserva(
                    dni,
                    cancha.idCancha,
                    horarioCanchaId,
                    fechaSeleccionadaGlobal
                  );

                  await crearCobro(
                    reserva.reservaId,
                    null,
                    dni,
                    cancha.tipoCancha.precio,
                    metodoPago,
                    "Reserva"
                  );

                  modalCobro.remove();
                  document.body.classList.remove("modal-open");

                  Swal.fire({
                    toast: true,
                    position: "bottom-end",
                    icon: "success",
                    title: "Reserva creada correctamente",
                    timer: 2500,
                    showConfirmButton: false
                  });

                  setTimeout(() => {
                    window.location.href = "reservas.html";
                  }, 2500);

                } catch (error) {
                  Swal.fire({
                    toast: true,
                    position: "bottom-end",
                    icon: "error",
                    title: error.message ?? "Error al crear la reserva",
                    timer: 2500,
                    showConfirmButton: false
                  });
                }
              });
            });
          });
        });

      } catch (error) {
        horariosContainer.innerHTML = "<p>Error al cargar horarios</p>";
      }
    });
  });

  modal.querySelector(".cerrar-modal").addEventListener("click", () => {
    modal.classList.remove("active");
    document.body.classList.remove("modal-open");
  });
}