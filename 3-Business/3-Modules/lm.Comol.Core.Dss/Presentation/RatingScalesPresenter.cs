using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dss.Business;
using lm.Comol.Core.Dss.Domain;
using lm.Comol.Core.Dss.Domain.Templates;
using LM.MathLibrary.Algorithms;

namespace lm.Comol.Core.Dss.Presentation.Evaluation
{
    public class RatingScalesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewRatingSetSelector View
            {
                get { return (IViewRatingSetSelector)base.View; }
            }
            private ServiceDss _Service;
            private ServiceDss Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceDss(AppContext);
                    return _Service;
                }
            }
            public RatingScalesPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RatingScalesPresenter(iApplicationContext oContext, IViewRatingSetSelector view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoSelectMethod method, Boolean disabled){
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.Disabled = disabled;
                if (method != null)
                {
                    View.IsFuzzy = method.IsFuzzy;
                    View.LoadRatingSet(Service.RatingSetGetAvailable(method.Id, UserContext.Language.Id, CurrentManager.GetDefaultIdLanguage()));
                }
            }
        }
        public void InitView(AlgorithmType type,Boolean isFuzzy, Boolean disabled)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.Disabled = disabled;
                List<long> idMethods = Service.MethodGetIdAvailable(type, isFuzzy);
                if (idMethods.Any())
                {
                    View.IsFuzzy = isFuzzy;
                    View.LoadRatingSet(Service.RatingSetGetAvailable(idMethods.FirstOrDefault(), UserContext.Language.Id, CurrentManager.GetDefaultIdLanguage()));
                }
            }
        }
        public Dictionary<long,List<dtoGenericRatingValue>> GetRatingSetValues(List<long> idItems)
        {
            return idItems.ToDictionary(i=> i, i=> Service.RatingValuesGet(i, UserContext.Language.Id, CurrentManager.GetDefaultIdLanguage()));
        }
    }
}