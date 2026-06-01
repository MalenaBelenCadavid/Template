import { postData } from '../Global/ApiServices.js';

export async function manejarAplicarDescuento() {
    const { value: formValues } = await Swal.fire({
        title: 'Aplicar Descuento Comercial',
        background: '#161616', color: '#ffffff',
        confirmButtonColor: '#ef4444', showCancelButton: true,
        confirmButtonText: 'Aplicar Deducción', cancelButtonText: 'Atrás',
        html: `
            <div style="text-align: left; display: flex; flex-direction: column; gap: 1rem; margin-top: 1rem;">
                <div>
                    <label style="color: #a3a3a3; font-size: 0.85rem; display: block; margin-bottom: 0.4rem;">ID del Cobro</label>
                    <input id="swal-desc-id" type="number" class="swal2-input" placeholder="Ej: 1" style="margin: 0; width: 100%; background: #262626; color: #fff; border: 1px solid #404040;">
                </div>
                <div>
                    <label style="color: #a3a3a3; font-size: 0.85rem; display: block; margin-bottom: 0.4rem;">Porcentaje a Deducir (%)</label>
                    <input id="swal-desc-porcentaje" type="number" class="swal2-input" placeholder="Min: 1 - Max: 100" style="margin: 0; width: 100%; background: #262626; color: #fff; border: 1px solid #404040;">
                </div>
            </div>
        `,
        preConfirm: () => {
            const idCobro = parseInt(document.getElementById('swal-desc-id').value);
            const porcentaje = parseInt(document.getElementById('swal-desc-porcentaje').value);
            if (isNaN(idCobro) || isNaN(porcentaje) || porcentaje < 1 || porcentaje > 100) {
                Swal.showValidationMessage('Ingresa un ID válido y un porcentaje entre 1 y 100.');
            }
            return { idCobro, porcentaje };
        }
    });

    if (formValues) {
        try {
            await postData('Cobro/AplicarDescuento', formValues);
            Swal.fire({
                title: 'Descuento Autorizado',
                text: `Se aplicó un -${formValues.porcentaje}% con éxito.`,
                icon: 'success',
                background: '#161616', color: '#ffffff',
                confirmButtonColor: '#ef4444'
            });
        } catch (err) {
            Swal.fire('Error', err.message, 'error');
        }
    }
}