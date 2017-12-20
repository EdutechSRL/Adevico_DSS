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
    public interface IViewNewProfileSubscriptionsList : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean HasAvailableSubscriptions { get; set; }
        List<dtoNewProfileSubscription> SelectedSubscriptions();

        void InitializeControl(List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> communityNodes);
        void LoadSubscriptions(List<dtoNewProfileSubscription> items, List<dtoRoleCommunityTypeTemplate> templates, Dictionary<Int32,String> translatedRoles);
        void LoadNothing();
    }
}