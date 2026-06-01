// js/perfil/perfil-modificar.js
// Sin imports circulares — importa solo lo que necesita de render

export function RenderizarClienteModificar(cliente) {

    return `
        <div class="card">

            <h2>Editar Perfil</h2>

            <input
                type="text"
                id="nombre"
                value="${cliente.Nombre}"
                placeholder="Nombre"
            >

            <input
                type="text"
                id="apellido"
                value="${cliente.Apellido}"
                placeholder="Apellido"
            >

            <input
                type="text"
                id="localidad"
                value="${cliente.Localidad}"
                placeholder="Localidad"
            >

            <input
                type="text"
                id="pais"
                value="${cliente.Pais}"
                placeholder="Pais"
            >

            <input
                type="email"
                id="correo"
                value="${cliente.Correo}"
                placeholder="Correo"
            >

            <br><br>

            <button id="btnGuardar">
                Guardar Cambios
            </button>

        </div>
    `;
}