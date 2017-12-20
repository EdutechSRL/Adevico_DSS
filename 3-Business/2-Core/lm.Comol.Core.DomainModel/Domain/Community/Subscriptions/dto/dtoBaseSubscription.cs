using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Subscriptions
{
    [Serializable]
    public class dtoBaseSubscription : DomainObject<int>
    {
        public virtual Int32 IdPerson { get; set; }
        public virtual Int32 IdCommunity {get; set;}
        public virtual String CommunityName {get; set;}
        public virtual String CommunityLogo {get; set;}
        public virtual DateTime? SubscriptedOn { get; set; }
        public virtual DateTime? LastAccessOn {get; set;}
        public virtual Boolean isEnabled {get; set;}
        public virtual Boolean isResponsabile { get; set; }
        public virtual Role Role { get; set; }
        public virtual SubscriptionStatus Status { get; set; }

        public dtoBaseSubscription() { }

        public dtoBaseSubscription(Boolean isAccepted, Boolean isEnabled, Boolean isResponsabile)
        {
            if (!isAccepted && !isEnabled)
                Status = SubscriptionStatus.waiting;
            else if (!isEnabled)
                Status = SubscriptionStatus.blocked;
            else
                Status = SubscriptionStatus.activemember;

            if (isResponsabile)
                Status |= SubscriptionStatus.responsible;
        }
    }
}