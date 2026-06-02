import { deleteData } from "./../../Global/ApiServices.js";

export function render(reserva) {

    const r = reserva.reservaHorarioCanchaResponse;

    const estado = reserva.esValida
        ? { text: "Pagada", class: "estado-pagada" }
        : { text: "Pendiente", class: "estado-pendiente" };

    const card = document.createElement("div");
    card.classList.add("reserva-card");

    card.innerHTML = `
      <div class="reserva-header">
        <div class="reserva-info">
          <span class="reserva-badge">Reserva #${reserva.reservaId}</span>
          <h3 class="reserva-title">${reserva.nombreCancha}</h3>
          <span class="reserva-sub">DNI ${reserva.dniCliente}</span>
          <span class="estado-badge ${estado.class}">${estado.text}</span>
        </div>
        <div class="reserva-price">$${reserva.total.toLocaleString("es-AR")}</div>
      </div>

      <div class="reserva-body">
        <div class="reserva-meta">
          <span class="meta-pill">${r.fecha}</span>
          <span class="meta-pill">${r.horaInicio} - ${r.horaFin}</span>
        </div>

        <div class="reserva-actions">
          <button class="cancelar-btn">Cancelar reserva</button>
        </div>
      </div>
    `;

    // Expandir card
    card.querySelector(".reserva-header")
        .addEventListener("click", () => {
            card.classList.toggle("open");
        });

    const btnCancelar = card.querySelector(".cancelar-btn");

    btnCancelar.addEventListener("click", async (e) => {
        e.stopPropagation();

        // =========================
        // CALCULAR DIFERENCIA REAL
        // =========================
        const [year, month, day] = r.fecha.split("-");
        const [hour, minute] = r.horaInicio.split(":");

        const inicioReserva = new Date(
            year,
            month - 1,
            day,
            hour,
            minute
        );

        const ahora = new Date();

        const diferenciaHoras =
            (inicioReserva - ahora) / (1000 * 60 * 60);

        // =========================
        // MENSAJE SWAL
        // =========================
        let confirmMessage =
            "¿Seguro que querés cancelar esta reserva?";

        if (diferenciaHoras <= 6) {
            confirmMessage =
                "¿Estás seguro que querés cancelar ahora? Si cancelás, se aplicará un recargo del 20%.";
        }

        const confirm = await Swal.fire({
            icon: "warning",
            title: "Cancelar reserva",
            text: confirmMessage,
            showCancelButton: true,
            confirmButtonText: "Sí, cancelar",
            cancelButtonText: "No"
        });

        if (!confirm.isConfirmed) return;

        try {
            await deleteData(`Reserva/${reserva.reservaId}`);

            Swal.fire({
                icon: "success",
                title: "Reserva cancelada",
                timer: 2000,
                showConfirmButton: false
            });

            card.remove();

        } catch (err) {
            Swal.fire({
                icon: "error",
                title: "Error al cancelar",
                text: err.message
            });
        }
    });

    return card;
}