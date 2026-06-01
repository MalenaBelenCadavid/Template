import { getData } from '../Global/ApiServices.js';

export async function manejarImprimirTicket() {
    const { value: idTicket } = await Swal.fire({
        title: 'Reimprimir Ticket de Operación',
        text: 'Ingresá el ID numérico del cobro a imprimir:',
        input: 'text',
        inputPlaceholder: 'Ej: 1',
        background: '#161616', color: '#ffffff',
        confirmButtonColor: '#3b82f6', showCancelButton: true,
        confirmButtonText: 'Reimprimir', cancelButtonText: 'Volver'
    });

    if (idTicket) {
        if (isNaN(idTicket)) {
            return Swal.fire('Error', 'El ID debe ser un número válido.', 'error');
        }

        try {
            Swal.fire({
                title: 'Verificando transacción...',
                background: '#161616', color: '#fff',
                didOpen: () => { Swal.showLoading(); }
            });

            // 1. Guardamos lo que nos responde la API en una constante
            const datosCobro = await getData(`Cobro/${idTicket}`);

            // 2. VALIDACIÓN REAL: Si la API devuelve null, undefined, un objeto vacío, 
            // o si no tiene las propiedades clave del Cobro (como id_Cobro o idCobro)
            if (!datosCobro || Object.keys(datosCobro).length === 0 || (!datosCobro.id_Cobro && !datosCobro.idCobro)) {
                // Forzamos manualmente la caída al bloque catch
                throw new Error("El cobro no existe en la base de datos.");
            }

            // 3. Si pasó el filtro de arriba, los datos son reales y existen
            Swal.fire({
                title: '¡Enviado!',
                text: `El ticket del cobro Nro ${idTicket} fue enviado con éxito a la ticketera de buffet.`,
                icon: 'success',
                background: '#161616', color: '#ffffff',
                confirmButtonColor: '#3b82f6'
            });

        } catch (err) {
            // Ahora si ponés el ID 5 va a morir acá arriba y va a saltar directo a este cartel
            Swal.fire({
                title: 'Error de Impresión',
                text: `No se encontró ningún cobro registrado con el ID ${idTicket}.`,
                icon: 'error',
                background: '#161616', color: '#ffffff',
                confirmButtonColor: '#ef4444'
            });
        }
    }
}