export function RenderBracket(cuadro){

   return `
   
      <div class="bracket-container">

         ${cuadro.fases.map(fase => `

            <div class="bracket-round">

               <h2 class="round-title">
                  ${fase.nombreFase}
               </h2>

               <div class="round-matches">

                  ${fase.partidos.map(p => `

                     <div class="match-card">

                        <div class="team-row">

                           <span class="team-name">
                              ${p.nombreLocal}
                           </span>

                           <span class="team-score">
                              ${p.golesLocal ?? "-"}
                           </span>

                        </div>

                        <div class="team-row">

                           <span class="team-name">
                              ${p.nombreVisitante}
                           </span>

                           <span class="team-score">
                              ${p.golesVis ?? "-"}
                           </span>

                        </div>

                        <div class="match-footer">

                           <span class="match-status ${p.estado.toLowerCase()}">
                              ${p.estado}
                           </span>

                        </div>

                     </div>

                  `).join("")}

               </div>

            </div>

         `).join("")}

      </div>

   `;
}