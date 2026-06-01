import { postData, putData, getData } from '../Global/ApiServices.js';

document.addEventListener('DOMContentLoaded', initRecibosListeners);

function initRecibosListeners() {
    
    // 1. GENERAR RECIBO
    document.getElementById('btn-generar-recibo')?.addEventListener('click', async () => {
        const { value: formValues } = await Swal.fire({
            title: 'Generar Nuevo Recibo',
            background: '#161616', color: '#fff',
            confirmButtonColor: '#22c55e', showCancelButton: true,
            html: `<input id="swal-id-cobro" type="number" class="swal2-input" placeholder="ID Cobro">
                   <input id="swal-id-reserva" type="number" class="swal2-input" placeholder="ID Reserva">
                   <input id="swal-monto" type="number" class="swal2-input" placeholder="Monto Total">`,
            preConfirm: () => {
                return {
                    idCobro: parseInt(document.getElementById('swal-id-cobro').value),
                    idReserva: parseInt(document.getElementById('swal-id-reserva').value),
                    montoTotal: parseFloat(document.getElementById('swal-monto').value),
                    fechaEmision: new Date().toISOString() // Formato ISO que requiere .NET
                }
            }
        });

        // Validamos que no viajen datos vacíos o erróneos
        if (formValues && !isNaN(formValues.idCobro) && !isNaN(formValues.idReserva) && !isNaN(formValues.montoTotal)) {
            try {
                // Cambiado 'recibos' por 'Recibo' para emparejar con .NET
                await postData('Recibo', formValues);
                Swal.fire('¡Éxito!', 'Recibo generado correctamente', 'success');
            } catch (err) { 
                Swal.fire('Error', err.message, 'error'); 
            }
        } else if (formValues) {
            Swal.fire('Error', 'Por favor, completa todos los campos numéricos correctamente.', 'error');
        }
    });

    // 2. MODIFICAR RECIBO
    document.getElementById('btn-modificar-recibo')?.addEventListener('click', async () => {
        const { value: nroRecibo } = await Swal.fire({ title: 'Nro de Recibo a modificar', input: 'text', background: '#161616', color: '#fff' });

        if (nroRecibo) {
            const { value: nuevosDatos } = await Swal.fire({
                title: `Editando Recibo Nro ${nroRecibo}`,
                html: `<input id="edit-id-cobro" type="number" class="swal2-input" placeholder="Confirmar ID Cobro">
                       <input id="edit-id-reserva" type="number" class="swal2-input" placeholder="Nuevo ID Reserva">
                       <input id="edit-monto" type="number" class="swal2-input" placeholder="Nuevo Monto Total">`,
                preConfirm: () => ({
                    idRecibo: parseInt(nroRecibo), // El ID va acá adentro, tal cual lo pide Swagger
                    idCobro: parseInt(document.getElementById('edit-id-cobro').value),
                    idReserva: parseInt(document.getElementById('edit-id-reserva').value),
                    montoTotal: parseFloat(document.getElementById('edit-monto').value),
                    fechaEmision: new Date().toISOString()
                })
            });

            if (nuevosDatos && !isNaN(nuevosDatos.idCobro) && !isNaN(nuevosDatos.idReserva) && !isNaN(nuevosDatos.montoTotal)) {
                try {
                    // CORREGIDO: Le pegamos a 'Recibo' a secas porque el ID ya va en el body
                    await putData('Recibo', nuevosDatos);
                    Swal.fire('Actualizado', 'Cambios guardados', 'success');
                } catch (err) { 
                    Swal.fire('Error', err.message, 'error'); 
                }
            } else if (nuevosDatos) {
                Swal.fire('Error', 'Por favor, completa todos los campos correctamente.', 'error');
            }
        }
    });

    // 3. CONSULTAR HISTÓRICO
    document.getElementById('btn-consultar-recibo')?.addEventListener('click', async () => {
        const { value: busqueda } = await Swal.fire({ title: 'Consultar Recibo', input: 'text', background: '#161616', color: '#fff' });
        if (busqueda) {
            try {
                // Cambiado 'recibos/' por 'Recibo/'
                const data = await getData(`Recibo/${busqueda}`);
                Swal.fire({ title: 'Resultado', html: `<pre style="color: #22c55e; text-align: left;">${JSON.stringify(data, null, 2)}</pre>`, background: '#161616', color: '#fff' });
            } catch (err) { Swal.fire('Error', 'No encontrado o error de conexión', 'error'); }
        }
    });

    // 4. IMPRIMIR / REIMPRIMIR
    document.getElementById('btn-imprimir-recibo')?.addEventListener('click', async () => {
        const { value: nroImprimir } = await Swal.fire({ 
            title: 'Reimpresión', input: 'text', background: '#161616', color: '#fff', confirmButtonColor: '#3b82f6' 
        });

        if (nroImprimir) {
            try {
                // Cambiado 'recibos/' por 'Recibo/'
                await getData(`Recibo/${nroImprimir}/imprimir`);
                
                Swal.fire({
                    title: 'Procesando...',
                    timer: 1500,
                    didOpen: () => { Swal.showLoading(); },
                    background: '#161616', color: '#fff'
                }).then(() => {
                    Swal.fire('¡Enviado!', `El recibo ${nroImprimir} se envió a la ticketera.`, 'success');
                });
            } catch (err) {
                Swal.fire('Error', 'No se pudo obtener el recibo para impresión: ' + err.message, 'error');
            }
        }
    });
}