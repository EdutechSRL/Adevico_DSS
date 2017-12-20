using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewAgenciesManagement : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        int PreLoadedPageSize {get;}
        Boolean PreloadedReloadFilters { get; }

        Boolean AllowImport { get; set; }
        Boolean OrderAscending { get; set; }
        OrderAgencyBy OrderBy { get; set; }
        SearchAgencyBy SelectedSearchBy { get; set; }
        AgencyAvailability SelectedAvailability { get; set; }

        String CurrentValue { get; set; }
        String CurrentStartWith { get; set; }
        Int32 CurrentPageSize { get; set; }
        dtoAgencyFilters GetCurrentFilters { get; }
        dtoAgencyFilters GetSavedFilters { get; }
        dtoAgencyFilters SearchFilters { get; set; }
        
        PagerBase Pager { get; set; }

        void SaveCurrentFilters(dtoAgencyFilters GetSavedFilters);
        void InitializeWordSelector(List<String> availableWords);
        void InitializeWordSelector(List<String> availableWords, String activeWord);
        void LoadSearchAgenciesBy(List<SearchAgencyBy> list, SearchAgencyBy defaultSearch);
        void LoadAgencyAvailability(List<AgencyAvailability> list, AgencyAvailability dValue);
        void NoPermission();
        void DisplaySessionTimeout();
        void LoadAgencies(List<dtoAgencyItem> items);
    }
}