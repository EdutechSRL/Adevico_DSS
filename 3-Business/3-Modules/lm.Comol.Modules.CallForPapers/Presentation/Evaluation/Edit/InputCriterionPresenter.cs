using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Presentation.Evaluation;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public class InputCriterionPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewInputCriterion View
            {
                get { return (IViewInputCriterion)base.View; }
            }
            private ServiceCallOfPapers _ServiceCall;
            private ServiceCallOfPapers CallService
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
                }
            }
            public InputCriterionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public InputCriterionPresenter(iApplicationContext oContext, IViewInputCriterion view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idCall, long idSubmission, long idEvaluation, Int32 idCommunity, dtoCriterionEvaluated criterion, Boolean disabled)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.Disabled = disabled;
                View.IdCall = idCall;
                View.IdSubmission = idSubmission;
                View.IdCallCommunity = idCommunity;
                View.IdEvaluation = idEvaluation;
                if (criterion == null)
                    View.DisplayEmptyCriterion();
                else
                    View.SetupView(criterion, idCommunity);
            }
        }
    }
}