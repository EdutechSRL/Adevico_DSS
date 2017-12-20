using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement.Business;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.CommunityManagement.Presentation
{
    public class StandardUnsubscriptionsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ServiceCommunityManagement _Service;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewStandardUnsubscriptionsList View
            {
                get { return (IViewStandardUnsubscriptionsList)base.View; }
            }
            private ServiceCommunityManagement Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceCommunityManagement(AppContext);
                    return _Service;
                }
            }
         
            public StandardUnsubscriptionsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public StandardUnsubscriptionsListPresenter(iApplicationContext oContext, IViewStandardUnsubscriptionsList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idProfile, List<dtoBaseCommunityNode> communityNodes)
        {
            if (UserContext.isAnonymous)
                View.LoadNothing();
            else
            {
                View.IdProfile = idProfile;
                LoadSubscriptions(idProfile, communityNodes);
            }
        }
       public void LoadSubscriptions(Int32 idProfile, List<dtoBaseCommunityNode> communityNodes)
       {
           List<dtoUserSubscription> list = Service.GetUserSubscriptions(idProfile, communityNodes);
           View.HasUnsubscriptions = (list.Count > 0);
          // View.LoadSubscriptions(list);
       }
    }
}