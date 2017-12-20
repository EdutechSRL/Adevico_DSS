using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.CommunityManagement.Business;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class MailProfilePolicyPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
            private ServiceCommunityManagement _ServiceCommunity;
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
            protected virtual IViewMailProfilePolicy View
            {
                get { return (IViewMailProfilePolicy)base.View; }
            }
            private ServiceCommunityManagement ServiceCommunity
            {
                get
                {
                    if (_ServiceCommunity == null)
                        _ServiceCommunity = new ServiceCommunityManagement(AppContext);
                    return _ServiceCommunity;
                }
            }
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            public MailProfilePolicyPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MailProfilePolicyPresenter(iApplicationContext oContext, IViewMailProfilePolicy view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(Int32 idProfile)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    View.IdProfile = idProfile;
                    Person person = CurrentManager.GetPerson(idProfile);
                    List<lm.Comol.Core.Communities.CommunityStatus> status = ServiceCommunity.GetCommunitiesAvailableStatus(idProfile);
                    if (status.Count > 0)
                    {
                        View.AvailableStatus= status;
                        LoadCommunities(View.CommunityFilters);
                    }
                    else
                        View.LoadNothing();
                }
            }

            public void LoadCommunities(dtoCommunitiesFilters filters)
            {
                Int32 idProfile = View.IdProfile;
                litePerson person = CurrentManager.GetLitePerson(View.IdProfile);
                dtoTreeCommunityNode tree = ServiceCommunity.GetGenericCommunitiesTree(filters, person);
                List<Int32> list = (from m in CurrentManager.GetIQ<LazySubscription>()
                                    where m.IdPerson == idProfile && m.DisplayMail == true
                                    && m.IdRole > 0
                                    select m.IdCommunity).ToList();
                View.LoadTree(tree, list);               
            }

            public Boolean SavePolicy(List<Int32> idSelected, List<Int32> previousCommunitiesPolicy) {
                return Service.SaveProfileMailPolicy(View.IdProfile,idSelected, previousCommunitiesPolicy);
            }
    }
}