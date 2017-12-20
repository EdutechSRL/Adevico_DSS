//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using lm.Comol.Core.DomainModel;
//using lm.Comol.Core.Communities;
//using lm.Comol.Core.BaseModules.CommunityManagement.Business;
//using lm.Comol.Core.BaseModules.CommunityManagement;
//using lm.Comol.Core.Business;

//namespace lm.Comol.Core.BaseModules.CommunityManagement.Presentation
//{
//    public class FindCommunitiesForAdministrationPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
//    {
//         #region "Initialize"
//            private int _ModuleID;
//            private ServiceCommunityManagement _Service;
//            private ProfileManagement.Business.ProfileManagementService _ProfileService;
//            //private int ModuleID
//            //{
//            //    get
//            //    {
//            //        if (_ModuleID <= 0)
//            //        {
//            //            _ModuleID = this.Service.ServiceModuleID();
//            //        }
//            //        return _ModuleID;
//            //    }
//            //}
//            public virtual BaseModuleManager CurrentManager { get; set; }
//            protected virtual IViewFindCommunitiesAdministration View
//            {
//                get { return (IViewFindCommunitiesAdministration)base.View; }
//            }
//            private ServiceCommunityManagement Service
//            {
//                get
//                {
//                    if (_Service == null)
//                        _Service = new ServiceCommunityManagement(AppContext);
//                    return _Service;
//                }
//            }
//            private ProfileManagement.Business.ProfileManagementService ProfileService
//            {
//                get
//                {
//                    if (_ProfileService == null)
//                        _ProfileService = new ProfileManagement.Business.ProfileManagementService(AppContext);
//                    return _ProfileService;
//                }
//            }
//            public FindCommunitiesForAdministrationPresenter(iApplicationContext oContext)
//                : base(oContext)
//            {
//                this.CurrentManager = new BaseModuleManager(oContext);
//            }
//            public FindCommunitiesForAdministrationPresenter(iApplicationContext oContext, IViewFindCommunitiesAdministration view)
//                : base(oContext, view)
//            {
//                this.CurrentManager = new BaseModuleManager(oContext);
//            }
//        #endregion

//            public void InitAdministrationView(Int32 idProfile, List<Int32> unloadIdCommunities, CommunityAvailability preloadedAvailability, List<Int32> onlyFromOrganizations)
//        {
//            if (UserContext.isAnonymous)
//                View.LoadNothing();
//            else
//            {
//                List<CommunityAvailability> loadAvailability = new List<CommunityAvailability>();
//                loadAvailability.Add(CommunityAvailability.All);
//                loadAvailability.Add(CommunityAvailability.NotSubscribed);
//                loadAvailability.Add(CommunityAvailability.Subscribed);
//                InitializeFilters(true, idProfile,unloadIdCommunities, preloadedAvailability, loadAvailability);
//            }
//        }

//        #region "ManageFilters"
//        private void InitializeFilters(Boolean ForAdministration, Int32 idProfile,List<Int32> unloadIdCommunities, CommunityAvailability preloadedAvailability, List<CommunityAvailability> loadAvailability)
//            {
//                View.IdProfile = idProfile;
//                litePerson currentUser =  CurrentManager.GetLitePerson(UserContext.CurrentUserID);
//                litePerson person = CurrentManager.GetLitePerson(idProfile);
//                List<CommunityStatus> status = Service.GetCommunitiesAvailableStatus();

//                if (status.Count > 0)
//                {
//                    dtoCommunitiesFilters filters = new dtoCommunitiesFilters();

