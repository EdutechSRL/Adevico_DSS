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

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class SearchUsersForModulePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
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
            protected virtual IViewSearchUsersForModule View
            {
                get { return (IViewSearchUsersForModule)base.View; }
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
            public SearchUsersForModulePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SearchUsersForModulePresenter(iApplicationContext oContext, IViewSearchUsersForModule view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idCommunity = View.PreLoadedIdCommunity;
                if (idCommunity == -1)
                    idCommunity = UserContext.CurrentCommunityID;

                View.CurrentModuleIdCommunity = idCommunity;
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                if (module.ViewProfiles || module.Administration)
                    InitializeFilters();
                else
                    View.NoPermission();
            }
        }

#region "ManageFilters"
        private void InitializeFilters()
        {
            View.OrderAscending = true;
            View.OrderBy = OrderProfilesBy.SurnameAndName;
            Boolean loadFilters = View.PreloadedReloadFilters;
            dtoFilters filters = null;
            if (loadFilters)
                filters = View.GetSavedFilters;

            Int32 idDefaultProfileType = 0;
            long idDefaultProvider = 0;
            long idDefaultAgency = 0;
            Int32 idDefaultOrganization = 0;
            StatusProfile defaultStatus = StatusProfile.Active;
            SearchProfilesBy defaultSearch = SearchProfilesBy.Contains;
            View.CurrentModuleView = View.PreLoadedView;
            View.CurrentModuleCode = View.PreLoadedModuleCode;
            int pageIndex = 0;
            if (loadFilters && filters != null)
            {
                idDefaultOrganization = filters.IdOrganization;
                idDefaultProfileType = filters.IdProfileType;
                idDefaultProvider = filters.idProvider;

                if (idDefaultProfileType == (int)UserTypeStandard.Employee)
                    idDefaultAgency = filters.IdAgency;
                defaultStatus = filters.Status;
                defaultSearch = filters.SearchBy;
                if (!String.IsNullOrEmpty(filters.StartWith) && !Service.DefaultChars().Contains(filters.StartWith))
                    filters.StartWith = "#";
                View.CurrentStartWith = filters.StartWith;
                View.CurrentValue = filters.Value;
                View.OrderAscending = filters.Ascending;
                View.OrderBy = filters.OrderBy;
                pageIndex = filters.PageIndex;
                View.CurrentPageSize = filters.PageSize;
            }
            else
                View.CurrentPageSize = View.PreLoadedPageSize;

            List<Organization> organizations = Service.GetAvailableOrganizations(UserContext.CurrentUserID, (View.CurrentModuleIdCommunity < 1) ? SearchCommunityFor.SystemManagement : SearchCommunityFor.CommunityManagement);
            if (organizations != null && organizations.Any())
            {
                View.LoadAvailableOrganizations(organizations, idDefaultOrganization);
                View.LoadProfileTypes(Service.GetAvailableProfileTypes(View.SelectedIdOrganization), idDefaultProfileType);

                List<dtoBaseProvider> providers = Service.GetAuthenticationProviders(UserContext.Language.Id, true).Where(p=> p.isEnabled).ToList();
                View.LoadAvailableStatus(Service.GetAvailableStatus(View.SelectedIdOrganization, View.SelectedIdProfileType), defaultStatus);
                View.LoadSearchProfilesBy(GetSearchByItems(), defaultSearch);
                if (idDefaultProfileType == (int)UserTypeStandard.Employee)
                    View.LoadAgencies(Service.GetAvailableAgencies(View.SelectedIdOrganization), idDefaultAgency);

                View.SearchFilters = View.GetCurrentFilters;
                if (loadFilters && filters != null)
                    LoadProfiles(filters.PageIndex, filters.PageSize);
                else if (filters==null)
                    View.InitializeWordSelector(Service.GetAvailableStartLetter(View.GetCurrentFilters));
            }
            else
                View.NoPermissionToAdmin();
        }
        private List<SearchProfilesBy> GetSearchByItems(){
            List<SearchProfilesBy> list = new List<SearchProfilesBy>();
            list.Add(SearchProfilesBy.All);
            list.Add(SearchProfilesBy.Contains);
            list.Add(SearchProfilesBy.Name);
            list.Add(SearchProfilesBy.Surname);

            list.Add(SearchProfilesBy.Mail);
            if (View.AllowSearchByTaxCode)
                list.Add(SearchProfilesBy.TaxCode);

            return list;
        }
        public void LoadProfiles(int currentPageIndex, int currentPageSize)
        {
            dtoFilters filters = View.SearchFilters;
            List<String> availableLetters = Service.GetAvailableStartLetter(filters);
            PagerBase pager = new PagerBase();

            if (filters.StartWith != View.CurrentStartWith)
                filters.StartWith = View.CurrentStartWith;

            if (filters.StartWith != "" && !availableLetters.Contains(filters.StartWith))
            {
                filters.StartWith = (availableLetters.Count > 0) ? availableLetters.FirstOrDefault() : "";
            }
                

            pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
            pager.Count = (int)Service.ProfilesCount(filters) - 1;
            pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            View.InitializeWordSelector(Service.GetAvailableStartLetter(filters), filters.StartWith);
            //if (view == UserCallForPaperStatus.Evaluated || view == UserCallForPaperStatus.ToEvaluate)
            //    View.LoadCallForPapersForEvaluation(Service.CallForPaperForEvaluation(CommunityId, UserContext.CurrentUserID, view, currentPageIndex, currentPageSize));
            //else
            //    View.LoadCallForPapers(Service.UserCallForPapers(CommunityId, UserContext.CurrentUserID, view, currentPageIndex, currentPageSize));
            //View.ActionListView(CommunityId, ModuleID);
            List<dtoBaseProvider> providers = Service.GetAuthenticationProviders(UserContext.Language.Id, true);

            List<ProfileColumn> columns = SetDefaultColumns(filters.IdProfileType, (from p in providers where p.IdProvider == filters.idProvider select p.Type).FirstOrDefault());

            if (filters.IdProfileType != (int)UserTypeStandard.Employee)
                View.UnLoadAgencies();
            
            switch (filters.IdProfileType) { 
                case (int)UserTypeStandard.Company:
                    List<dtoProfileItem<dtoCompany>> companyUsers = Service.GetCompanyUserProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers);
                    if (companyUsers.Where(i => i.Id == UserContext.CurrentUserID).Any())
                        companyUsers.Where(i => i.Id == UserContext.CurrentUserID).ToList().ForEach(i => i.Permission.LogonAs = false);
                    View.LoadProfiles(companyUsers);
                    break;
                case (int)UserTypeStandard.Employee:
                    List<dtoProfileItem<dtoEmployee>> employeeUsers = Service.GetEmployeeProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers);
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
                    List<dtoProfileItem<dtoBaseProfile>> items = Service.GetProfiles(filters, pager.PageIndex, currentPageSize, View.GetTranslatedProfileTypes, providers);
                    if (items.Where(i => i.Id == UserContext.CurrentUserID).Any())
                        items.Where(i => i.Id == UserContext.CurrentUserID).ToList().ForEach(i => i.Permission.LogonAs = false);
                    View.LoadProfiles(items);
                    break;
            }

        }
        public void ChangeOrganization(Int32 idOrganization, Int32 idProfileType, int currentPageIndex, int currentPageSize)
        {
            View.LoadProfileTypes(Service.GetAvailableProfileTypes(idOrganization), idProfileType);
            idProfileType = View.SelectedIdProfileType;
            View.LoadAvailableStatus(Service.GetAvailableStatus(idOrganization, idProfileType), View.SelectedStatusProfile);
            View.LoadSearchProfilesBy(GetSearchByItems(), View.SelectedSearchBy);
            LoadProfiles(currentPageIndex, currentPageSize);
        }
        public void ChangeProfileType(Int32 idOrganization, Int32 idProfileType, int currentPageIndex, int currentPageSize)
        {
            View.LoadAvailableStatus(Service.GetAvailableStatus(idOrganization, idProfileType), View.SelectedStatusProfile);
            View.LoadSearchProfilesBy(GetSearchByItems(), View.SelectedSearchBy);
            idProfileType = View.SelectedIdProfileType;
            if (idProfileType == (int)UserTypeStandard.Employee)
                View.LoadAgencies(Service.GetAvailableAgencies(View.SelectedIdOrganization), 0);

            LoadProfiles(currentPageIndex, currentPageSize);
        }
        public void ChangeAgency(int currentPageIndex, int currentPageSize)
        {
            LoadProfiles(currentPageIndex, currentPageSize);
        }
#endregion

        public void SearchProfiles(int currentPageIndex, int currentPageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                LoadProfiles(currentPageIndex, currentPageSize);
        }

        private List<ProfileColumn> SetDefaultColumns(Int32 IdProfileType, AuthenticationProviderType authentication)
        {
            List<ProfileColumn> columns = new List<ProfileColumn>();
            columns.Add(ProfileColumn.name);

            columns.Add(ProfileColumn.statusIcon);
            columns.Add(ProfileColumn.status);
            columns.Add(ProfileColumn.authentication);

            if (IdProfileType== (int) UserTypeStandard.Company)
                columns.Add(ProfileColumn.companyName);
            if (IdProfileType == (int)UserTypeStandard.Employee)
                columns.Add(ProfileColumn.agency);
            View.AvailableColumns=columns;
            return columns;
        }
    }
}