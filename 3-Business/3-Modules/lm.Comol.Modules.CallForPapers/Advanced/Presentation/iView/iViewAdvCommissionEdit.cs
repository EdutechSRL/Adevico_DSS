using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
{
    /// <summary>
    /// Vista per la modifica di una commissione
    /// </summary>
    public interface iViewAdvCommissionEdit : CallForPapers.Presentation.IViewBase
    {
        /// <summary>
        /// Id bando
        /// </summary>
        Int64 IdCall { get; set; }

        /// <summary>
        /// Id Commissione Avanzata
        /// </summary>
        Int64 IdComm { get; set; }

        /// <summary>
        /// Indica alla vista se il salvataggio è abilitato
        /// </summary>
        bool AllowSave { get; set; }
      
        /// <summary>
        /// Inizializza la vista
        /// </summary>
        /// <param name="CommissionData">Dati commissione</param>
        /// <param name="isManager">Se è managar</param>
        void Init(dto.dtoCommissionEdit CommissionData, bool isManager);

        /// <summary>
        /// Salva il verbale
        /// </summary>
        /// <param name="commission">Commissione, che sarà associata al verbale</param>
        /// <param name="moduleCode">Codice del modulo (Call For Peaper)</param>
        /// <param name="idModule">Id Modulo (Call for Peaper)</param>
        /// <param name="moduleAction">Azione modulo: (int)ModuleCallForPaper.ActionType</param>
        /// <param name="objectType">Tipo oggetto: (int)ModuleCallForPaper.ObjectType</param>
        /// <returns></returns>
        lm.Comol.Core.DomainModel.ModuleActionLink AddInternalFile(
                       Advanced.Domain.AdvCommission commission,
                       String moduleCode,
                       int idModule,
                       int moduleAction,
                       int objectType);

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
