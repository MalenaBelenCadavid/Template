export function CrearModalBracket(cuadro){

   return `
   
      <div class="modal-overlay" id="modal-bracket">

         <div class="modal-content modal-bracket">

            <div class="modal-header">

               <h2>Cuadro del torneo</h2>

               <button class="cerrar-modal">
                  ✕
               </button>

            </div>

            <div class="modal-body">

               ${RenderBracket(cuadro)}

            </div>

         </div>

      </div>
   `;
}