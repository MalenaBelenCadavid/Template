import { getData } from "../../Global/ApiServices.js";

export async function renderMetricas(container) {

    const ingresosPorReservas =
        await getData("Metricas/ingresosPorReserva");

    const ingresosPorInscripciones =
        await getData("Metricas/ingresosPorInscripcion");

    const canchasMasReservadas =
        await getData("Metricas/canchasMasReservadas");

    container.innerHTML = `
    
        <div class="metricas-container">

            <!-- HEADER -->
            <div class="metricas-header">
                <h2>Métricas del sistema</h2>
                <p>
                    Resumen general de ingresos y actividad
                </p>
            </div>

            <!-- CARDS -->
            <div class="metricas-cards">

                <div class="metrica-card">
                    <h3>Ingresos por reservas</h3>

                    <span class="metrica-valor">
                        $${ingresosPorReservas}
                    </span>
                </div>

                <div class="metrica-card">
                    <h3>Ingresos por inscripciones</h3>

                    <span class="metrica-valor">
                        $${ingresosPorInscripciones}
                    </span>
                </div>

            </div>

            <!-- CANCHAS -->
            <div class="canchas-section">

                <h3>
                    Canchas más reservadas
                </h3>

                <div class="canchas-list">

                    ${
                        canchasMasReservadas.map((cancha, index) => `
                            
                            <div class="cancha-card">

                                <div class="cancha-ranking">
                                    #${index + 1}
                                </div>

                                <div class="cancha-info">

                                    <h4>
                                        ${cancha.nombre}
                                    </h4>

                                    <p>
                                        ${cancha.tipoCancha.nombre}
                                    </p>

                                    <span>
                                        ${cancha.tipoCancha.superficie}
                                    </span>

                                </div>

                            </div>

                        `).join("")
                    }

                </div>

            </div>

        </div>

    `;
}