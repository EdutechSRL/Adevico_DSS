using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Subscriptions;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewProfileCommunitySubscriptions : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }
        Boolean IsInitialized { get; set; }
        Boolean OrderAscending { get; set; }
        Int32 CurrentPageSize { get; set; }
        PagerBase Pager { get; set; }
        SubscriptionStatus SelectedStatus { get; set; }

        dtoSubscriptionFilters GetCurrentFilters { get; }

        void InitializeControl(Int32 idProfile);
        void LoadAvaliableStatus(List<SubscriptionStatus> items);
        void LoadCommunities(List<lm.Comol.Core.Subscriptions.dtoBaseSubscription> items);
        void DisplayEmpty();
    }
}