import { postData } from "../../Global/ApiServices.js";

const SESSION_KEYS = ["dni", "nombre", "logged", "Adminlogged", "Profesorlogged", "Entrenadorlogged"];
const REDIRECT_DELAY = 2500;

export async function EntrenadorLogin() {
    const correo = document.getElementById("email").value.trim();
    const psw = document.getElementById("password").value;
    const endpointUrl = "Profesionales/login/entrenador";

    if (!correo || !psw) {
        showLoginError(new Error("Por favor completá todos los campos."));
        return;
    }

    const body = { correo, password: psw };

    try {
        const response = await postData(endpointUrl, body);

        SESSION_KEYS.forEach(key => localStorage.removeItem(key));

        localStorage.setItem("dni", response.dni);
        localStorage.setItem("nombre", response.nombre);
        localStorage.setItem("logged", "true");

        Swal.fire({
            toast: true,
            position: "bottom-end",
            icon: "success",
            title: "Sesión de Entrenador iniciada",
            showConfirmButton: false,
            timer: REDIRECT_DELAY,
            timerProgressBar: true,
            customClass: { popup: "toast-golahora toast-popup-success", title: "toast-title" }
        });

        setTimeout(() => { window.location.href = "entrenador.html"; }, REDIRECT_DELAY);

    } catch (error) {
        showLoginError(error);
    }
}

function showLoginError(error) {
    Swal.fire({
        toast: true,
        position: "bottom-end",
        icon: "error",
        title: error.message || "Error al iniciar sesión",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        customClass: { popup: "toast-golahora toast-popup-error", title: "toast-title" }
    });
}

window.EntrenadorLogin = EntrenadorLogin;