//                    Dictionary<Int32, String> organizations = null;
//                    filters.OnlyFromAvailableOrganizations = !ForAdministration;
//                    if (ForAdministration)
//                        organizations = Service.GetDisplayOrganizations(currentUser);
//                    else
//                        organizations = Service.GetDisplayOrganizations(currentUser, idProfile);
//                    if (organizations.Count > 0)
//                    {
//                        filters.IdOrganization = organizations.FirstOrDefault().Key;
//                        filters.AvailableIdOrganizations = organizations.Select(o => o.Key).ToList();
//                    }
//                    else
//                    {
//                        filters.IdOrganization = -1;
//                    }
//                    View.LoadOrganizations(organizations);
//                    filters.OrderBy = OrderCommunitiesBy.Name;
//                    filters.Ascending = true;

//                    List<CommunityAvailability> availabilities = GetCommunityAvailability(person, loadAvailability);
//                    if (preloadedAvailability == CommunityAvailability.None)
//                        preloadedAvailability = CommunityAvailability.NotSubscribed;
//                    if (availabilities.Any() && !availabilities.Contains(preloadedAvailability) )
//                        preloadedAvailability = availabilities.FirstOrDefault();

//                    filters.Availability = preloadedAvailability;
//                    View.LoadAvailabilities(availabilities, preloadedAvailability);

//                    CommunityStatus defaultStatus = CommunityStatus.Active;
//                    List<CommunityStatus> statusList = Service.GetCommunitiesAvailableStatus();
//                    if (preloadedAvailability == CommunityAvailability.NotSubscribed && !ForAdministration)
//                        statusList = statusList.Where(s => s != CommunityStatus.Blocked).ToList();
//                    if (statusList.Contains(defaultStatus))
//                        defaultStatus = statusList.FirstOrDefault();
//                    View.LoadAvailableStatus(statusList, defaultStatus);
//                    filters.Status = defaultStatus;

//                    SearchCommunitiesBy defaultSearch = SearchCommunitiesBy.Contains;
//                    View.LoadSearchCommunitiesBy(GetSearchByItems(), defaultSearch);
//                    filters.SearchBy = defaultSearch;

                    
                  

//                    filters.Value = "";
//                    filters.StartWith = "";
//                    View.ExcludeCommunities = unloadIdCommunities;                  
//                    LoadCommunities(View.GetCurrentFilters);
//                }
//                else
//                    View.LoadNothing();
//            }

//        private void AnalyzeNodes(List<Int32> unloadIdCommunities, litePerson person)
//        {
//            dtoTreeCommunityNode tree = Service.GetAllCommunitiesTree(person);
//            List<dtoTreeCommunityNode> nodes = tree.GetAllNodes();
//           // nodes =  
//        }
       
//        //private void LoadOrganizations(Int32 idProfile)
//        //{
//        //    List<Organization> items = ProfileService.GetAvailableOrganizations(CurrentManager.GetPerson(UserContext.CurrentUserID), idProfile);
            
//        //    View.LoadOrganizations(items
//        //}

//        //public void LoadProfiles(int currentPageIndex, int currentPageSize)
//        //{
//        //    dtoFilters filters = View.SearchFilters;
//        //    PagerBase pager = new PagerBase();

//        //    if (filters.StartWith != View.CurrentStartWith)
//        //        filters.StartWith = View.CurrentStartWith;

//        //    pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
//        //    pager.Count = (int)Service.ProfilesCount(filters) - 1;
//        //    pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
//        //    View.Pager = pager;
//        //    View.LoadAvailableStartLetter(Service.GetAvailableStartLetter(filters));
//        //    //if (view == UserCallForPaperStatus.Evaluated || view == UserCallForPaperStatus.ToEvaluate)
//        //    //    View.LoadCallForPapersForEvaluation(Service.CallForPaperForEvaluation(CommunityId, UserContext.CurrentUserID, view, currentPageIndex, currentPageSize));
//        //    //else
//        //    //    View.LoadCallForPapers(Service.UserCallForPapers(CommunityId, UserContext.CurrentUserID, view, currentPageIndex, currentPageSize));
//        //    //View.ActionListView(CommunityId, ModuleID);
//        //    List<dtoBaseProvider> providers = Service.GetAuthenticationProviders(UserContext.Language.Id, true);

