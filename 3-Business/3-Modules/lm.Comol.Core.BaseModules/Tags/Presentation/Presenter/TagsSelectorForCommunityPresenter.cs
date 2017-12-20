using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Tag.Domain;
using lm.Comol.Core.BaseModules.Tags.Business;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    public class TagsSelectorForCommunityPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            private ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTagsSelectorForCommunity View
            {
                get { return (IViewTagsSelectorForCommunity)base.View; }
            }
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceCommunities
            {
                get
                {
                    if (serviceCommunities == null)
                        serviceCommunities = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return serviceCommunities;
                }
            }
            private ServiceTags ServiceTags
            {
                get
                {
                    if (servicetag == null)
                        servicetag = new ServiceTags(AppContext);
                    return servicetag;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleTags.UniqueCode);
                    return currentIdModule;
                }
            }
            public TagsSelectorForCommunityPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TagsSelectorForCommunityPresenter(iApplicationContext oContext, IViewTagsSelectorForCommunity view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitViewForCommunity(Int32 idCommunity)
        {
            InitializeView(idCommunity, ServiceCommunities.GetAllAvailableOrganizations(idCommunity));
        }
        public void InitViewForCommunityToAdd(Int32 idFatherCommunity, Int32 idCommunityType)
        {
            InitializeView(0, ServiceCommunities.GetAllAvailableOrganizations(idFatherCommunity), idCommunityType);
        }
        public void InitViewForOrganization(Int32 idOrganization, Int32 idCommunityType = -1)
        {
            InitializeView(0, new List<Int32>() { idOrganization }, idCommunityType);
        }
        private void InitializeView(Int32 idCommunity, List<Int32> idOrganizations, Int32 idCommunityType = -1)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                View.IdCommunityToApply = idCommunity;
                View.IdOrganizations = idOrganizations;
                View.LoadTags(ServiceTags.GetTags(TagType.Community, idCommunity, idOrganizations, idCommunityType));
            }
        }

        public void ReloadTags(Int32 idCommunity, Int32 idOrganization, Int32 idCommunityType, List<long> selectedTags)
        {
            View.IdOrganizations = new List<int>() { idOrganization };
            View.LoadTags(ServiceTags.GetTags(TagType.Community, idCommunity, new List<int>() { idOrganization }, idCommunityType, selectedTags));
        }

        public void ReloadTags(Int32 idCommunity, List<Int32> idOrganizations, Int32 idCommunityType, List<long> selectedTags)
        {
            View.LoadTags(ServiceTags.GetTags(TagType.Community, idCommunity, idOrganizations, idCommunityType, selectedTags));
        }
        public void ApplyTags(List<long> selectedTags)
        {
            ApplyTags(new List<Int32>() { View.IdCommunityToApply }, selectedTags);
        }
        public void ApplyTags(Int32 idCommunity,List<long> selectedTags)
        {
            ApplyTags(new List<Int32>() { idCommunity }, selectedTags);
        }
        public void ApplyTags(List<Int32> idCommunities, List<long> selectedTags)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ServiceTags.ApplyTagsToCommunities(idCommunities, selectedTags);
            }
        }
    }
}