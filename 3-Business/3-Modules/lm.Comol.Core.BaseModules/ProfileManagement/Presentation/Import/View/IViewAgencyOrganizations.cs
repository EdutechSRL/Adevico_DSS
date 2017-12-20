using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Subscriptions;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewAgencyOrganizations : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        Boolean isValid { get; }

        Boolean AvailableForAll { get; set; }
        List<Int32> AvailableOrganizations { get; }
        Dictionary<Int32, String> SelectedOrganizations { get;  }
        Boolean HasAvailableOrganizations { get; }
        void InitializeControl();
        void LoadItems(Dictionary<Int32, String> items);
        void DisplaySessionTimeout();
    }
}