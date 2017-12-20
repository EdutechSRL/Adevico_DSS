using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewSearchUsersForModule : IViewBaseProfilesManagement
    {
        String PreLoadedView { get; }
        Int32 PreLoadedIdCommunity { get; }
        String PreLoadedModuleCode { get; }
        String CurrentModuleView { get; set; }
        String CurrentModuleCode { get; set; }
        Int32 CurrentModuleIdCommunity { get; set; }
    }
}