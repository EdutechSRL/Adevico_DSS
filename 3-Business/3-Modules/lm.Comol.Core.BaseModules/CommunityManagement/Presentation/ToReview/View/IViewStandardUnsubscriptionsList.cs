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
    public interface IViewStandardUnsubscriptionsList : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }
        Boolean HasUnsubscriptions { get; set; }
        List<dtoBaseUserSubscription> SelectedUnsubscriptions();

        void InitializeControl(Int32 idProfile, List<dtoBaseUserSubscription> unsubscriptions);
       // void LoadSubscriptions(List<dtoUserSubscription> items);
        void LoadNothing();
    }
}