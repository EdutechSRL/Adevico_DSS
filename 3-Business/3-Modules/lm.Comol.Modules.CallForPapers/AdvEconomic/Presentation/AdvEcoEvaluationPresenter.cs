using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.CallForPapers.Business;


using Eco = lm.Comol.Modules.CallForPapers.AdvEconomic;

using Sp = Telerik.Web.Spreadsheet;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Presentation
{
    /// <summary>
    /// Presenter valutazione economica
    /// </summary>
    public class AdvEcoEvaluationPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        /// <summary>
        /// Manager
        /// </summary>
        public virtual BaseModuleManager CurrentManager { get; set; }
        /// <summary>
        /// View: pagina di valutazione economica
        /// </summary>
        protected virtual Eco.Presentation.View.iViewEcoEvaluation View
        {
            get { return (Eco.Presentation.View.iViewEcoEvaluation)base.View; }
        }
        /// <summary>
        /// Servizio valutazioni
        /// </summary>
        private ServiceEvaluation _Service;
        /// <summary>
        /// Servizio valutazioni
        /// </summary>
        private ServiceEvaluation Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceEvaluation(AppContext);
                return _Service;
            }
        }
       /// <summary>
       /// Servizio Bandi
       /// </summary>
        private ServiceCallOfPapers _ServiceCall;
        /// <summary>
        /// Servizio bandi
        /// </summary>
        private ServiceCallOfPapers CallService
        {
            get
            {
                if (_ServiceCall == null)
                    _ServiceCall = new ServiceCallOfPapers(AppContext);
                return _ServiceCall;
            }
        }
        /// <summary>
        /// Servizio Richieste adesione
        /// </summary>
        private ServiceRequestForMembership _ServiceRequest;
        /// <summary>
        /// Servizio richieste di adesione
        /// </summary>
        private ServiceRequestForMembership RequestService
        {
            get
            {
                if (_ServiceRequest == null)
                    _ServiceRequest = new ServiceRequestForMembership(AppContext);
                return _ServiceRequest;
            }
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application Context</param>
        public AdvEcoEvaluationPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application context</param>
        /// <param name="view">vista: pagina valutazione economica</param>
        public AdvEcoEvaluationPresenter(iApplicationContext oContext, Eco.Presentation.View.iViewEcoEvaluation view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        /// <summary>
        /// Inizializzazione view
        /// </summary>
        public void InitView()
        {

            dto.dtoEcoEvaluation Eval = CallService.EcoEvaluationGet(View.CommId, View.EvalId);

            View.BindView(Eval);
        }

        #region Action management
        /// <summary>
        /// Aggiorna una valutazione
        /// </summary>
        /// <param name="items">Elenco di elementi da aggiornare</param>
        public void UpdateItems(IList<dto.dtoEcoEvlItem> items)
        {
            bool success = CallService.EcoEvalsUpdate(items);
            if (success)
                InitView();
        }
        /// <summary>
        /// [depreacto] Prensa in caricao di una valutazione
        /// </summary>
        public void TakeAssignment()
        {
            bool success = CallService.EcoEvalsAssignCurrent(View.EvalId);

            if (success)
                InitView();
        }
     
        /// <summary>
        /// Salva le valutazioni
        /// </summary>
        /// <param name="Items">Elementi da aggiornare</param>
        /// <param name="refresh">True per ricaricare i dati ed aggiornare la pagina</param>
        public void SaveEvaluation(IList<dto.dtoEcoEvlItem> Items, bool refresh)
        {
            bool success = CallService.EcoEvalsUpdate(Items);
            
            if (refresh && success)
                InitView();
        }
        /// <summary>
        /// Imposta come completata una valutazione economica
        /// </summary>
        public void CompleteEvaluation()
        {
            bool success = CallService.EcoEvalsChangeStatus(View.EvalId, EvalStatus.completed);

            if (success)
                InitView();
        }
        /// <summary>
        /// Rimette in bozza una valutazione economica
        /// </summary>
        public void DraftEvaluation()
        {
            bool success = CallService.EcoEvalsChangeStatus(View.EvalId, EvalStatus.draft);

            if (success)
                InitView();
        }
        /// <summary>
        /// Chiude la valutazione economica (conferma definitiva)
        /// </summary>
        public void CloseEvaluation()
        {
            bool success = CallService.EcoEvalsChangeStatus(View.EvalId, EvalStatus.confirmed);

            if (success)
                InitView();
        }
        #endregion

        /// <summary>
        /// Invio UserActions
        /// </summary>
        /// <param name="actionType">Tipo azione</param>
        /// <param name="objectType">Tipo oggetto</param>
        /// <param name="ObjectId">Id Oggetto</param>
        private void SendAction(
          CallForPapers.Domain.ModuleCallForPaper.ActionType actionType,
          CallForPapers.Domain.ModuleCallForPaper.ObjectType objectType,
          String ObjectId
          )
        {
            View.SendUserAction(
                UserContext.CurrentCommunityID,
                 CallService.ServiceModuleID(),
                 actionType,
                 objectType,
                 ObjectId);

        }




       
    }
}
