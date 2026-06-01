// ==========================================================================
// CONTROLADOR OPERATIVO DE COBROS - GOLAHORA (REFACTORIZADO)
// ==========================================================================
import { manejarModificarCobro } from './modificarCobro.js';
import { manejarImprimirTicket } from './imprimirTicket.js';
import { manejarConsultarCobro } from './consultarCobro.js';
import { manejarAplicarDescuento } from './aplicarDescuento.js';

document.addEventListener('DOMContentLoaded', () => {
    initCobrosListeners();
});

function initCobrosListeners() {
    
    // 1. COBRO ALQUILER (Mantiene tu flujo local)
    const btnAlquiler = document.getElementById('btn-cobro-alquiler');
    if (btnAlquiler) {
        btnAlquiler.addEventListener('click', async () => {
            const { value: formValues } = await Swal.fire({
                title: 'Registrar Cobro de Alquiler',
                background: '#161616', color: '#ffffff',
                confirmButtonColor: '#22c55e', cancelButtonColor: '#ef4444',
                showCancelButton: true, confirmButtonText: 'Procesar Pago', cancelButtonText: 'Cancelar',
                html: `
                    <div style="text-align: left; display: flex; flex-direction: column; gap: 1rem; margin-top: 1rem;">
                        <div>
                            <label style="color: #a3a3a3; font-size: 0.85rem; display: block; margin-bottom: 0.4rem;">Cliente / Grupo</label>
                            <input id="swal-cliente-alq" class="swal2-input" placeholder="Ej: Santiago Alarcón" style="margin: 0; width: 100%; background: #262626; color: #fff; border: 1px solid #404040;">
                        </div>
                        <div>
                            <label style="color: #a3a3a3; font-size: 0.85rem; display: block; margin-bottom: 0.4rem;">Monto Recibido ($)</label>
                            <input id="swal-monto-alq" type="number" class="swal2-input" placeholder="Ej: 15000" style="margin: 0; width: 100%; background: #262626; color: #fff; border: 1px solid #404040;">
                        </div>
                    </div>
                `,
                focusConfirm: false,
                preConfirm: () => {
                    const cliente = document.getElementById('swal-cliente-alq').value.trim();
                    const monto = document.getElementById('swal-monto-alq').value.trim();
                    if (!cliente || !monto) { Swal.showValidationMessage('Por favor, completá el nombre y el monto.'); }
                    return { cliente, monto };
                }
            });

            if (formValues) {
                Swal.fire({
                    title: '¡Cobro Registrado!',
                    text: `Se asentaron $${formValues.monto} a nombre de ${formValues.cliente}.`,
                    icon: 'success', background: '#161616', color: '#ffffff', confirmButtonColor: '#22c55e'
                });
            }
        });
    }

    // 2. COBRO INSCRIPCIÓN (Mantiene tu flujo local)
    const btnInscripcion = document.getElementById('btn-cobro-inscripcion');
    if (btnInscripcion) {
        btnInscripcion.addEventListener('click', async () => {
            const { value: formValues } = await Swal.fire({
                title: 'Cobro de Inscripción a Torneo',
                background: '#161616', color: '#ffffff',
                confirmButtonColor: '#22c55e', cancelButtonColor: '#ef4444',
                showCancelButton: true, confirmButtonText: 'Registrar Matrícula', cancelButtonText: 'Cancelar',
                html: `
                    <div style="text-align: left; display: flex; flex-direction: column; gap: 1rem; margin-top: 1rem;">
                        <div>
                            <label style="color: #a3a3a3; font-size: 0.85rem; display: block; margin-bottom: 0.4rem;">Nombre del Equipo / Jugador</label>
                            <input id="swal-equipo" class="swal2-input" placeholder="Ej: Lucas Pérez" style="margin: 0; width: 100%; background: #262626; color: #fff; border: 1px solid #404040;">
                        </div>
                        <div>
                            <label style="color: #a3a3a3; font-size: 0.85rem; display: block; margin-bottom: 0.4rem;">Costo de Inscripción ($)</label>
                            <input id="swal-monto-ins" type="number" class="swal2-input" placeholder="Ej: 8500" style="margin: 0; width: 100%; background: #262626; color: #fff; border: 1px solid #404040;">
                        </div>
                    </div>
                `,
                focusConfirm: false,
                preConfirm: () => {
                    const equipo = document.getElementById('swal-equipo').value.trim();
                    const monto = document.getElementById('swal-monto-ins').value.trim();
                    if (!equipo || !monto) { Swal.showValidationMessage('Todos los campos son obligatorios para el torneo.'); }
                    return { equipo, monto };
                }
            });

            if (formValues) {
                Swal.fire({
                    title: '¡Inscripción Exitosa!',
                    text: `Cobro de torneo registrado por $${formValues.monto} para el equipo/jugador de ${formValues.equipo}.`,
                    icon: 'success', background: '#161616', color: '#ffffff', confirmButtonColor: '#22c55e'
                });
            }
        });
    }

    // 3. MODIFICAR COBRO (Llamada al archivo externo)
    document.getElementById('btn-modificar-cobro')?.addEventListener('click', manejarModificarCobro);

    // 4. IMPRIMIR TICKET (Llamada al archivo externo)
    document.getElementById('btn-imprimir-cobro')?.addEventListener('click', manejarImprimirTicket);

    // 5. CONSULTAR AUDITORÍA / COBROS (Llamada al archivo externo)
    document.getElementById('btn-consultar-cobro')?.addEventListener('click', manejarConsultarCobro);

    // 6. APLICAR DESCUENTO (Llamada al archivo externo)
    document.getElementById('btn-generar-descuento')?.addEventListener('click', manejarAplicarDescuento);
}