using System;
using lm.Comol.Core.Subscriptions;
namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class Subscription : DomainObject<int>, iSubscription
    {
        public string Name { get; set; }
        public iCommunity Community { get; set; }
        public iRole Role { get; set; }
        public iPerson Person { get; set; }
        public System.DateTime? SubscriptedOn { get; set; }
        public System.DateTime? LastAccessOn { get; set; }
        public bool Accepted { get; set; }
        public bool Enabled { get; set; }
        public bool isResponsabile { get; set; }
        public Boolean DisplayMail { get; set; }
        public SubscriptionStatus Status { 
            get {
                SubscriptionStatus status = SubscriptionStatus.none;
                if (!Accepted && !Enabled)
                    status = SubscriptionStatus.waiting;
                else if (!Enabled)
                    status = SubscriptionStatus.blocked;
/*                else if (Accepted && Enabled)
                    status= SubscriptionStatus.*/
                if (isResponsabile)
                    status |= SubscriptionStatus.responsible;
                return status;
            }
            
        }
    }
}