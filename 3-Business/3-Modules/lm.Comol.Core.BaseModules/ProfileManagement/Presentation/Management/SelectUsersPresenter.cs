using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;
using lm.Comol.Core.DomainModel.Domain;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class SelectUsersPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement _ServiceCommunity;
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
            protected virtual IViewSelectUsers View
            {
                get { return (IViewSelectUsers)base.View; }
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
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement ServiceCommunity
            {
                get
                {
                    if (_ServiceCommunity == null)
                        _ServiceCommunity = new lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(AppContext);
                    return _ServiceCommunity;
                }
            }
            public SelectUsersPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SelectUsersPresenter(iApplicationContext oContext, IViewSelectUsers view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(UserSelectionType loadMode, Boolean fromAllMyCommunity, List<Int32> unloadIdUsers, List<Int32> selectedIdUsers)
        {
            InitView(loadMode, fromAllMyCommunity, new List<Int32>(), unloadIdUsers, selectedIdUsers);
        }
        public void InitView(UserSelectionType loadMode, Boolean fromAllMyCommunity, Int32 idCommunity, List<Int32> unloadIdUsers, List<Int32> selectedIdUsers)
        {
            List<Int32> idCommunities = new List<Int32>(){ idCommunity};
            InitView(loadMode, fromAllMyCommunity, idCommunities, unloadIdUsers, selectedIdUsers);
        }
        public void InitView(UserSelectionType loadMode, Boolean fromAllMyCommunity, List<Int32> idCommunities, List<Int32> unloadIdUsers, List<Int32> selectedIdUsers)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (unloadIdUsers == null)
                    unloadIdUsers = new List<Int32>();
                if (selectedIdUsers == null)
                    selectedIdUsers = new List<Int32>();
                View.isInitialized = true;
                View.FromAllMyCommunity = fromAllMyCommunity;
                View.FromCommunities = (fromAllMyCommunity) ? new List<Int32>() : idCommunities;
                View.UnavailableIdUsers = unloadIdUsers;
                View.SelectAllUsers = false;
                View.SelectedIdUsers = (selectedIdUsers == null) ? new List<int>() : selectedIdUsers.Where(s => !unloadIdUsers.Contains(s)).ToList();
                View.IsFirstLoad = true;
                View.IsFirstPreviewLoad = true;
                switch (loadMode) { 
                    case UserSelectionType.SystemUsers:
                        ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                        InitializeProfileFilters(module);
                        break;
                    case UserSelectionType.CommunityUsers:
                        if (idCommunities.Any()){
                            if (idCommunities.Count==1){
                                ModuleCommunityManagement cModule = ServiceCommunity.GetModulePermission(UserContext.CurrentUserID, idCommunities[0]);
                                InitializeCommunityFilters(cModule, idCommunities[0], unloadIdUsers);
                            }
                        }
                        break;
                }
            }
        }

        #region "Profiles view"
            #region "Manage Filters"
                private void InitializeProfileFilters(ModuleProfileManagement module)
                {
                    View.OrderAscending = true;
                    View.OrderProfilesBy = OrderProfilesBy.SurnameAndName;
                    Int32 idDefaultProfileType = 0;
                    long idDefaultAgency = 0;
                    long idDefaultProvider = 0;
                    Int32 idDefaultOrganization = 0;
                    StatusProfile defaultStatus = StatusProfile.Active;
                    SearchProfilesBy defaultSearch = SearchProfilesBy.Contains;

                    List<Organization> organizations = Service.GetAvailableOrganizations(UserContext.CurrentUserID, (module.ViewProfiles || module.Administration) ? SearchCommunityFor.SystemManagement : SearchCommunityFor.CommunityManagement);
                    if (organizations.Any() || module.Administration || module.ViewProfiles)
                    {
                        View.LoadAvailableOrganizations(organizations, idDefaultOrganization);
                        View.LoadProfileTypes(Service.GetAvailableProfileTypes(View.SelectedIdOrganization), idDefaultProfileType);

                        List<dtoBaseProvider> providers = Service.GetAuthenticationProviders(UserContext.Language.Id, true).Where(p => p.isEnabled).ToList();

                        View.LoadAuthenticationProviders(providers, idDefaultProvider);

                        View.LoadAvailableStatus(Service.GetAvailableStatus(View.SelectedIdOrganization, View.SelectedIdProfileType), defaultStatus);
                        View.LoadSearchProfilesBy(GetSearchByItems(module), defaultSearch, UserSelectionType.SystemUsers);
                        if (idDefaultProfileType == (int)UserTypeStandard.Employee)
                            View.LoadAgencies(Service.GetAvailableAgencies(View.SelectedIdOrganization), idDefaultAgency, UserSelectionType.SystemUsers);

                        dtoFilters filter = View.GetCurrentProfileFilters;
                        View.SearchProfileFilters = filter;
                        View.InitializeWordSelector(Service.GetAvailableStartLetter(filter, View.UnavailableIdUsers));
                        SetDefaultColumns(filter.IdProfileType, module.Administration || module.ViewProfiles, filter.Status);
                        View.LoadProfiles(new List<dtoProfileItem<dtoBaseProfile>>());
                    }
                    else
                        View.NoPermission();
                    
                }
                private List<SearchProfilesBy> GetSearchByItems(ModuleProfileManagement module)
                {
                    List<SearchProfilesBy> list = new List<SearchProfilesBy>();
                    list.Add(SearchProfilesBy.All);
                    list.Add(SearchProfilesBy.Contains);
                    list.Add(SearchProfilesBy.Name);
                    list.Add(SearchProfilesBy.Surname);
                    if (module.Administration || module.ViewProfiles)
                        list.Add(SearchProfilesBy.Mail);
                    if (View.AllowSearchByTaxCode && (module.Administration || module.ViewProfiles))
                        list.Add(SearchProfilesBy.TaxCode);

                    return list;
                }

                public void ChangeOrganization(Int32 idOrganization, Int32 idProfileType, int currentPageIndex, int currentPageSize)
                {
                    View.LoadProfileTypes(Service.GetAvailableProfileTypes(idOrganization), idProfileType);
                    idProfileType = View.SelectedIdProfileType;
                    View.LoadAvailableStatus(Service.GetAvailableStatus(idOrganization, idProfileType), View.SelectedProfileStatus);
                    View.LoadSearchProfilesBy(GetSearchByItems(ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID)), View.SelectedSearchBy, UserSelectionType.SystemUsers);
                    LoadProfiles(currentPageIndex, currentPageSize,true);
                }
                public void ChangeProfileType(Int32 idOrganization, Int32 idProfileType, int currentPageIndex, int currentPageSize)
                {
                    View.LoadAvailableStatus(Service.GetAvailableStatus(idOrganization, idProfileType), View.SelectedProfileStatus);
                    View.LoadSearchProfilesBy(GetSearchByItems(ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID)), View.SelectedSearchBy, UserSelectionType.SystemUsers);
                    idProfileType = View.SelectedIdProfileType;
                    if (idProfileType == (int)UserTypeStandard.Employee)
                        View.LoadAgencies(Service.GetAvailableAgencies(View.SelectedIdOrganization), 0, UserSelectionType.SystemUsers);

                    LoadProfiles(currentPageIndex, currentPageSize, true);
                }
                public void ChangeProfileAgency(int currentPageIndex, int currentPageSize)
                {
                    LoadProfiles(currentPageIndex, currentPageSize, true);
                }
            #endregion

            public void SearchProfiles(int currentPageIndex, int currentPageSize)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                    LoadProfiles(currentPageIndex, currentPageSize, true);
            }
            private List<ProfileColumn> SetDefaultColumns(Int32 IdProfileType, Boolean displayMail,StatusProfile status)
            {
                List<ProfileColumn> columns = new List<ProfileColumn>();
                columns.Add(ProfileColumn.name);
                if (displayMail)
                    columns.Add(ProfileColumn.mail);
                if (IdProfileType<=0)
                    columns.Add(ProfileColumn.type);
                if (IdProfileType == (int)UserTypeStandard.Company)
                    columns.Add(ProfileColumn.companyName);
                if (IdProfileType == (int)UserTypeStandard.Employee)
                    columns.Add(ProfileColumn.agency);
                if (status == StatusProfile.AllStatus || status== StatusProfile.None)
                    columns.Add(ProfileColumn.status);
                View.AvailableColumns = columns;
                return columns;
            }
            public void LoadProfiles(int currentPageIndex, int currentPageSize, Boolean updateSelection)
            {
                List<Int32> idRemoveUsers = View.UnavailableIdUsers;
                dtoFilters filters = View.SearchProfileFilters;
                PagerBase pager = new PagerBase();

                //if (filters.StartWith != View.CurrentStartWith)
                //    filters.StartWith = View.CurrentStartWith;

                pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
                pager.Count = (int)Service.ProfilesCount(filters, idRemoveUsers) - 1;
                pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;
                View.IsFirstLoad = false;
                
                //List<lm.Comol.Core.DomainModel.Helpers.AlphabetItem> aItems = ;
                //if (aItems.Where(i => i.Value == View.CurrentStartWith && i.isEnabled).Any())
                //    aItems.Where(i => i.Value == View.CurrentStartWith && i.isEnabled).FirstOrDefault().isSelected = true;
                //else if (aItems.Where(i => i.Value == View.CurrentStartWith && !i.isEnabled).Any())
                //{
                //    aItems.Where(i => i.Type == DomainModel.Helpers.AlphabetItemType.all).FirstOrDefault().isSelected = true;
                //    View.CurrentStartWith= "";
                //    filters.StartWith = "";
                //}
                if (updateSelection)
                    View.SelectedIdUsers = UpdateItemsSelection();
                View.InitializeWordSelector(Service.GetAvailableStartLetter(filters, idRemoveUsers), filters.StartWith);
                filters.StartWith = View.CurrentStartWith;
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);

                List<dtoBaseProvider> providers = Service.GetAuthenticationProviders(UserContext.Language.Id, true);
                List<ProfileColumn> columns = SetDefaultColumns(filters.IdProfileType, module.Administration || module.ViewProfiles, filters.Status);

                if (View.SelectedIdProfileType != (int)UserTypeStandard.Employee)
                    View.UnLoadAgencies();

                switch (filters.IdProfileType)
                {
                    case (int)UserTypeStandard.Company:
                        List<dtoProfileItem<dtoCompany>> companyUsers = Service.GetCompanyUserProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers, idRemoveUsers);
                        if (companyUsers.Where(i => i.Id == UserContext.CurrentUserID).Any())
                            companyUsers.Where(i => i.Id == UserContext.CurrentUserID).ToList().ForEach(i => i.Permission.LogonAs = false);
                        View.LoadProfiles(companyUsers);
                        break;
                    case (int)UserTypeStandard.Employee:
                        List<dtoProfileItem<dtoEmployee>> employeeUsers = Service.GetEmployeeProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers, idRemoveUsers);
                        if (employeeUsers.Where(i => i.Id == UserContext.CurrentUserID).Any())
                            employeeUsers.Where(i => i.Id == UserContext.CurrentUserID).ToList().ForEach(i => i.Permission.LogonAs = false);
                        View.LoadProfiles(employeeUsers);
                        break;
                    //case (int)UserTypeStandard.ExternalUser:
                    //    List<dtoProfileItem<dtoExternal>> externalUsers = Service.GetEmployeeProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers);
                    //    if (!columns.Contains(ProfileColumn.login))
                    //        View.AvailableLogins = Service.GetProfilesLogin(externalUsers.Select(i => i.Profile.Id).ToList());
                    //    if (externalUsers.Where(i => i.Id == UserContext.CurrentUserID).Any())
                    //        externalUsers.Where(i => i.Id == UserContext.CurrentUserID).ToList().ForEach(i => i.Permission.LogonAs = false);
                    //    View.LoadProfiles(externalUsers);
                    //    break;
                    //case (int)UserTypeStandard.Undergraduate:

                    default:
                        List<dtoProfileItem<dtoBaseProfile>> items = Service.GetProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers, idRemoveUsers);
                        View.LoadProfiles(items);
                        break;
                }

            }
        #endregion

        #region "Community view"
            #region "Manage Filters"
                private void InitializeCommunityFilters(ModuleCommunityManagement module, Int32 idCommunity, List<Int32> unavailableIdUsers)
                {
                    View.OrderAscending = true;
                    View.OrderUsersBy = OrderUsersBy.SurnameAndName;
                    Int32 idDefaultProfileType = 0;
                    Int32 idDefaultRole = 0;
                    long idDefaultAgency = 0;
                    SubscriptionStatus defaultStatus = SubscriptionStatus.activemember;
                    SearchProfilesBy defaultSearch = SearchProfilesBy.Contains;

                    //if (module.Administration || module.Manage)
                    //{
                        View.LoadRolesTypes(Service.GetAvailableSubscriptionsIdRoles(idCommunity, unavailableIdUsers), idDefaultRole);
                        View.LoadAvailableSubscriptionsStatus(Service.GetAvailableSubscriptionsStatus(idCommunity, unavailableIdUsers), defaultStatus);                      
                        if (View.ShowSubscriptionsFilterByProfile){
                            View.LoadProfileTypes(Service.GetAvailableProfileTypes(idCommunity, View.SelectedIdRole, View.SelectedSubscriptionStatus), idDefaultProfileType);
                            if (View.SelectedIdProfileType == (int)UserTypeStandard.Employee)
                                View.LoadAgencies(Service.GetAvailableAgencies(idCommunity, View.SelectedIdRole, View.SelectedSubscriptionStatus), idDefaultAgency, UserSelectionType.CommunityUsers);
                        }
                        else if (HasEmployeeUsers(UserSelectionType.CommunityUsers) && (module.Administration || module.Manage))
                            View.LoadAgencies(Service.GetAvailableAgencies(idCommunity, View.SelectedIdRole, View.SelectedSubscriptionStatus), idDefaultAgency, UserSelectionType.CommunityUsers);
                        View.LoadSearchProfilesBy(GetSearchByItems(module), defaultSearch, UserSelectionType.CommunityUsers);
                        
                        dtoUserFilters filter = View.GetCurrentUserFilters;
                        View.SearchUserFilters = filter;
                        View.InitializeWordSelector(Service.GetAvailableSubsriptionStartLetter(filter, View.UnavailableIdUsers));
                        ModuleProfileManagement pMmodule = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                        SetDefaultColumns(filter.IdRole, filter.IdProfileType, module.Administration || module.Manage, pMmodule, filter.Status);
                        View.LoadSubscriptions(new List<dtoSubscriptionProfileItem<dtoBaseProfile>>());
                    //}
                    //else
                    //    View.NoPermission();

                }
                private List<SearchProfilesBy> GetSearchByItems(ModuleCommunityManagement module)
                {
                    ModuleProfileManagement portalModule = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);

                    List<SearchProfilesBy> list = new List<SearchProfilesBy>();
                    list.Add(SearchProfilesBy.All);
                    list.Add(SearchProfilesBy.Contains);
                    list.Add(SearchProfilesBy.Name);
                    list.Add(SearchProfilesBy.Surname);
                    if (module.Administration || module.Manage || portalModule.Administration || portalModule.ViewProfiles)
                        list.Add(SearchProfilesBy.Mail);
                    if (View.AllowSearchByTaxCode && (module.Administration || module.Manage) && (portalModule.Administration || portalModule.ViewProfiles))
                        list.Add(SearchProfilesBy.TaxCode);

                    return list;
                }
                public void ChangeRoleType(Int32 idRole, Int32 idProfileType, int currentPageIndex, int currentPageSize)
                {
                    List<Int32> idCommunities = View.FromCommunities;
                    if (idCommunities.Count == 1)
                    {
                        //View.LoadAvailableStatus(Service.GetAvailableStatus(idOrganization, idProfileType), View.SelectedProfileStatus);
                        ModuleCommunityManagement cModule = ServiceCommunity.GetModulePermission(UserContext.CurrentUserID, idCommunities[0]);
                        if (View.ShowSubscriptionsFilterByProfile)
                            View.LoadProfileTypes(Service.GetAvailableProfileTypes(idCommunities[0], View.SelectedIdRole, View.SelectedSubscriptionStatus), View.SelectedIdProfileType);
                        View.LoadSearchProfilesBy(GetSearchByItems(cModule), View.SelectedSearchBy, UserSelectionType.CommunityUsers);
                        idProfileType = View.SelectedIdProfileType;
                        if (idProfileType == (int)UserTypeStandard.Employee || (HasEmployeeUsers(UserSelectionType.CommunityUsers) && (cModule.Administration || cModule.Manage)))
                            View.LoadAgencies(Service.GetAvailableAgencies(idCommunities[0], View.SelectedIdRole, View.SelectedSubscriptionStatus), View.SelectedIdAgency, UserSelectionType.CommunityUsers);
                    }
                    LoadSubscriptions(currentPageIndex, currentPageSize, true);
                }
                public void ChangeCommunityProfileType(Int32 idProfileType, int currentPageIndex, int currentPageSize)
                {
                    //View.LoadAvailableStatus(Service.GetAvailableStatus(idOrganization, idProfileType), View.SelectedProfileStatus);
                    List<Int32> idCommunities = View.FromCommunities;
                    if (idCommunities.Count == 1)
                    {
                        ModuleCommunityManagement cModule = ServiceCommunity.GetModulePermission(UserContext.CurrentUserID, idCommunities[0]);
                        View.LoadSearchProfilesBy(GetSearchByItems(cModule), View.SelectedSearchBy, UserSelectionType.CommunityUsers);
                        idProfileType = View.SelectedIdProfileType;
                        if (idProfileType == (int)UserTypeStandard.Employee)
                            View.LoadAgencies(Service.GetAvailableAgencies(idCommunities[0], View.SelectedIdRole, View.SelectedSubscriptionStatus), View.SelectedIdAgency, UserSelectionType.CommunityUsers);
                    }
                    LoadSubscriptions(currentPageIndex, currentPageSize, true);
                }
                public void ChangeCommunityProfileAgency(int currentPageIndex, int currentPageSize)
                {
                    LoadSubscriptions(currentPageIndex, currentPageSize, true);
                }
            #endregion
            public void LoadSubscriptions(int currentPageIndex, int currentPageSize, Boolean updateSelection)
            {
                List<Int32> idRemoveUsers = View.UnavailableIdUsers;
                dtoUserFilters filters = View.SearchUserFilters;
                PagerBase pager = new PagerBase();

                //if (filters.StartWith != View.CurrentStartWith)
                //    filters.StartWith = View.CurrentStartWith;

                pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
                pager.Count = (int)Service.CommunitySubscriptionsCount(filters, idRemoveUsers) - 1;
                pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;
                View.IsFirstLoad = false;
                View.InitializeWordSelector(Service.GetAvailableSubsriptionStartLetter(filters, idRemoveUsers), filters.StartWith);
                filters.StartWith = View.CurrentStartWith;
                if (updateSelection)
                    View.SelectedIdUsers = UpdateItemsSelection();

                List<Int32> idCommunities = View.FromCommunities;
                if (idCommunities.Count == 1)
                {
                    ModuleCommunityManagement cModule = ServiceCommunity.GetModulePermission(UserContext.CurrentUserID, idCommunities[0]);
                    ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                    List<ProfileColumn> columns = SetDefaultColumns(filters.IdRole, filters.IdProfileType, cModule.Administration || cModule.Manage, module, filters.Status);

                    if (View.SelectedIdProfileType != (int)UserTypeStandard.Employee && View.ShowSubscriptionsFilterByProfile)
                        View.UnLoadAgencies();

                    switch (filters.IdProfileType)
                    {
                        case (int)UserTypeStandard.Company:
                            List<dtoSubscriptionProfileItem<dtoCompany>> companyUsers = Service.GetCompanyUserProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, View.GetTranslatedRoles, idRemoveUsers);
                            View.LoadSubscriptions(companyUsers);
                            break;
                        case (int)UserTypeStandard.Employee:
                            List<dtoSubscriptionProfileItem<dtoEmployee>> employeeUsers = Service.GetEmployeeProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, View.GetTranslatedRoles, idRemoveUsers);
                            View.LoadSubscriptions(employeeUsers);
                            break;
                        default:
                            List<dtoSubscriptionProfileItem<dtoBaseProfile>> items = Service.GetProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, View.GetTranslatedRoles, idRemoveUsers);
                            View.LoadSubscriptions(items);
                            break;
                    }
                }
            }
            private List<ProfileColumn> SetDefaultColumns(Int32 idRole, Int32 idProfileType, Boolean displayMail, ModuleProfileManagement pModule, SubscriptionStatus status)
            {
                List<ProfileColumn> columns = new List<ProfileColumn>();
                columns.Add(ProfileColumn.name);
                if (displayMail)
                    columns.Add(ProfileColumn.mail);
                if (idRole <= 0)
                    columns.Add(ProfileColumn.communityrole);
                if (idProfileType <= 0 && (pModule.Administration || pModule.ViewProfiles) && View.ShowSubscriptionsProfileTypeColumn)
                    columns.Add(ProfileColumn.type);
                if (idProfileType == (int)UserTypeStandard.Company)
                    columns.Add(ProfileColumn.companyName);
                if (idProfileType == (int)UserTypeStandard.Employee)
                    columns.Add(ProfileColumn.agency);
                if (status == SubscriptionStatus.all || status == SubscriptionStatus.none)
                    columns.Add(ProfileColumn.status);
                View.AvailableColumns = columns;
                return columns;
            }
        #endregion

        public void EditItemsSelection(Boolean selectAll)
        {
            View.SelectAllUsers = selectAll;
            View.SelectedIdUsers = (selectAll) ? UpdateItemsSelection().Distinct().ToList() : new List<Int32>();
        }
        public List<Int32> GetSelectedIdUsers()
        {
            switch (View.SelectionMode) { 
                case UserSelectionType.CommunityUsers:
                    return Service.GetSubscriptionsIdUsers(View.UnavailableIdUsers, View.SelectAllUsers, View.SearchUserFilters, UpdateItemsSelection());
                case UserSelectionType.SystemUsers:
                    return Service.GetIdUsers(View.UnavailableIdUsers, View.SelectAllUsers, View.SearchProfileFilters, UpdateItemsSelection());
                default:
                    return new List<Int32>();
            }
        }
        private List<Int32> UpdateItemsSelection()
        {
            // SELECTED ITEMS
            List<Int32> idUsers = View.SelectedIdUsers;
            List<dtoSelectItem<Int32>> cSelectedItems = View.GetCurrentSelectedItems();

            // REMOVE ITEMS
            idUsers = idUsers.Where(i => !cSelectedItems.Where(si => !si.Selected && si.Id == i).Any()).ToList();
            // ADD ITEMS
            idUsers.AddRange(cSelectedItems.Where(si => si.Selected && !idUsers.Contains(si.Id)).Select(si => si.Id).Distinct().ToList());
            return idUsers;
        }
        #region "Preview View"
            public void DisplayPreviewSelection() {
                Int32 systemItems = View.SystemMaxGridItems;
                Int32 defaultMaxItems = View.DefaultMaxPreviewItems;
                View.AllowSelectAllFromPreview = false;
                if (defaultMaxItems > systemItems){
                    defaultMaxItems = systemItems;
                    View.DefaultMaxPreviewItems = defaultMaxItems;
                }
                else if (defaultMaxItems<=0){
                    defaultMaxItems = systemItems;
                    View.DefaultMaxPreviewItems = defaultMaxItems;
                    View.AllowSelectAllFromPreview = true;
                }
                List<Int32> idSelectedUsers = null;
                if (View.SelectAllUsers)
                    idSelectedUsers = GetSelectedIdUsers();
                else
                    idSelectedUsers = UpdateItemsSelection();
                View.TemporaryItemsCount = idSelectedUsers.Count();
                View.SelectedIdUsers = idSelectedUsers;
                View.TemporaryIdUsers = idSelectedUsers;
                
                View.IsFirstPreviewLoad = true;
                View.SearchPreviewLetter = "";
                View.SearchPreviewValue = "";
                List<dtoBaseProfile> items = Service.GetBaseProfiles(idSelectedUsers, "", "", defaultMaxItems);
                View.DisplayStartUsersPreview(items, Service.GetAvailableStartLetter(idSelectedUsers, "", ""), idSelectedUsers.Count);
            }
            public void LoadPreviewItems(String value, String currentLetter)
            {
                LoadPreviewItems(value,currentLetter,0, View.DefaultMaxPreviewItems,true);
            }
            public void LoadPreviewItems(int currentPageIndex, int currentPageSize, Boolean updateSelection) { 
                LoadPreviewItems(View.SearchPreviewValue,View.SearchPreviewLetter,currentPageIndex, currentPageSize,updateSelection);
            }

            public void LoadPreviewItems(String value, String currentLetter,int currentPageIndex, int currentPageSize, Boolean updateSelection)
            {
                List<Int32> idSelectedUsers = UpdateTemporaryItemsSelection();
                Boolean allowAll = View.AllowSelectAllFromPreview;
                if (updateSelection){
                    View.TemporaryIdUsers = idSelectedUsers;
                    View.TemporaryItemsCount = idSelectedUsers.Count();
                }
                View.IsFirstPreviewLoad = false;
                if (allowAll)
                    currentPageSize = View.SystemMaxGridItems;
                PagerBase pager = new PagerBase();
                pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
                pager.Count =  Service.GetBaseProfilesCount(idSelectedUsers, value, currentLetter) - 1;
                pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
                View.PreviewPager = pager;

                if (updateSelection)
                    View.SelectedIdUsers = UpdateItemsSelection();
                List<dtoBaseProfile> items = Service.GetBaseProfiles(idSelectedUsers, value, currentLetter, (allowAll) ? 0 : View.DefaultMaxPreviewItems);
                View.DisplayUsersPreview(items.Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList(), pager.Count - 1);

            }
        public List<Int32> ConfirmUserSelection(Boolean reload)
        {
            List<Int32> idSelectedUsers = UpdateTemporaryItemsSelection();
            if (View.SelectAllUsers)
                View.SelectAllUsers = !View.SelectedIdUsers.Any(idSelectedUsers.Contains);
            View.SelectedIdUsers = idSelectedUsers;
            if (reload) { 
                switch(View.SelectionMode){
                    case  UserSelectionType.SystemUsers:
                        LoadProfiles(View.Pager.PageIndex, View.CurrentPageSize, false);
                        break;
                    case UserSelectionType.CommunityUsers:
                        LoadSubscriptions(View.Pager.PageIndex, View.CurrentPageSize, false);
                        break;
                }
            }
            return idSelectedUsers;
        }
        private List<Int32> UpdateTemporaryItemsSelection()
        {
            // SELECTED ITEMS
            List<Int32> idUsers = View.TemporaryIdUsers;
            List<dtoSelectItem<Int32>> cSelectedItems = View.GetTemporarySelectedItems();

            // REMOVE ITEMS
            idUsers = idUsers.Where(i => !cSelectedItems.Where(si => !si.Selected && si.Id == i).Any()).ToList();
            // ADD ITEMS
            idUsers.AddRange(cSelectedItems.Where(si => si.Selected && !idUsers.Contains(si.Id)).Select(si => si.Id).Distinct().ToList());
            return idUsers;
        }
      
        #endregion
        private Boolean HasEmployeeUsers(UserSelectionType mode){
            switch (mode) { 
                case UserSelectionType.CommunityUsers:
                    dtoUserFilters filter = (dtoUserFilters)View.GetCurrentUserFilters.Clone();
                    filter.IdProfileType = (int)UserTypeStandard.Employee;
                    filter.IdRole = -1;
                    filter.IdAgency = -1;
                    return (int)Service.CommunitySubscriptionsCount(filter, View.UnavailableIdUsers) > 0;
                case UserSelectionType.SystemUsers:
                    dtoFilters pFilter = (dtoFilters)View.GetCurrentProfileFilters.Clone();
                    pFilter.IdProfileType = (int)UserTypeStandard.Employee;
                    pFilter.IdAgency = -1;
                    return Service.ProfilesCount(pFilter, View.UnavailableIdUsers) > 0;
                default:
                    return false;
            }
        }
    }
}