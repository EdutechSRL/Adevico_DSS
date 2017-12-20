using System;
using lm.Comol.Core.Subscriptions;
namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class liteSubscription : lm.Comol.Core.DomainModel.DomainObject<int>
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdRole { get; set; }
        public litePerson Person { get; set; }
        public System.DateTime? SubscriptedOn { get; set; }
        public System.DateTime? LastAccessOn { get; set; }
        public Boolean Accepted { get; set; }
        public Boolean Enabled { get; set; }
        public Boolean isResponsabile { get; set; }
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