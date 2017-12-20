using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Communities;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.CommunityManagement.Presentation
{
    public interface IViewFindCommunitiesByService : IViewFindCommunities
    {
        Int32 IdProfile { get; set; }
        void InitializeAdministrationControl(Int32 idProfile, List<Int32> unloadIdCommunities, CommunityAvailability preloadedAvailability, List<Int32> onlyFromOrganizations);
        void InitializeControlByModules(Int32 idProfile, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, CommunityAvailability preloadedAvailability);
        void InitializeControlByModules(Int32 idProfile, Dictionary<Int32, long> requiredPermissions, List<Int32> unloadIdCommunities, CommunityAvailability preloadedAvailability, List<Int32> onlyFromOrganizations);

        void ReloadAdministrationControl(List<Int32> unloadIdCommunities, CommunityAvailability preloadedAvailability);
        void ReloadControlByModules(List<Int32> unloadIdCommunities, CommunityAvailability preloadedAvailability);
    }
}