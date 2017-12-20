using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement.Business;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class SearchCommunitiesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            //private ServiceCommunityManagement _Service;
            private ProfileManagement.Business.ProfileManagementService _ProfileService;
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
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
            protected virtual IViewSearchCommunities View
            {
                get { return (IViewSearchCommunities)base.View; }
            }
            //private ServiceCommunityManagement Service
            //{
            //    get
            //    {
            //        if (_Service == null)
            //            _Service = new ServiceCommunityManagement(AppContext);
            //        return _Service;
            //    }
            //}
            private ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (_ProfileService == null)
                        _ProfileService = new ProfileManagement.Business.ProfileManagementService(AppContext);
                    return _ProfileService;
                }
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
            private lm.Comol.Core.Tag.Business.ServiceTags ServiceTags
            {
                get
                {
                    if (servicetag == null)
                        servicetag = new lm.Comol.Core.Tag.Business.ServiceTags(AppContext);
                    return servicetag;
                }
            }
            public SearchCommunitiesPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SearchCommunitiesPresenter(iApplicationContext oContext, IViewSearchCommunities view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitAdministrationView(Int32 idProfile, List<Int32> unloadIdCommunities, CommunityAvailability availability, List<Int32> onlyFromOrganizations)
        {
            View.SelectedIdCommunities = new List<Int32>();
            if (UserContext.isAnonymous)
                View.LoadNothing();
            else
            {
                View.isInitialized = true;
                View.AdministrationMode = true;
                InitializeView(true, idProfile, unloadIdCommunities, availability, onlyFromOrganizations);
            }
        }
        public void InitByModulesView(Int32 idProfile, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, CommunityAvailability availability, List<Int32> onlyFromOrganizations)
        {
            View.SelectedIdCommunities = new List<Int32>();
            if (UserContext.isAnonymous)
                View.LoadNothing();
            else
            {
                View.isInitialized = true;
                View.AdministrationMode = false;
                View.RequiredPermissions = requiredPermissions;
                InitializeView(false, idProfile, unloadIdCommunities, availability, onlyFromOrganizations, requiredPermissions);
            }
        }

        private void InitializeView(Boolean forAdministration, Int32 idProfile, List<Int32> unloadIdCommunities, CommunityAvailability availability, List<Int32> onlyFromOrganizations, Dictionary<Int32, long> requiredPermissions = null)
        {
            View.IdProfile = idProfile;
            litePerson currentUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            litePerson person = CurrentManager.GetLitePerson(idProfile);
            View.OnlyFromOrganizations = onlyFromOrganizations;
            View.SelectedIdCommunities = new List<int>();
            View.CurrentAvailability = availability;
            View.ExcludeCommunities = unloadIdCommunities;
            List<lm.Comol.Core.DomainModel.Filters.Filter> fToLoad = ServiceCommunities.GetDefaultFilters(idProfile, "",-1,-1,null, null, availability, -2, unloadIdCommunities, onlyFromOrganizations, requiredPermissions).OrderBy(f => f.DisplayOrder).ToList();
            View.LoadDefaultFilters(fToLoad, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations);
            if (fToLoad != null && fToLoad.Any())
            {
                dtoCommunitiesFilters filters = new lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters(fToLoad, availability);
                filters.AvailableIdOrganizations = onlyFromOrganizations;
                filters.OnlyFromAvailableOrganizations = (onlyFromOrganizations != null && onlyFromOrganizations.Any());
                filters.RequiredPermissions = (requiredPermissions==null)? new List<dtoModulePermission>() : requiredPermissions.Select(r=> new dtoModulePermission() { IdModule= r.Key, Permissions=r.Value}).ToList();
                LoadCommunities(idProfile,filters,unloadIdCommunities, 0, View.CurrentPageSize, false);
                View.HasAvailableCommunities = true;
            }
            else
                View.LoadNothing();
        }
        public void LoadCommunities(Int32 idProfile, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, List<Int32> unloadIdCommunities, Int32 pageIndex, Int32 pageSize, Boolean useCache = true)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<Int32> selectedItems = View.SelectedIdCommunities;
                Dictionary<Boolean, List<Int32>> selections = View.GetCurrentSelection();
                selectedItems = selectedItems.Except(selections[false]).ToList();
                selectedItems.AddRange(selections[true]);

                View.SelectedIdCommunities = selectedItems.Distinct().ToList();
                View.SearchFilters = filters;
                List<dtoCommunityPlainItem> items = ServiceCommunities.GetPlainCommunities(idProfile, filters,unloadIdCommunities, useCache);
                if (items == null)
                    View.LoadNothing();
                else
                {
                    Int32 itemsCount = items.Count();
                    PagerBase pager = new PagerBase();
                    pager.PageSize = pageSize;
                    pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;
                    pager.PageIndex = pageIndex;
                    View.Pager = pager;


                    items = ServiceCommunities.GetCommunities(items, pageIndex, pageSize, Core.Dashboard.Domain.OrderItemsBy.Name, true);

                    if (items != null)
                    {
                        Language l = CurrentManager.GetDefaultLanguage();
                        Dictionary<Int32, List<String>> tags = ServiceTags.GetCommunityAssociationToString(items.Select(i => i.Community.Id).ToList(), UserContext.Language.Id, l.Id, true);
                        if (tags.Any())
                        {
                            foreach (dtoCommunityPlainItem item in items.Where(i => tags.ContainsKey(i.Community.Id)))
                            {
                                item.Community.Tags = tags[item.Community.Id];
                            }
                        }
                        View.LoadItems(items);
                    }
                    else
                        View.LoadItems(new List<dtoCommunityPlainItem>());
                }
            }
        }

        //private List<dtoTreeCommunityNode> RetrieveItems()
        //{
          

        //    //if (filters == null)
        //    //    filters = View.GetCurrentFilters;
        //    //litePerson person = CurrentManager.GetLitePerson(View.IdProfile);
        //    //dtoTreeCommunityNode tree = Service.GetAllCommunitiesTree(person);
        //    //List<dtoTreeCommunityNode> nodes = GetAvailableNodes(tree.Filter(filters, 0).GetAllNodes(), View.ExcludeCommunities, View.OnlyFromOrganizations, View.RequiredPermissions);
        //    //// TO CHECK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //    //nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable && (n.IdOrganization == filters.IdOrganization || (filters.IdOrganization == -1 && !filters.OnlyFromAvailableOrganizations) || (filters.OnlyFromAvailableOrganizations && filters.AvailableIdOrganizations.Contains(n.IdOrganization)))).ToList();
        //    //return nodes;
        //    return new List<dtoTreeCommunityNode>();
        //}



        public List<dtoCommunityPlainItem> GetSelectedCommunities(List<Int32> idCommunities, List<Int32> unloadIdCommunities, Int32 idProfile)
        {
            List<Int32> selectedItems = View.SelectedIdCommunities;
            Dictionary<Boolean, List<Int32>> selections = View.GetCurrentSelection();
            selectedItems = selectedItems.Except(selections[false]).ToList();
            selectedItems.AddRange(selections[true]);

            lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters = new dtoCommunitiesFilters();
            filters.IdcommunityType = -1;
            filters.IdOrganization = -1;
            filters.IdCourseTime = -1;
            filters.IdDegreeType = -1;
            filters.Year = -1;
            filters.RequiredPermissions = View.RequiredPermissions.Select(r => new dtoModulePermission() { IdModule = r.Key, Permissions = r.Value }).ToList();
            filters.Availability = View.CurrentAvailability;
            filters.AvailableIdOrganizations=  View.OnlyFromOrganizations;

            List<dtoCommunityPlainItem> items = ServiceCommunities.GetPlainCommunities(idProfile, filters,unloadIdCommunities, true);
            return items.Where(i=> selectedItems.Contains(i.Community.Id)).ToList();
        }
        public void RefreshAdministrationView(List<Int32> unloadIdCommunities, CommunityAvailability availability)
        {
            View.SelectedIdCommunities = new List<Int32>();
            InitializeView(true, View.IdProfile, unloadIdCommunities, availability, View.OnlyFromOrganizations);
        }
        public void RefreshByModuleView(List<Int32> unloadIdCommunities, CommunityAvailability availability)
        {
            View.SelectedIdCommunities = new List<Int32>();
            InitializeView(false, View.IdProfile, unloadIdCommunities, availability, View.OnlyFromOrganizations, View.RequiredPermissions);
        }
    }
}