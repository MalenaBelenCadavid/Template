import { getData } from '../Global/ApiServices.js';

function getBase64ImageFromURL(url) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.setAttribute('crossOrigin', 'anonymous');

        img.onload = () => {
            const canvas = document.createElement('canvas');
            canvas.width = img.width;
            canvas.height = img.height;

            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0);

            resolve(canvas.toDataURL('image/png'));
        };

        img.onerror = reject;
        img.src = url;
    });
}

export async function imprimirRecibo() {


    const { value: idCobro,isDismissed } = await Swal.fire({
        title: 'Imprimir Recibo',
        text: 'Ingresá el ID del cobro:',
        input: 'text',
        inputPlaceholder: 'Ej: 1',
        background: '#161616',
        color: '#ffffff',
        confirmButtonColor: '#3b82f6',
        showCancelButton: true,
        confirmButtonText: 'Generar',
        cancelButtonText: 'Volver'
    });
    
    if (isDismissed) return;

    if (!idCobro || isNaN(idCobro)) {
        return Swal.fire('Error', 'El ID debe ser numérico.', 'error');
    }

    try {

        Swal.fire({
            title: 'Generando recibo...',
            background: '#161616',
            color: '#fff',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        // =========================
        // DATA
        // =========================
        const datosCobro = await getData(`Cobro/${idCobro}`);

        if (!datosCobro || Object.keys(datosCobro).length === 0) {
            throw new Error("Cobro inexistente");
        }

        const logo = "/Images/golAhoraIcon.png";
        const logoBase64 = await getBase64ImageFromURL(logo);

        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();

        // =========================
        // HEADER SIMPLE
        // =========================
        doc.setFillColor(20, 20, 20);
        doc.rect(0, 0, 210, 35, "F");

        doc.addImage(logoBase64, 'PNG', 12, 5, 22, 22);

        doc.setTextColor(255, 255, 255);
        doc.setFontSize(16);
        doc.text("RECIBO DE PAGO", 45, 18);

        doc.setFontSize(10);
        doc.setTextColor(200, 200, 200);
        doc.text("Gol Ahora - Sistema de Reservas", 45, 26);

        // =========================
        // CUADRO PRINCIPAL
        // =========================
        doc.setDrawColor(0);
        doc.rect(10, 45, 190, 110);

        doc.setTextColor(0, 0, 0);

        doc.setFontSize(12);
        doc.setFont("helvetica", "bold");
        doc.text("DETALLE DEL PAGO", 20, 60);

        doc.line(20, 63, 190, 63);

        doc.setFont("helvetica", "normal");

        // =========================
        // DATOS
        // =========================
        const startY = 75;

        doc.text("ID Cobro:", 20, startY);
        doc.text(`${idCobro}`, 80, startY);

        doc.text("Cliente DNI:", 20, startY + 10);
        doc.text(`${datosCobro.clienteDni}`, 80, startY + 10);

        doc.text("Método de Pago:", 20, startY + 20);
        doc.text(`${datosCobro.metodoPago}`, 80, startY + 20);

        doc.text("Fecha:", 20, startY + 30);
        doc.text(`${new Date().toLocaleString()}`, 80, startY + 30);

        // =========================
        // MONTO DESTACADO
        // =========================
        doc.setFontSize(14);
        doc.setTextColor(0, 102, 204);
        doc.setFont("helvetica", "bold");

        doc.text(`TOTAL PAGADO: $${datosCobro.montoTotal}`, 20, startY + 50);

        // =========================
        // FOOTER
        // =========================
        doc.setFontSize(10);
        doc.setTextColor(120, 120, 120);

        doc.line(10, 165, 200, 165);

        doc.text("Este recibo certifica el pago realizado.", 20, 175);
        doc.text("Gol Ahora - Gracias por su confianza.", 20, 182);

        // =========================
        // DESCARGA
        // =========================
        doc.save(`recibo-${idCobro}.pdf`);

        Swal.fire({
            title: 'Recibo generado',
            text: 'El recibo se descargó correctamente.',
            icon: 'success',
            background: '#161616',
            color: '#ffffff',
            confirmButtonColor: '#3b82f6'
        });

    } catch (err) {

        console.error(err);

        Swal.fire({
            title: 'Error',
            text: `No existe un cobro con ID ${idCobro}.`,
            icon: 'error',
            background: '#161616',
            color: '#ffffff',
            confirmButtonColor: '#ef4444'
        });
    }
}