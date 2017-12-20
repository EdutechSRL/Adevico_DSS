using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public class RenderCriterionPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewRenderCriterion View
            {
                get { return (IViewRenderCriterion)base.View; }
            }
            private ServiceEvaluation _Service;
            private ServiceEvaluation CallService
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceEvaluation(AppContext);
                    return _Service;
                }
            }
            public RenderCriterionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RenderCriterionPresenter(iApplicationContext oContext, IViewRenderField view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoCriterionEvaluated criterion, Boolean disabled, Boolean isPublic)
        {
            if (UserContext.isAnonymous && !isPublic)
                View.DisplaySessionTimeout();
            else
            {
                View.Disabled = disabled;
                if (criterion == null)
                    View.DisplayEmptyCriterion();
                else
                    View.SetupView(criterion, isPublic);
            }
        }
    }
}