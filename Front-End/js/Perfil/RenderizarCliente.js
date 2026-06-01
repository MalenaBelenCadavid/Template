
export function RenderizarCliente(cliente) {
    return `
        <div class="card">

            <h2>${cliente.Nombre} ${cliente.Apellido}</h2>

            <p><strong>DNI:</strong> ${cliente.Dni}</p>
            <p><strong>Correo:</strong> ${cliente.Correo}</p>
            <p><strong>Localidad:</strong> ${cliente.Localidad}</p>
            <p><strong>País:</strong> ${cliente.Pais}</p>

        </div>
    `;
}