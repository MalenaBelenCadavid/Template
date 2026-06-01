import { getCanchas } from "../Canchas/GetCanchas.js";
import { getTipoCanchas } from "../TipoCancha/GetTipoCancha.js";
import { RenderAdminCards } from "./renderAdminCards.js";
import { RenderTcCards } from "./renderTcCards.js";
import { crearCancha } from "./Modals/crearCancha.js";
import { editarCancha } from "./Modals/editarCancha.js";
import { editarTipoCancha } from "./Modals/editarTipoCancha.js";
import { getProfesionales } from "../Profesionales/getProfesionales.js";
import { RenderProfesionalCards } from "../Profesionales/Cards/renderProfesionalCards.js";
import { abrirModalCrearProfesional } from "./Modals/crearProfesional.js  ";
import { crearTipoCancha } from "./Modals/crearTipoCancha.js";
import { getCompeticiones } from "../Competiciones/getCompeticiones.js";
import { RenderCompetitionAdminCards } from "./renderCompetitionAdminCards.js";
import { crearCompetencia } from "./Modals/crearCompetencia.js";
import { getEntrenamientos } from "../Actividades/GetEntrenamientos.js";
import { getClases } from "../Actividades/GetClases.js";
import { RenderAdminClasesCards } from "./renderClases.js";
import { RenderAdminEntrenamientoCards } from "./renderEntrenamientos.js";
import { crearEntrenamiento } from "./Modals/crearEntrenamiento.js";
import { eliminarEntrenamiento } from "./eliminarEntrenamiento.js";
import { crearClase } from "./Modals/crearClases.js";
import { eliminarClase } from "./eliminarClase.js";
import { eliminarCancha } from "./eliminarCancha.js";
import { eliminarProfesional } from "./eliminarProfesional.js";
import { renderAccionesCobro } from "./cobrosBtns.js";
import { renderAccionesRecibo } from "./recibosBtns.js";
import { editarCompetencia } from "./Modals/editarCompetencia.js";



