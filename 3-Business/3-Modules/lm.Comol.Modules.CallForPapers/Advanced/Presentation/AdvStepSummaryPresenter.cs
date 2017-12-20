using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation
{
    /// <summary>
    /// Presenter sommario Step
    /// </summary>
    public class AdvStepSummaryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {

        #region "Initialize"
        /// <summary>
        /// Manager
        /// </summary>
        public virtual BaseModuleManager CurrentManager { get; set; }
        /// <summary>
        /// View
        /// </summary>
        protected virtual Advanced.Presentation.iView.iViewAdvStepSummary View
        {
            get { return (Advanced.Presentation.iView.iViewAdvStepSummary)base.View; }
        }
        /// <summary>
        /// Service Evaluation
        /// </summary>
        private ServiceEvaluation _Service;
        /// <summary>
        /// Service Evaluation
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
        /// Service Call For Peaper
        /// </summary>
        private ServiceCallOfPapers _ServiceCall;
        /// <summary>
        /// Service Call For Peaper
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
        /// Service Richieste adesione
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
        /// Presenter
        /// </summary>
        /// <param name="oContext">Application context</param>
        public AdvStepSummaryPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Presenter
        /// </summary>
        /// <param name="oContext">Application Context</param>
        /// <param name="view">View: pagina</param>
        public AdvStepSummaryPresenter(iApplicationContext oContext, Advanced.Presentation.iView.iViewAdvStepSummary view)
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
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return;
            }

            GenericStepPermission permission = CallService.StepGetPermissionNoSubmitter(View.StepId, UserContext.CurrentUserID);


            if (permission == GenericStepPermission.none)
            {
                View.DisplayNoPermission(UserContext.CurrentCommunityID, CallService.ServiceModuleID());
                return;
            }

            CallService.StepEvUpdateAll(View.StepId);

            dto.dtoStepSummary summary = CallService.StepSummaryGet(View.StepId);

            View.BindNavigationUrl(UserContext.CurrentCommunityID, summary.CallId, summary.CommissionId);
            View.BindView(summary);
        }

        #region Action management
        /// <summary>
        /// Conferma le sottomissioni che superano lo step corrente
        /// </summary>
        /// <param name="passed">Elenco degli ID delle sottomissioni che sono state selezionate dal presidente</param>
        public void ConfirmAdmit(IList<Int64> passed)
        {
            bool success = CallService.CloseStep(passed, View.StepId);

            InitView();
        }
        #endregion

    }
}