//        //    SetDefaultColumns(filters.IdProfileType, (from p in providers where p.IdProvider == filters.idProvider select p.Type).FirstOrDefault());
//        //    if (filters.IdProfileType == (int)UserTypeStandard.Company)
//        //        View.LoadProfiles(Service.GetCompanyUserProfiles(filters, currentPageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers));
//        //    else
//        //        View.LoadProfiles(Service.GetProfiles(filters, currentPageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers));
//        //}

//        //public void ChangeOrganization(Int32 IdIOrganization, Int32 IdProfileType, int currentPageIndex, int currentPageSize)
//        //{
//        //    View.LoadProfileTypes(Service.GetAvailableProfileTypes(IdIOrganization), IdProfileType);
//        //    List<dtoBaseProvider> providers = Service.GetAuthenticationProviders(UserContext.Language.Id, true);
//        //    View.LoadAuthenticationProviders(providers, View.SelectedIdProvider);
//        //    View.LoadAvailableStatus(Service.GetAvailableStatus(IdIOrganization, IdProfileType), View.SelectedStatusProfile);
//        //    View.LoadSearchProfilesBy(GetSearchByItems((from p in providers where p.IdProvider == View.SelectedIdProvider select p).FirstOrDefault()), View.SelectedSearchBy);

//        //    LoadProfiles(currentPageIndex, currentPageSize);
//        //}
//        //public void ChangeProfileType(Int32 IdIOrganization, Int32 IdProfileType, int currentPageIndex, int currentPageSize)
//        //{
//        //    List<dtoBaseProvider> providers = Service.GetAuthenticationProviders(UserContext.Language.Id, true);
//        //    View.LoadAuthenticationProviders(providers, View.SelectedIdProvider);
//        //    View.LoadAvailableStatus(Service.GetAvailableStatus(IdIOrganization, IdProfileType), View.SelectedStatusProfile);
//        //    View.LoadSearchProfilesBy(GetSearchByItems((from p in providers where p.IdProvider == View.SelectedIdProvider select p).FirstOrDefault()), View.SelectedSearchBy);
//        //    LoadProfiles(currentPageIndex, currentPageSize);
//        //}
//        #endregion

//        //public void ChangeStatus(CommunityStatus status) {
//        //    View.LoadTypes(Service.GetCommunitiesAvailableTypes(status));
//        //    LoadCommunities(View.CommunityFilters);
//        //}

//        private void LoadCommunities(dtoCommunitiesFilters filters)
//        {
//            dtoTreeCommunityNode tree = Service.GetFilteredCommunitiesTree(filters, CurrentManager.GetLitePerson(View.IdProfile));
//            View.HasAvailableCommunities = (tree.Nodes.Count > 0);
//            //View.LoadTree(tree);
//        }

//        #region "Filters"
//            private List<SearchCommunitiesBy> GetSearchByItems()
//            {
//                List<SearchCommunitiesBy> list = new List<SearchCommunitiesBy>();
//                list.Add(SearchCommunitiesBy.All);
//                list.Add(SearchCommunitiesBy.Contains);
//                list.Add(SearchCommunitiesBy.NameStartAs);

//                return list;
//            }
//            private List<CommunityAvailability> GetCommunityAvailability(litePerson person, List<CommunityAvailability> loadAvailability)
//            {
//                List<CommunityAvailability> availabilities = new List<CommunityAvailability>();
//                if (person != null)
//                {
//                    availabilities.Add(CommunityAvailability.All);
//                    availabilities.Add(CommunityAvailability.Subscribed);
//                    availabilities.Add(CommunityAvailability.NotSubscribed);
//                }
//                else
//                    availabilities.Add(CommunityAvailability.NotSubscribed);

//                return availabilities.Where(a => loadAvailability.Contains(a)).ToList();
//            }
//        #endregion
      

//        public void LoadCommunities(Int32 pageIndex, Int32 pageSize)
//        {
//        }
//    }
//}