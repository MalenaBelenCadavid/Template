import { getData } from '../Global/ApiServices.js';

export async function manejarConsultarRecibo() {
    const { value: idRecibo } = await Swal.fire({
        title: 'Consultar Recibo',
        text: 'Ingresá el ID del recibo que deseás consultar:',
        input: 'text',
        inputPlaceholder: 'Ej: 1',
        background: '#161616', color: '#ffffff',
        confirmButtonColor: '#3b82f6', showCancelButton: true,
        confirmButtonText: 'Buscar', cancelButtonText: 'Volver'
    });

    if (!idRecibo) return;

    if (isNaN(idRecibo)) {
        return Swal.fire('Error', 'El ID debe ser un número válido.', 'error');
    }

    try {
        Swal.fire({
            title: 'Buscando...',
            background: '#161616', color: '#fff',
            didOpen: () => { Swal.showLoading(); }
        });

        const datosRecibo = await getData(`Recibo/${idRecibo}`);

        // Si el backend responde, pero es un 404 o 500, lanzamos error manualmente
        if (!datosRecibo || datosRecibo.StatusCode === 404 || datosRecibo.StatusCode === 500) {
            throw new Error("No encontrado");
        }

        // Si todo está bien:
        Swal.fire({
            title: `Datos de Recibo Nro ${idRecibo}`,
            html: `<pre style="text-align: left; background: #222; padding: 10px; border-radius: 5px; color: #4ade80;">${JSON.stringify(datosRecibo, null, 4)}</pre>`,
            background: '#161616', color: '#ffffff',
            confirmButtonColor: '#22c55e'
        });

    } catch (err) {
        // AQUÍ ES DONDE ATRAPAMOS EL ERROR 500 QUE VEÍAS EN LA PANTALLA
        Swal.fire({
            title: 'Error al Consultar',
            text: `No se encontró ningún recibo registrado con el ID ${idRecibo}.`,
            icon: 'error',
            background: '#161616', color: '#ffffff',
            confirmButtonColor: '#ef4444'
        });
    }
}