import { getData } from "../Global/ApiServices.js";
import { ModificarCliente } from "./ModificarCliente.js";

async function cargarCliente() {

    const dni = localStorage.getItem("dni");

    if (!dni) {
        console.error("No hay DNI en localStorage");
        return;
    }

    const cliente = await getData(`Clientes/${dni}`);

    if (!cliente) {
        console.error("Cliente no encontrado");
        return;
    }

    // inputs
    document.getElementById("nombre").value = cliente.nombre ?? "";
    document.getElementById("apellido").value = cliente.apellido ?? "";
    document.getElementById("correo").value = cliente.correo ?? "";
    
    document.getElementById("localidad").value = cliente.localidad ?? "";
    document.getElementById("pais").value = cliente.pais ?? "";

    const btnSocio = document.getElementById("btnSocio");

    if (btnSocio) {
        if (!cliente.esSocio) {
            btnSocio.classList.remove("hidden");
        } else {
            btnSocio.classList.add("hidden");
        }
    }
}

async function guardarCliente() {

    const dni = localStorage.getItem("dni");

    const actualizado = {
        dni: parseInt(dni),
        nombre: document.getElementById("nombre").value,
        apellido: document.getElementById("apellido").value,
        correo: document.getElementById("correo").value,
        localidad: document.getElementById("localidad").value,
        pais: document.getElementById("pais").value,
        esSocio: false // no lo tocás acá
    };

    const nombre = document.getElementById("nombre").value;
    localStorage.removeItem("nombre");
    localStorage.setItem("nombre",nombre);

    try {

        await ModificarCliente(actualizado);

        Swal.fire({
                toast: true,
                position: "bottom-end",
                icon: "success",
                title: "Perfil actualizado con éxito",
                showConfirmButton: false,
                timer: 2500,
                timerProgressBar: true,
                customClass: {
                    popup: "toast-golahora toast-popup-success",
                    title: "toast-title"
                }
            });

    } catch (err) {

        console.error(err);

           Swal.fire({
                toast: true,
                position: "bottom-end",
                icon: "error",
                title: "Error al actualizar perfil",
                showConfirmButton: false,
                timer: 2500,
                timerProgressBar: true,
                customClass: {
                    popup: "toast-golahora toast-popup-error",
                    title: "toast-title"
                }
            });
    }
}

async function convertirseEnSocio() {

    const dni = localStorage.getItem("dni");

    const actualizado = {
        dni: parseInt(dni),
        esSocio: true
    };

    try {

        await ModificarCliente(actualizado);

        Swal.fire({
            icon: "success",
            title: "¡Ahora sos socio!",
            text: "Tu cuenta fue actualizada correctamente"
        });

        
        const btnSocio = document.getElementById("btnSocio");
        if (btnSocio) btnSocio.classList.add("hidden");

    } catch (err) {

        console.error(err);

        Swal.fire({
            icon: "error",
            title: "Error",
            text: "No se pudo actualizar a socio"
        });
    }
}

document.addEventListener("DOMContentLoaded", () => {

    cargarCliente();

    document
        .getElementById("btnGuardar")
        .addEventListener("click", guardarCliente);

    const btnSocio = document.getElementById("btnSocio");

    if (btnSocio) {
        btnSocio.addEventListener("click", convertirseEnSocio);
    }
});