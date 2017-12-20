using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.CallForPapers.Business;

namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation
{
    /// <summary>
    /// Presenter CallStep
    /// </summary>
    public class AdvCallStepsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region Initialize

        /// <summary>
        /// Manager
        /// </summary>
        public virtual BaseModuleManager CurrentManager { get; set; }
        /// <summary>
        /// View
        /// </summary>
        protected virtual iView.iViewAdvCallSteps View
        {
            get { return (iView.iViewAdvCallSteps)base.View; }
        }

        /// <summary>
        /// Service CFP
        /// </summary>
        private ServiceCallOfPapers _Service;
        /// <summary>
        /// Service CFP
        /// </summary>
        private ServiceCallOfPapers Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceCallOfPapers(AppContext);
                return _Service;
            }
        }
        /// <summary>
        /// Presenter
        /// </summary>
        /// <param name="oContext">Application context</param>
        public AdvCallStepsPresenter(iApplicationContext oContext) : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Presenter
        /// </summary>
        /// <param name="oContext">Application Context</param>
        /// <param name="view">Pagina</param>
        /// <remarks>Usare questo inizializzatore</remarks>
        public AdvCallStepsPresenter(iApplicationContext oContext, iDomainView view) : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Modulo CallForPeaper
        /// </summary>
         lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper _module = null;
        private CallForPapers.Domain.ModuleCallForPaper CallModule()
        {
            if(_module == null)
                _module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, UserContext.CurrentCommunityID);

            return _module;
        }

        #endregion

        #region Helper permission

        /// <summary>
        /// Indica se l'utente è sottomittore
        /// </summary>
        /// <returns></returns>
        private bool isSubmitter()
        {
            return CallModule().AddSubmission;
        }
        /// <summary>
        /// Indica se l'utente è manager (permessi di comunità)
        /// </summary>
        /// <returns></returns>
        private bool isManager()
        {

            //module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, UserContext.CurrentCommunityID);
            bool allowManage = CallModule().CreateCallForPaper 
                || CallModule().Administration 
                || CallModule().ManageCallForPapers 
                || CallModule().EditCallForPaper;

            return allowManage;
        }
        #endregion

        /// <summary>
        /// Inizializzazione view.
        /// </summary>
        /// <param name="callId">Id Bando</param>
        public void InitView(Int64 callId)
        {
            if(UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return;
            }


            if (!isManager())
            {
                bool isEvaluator = Service.UserIsInCallCommission(callId, UserContext.CurrentUserID);
                bool isCallAvailable = Service.IsCallAvailableByUser(callId, UserContext.CurrentUserID);
                
                if (!isSubmitter() && !isEvaluator && !isCallAvailable)
                {
                    SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, callId.ToString());
                    View.DisplayNoPermission(UserContext.CurrentCommunityID, Service.ServiceModuleID());
                    return;
                }
                //if(!)
                //{
                //    View.DisplayNoPermission(UserContext.CurrentCommunityID, Service.ServiceModuleID());
                //    return;
                //} 
            }

            dto.dtoStepsEdit steps = Service.StepContainerGet(callId);

            if (steps == null || steps.CallId <= 0)
            {
                SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.None, CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, "-1");
                View.ShowNoCall();
            }                
            else
            {
                bool showSubmission = isManager() || Service.UserIsInCallCommissionViewSubmission(callId, UserContext.CurrentUserID);
                View.Initialize(steps, isManager(), showSubmission);
                SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.AdvStepsView, CallForPapers.Domain.ModuleCallForPaper.ObjectType.AdvStep, callId.ToString());
            }
        }


        #region Action management

        /// <summary>
        /// Aggiunge uno step all'iter valutativo
        /// </summary>
        /// <param name="callId">Id Bando</param>
        public void AddStep(Int64 callId)
        {
            Int64 NewStepId = Service.StepAdd(callId, "Step", "Commissione");
            if (NewStepId > 0)
            {
                this.InitView(callId);
            }

            SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.AdvStepAdd, CallForPapers.Domain.ModuleCallForPaper.ObjectType.AdvStep, NewStepId.ToString());
        }

        /// <summary>
        /// Cancella uno step: NON USATO
        /// </summary>
        /// <param name="callId">Id Bando</param>
        /// <param name="StepId">Id step da cancellare</param>
        /// <remarks>
        /// DEPRECATO: uno step viene automaticamente cancellato alla cancellazione di tutte le commissioni presenti.
        /// </remarks>
        public void DelStep(Int64 callId, Int64 StepId)
        {
            bool success = Service.StepDelete(callId, StepId);
            if (success)
                this.InitView(callId);
        }
        /// <summary>
        /// Aggiunge una commissione
        /// </summary>
        /// <param name="callId">Id Bando</param>
        /// <param name="CommId">Id commissione da cui aggiungere una sorella</param>
        /// <remarks>
        /// Aggingendo uno step viene in automatico creata anche una commissione.
        /// La creazione di una nuova commissione parte da questa o da altre commissioni
        /// </remarks>
        public void AddCommission(Int64 callId, Int64 CommId)
        {
            long NewCommId = Service.CommissionAdd(CommId, "Commissione");

            if (NewCommId > 0)
            {
                this.InitView(callId);
                
            }

            SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.AdvCommissionAdd, CallForPapers.Domain.ModuleCallForPaper.ObjectType.AdvCommission, NewCommId.ToString());

        }
        /// <summary>
        /// Cancellazione commissione
        /// </summary>
        /// <param name="callId">Id Bando</param>
        /// <param name="CommId">Id Commissione da cancellare</param>
        /// <remarks>
        /// Se la commissione è l'unica presente per lo step, lo step viene cancellato in automatico.
        /// </remarks>
        public void DelCommission(Int64 callId, Int64 CommId)
        {
            bool success = Service.CommissionDelete(CommId);

            if (success)
                this.InitView(callId);

        }

        /// <summary>
        /// Riordino STEP
        /// </summary>
        /// <param name="CallId">Id bando</param>
        /// <param name="Orders">Lista di chiave/valore con l'id dello step e l'indice di ordinamento dello step.</param>
        public void Reorder(Int64 CallId, IList<KeyValuePair<Int64, int>> Orders)
        {
            if (Orders == null || !Orders.Any())
                return;

            bool success = Service.StepReorder(Orders);

            if (success)
                this.InitView(CallId);



        }

        #endregion

        private void SendAction(
          CallForPapers.Domain.ModuleCallForPaper.ActionType actionType,
          CallForPapers.Domain.ModuleCallForPaper.ObjectType objectType,
          String ObjectId
          )
        {
            View.SendUserAction(
                UserContext.CurrentCommunityID,
                 Service.ServiceModuleID(),
                 actionType,
                 objectType,
                 ObjectId);

        }

    }
}
