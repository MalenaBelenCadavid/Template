import { getData } from '../Global/ApiServices.js';

export async function manejarConsultarRecibo() {
        const { value: idRecibo, isDismissed } = await Swal.fire({
            title: 'Consultar Recibo',
            text: 'Ingresá el ID del recibo que deseás consultar:',
            input: 'text',
            inputPlaceholder: 'Ej: 1',
            background: '#161616',
            color: '#ffffff',
            confirmButtonColor: '#3b82f6',
            showCancelButton: true,
            confirmButtonText: 'Buscar',
            cancelButtonText: 'Volver'
        });

        
        if (isDismissed) return;

        // SI ESTÁ VACÍO
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

        if (!datosRecibo || datosRecibo.StatusCode === 404 || datosRecibo.StatusCode === 500) {
            throw new Error("No encontrado");
        }

       Swal.fire({
            title: `Recibo N° ${datosRecibo.id_Recibo ?? idRecibo}`,

            html: `
                <div style="
                    text-align:left;
                    background:#1e1e1e;
                    padding:18px;
                    border-radius:12px;
                    color:#fff;
                    font-family: Arial;
                    box-shadow:0 0 10px rgba(0,0,0,0.4);
                ">

                    <div style="display:flex; justify-content:space-between; margin-bottom:12px;">
                        <span style="color:#9ca3af;">Estado</span>
                        <span style="color:#22c55e; font-weight:bold;">PAGADO</span>
                    </div>

                    <hr style="border:0; border-top:1px solid #333; margin:10px 0;" />

                    <p><b> ID Cobro:</b> ${datosRecibo.id_Cobro}</p>
                    <p><b> ID Reserva:</b> ${datosRecibo.id_Reserva}</p>

                    <hr style="border:0; border-top:1px solid #333; margin:10px 0;" />

                    <p style="font-size:18px;">
                        <b> Monto:</b> 
                        <span style="color:#3b82f6;">$${datosRecibo.montoTotal}</span>
                    </p>

                    <p>
                        <b> Fecha:</b> 
                        ${new Date(datosRecibo.fechaEmision).toLocaleString()}
                    </p>

                </div>
            `,

            background: '#161616',
            color: '#ffffff',
            confirmButtonColor: '#22c55e',
            confirmButtonText: 'Cerrar'
        });

    } catch (err) {
        
        Swal.fire({
            title: 'Error al Consultar',
            text: `No se encontró ningún recibo registrado con el ID ${idRecibo}.`,
            icon: 'error',
            background: '#161616', color: '#ffffff',
            confirmButtonColor: '#ef4444'
        });
    }
}