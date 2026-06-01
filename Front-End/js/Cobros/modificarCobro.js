import { getData } from '../Global/ApiServices.js';

export async function manejarModificarCobro() {
    // 1. Pedimos el ID del cobro a modificar
    const { value: idCobro } = await Swal.fire({
        title: 'Modificar Registro de Cobro',
        text: 'Ingresá el ID del cobro que deseás editar:',
        input: 'text',
        inputPlaceholder: 'Ej: 1',
        background: '#161616', color: '#ffffff',
        confirmButtonColor: '#3b82f6', showCancelButton: true,
        confirmButtonText: 'Buscar', cancelButtonText: 'Volver'
    });

    if (!idCobro) return; // Si cancela, no hace nada

    if (isNaN(idCobro)) {
        return Swal.fire('Error', 'El ID debe ser un número válido.', 'error');
    }

    try {
        Swal.fire({
            title: 'Buscando datos actuales...',
            background: '#161616', color: '#fff',
            didOpen: () => { Swal.showLoading(); }
        });

        // 2. Consultamos al backend para traer los datos vivos del cobro
        const datosActuales = await getData(`Cobro/${idCobro}`);

        // Validación de existencia
        if (!datosActuales || datosActuales.StatusCode === 404 || datosActuales.statusCode === 404) {
            return Swal.fire({
                title: 'Error',
                text: `No se encontró ningún cobro con el ID ${idCobro}.`,
                icon: 'error',
                background: '#161616', color: '#ffffff',
                confirmButtonColor: '#ef4444'
            });
        }

        // 3. Si existe, abrimos un segundo modal con el formulario de edición
        // Usamos los campos reales de tu JSON (id_Reserva, montoTotal, estaCompleto)
        const { value: formValues } = await Swal.fire({
            title: `Modificando Cobro Nro ${idCobro}`,
            background: '#161616', color: '#ffffff',
            html: `
                <div style="text-align: left; width: 80%; margin: 0 auto;">
                    <label style="display:block; margin-bottom: 5px; color: #a3a3a3;">Monto Total ($):</label>
                    <input id="swal-monto" class="swal2-input" type="number" value="${datosActuales.montoTotal || ''}" style="margin-bottom: 15px; width: 100%;">
                    
                    <label style="display:block; margin-bottom: 5px; color: #a3a3a3;">Estado del Pago:</label>
                    <select id="swal-estado" class="swal2-input" style="width: 100%;">
                        <option value="false" ${datosActuales.estaCompleto === false ? 'selected' : ''}>Pendiente / Incompleto</option>
                        <option value="true" ${datosActuales.estaCompleto === true ? 'selected' : ''}>Completo / Pagado</option>
                    </select>
                </div>
            `,
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonColor: '#22c55e', cancelButtonColor: '#ef4444',
            confirmButtonText: 'Guardar Cambios', cancelButtonText: 'Cancelar',
            preConfirm: () => {
                return {
                    id_Cobro: parseInt(idCobro),
                    id_Reserva: datosActuales.id_Reserva, // Mantenemos la relación original
                    montoTotal: parseFloat(document.getElementById('swal-monto').value),
                    estaCompleto: document.getElementById('swal-estado').value === 'true'
                };
            }
        });

        if (!formValues) return; // Si cancela el formulario, salimos

        // Validamos que el monto ingresado sea lógico
        if (isNaN(formValues.montoTotal) || formValues.montoTotal < 0) {
            return Swal.fire('Error', 'Por favor ingresá un monto válido.', 'error');
        }

        // 4. Mandamos la actualización al PUT de tu API usando el puerto 7012
        Swal.fire({
            title: 'Guardando cambios en el servidor...',
            background: '#161616', color: '#fff',
            didOpen: () => { Swal.showLoading(); }
        });

        const response = await fetch(`https://localhost:7012/api/v1/Cobro`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formValues)
        });

        if (!response.ok) {
            throw new Error(`Error en el servidor: ${response.status}`);
        }

        // 5. ¡Éxito total!
        Swal.fire({
            title: '¡Modificado!',
            text: `El cobro Nro ${idCobro} fue actualizado correctamente en la base de datos.`,
            icon: 'success',
            background: '#161616', color: '#ffffff',
            confirmButtonColor: '#3b82f6'
        });

    } catch (err) {
        Swal.fire({
            title: 'Error al Guardar',
            text: 'No se pudieron salvar los cambios: ' + err.message,
            icon: 'error',
            background: '#161616', color: '#ffffff',
            confirmButtonColor: '#ef4444'
        });
    }
}