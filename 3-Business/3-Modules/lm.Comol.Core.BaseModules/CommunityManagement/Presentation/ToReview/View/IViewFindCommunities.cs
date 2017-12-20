using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Communities;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.CommunityManagement.Presentation
{
    public interface IViewFindCommunities : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean RaiseCommunityChangedEvent { get; set; }
        Boolean MultipleSelection { get; }
        Boolean OrderAscending { get; set; }
        Boolean AdministrationMode { get; set; }
        OrderCommunitiesBy OrderBy { get; set; }
        Boolean isInitialized { get; set; }
        //Int32 SelectedIdOrganization { get; set; }
        //Int32 SelectedIdCommunityType { get; set; }
        //Int32 SelectedIdResponsible { get; set; }
        //CommunityStatus SelectedStatus { get; }
        //SearchCommunitiesBy SelectedSearchBy { get; set; }
        //String CurrentValue { get; set; }
        //String CurrentStartWith { get; set; }
        Int32 CurrentPageSize { get; set; }
        
        dtoCommunitiesFilters SearchFilters { get; set; }
        List<Int32> ExcludeCommunities { get; set; }
        List<Int32> OnlyFromOrganizations { get; set; }
        Dictionary<Int32, long> RequiredPermissions { get; set; }
        PagerBase Pager { get; set; }

        List<Int32> SelectedIdCommunities  { get; set; }
        Dictionary<Boolean, List<Int32>> GetCurrentSelection();
        Boolean HasAvailableCommunities { get; set; }



        void LoadItems(List<lm.Comol.Core.Dashboard.Domain.dtoSubscriptionItem> items);
        dtoCommunitiesFilters GetSubmittedFilters();
        void LoadNothing();
        //void LoadOrganizations(Dictionary<Int32, String> organizations);
        //void LoadTypes(List<Int32> IdTypes);
        //void LoadSearchCommunitiesBy(List<SearchCommunitiesBy> list, SearchCommunitiesBy defaultSearch);
        //void LoadAvailableStatus(List<CommunityStatus> list, CommunityStatus defaultStatus);
        //void LoadAvailabilities(List<CommunityAvailability> list, CommunityAvailability defaultAvailability);
        //void InitializeWordSelector(List<String> availableWords);
        //void InitializeWordSelector(List<String> availableWords, String activeWord);
        //void LoadResponsible(List<litePerson> users);
        void DisplaySessionTimeout();

        List<dtoCommunityPlain> GetSelectedItems();
        List<Int32> GetIdSelectedItems();
        void LoadDefaultFilters(List<lm.Comol.Core.DomainModel.Filters.Filter> filters);
    }
}