export function adminPanel(){
const botones = document.querySelectorAll(".sidebar-btn");
const secciones = document.querySelectorAll(".content-section");

botones.forEach((boton, index) => {
  boton.addEventListener("click", async() => {

    botones.forEach(btn => {
      btn.classList.remove("active");
    });

    secciones.forEach(section => {
      section.classList.add("hidden");
    });

    
    boton.classList.add("active");   
    secciones[index].classList.remove("hidden");
    const seccionAct = boton.textContent.trim();;



 if(seccionAct == "Dashboard")
  {
        console.log("AAAA")
  }

 if (seccionAct == "Canchas")
  {
  const canchas = await getCanchas();

  const container = document.querySelector("#canchas");

  const contenido = `<div class="section-header">
      <button class="btn-primary" id="btn-crear-cancha">
        + Crear cancha
      </button>
  </div>
    ${RenderAdminCards(canchas)}
  `;

  container.innerHTML = contenido;
  container.querySelector("#btn-crear-cancha")
    .addEventListener("click", crearCancha);

  container.querySelectorAll(".admin-btn-edit")
    .forEach(btn => {
      btn.addEventListener("click", () => {
        const id = Number(btn.dataset.id);
        const cancha = canchas.find(c => c.idCancha === id);
        if (cancha) {
          editarCancha(cancha);
        }
      });

    });
    document.querySelectorAll(".admin-btn-delete")
  .forEach(btn => {

    btn.addEventListener("click", async () => {

      const id = Number(btn.dataset.id);
      const result = await Swal.fire({

      title: "¿Eliminar cancha?",
      text: "Esta acción no se puede deshacer",
      icon: "warning",

      showCancelButton: true,

      confirmButtonText: "Eliminar",
      cancelButtonText: "Cancelar",

      reverseButtons: true,

      background: "#1e1e1e",
      color: "#f5f5f5",

      customClass: {

        popup: "swal-golahora-popup",

        title: "swal-golahora-title",

        htmlContainer: "swal-golahora-text",

        confirmButton: "swal-golahora-confirm",

        cancelButton: "swal-golahora-cancel"

      }

    });

      if (!result.isConfirmed) return;

        await eliminarCancha(id);
      const card = document.querySelector(
          `.admin-card[data-id="${id}"]`
        );
        card.remove();

    });

  });    


}

 if(seccionAct == "Tipo de canchas")
{
    const tipos = await getTipoCanchas();

    const container = document.querySelector("#tipoCanchas");

    container.innerHTML = `

      <div class="section-header">

        <button 
          class="btn-primary" 
          id="btn-crear-tipo-cancha"
        >
          + Crear tipo de cancha
        </button>

      </div>

      ${RenderTcCards(tipos)}

    `;

    container
      .querySelector("#btn-crear-tipo-cancha")
      .addEventListener("click", () => {
        crearTipoCancha();
      });

   container.querySelectorAll(".admin-btn-edit")
    .forEach(btn => {
      btn.addEventListener("click", () => {
        const id = Number(btn.dataset.id);
        const tipoCancha = tipos.find(tc => tc.id === id);
        if (tipoCancha) {
          editarTipoCancha(tipoCancha);
        }
      });

    });
    document.querySelectorAll(".admin-btn-delete")
  .forEach(btn => {

    btn.addEventListener("click", async () => {

      const id = Number(btn.dataset.id);
      const result = await Swal.fire({

      title: "¿Eliminar tipo de cancha?",
      text: "Esta acción no se puede deshacer",
      icon: "warning",

      showCancelButton: true,

      confirmButtonText: "Eliminar",
      cancelButtonText: "Cancelar",

      reverseButtons: true,

      background: "#1e1e1e",
      color: "#f5f5f5",

      customClass: {

        popup: "swal-golahora-popup",

        title: "swal-golahora-title",

        htmlContainer: "swal-golahora-text",

        confirmButton: "swal-golahora-confirm",

        cancelButton: "swal-golahora-cancel"

      }

    });

      if (!result.isConfirmed) return;

        await eliminarCancha(id);
      const card = document.querySelector(
          `.admin-card[data-id="${id}"]`
        );
        card.remove();

    });

  });    


}
   
 if (seccionAct == "Profesionales") {

  const container = document.querySelector("#profesionales");

  container.innerHTML = `
    
    <div class="section-header">

      <button class="btn-primary" id="btn-crear-profesional">
        + Crear profesional
      </button>

      <select id="tipoProfesional" class="select-profesional">
        <option value="Profesor">Profesor</option>
        <option value="Entrenador">Entrenador</option>
      </select>

    </div>

    <div id="profesionales-list">

      <p style="color: rgba(255,255,255,0.4)">
        Cargando profesionales...
      </p>

    </div>
  `;

  const select = document.querySelector("#tipoProfesional");
  const list = document.querySelector("#profesionales-list");

  async function cargarProfesionales(tipo) {

    try {

      const profesionales = await getProfesionales(tipo);

      list.innerHTML = RenderProfesionalCards(profesionales,tipo);

    } catch (error) {

      console.error(error);

      list.innerHTML = `
        <p style="color:red;">
          Error al cargar profesionales
        </p>
      `;
    }
  }

  await cargarProfesionales(select.value);

  select.addEventListener("change", async () => {

    await cargarProfesionales(select.value);

  });

  container.querySelector("#btn-crear-profesional")
  .addEventListener("click", () => {

    const tipoSeleccionado = select.value;

    abrirModalCrearProfesional(tipoSeleccionado);

  });

   document.querySelectorAll(".admin-btn-delete")
  .forEach(btn => {

    btn.addEventListener("click", async () => {

      const id = Number(btn.dataset.id);
      const result = await Swal.fire({

      title: "¿Eliminar profesional?",
      text: "Esta acción no se puede deshacer",
      icon: "warning",

      showCancelButton: true,

      confirmButtonText: "Eliminar",
      cancelButtonText: "Cancelar",

      reverseButtons: true,

      background: "#1e1e1e",
      color: "#f5f5f5",

      customClass: {

        popup: "swal-golahora-popup",

        title: "swal-golahora-title",

        htmlContainer: "swal-golahora-text",

        confirmButton: "swal-golahora-confirm",

        cancelButton: "swal-golahora-cancel"

      }

    });

      if (!result.isConfirmed) return;

        await eliminarProfesional(id);
      const card = document.querySelector(
          `.admin-card[data-id="${id}"]`
        );
        card.remove();

    });

  });  


}

if (seccionAct == "Competencias") {

  const container = document.querySelector("#competiciones");

  container.innerHTML = `
    <div class="section-header">
      <button class="btn-primary" id="btn-crear-competencia">
        + Crear competencia
      </button>
    </div>

    <div id="competitions-container"></div>
  `;

  const competiciones = await getCompeticiones();

  // estado global (para eventos)
  window.competicionesGlobal = competiciones;

  const listContainer = container.querySelector("#competitions-container");

  listContainer.innerHTML = RenderCompetitionAdminCards(competiciones);

  // =========================
  // CREAR COMPETENCIA
  // =========================
  const btnCrear = container.querySelector("#btn-crear-competencia");

  if (btnCrear) {
    btnCrear.addEventListener("click", crearCompetencia);
  }

  if (!window.eventEditarCompetencia) {

  document.addEventListener("click", (e) => {

    const btn = e.target.closest(".admin-btn-edit");
    if (!btn) return;

    const id = Number(btn.dataset.id);

    const competencia = window.competicionesGlobal?.find(
      c => c.competenciaId === id
    );

    if (!competencia) return;

    editarCompetencia(competencia);
  });

  window.eventEditarCompetencia = true;
}


}

  if(seccionAct =="Clases")
  {
    const container = document.querySelector("#clases");
    container.innerHTML = `

      <div class="section-header">

        <button 
          class="btn-primary"
          id="btn-crear-clase"
        >
          + Crear clase
        </button>

      </div>

      <div id="clases-container"></div>
    `;
    const clases = await getClases();

     document.querySelector("#clases-container").innerHTML =
      RenderAdminClasesCards(clases);
    
    document.querySelector("#btn-crear-clase").addEventListener('click',()=>
    {
      crearClase();
    })

     document.querySelectorAll(".admin-btn-delete")
  .forEach(btn => {

    btn.addEventListener("click", async () => {

      const id = Number(btn.dataset.id);
      const result = await Swal.fire({

      title: "¿Eliminar clase?",
      text: "Esta acción no se puede deshacer",
      icon: "warning",

      showCancelButton: true,

      confirmButtonText: "Eliminar",
      cancelButtonText: "Cancelar",

      reverseButtons: true,

      background: "#1e1e1e",
      color: "#f5f5f5",

      customClass: {
        popup: "swal-golahora-popup",
        title: "swal-golahora-title",
        htmlContainer: "swal-golahora-text",
        confirmButton: "swal-golahora-confirm",
        cancelButton: "swal-golahora-cancel"
      }

    });

      if (!result.isConfirmed) return;

        await eliminarClase(id);
      const card = document.querySelector(
          `.admin-card[data-id="${id}"]`
        );
        card.remove();

    });

  });  
  

  
    }


  if(seccionAct =="Entrenamientos")
  {
    const container = document.querySelector("#entrenamientos");
     container.innerHTML = `

      <div class="section-header">

        <button 
          class="btn-primary"
          id="btn-crear-entrenamiento"
        >
          + Crear entrenamiento
        </button>

      </div>

      <div id="entrenamientos-container"></div>
    `;
    
    const entrenamientos = await getEntrenamientos();
      document.querySelector("#entrenamientos-container").innerHTML =
      RenderAdminEntrenamientoCards(entrenamientos);
      
      
    document.querySelector("#btn-crear-entrenamiento").addEventListener('click',()=>
    {
      crearEntrenamiento();
    })
  
      document.querySelectorAll(".admin-btn-delete")
  .forEach(btn => {

    btn.addEventListener("click", async () => {

      const id = Number(btn.dataset.id);
      const result = await Swal.fire({

      title: "¿Eliminar entrenamiento?",
      text: "Esta acción no se puede deshacer",
      icon: "warning",

      showCancelButton: true,

      confirmButtonText: "Eliminar",
      cancelButtonText: "Cancelar",

      reverseButtons: true,

      background: "#1e1e1e",
      color: "#f5f5f5",

      customClass: {

        popup: "swal-golahora-popup",

        title: "swal-golahora-title",

        htmlContainer: "swal-golahora-text",

        confirmButton: "swal-golahora-confirm",

        cancelButton: "swal-golahora-cancel"

      }

    });

      if (!result.isConfirmed) return;


      const card = document.querySelector(
        `.admin-card[data-id="${id}"]`
      );
        await eliminarEntrenamiento(id);

        card.remove();

    });

  });  
  
  
  }

  if(seccionAct == "Cobros y recibos")
    {
      const container = document.querySelector("#cobrosYrecibos");
       if (!container) {
        console.error("No existe el container #cobrosYrecibos");
        return;
      }
  
       if(seccionAct == "Descuentos")
       {
         window.location.href = "./descuentos.html";
       }

      container.innerHTML = renderAccionesCobro() + renderAccionesRecibo();
    }






  });

});

}

