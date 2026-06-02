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

export async function manejarImprimirTicket() {

    const { value: idTicket } = await Swal.fire({
        title: 'Reimprimir Ticket de Operación',
        text: 'Ingresá el ID del cobro:',
        input: 'text',
        inputPlaceholder: 'Ej: 1',
        background: '#161616',
        color: '#ffffff',
        confirmButtonColor: '#3b82f6',
        showCancelButton: true,
        confirmButtonText: 'Descargar PDF',
        cancelButtonText: 'Volver'
    });

    if (!idTicket || isNaN(idTicket)) {
        return Swal.fire('Error', 'El ID debe ser numérico.', 'error');
    }

    try {

        Swal.fire({
            title: 'Generando PDF...',
            background: '#161616',
            color: '#fff',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        // =========================
        // DATA
        // =========================
        const datosCobro = await getData(`Cobro/${idTicket}`);

        if (!datosCobro || Object.keys(datosCobro).length === 0) {
            throw new Error("Cobro inexistente");
        }

        // =========================
        // LOGO
        // =========================
        const logo = "/Images/golAhoraIcon.png";
        const logoBase64 = await getBase64ImageFromURL(logo);

        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();

        // =========================
        // HEADER
        // =========================
        doc.setFillColor(20, 20, 20);
        doc.rect(0, 0, 210, 45, "F");

        doc.addImage(logoBase64, 'PNG', 12, 8, 28, 28);

        doc.setTextColor(255, 255, 255);
        doc.setFontSize(18);
        doc.text("GOL AHORA", 45, 20);

        doc.setFontSize(11);
        doc.setTextColor(180, 180, 180);
        doc.text("Sistema de Cobros y Reservas", 45, 28);

        // =========================
        // CUADRO PRINCIPAL
        // =========================
        doc.setDrawColor(180);
        doc.roundedRect(10, 55, 190, 90, 3, 3);

        doc.setTextColor(0, 0, 0);
        doc.setFontSize(12);
        doc.setFont("helvetica", "bold");
        doc.text("COMPROBANTE DE COBRO", 20, 70);

        doc.line(20, 75, 190, 75);

        doc.setFont("helvetica", "normal");

        // =========================
        // DATOS
        // =========================
        const startY = 85;

        doc.text("ID Cobro:", 20, startY);
        doc.text(`${idTicket}`, 80, startY);

        doc.text("Cliente DNI:", 20, startY + 10);
        doc.text(`${datosCobro.clienteDni}`, 80, startY + 10);

        doc.text("Método de Pago:", 20, startY + 20);
        doc.text(`${datosCobro.metodoPago}`, 80, startY + 20);

        doc.text("Monto Total:", 20, startY + 30);
        doc.setTextColor(0, 102, 204);
        doc.setFont("helvetica", "bold");
        doc.text(`$${datosCobro.montoTotal}`, 80, startY + 30);

        doc.setTextColor(0, 0, 0);
        doc.setFont("helvetica", "normal");

        doc.text("Fecha:", 20, startY + 40);
        doc.text(`${new Date().toLocaleDateString()}`, 80, startY + 40);

        // =========================
        // FOOTER
        // =========================
        doc.setDrawColor(200);
        doc.line(10, 155, 200, 155);

        doc.setFontSize(10);
        doc.setTextColor(120, 120, 120);

        doc.text("Gracias por confiar en Gol Ahora", 20, 165);
        doc.text("Este comprobante es válido como constancia de pago", 20, 172);

        // =========================
        // MARCA DE AGUA (LOGO GRANDE)
        // =========================
        try {
            doc.setGState?.(new doc.GState({ opacity: 0.08 }));

            doc.addImage(
                logoBase64,
                'PNG',
                35,
                60,
                140,
                140
            );

            doc.setGState?.(new doc.GState({ opacity: 1 }));
        } catch (e) {
            // fallback texto si no soporta GState
            doc.setTextColor(230, 230, 230);
            doc.setFontSize(50);
            doc.text("GOL AHORA", 35, 140, { angle: 45 });
        }

        // =========================
        // DESCARGA (AL FINAL SIEMPRE)
        // =========================
        doc.save(`ticket-${idTicket}.pdf`);

        Swal.fire({
            title: 'PDF generado',
            text: 'El ticket se descargó correctamente.',
            icon: 'success',
            background: '#161616',
            color: '#ffffff',
            confirmButtonColor: '#3b82f6'
        });

    } catch (err) {

        console.error(err);

        Swal.fire({
            title: 'Error',
            text: `No existe un cobro con ID ${idTicket}.`,
            icon: 'error',
            background: '#161616',
            color: '#ffffff',
            confirmButtonColor: '#ef4444'
        });
    }
}