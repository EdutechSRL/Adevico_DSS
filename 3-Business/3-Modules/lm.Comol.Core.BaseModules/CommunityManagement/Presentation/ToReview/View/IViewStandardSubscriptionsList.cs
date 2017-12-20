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
    public interface IViewStandardSubscriptionsList : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }
        Boolean HasAvailableSubscriptions { get; set; }
        List<dtoUserSubscription> SelectedSubscriptions();

        void InitializeControl(Int32 idProfile, List<dtoBaseCommunityNode> communityNodes);
        void LoadSubscriptions(List<dtoUserSubscription> items, List<dtoRoleCommunityTypeTemplate> templates);
        void LoadNothing();
    }
}