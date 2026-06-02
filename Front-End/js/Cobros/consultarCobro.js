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
                title: `Cobro N° ${idCobro}`,
                background: '#161616',
                color: '#ffffff',
                confirmButtonColor: '#22c55e',
                confirmButtonText: 'OK',

                html: `
                    <div style="
                        text-align:left;
                        padding:20px;
                        border-radius:12px;
                        background: #1f1f1f;
                        box-shadow: 0 0 15px rgba(0,0,0,0.4);
                        font-family: Arial, sans-serif;
                    ">

                        <div style="display:flex; justify-content:space-between; margin-bottom:15px;">
                            <h2 style="margin:0; font-size:18px; color:#22c55e;">
                                Comprobante de Cobro
                            </h2>
                        </div>

                        <hr style="border:0; border-top:1px solid #333; margin-bottom:15px;" />

                        <p><strong style="color:#9ca3af;">ID Cobro:</strong> ${datosCobro.id_Cobro}</p>
                        <p><strong style="color:#9ca3af;">ID Reserva:</strong> ${datosCobro.id_Reserva}</p>
                        <p><strong style="color:#9ca3af;">Monto Total:</strong> 
                            <span style="color:#22c55e; font-weight:bold;">$${datosCobro.montoTotal}</span>
                        </p>
                

                        <div style="margin-top:15px; padding:10px; background:#0f0f0f; border-radius:8px; text-align:center; color:#9ca3af;">
                            Gol Ahora • Sistema de gestión deportiva
                        </div>
                    </div>
                `
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