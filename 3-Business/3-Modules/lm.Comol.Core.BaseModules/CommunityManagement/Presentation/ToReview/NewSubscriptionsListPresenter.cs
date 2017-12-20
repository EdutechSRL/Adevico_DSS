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
    public class NewSubscriptionsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewNewProfileSubscriptionsList View
            {
                get { return (IViewNewProfileSubscriptionsList)base.View; }
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
         
            public NewSubscriptionsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public NewSubscriptionsListPresenter(iApplicationContext oContext, IViewNewProfileSubscriptionsList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> communityNodes)
        {
            if (UserContext.isAnonymous)
                View.LoadNothing();
            else
                LoadSubscriptions(communityNodes);
        }
        public void LoadSubscriptions(List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> communityNodes)
        {
            List<dtoNewProfileSubscription> enrollements = Service.GetNewUserSubscriptions(communityNodes);
            View.HasAvailableSubscriptions = (enrollements.Count > 0);
            View.LoadSubscriptions(enrollements.OrderBy(e => e.Name).ToList(), Service.GetRoleCommunityTemplates(enrollements.Select(i => i.Node.Community.IdType).Distinct().ToList()),CurrentManager.GetTranslatedRoles((UserContext.Language==null ? CurrentManager.GetDefaultIdLanguage() : UserContext.Language.Id)).ToDictionary(r=> r.Id, r=> r.Name));
        }
    }
}