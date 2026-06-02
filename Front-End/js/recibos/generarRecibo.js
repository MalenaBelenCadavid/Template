import { getData, postData } from '../Global/ApiServices.js';

export async function registrarRecibo() {

    const { value: idCobro,isDismissed } = await Swal.fire({
        title: 'Registrar Recibo',
        text: 'Ingresá el ID del cobro:',
        input: 'text',
        inputPlaceholder: 'Ej: 1',
        background: '#161616',
        color: '#ffffff',
        confirmButtonColor: '#3b82f6',
        showCancelButton: true,
        confirmButtonText: 'Buscar',
        cancelButtonText: 'Cancelar'
    });
    if (isDismissed) return;

    if (!idCobro || isNaN(idCobro)) {
        return Swal.fire('Error', 'ID inválido', 'error');
    }

    try {

        Swal.fire({
            title: 'Buscando cobro...',
            background: '#161616',
            color: '#fff',
            didOpen: () => Swal.showLoading()
        });

        // =========================
        // TRAER COBRO
        // =========================
        const cobro = await getData(`Cobro/${idCobro}`);

        if (!cobro || Object.keys(cobro).length === 0) {
            throw new Error("Cobro inexistente");
        }

        // =========================
        // ARMAR RECIBO AUTOMÁTICO
        // =========================
        const recibo = {
            idCobro: Number(idCobro),
            idReserva: cobro.idReserva,
            montoTotal: cobro.montoTotal,
            fechaEmision: new Date().toISOString()
        };

        // =========================
        // ENVIAR AL BACKEND
        // =========================
        const response = await postData("Recibo", recibo);

        Swal.fire({
            title: 'Recibo registrado',
            text: 'El recibo se generó correctamente.',
            icon: 'success',
            background: '#161616',
            color: '#ffffff',
            confirmButtonColor: '#3b82f6'
        });

        return response;

    } catch (err) {

        console.error(err);

        Swal.fire({
            title: 'Error',
            text: 'No se pudo generar el recibo para ese cobro.',
            icon: 'error',
            background: '#161616',
            color: '#ffffff',
            confirmButtonColor: '#ef4444'
        });
    }
}