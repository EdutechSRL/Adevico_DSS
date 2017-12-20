using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Communities;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewSearchCommunitiesModal : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        #region "Context"
            Boolean OrderAscending { get; set; }
            lm.Comol.Core.Dashboard.Domain.OrderItemsBy OrderBy { get; set; }
            Boolean isInitialized { get; }
            Boolean HasAvailableCommunities { get; }
        #endregion


        #region "Initialize"
            void InitializeAdministrationControl(Int32 idProfile, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability, List<Int32> onlyFromOrganizations);
            void InitializeControlByModules(Int32 idProfile, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability);
            void InitializeControlByModules(Int32 idProfile, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability, List<Int32> onlyFromOrganizations);
            void ReloadAdministrationControl(List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability);
            void ReloadControlByModules(List<Int32> unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability preloadedAvailability);
        #endregion

        #region "Load Items"
            void DisplaySessionTimeout();
        #endregion

        #region "Get selection"
            List<Int32> GetIdSelectedItems();
            List<dtoCommunityPlainItem> GetSelectedItems();
        #endregion
    }
}