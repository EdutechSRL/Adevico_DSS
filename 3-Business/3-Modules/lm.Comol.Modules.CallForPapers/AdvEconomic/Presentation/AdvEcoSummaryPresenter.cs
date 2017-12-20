using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.CallForPapers.Business;

using Eco = lm.Comol.Modules.CallForPapers.AdvEconomic;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Presentation
{
    /// <summary>
    /// Presenter sommario valutazione economica
    /// </summary>
    public class AdvEcoSummaryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        /// <summary>
        /// Manager
        /// </summary>
        public virtual BaseModuleManager CurrentManager { get; set; }
        /// <summary>
        /// View
        /// </summary>
        protected virtual Eco.Presentation.View.iViewEcoSummary View
        {
            get { return (Eco.Presentation.View.iViewEcoSummary)base.View; }
        }
        /// <summary>
        /// Service valutazioni
        /// </summary>
        private ServiceEvaluation _Service;
        /// <summary>
        /// Service valutazioni
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
        //private ServiceDss _ServiceDss;
        //private ServiceDss ServiceDss
        //{
        //    get
        //    {
        //        if (_ServiceDss == null)
        //            _ServiceDss = new ServiceDss(AppContext);
        //        return _ServiceDss;
        //    }
        //}
        /// <summary>
        /// Service Bandi
        /// </summary>
        private ServiceCallOfPapers _ServiceCall;
        /// <summary>
        /// Service Bandi
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
        /// Service richieste adesione
        /// </summary>
        private ServiceRequestForMembership _ServiceRequest;
        /// <summary>
        /// Service richieste adesione
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
        public AdvEcoSummaryPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application Context</param>
        /// <param name="view">Vista: pagina sommario valutazioni economiche</param>
        public AdvEcoSummaryPresenter(iApplicationContext oContext, Eco.Presentation.View.iViewEcoSummary view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion
        /// <summary>
        /// Inizializzazione View
        /// </summary>
        public void InitView()
        {
            Eco.dto.dtoEcoSummaryContainer summary = CallService.EcoSummaryGet(View.CommissionId);


            if (summary.hasPermission)
            {
                View.BindView(summary);
                SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.AdvStepSummary, CallForPapers.Domain.ModuleCallForPaper.ObjectType.AdvStepEcoSummary, View.CommissionId.ToString());
            }
            else
            { 
                View.DisplayNoPermission(UserContext.CurrentCommunityID, 0);
                SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, CallForPapers.Domain.ModuleCallForPaper.ObjectType.AdvStepEcoSummary, View.CommissionId.ToString());
            }
        }

        #region Action management
        /// <summary>
        /// Chiusura definitiva della commissione economica
        /// </summary>
        public void CloseCommission()
        {
            bool success = CallService.CommissionClose(View.CommissionId);

            if (success)
            {
                InitView();
                SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.AdvStepClose, CallForPapers.Domain.ModuleCallForPaper.ObjectType.AdvStepEcoSummary, View.CommissionId.ToString());
            } else
            {
                SendAction(CallForPapers.Domain.ModuleCallForPaper.ActionType.GenericError, CallForPapers.Domain.ModuleCallForPaper.ObjectType.AdvStepEcoSummary, View.CommissionId.ToString());
            }
                
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
                 CallService.ServiceModuleID(),
                 actionType,
                 objectType,
                 ObjectId);

        }
    }
}
