using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewBaseProfilesManagement : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        int PreLoadedPageSize {get;}
        Boolean PreloadedReloadFilters { get; }
        
        Boolean AllowAutoupdate { get; }
        Boolean AllowSearchByTaxCode { get; }
        Boolean OrderAscending { get; set; }
        OrderProfilesBy OrderBy { get; set; }

        Int32 SelectedIdOrganization { get; set; }
        Int32 SelectedIdProfileType { get; set; }
        long SelectedIdAgency { get; set; }
        StatusProfile SelectedStatusProfile { get; set; }
        SearchProfilesBy SelectedSearchBy { get; set; }
        String CurrentValue { get; set; }
        String CurrentStartWith { get; set; }
        Int32 CurrentPageSize { get; set; }
        dtoFilters GetCurrentFilters { get; }
        dtoFilters GetSavedFilters { get;}
        dtoFilters SearchFilters { get; set; }
        PagerBase Pager { get; set; }
        List<ProfileColumn> AvailableColumns { get; set; }
        List<Int32> AvailableOrganizations { get; set; }
        List<TranslatedItem<Int32>> GetTranslatedProfileTypes{ get;}

        void SaveCurrentFilters(dtoFilters GetSavedFilters);
        //void LoadOrganizations();
        void LoadProfileTypes(List<Int32> idProfileTypes, Int32 IdDefaultProfileType);
        void LoadAgencies(Dictionary<long, String> items, long idDefaultAgency);
        void UnLoadAgencies();
        void LoadSearchProfilesBy(List<SearchProfilesBy> list, SearchProfilesBy defaultSearch);
        void LoadAvailableOrganizations(List<Organization> items, Int32 idDefaultOrganization);
        void LoadAvailableStatus(List<StatusProfile> list, StatusProfile defaultStatus);
        void InitializeWordSelector(List<String> availableWords);
        void InitializeWordSelector(List<String> availableWords, String activeWord);
        void NoPermission();
        void NoPermissionToAdmin();
        void DisplaySessionTimeout();
      

        void LoadProfiles(List<dtoProfileItem<dtoBaseProfile>> items);
        
        void LoadProfiles(List<dtoProfileItem<dtoCompany>> items);
        void LoadProfiles(List<dtoProfileItem<dtoEmployee>> items);
        void LoadProfiles(List<dtoProfileItem<dtoExternal>> items);
    }
}