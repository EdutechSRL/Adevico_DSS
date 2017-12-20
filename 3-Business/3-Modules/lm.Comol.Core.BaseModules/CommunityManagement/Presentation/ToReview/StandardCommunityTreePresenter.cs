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
    public class StandardCommunityTreePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewStandardCommunityTree View
            {
                get { return (IViewStandardCommunityTree)base.View; }
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
         
            public StandardCommunityTreePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public StandardCommunityTreePresenter(iApplicationContext oContext, IViewStandardCommunityTree view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idProfile, CommunityAvailability preloadedAvailability)
        {
            if (UserContext.isAnonymous)
                View.LoadNothing();
            else
            {
                View.IdProfile = idProfile;
                Person person = CurrentManager.GetPerson(idProfile);
                List<CommunityStatus> status = Service.GetCommunitiesAvailableStatus();
                if (status.Count > 0)
                {
                    List<Int32> types = Service.GetCommunitiesAvailableTypes(status[0]);
                    View.InitializeTree(status, types, GetCommunityAvailability(idProfile));
                    if (preloadedAvailability == CommunityAvailability.None)
                        preloadedAvailability = CommunityAvailability.NotSubscribed;
                    View.CurrentAvailability = preloadedAvailability;
                    LoadCommunities(View.CommunityFilters);
                }
                else 
                    View.LoadNothing();
            }
        }
        public void ChangeStatus(CommunityStatus status) {
            View.LoadTypes(Service.GetCommunitiesAvailableTypes(status));
            LoadCommunities(View.CommunityFilters);
        }

        public void LoadCommunities(dtoCommunitiesFilters filters)
        {
            //if (person == null)
            //{
            //    View.DisplayProfileUnknown();
            //    View.HasAvailableCommunities = false;
            //}
            //else
            //{
            //    dtoCommunityNode tree = ServiceCommunity.GetFilteredCommunitiesTree(View.CurrentStatus, person);
            //    View.HasAvailableCommunities = (tree.Nodes.Count > 0);
            //    View.LoadTree(tree);
            //}
            dtoTreeCommunityNode tree = Service.GetFilteredCommunitiesTree(filters, CurrentManager.GetLitePerson(View.IdProfile));
            View.HasAvailableCommunities = (tree.Nodes.Count > 0);
            View.LoadTree(tree);
        }

        private List<CommunityAvailability> GetCommunityAvailability(Int32 idProfile)
        {
            List<CommunityAvailability> availabilities = new List<CommunityAvailability>();
            if (idProfile > 0) {
                availabilities.Add(CommunityAvailability.All);
                availabilities.Add(CommunityAvailability.Subscribed);
                availabilities.Add(CommunityAvailability.NotSubscribed);
            }
            else
                availabilities.Add(CommunityAvailability.NotSubscribed);

            return availabilities;
        }
    }
}