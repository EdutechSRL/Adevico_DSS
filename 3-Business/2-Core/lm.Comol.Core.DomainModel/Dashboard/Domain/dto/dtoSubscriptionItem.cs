using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoSubscriptionItem : lm.Comol.Core.DomainModel.DomainObject<Int32>
    {
        public dtoCommunityItem Community {get;set;}
        public Int32 IdRole {get;set;}
        public DateTime? LastAccessOn {get;set;}
        public DateTime? SubscribedOn { get; set; }
        public DateTime? PreviousAccessOn { get; set; }
        public Boolean HasNews {get;set;}
        public Boolean AllowUnsubscriptionFromOrganization { get; set; }
        public lm.Comol.Core.DomainModel.SubscriptionStatus Status { get; set; }
        public Boolean HasConstraints { get; set; }
        //public Boolean HasConstraints { get { return Constraints != null && Constraints.Any(); } }
        //public List<lm.Comol.Core.DomainModel.dtoCommunityConstraint> Constraints { get; set; }
        public dtoSubscriptionItem(){
            //Constraints = new List<DomainModel.dtoCommunityConstraint>();
        }
            
        public dtoSubscriptionItem(lm.Comol.Core.DomainModel.liteSubscriptionInfo s){
            IdRole = s.IdRole;
            Id = s.Id;
            LastAccessOn = s.LastAccessOn;
            SubscribedOn = s.SubscribedOn;
            PreviousAccessOn = s.PreviousAccessOn;
            Community = new dtoCommunityItem(s.Community, s.IdCommunity);
            if (s.Accepted && s.Enabled)
            {
                if (Community != null && Community.Status != Communities.CommunityStatus.Blocked)
                    Status = DomainModel.SubscriptionStatus.activemember;
                else
                    Status = DomainModel.SubscriptionStatus.communityblocked;
            }
            else if (!s.Enabled && s.Accepted)
                Status = DomainModel.SubscriptionStatus.blocked;
            else if (!s.Accepted)
                Status = DomainModel.SubscriptionStatus.waiting;
            //Constraints = new List<DomainModel.dtoCommunityConstraint>();
        }
        public Boolean AllowUnsubscribe()
        {
            return AllowUnsubscriptionFromOrganization && ((Community.AllowUnsubscribe && Status != DomainModel.SubscriptionStatus.blocked)) && Community.Status != lm.Comol.Core.Communities.CommunityStatus.Blocked;
        }
    }
}