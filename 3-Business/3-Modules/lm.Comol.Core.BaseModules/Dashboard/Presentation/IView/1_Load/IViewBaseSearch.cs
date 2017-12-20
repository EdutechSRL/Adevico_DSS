using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewBaseSearch : IViewDefaultDashboardLoader
    {
        DisplaySearchItems PreloadSearch { get; }
        String PreloadSearchText { get; }
        Int32 PreloadIdCommunityType { get; }
        lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters GetSubmittedFilters();
        void EnableFullWidth(Boolean value);
    }
}