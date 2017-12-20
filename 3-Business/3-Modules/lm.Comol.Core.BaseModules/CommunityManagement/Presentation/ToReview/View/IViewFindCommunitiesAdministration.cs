using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Communities;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.CommunityManagement.Presentation
{
    public interface IViewFindCommunitiesAdministration : IViewFindCommunities
    {
        Int32 IdProfile { get; set; }
        void InitializeControl(Int32 idProfile, List<Int32> unloadIdCommunities, CommunityAvailability preloadedAvailability);
    }
}