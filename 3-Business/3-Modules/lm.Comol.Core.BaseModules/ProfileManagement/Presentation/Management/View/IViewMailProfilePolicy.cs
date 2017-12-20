using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.Communities;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewMailProfilePolicy : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }
        Boolean IsInitialized { get; set; }
        String ClientScript { set; }

        dtoCommunitiesFilters CommunityFilters { get; set; }

        List<Int32> PreviousSelectedCommunities { get; }
        List<Int32> SelectedCommunities { get; }
        List<CommunityStatus> AvailableStatus { set; }

        void InitializeControl(Int32 idProfile);
        void LoadTree(dtoTreeCommunityNode tree,List<Int32> idCommunities);
        void LoadNothing();

        void DisplayError();
        void DisplayPolicySaved();
        void DisplayProfileUnknown();
        void DisplaySessionTimeout();
    }
}