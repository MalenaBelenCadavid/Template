// login.js
import { Login } from "../PostCliente.js";

export async function handleLogin() {

  const email = document.getElementById('email');
  const password = document.getElementById('password');

  let valid = true;

  email.classList.remove('error-input');
  password.classList.remove('error-input');

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  if (!emailRegex.test(email.value)) {

    email.classList.add('error-input');

    Swal.fire({
      toast: true,
      position: "bottom-end",
      icon: "error",
      title: "Ingresá un correo válido",
      showConfirmButton: false,
      timer: 2500,
      timerProgressBar: true,

      customClass: {
        popup: "toast-golahora toast-popup-error",
        title: "toast-title"
      }
    });

    valid = false;
  }

  if (password.value.length < 6) {

    password.classList.add('error-input');

    Swal.fire({
      toast: true,
      position: "bottom-end",
      icon: "error",
      title: "La contraseña debe tener mínimo 6 caracteres",
      showConfirmButton: false,
      timer: 2500,
      timerProgressBar: true,

      customClass: {
        popup: "toast-golahora toast-popup-error",
        title: "toast-title"
      }
    });

    valid = false;
  }

  if (!valid) return;

  try {

    const response = await Login(email.value, password.value);
    SESSION_KEYS.forEach(key => localStorage.removeItem(key));

    localStorage.setItem("dni",response.dni);
    localStorage.setItem("nombre",response.nombre);
    localStorage.setItem("logged",true);

    /* 
    CHEQUEO SIMPLE ADMIN
    */

if(
email.value.includes("admin")
)
{
  localStorage.setItem(
    "adminLogueado",
    "true"
  );
}
else
{
  localStorage.removeItem(
    "adminLogueado"
  );
}

    Swal.fire({
      toast: true,
      position: "bottom-end",
      icon: "success",
      title: "Sesión iniciada correctamente",
      showConfirmButton: false,
      timer: 2500,
      timerProgressBar: true,

      customClass: {
        popup: "toast-golahora toast-popup-success",
        title: "toast-title"
      }
    });

    setTimeout(() => {

      window.location.href = 'canchas.html';

    }, 2500);

  } 
  catch (error) {

    Swal.fire({
      toast: true,
      position: "bottom-end",
      icon: "error",
      title: error.message ?? "Credenciales invalidas",
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

// Enter key
document.addEventListener('keydown', e => {

  if (e.key === 'Enter') {
    handleLogin();
  }

});

window.handleLogin = handleLogin;
