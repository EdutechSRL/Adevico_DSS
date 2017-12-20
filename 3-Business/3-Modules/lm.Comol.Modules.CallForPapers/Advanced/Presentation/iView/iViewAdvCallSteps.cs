using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Advanced.dto;

namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
{
    /// <summary>
    /// iView "processo di valutazione"
    /// </summary>
    public interface iViewAdvCallSteps : lm.Comol.Modules.CallForPapers.Presentation.IViewBase
    {
        /// <summary>
        /// Inizializzazione dati vista
        /// </summary>
        /// <param name="Steps">dto con l'elenco Step e relative commissioni</param>
        /// <param name="IsManager">Indica alla vista se l'utente corrente è Manager</param>
        /// <param name="CanShowSubmission">Indica alla vista se mostrare il tasto "visualizza tutte le sottomissioni"</param>
        void Initialize(dtoStepsEdit Steps, bool IsManager, bool CanShowSubmission);

        /// <summary>
        /// Inizializza la vista con "Bando non trovato"
        /// </summary>
        void ShowNoCall();

        /// <summary>
        /// Invia azione utente
        /// </summary>
        /// <param name="CommunityId">Id comunità</param>
        /// <param name="ModuleId">Id modulo</param>
        /// <param name="action">Tipo azione</param>
        /// <param name="objectType">Tipo oggetto</param>
        /// <param name="ObjectId">Id oggetto</param>
        void SendUserAction(
            int CommunityId,
            int ModuleId,
            lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType action,
            lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType objectType,
            string ObjectId
            );
    }
}
