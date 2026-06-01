import { postData } from "../../Global/ApiServices.js";

const SESSION_KEYS = ["dni", "nombre", "logged", "Adminlogged", "Profesorlogged", "Entrenadorlogged"];
const REDIRECT_DELAY = 2500;

export async function ProfesorLogin() {
    const correo = document.getElementById("email").value.trim();
    const psw = document.getElementById("password").value;
    const endpointUrl = "Profesionales/login/profesor";

    if (!correo || !psw) {
        showLoginError(new Error("Por favor completá todos los campos."));
        return;
    }

    const body = { correo, password: psw };

    try {
        const response = await postData(endpointUrl, body);

        // Clear previous sessions
        SESSION_KEYS.forEach(key => localStorage.removeItem(key));

        // Persist session data
        localStorage.setItem("dni", response.dni);
        localStorage.setItem("nombre", response.nombre);
        localStorage.setItem("logged", "true");
        

        Swal.fire({
            toast: true,
            position: "bottom-end",
            icon: "success",
            title: "Sesión de Profesor iniciada",
            showConfirmButton: false,
            timer: REDIRECT_DELAY,
            timerProgressBar: true,
            customClass: {
                popup: "toast-golahora toast-popup-success",
                title: "toast-title"
            }
        });

        setTimeout(() => {
            window.location.href = "profesor.html";
        }, REDIRECT_DELAY);

    } catch (error) {
        showLoginError(error);
    }
}

// Global binding for HTML
window.ProfesorLogin = ProfesorLogin;