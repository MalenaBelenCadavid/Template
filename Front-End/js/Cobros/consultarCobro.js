import { getData } from '../Global/ApiServices.js';

export async function manejarConsultarCobro() {
    const { value: idCobro } = await Swal.fire({
        title: 'Consultar Cobro',
        text: 'Ingresá el ID del cobro que deseás consultar:',
        input: 'text',
        inputPlaceholder: 'Ej: 1',
        background: '#161616', color: '#ffffff',
        confirmButtonColor: '#3b82f6', showCancelButton: true,
        confirmButtonText: 'Buscar', cancelButtonText: 'Volver'
    });

    if (!idCobro) return; // Si el usuario cancela o cierra el modal, no hace nada

    if (isNaN(idCobro)) {
        return Swal.fire('Error', 'El ID debe ser un número válido.', 'error');
    }

    try {
        Swal.fire({
            title: 'Buscando...',
            background: '#161616', color: '#fff',
            didOpen: () => { Swal.showLoading(); }
        });

        // Realizamos la consulta al controlador de .NET
        const datosCobro = await getData(`Cobro/${idCobro}`);

        // VALIDACIÓN: Interceptamos si el objeto es nulo o si viene el mensaje de error 404 del backend
        if (!datosCobro || datosCobro.StatusCode === 404 || datosCobro.statusCode === 404) {
            return Swal.fire({
                title: 'Error al Consultar',
                text: `No se encontró ningún cobro registrado con el ID ${idCobro}.`,
                icon: 'error',
                background: '#161616', color: '#ffffff',
                confirmButtonColor: '#ef4444'
            });
        }

        // Si pasó el filtro, el cobro existe de verdad. Mostramos la info limpia.
        Swal.fire({
            title: `Datos de Cobro Nro ${idCobro}`,
            html: `<pre style="text-align: left; background: #222; padding: 10px; border-radius: 5px; color: #4ade80;">${JSON.stringify(datosCobro, null, 4)}</pre>`,
            background: '#161616', color: '#ffffff',
            confirmButtonColor: '#22c55e',
            confirmButtonText: 'OK'
        });

    } catch (err) {
        // En caso de que se caiga el servidor local o la red
        Swal.fire({
            title: 'Error de Red',
            text: 'No se pudo establecer conexión con el servidor.',
            icon: 'error',
            background: '#161616', color: '#ffffff',
            confirmButtonColor: '#ef4444'
        });
    }
}