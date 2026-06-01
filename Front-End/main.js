import { CreateCards } from "./js/Canchas/Cards/CreateCards.js";
import { actualizarNavbar } from "./js/Global/NavBar/ActNavbar.js";
import { adminPanel } from "./js/Admin/admin.js";
import { createReservaCard } from "./js/Reserva/Cards/CreateReservaCars.js";
import { CreateCompetitionCards } from "./js/Competiciones/Cards/CreateCards.js";
import {CreateAllActivitiesCards} from "./js/Actividades/Cards/RenderActivities.js"
import { entrenadorPanel } from "./js/entrenador/entrenador.js";
import { profesorPanel } from "./js/profesor/profesor.js";

document.addEventListener("DOMContentLoaded", () => {
    actualizarNavbar();
  const page = window.location.pathname.split("/").pop();

  if (page === "canchas.html") {
    CreateCards();
  }
  if (page === "competencias.html") {
    CreateCompetitionCards()
  }
  if (page === "actividades.html") {
    CreateAllActivitiesCards();
  }
  if (page === "admin.html") {
    adminPanel();
  }
  if (page === "reservas.html") {
    createReservaCard();
    }
  
   if(page === "entrenador.html"){
      entrenadorPanel();
    }  
    
    if(page === "profesor.html"){
      profesorPanel();
    }  

});