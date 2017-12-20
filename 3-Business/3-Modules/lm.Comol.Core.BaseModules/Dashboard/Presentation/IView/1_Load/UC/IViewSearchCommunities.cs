using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Communities;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewSearchCommunities : lm.Comol.Core.DomainModel.Common.iDomainView
    {

        #region "Context"
            Int32 IdProfile { get; set; }
            Boolean MultipleSelection { get; }
            Boolean OrderAscending { get; set; }
            Boolean AdministrationMode { get; set; }
            lm.Comol.Core.Dashboard.Domain.OrderItemsBy OrderBy { get; set; }
            Boolean isInitialized { get; set; }
            Int32 CurrentPageSize { get; set; }

            List<Int32> ExcludeCommunities { get; set; }
            List<Int32> OnlyFromOrganizations { get; set; }
            Dictionary<Int32, long> RequiredPermissions { get; set; }
            PagerBase Pager { get; set; }

            List<Int32> SelectedIdCommunities { get; set; }
            Dictionary<Boolean, List<Int32>> GetCurrentSelection();

            Boolean RaiseCommunityChangedEvent { get; set; }

            lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters SearchFilters { get; set; }

            Boolean HasAvailableCommunities { get; set; }
            lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability CurrentAvailability  { get; set; }

        #endregion



        #region "Initialize"
            void InitializeAdministrationControl(Int32 idProfile, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability, List<Int32> onlyFromOrganizations);
            void InitializeControlByModules(Int32 idProfile, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability);
            void InitializeControlByModules(Int32 idProfile, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability, List<Int32> onlyFromOrganizations);
            void ReloadAdministrationControl(List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability);
            void ReloadControlByModules(List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability);
        #endregion

        #region "Load Items"
            lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters GetSubmittedFilters();
            void LoadNothing();
            void LoadDefaultFilters(List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Dictionary<Int32, long> requiredPermissions,List<Int32> unloadIdCommunities ,lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability availability, List<Int32> onlyFromOrganizations);
            void LoadItems(List<dtoCommunityPlainItem> items);
            void DisplaySessionTimeout();

        #endregion

        #region "Get selection"
            List<Int32> GetIdSelectedItems();
            List<dtoCommunityPlainItem> GetSelectedItems();
        #endregion
        
    }
}