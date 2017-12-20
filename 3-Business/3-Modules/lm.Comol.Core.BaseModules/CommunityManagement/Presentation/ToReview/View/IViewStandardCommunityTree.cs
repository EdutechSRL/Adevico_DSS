using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Subscriptions;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.CommunityManagement.Presentation
{
    public interface IViewStandardCommunityTree : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }

        CommunityStatus CurrentStatus { get; }
        CommunityAvailability CurrentAvailability { get; set; }
        dtoCommunitiesFilters CommunityFilters { get; set; }
        List<Int32> SelectedIdCommunities();
        List<Int32> SelectedIdOrganizations();
        List<dtoBaseCommunityNode> GetNodesById(List<Int32> idCommunities);


        void InitializeControl(Int32 idProfile, CommunityAvailability preloadedAvailability);
        void InitializeTree(List<CommunityStatus> items, List<Int32> IdTypes, List<CommunityAvailability> availabilities);
        
        Boolean HasAvailableCommunities { get; set; }
        List<dtoBaseCommunityNode> SelectedCommunities();
        void LoadTree(dtoTreeCommunityNode tree);
        void LoadTypes(List<Int32> IdTypes);
        void LoadNothing();
    }
}