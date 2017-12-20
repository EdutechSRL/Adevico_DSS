using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoCommunityItem :dtoBaseCommunityItem
    {
        public DateTime CreatedOn {get;set;}
        public DateTime? ClosedOn {get;set;}
        public DateTime? SubscriptionStartOn {get;set;}
        public DateTime? SubscriptionEndOn {get;set;}
        public Int32 MaxUsersWithDefaultRole  {get;set;}
        public Int32 MaxOverDefaultSubscriptionsAllowed  {get;set;}
        public Boolean AllowUnsubscribe {get;set;}
        public Boolean AllowSubscription {get;set;}
        public Boolean ConfirmSubscription { get; set; }
        public String Path { get; set; }
        public String CommunityType { get; set; }
        public String DegreeType { get; set; }
        public String Year { get; set; }
        public String CourseTime { get; set; }
        public String Responsible { get; set; }
        public dtoCommunityItem(){}

        public dtoCommunityItem(lm.Comol.Core.DomainModel.liteCommunityInfo community, Int32 idCommunity){
            if (community != null)
            {
                Id = community.Id;
                Name = community.Name;
                IdOrganization = community.IdOrganization;
                IdType = community.IdTypeOfCommunity;
                CreatedOn = community.CreatedOn;
                ClosedOn = community.ClosedOn;
                SubscriptionStartOn = community.SubscriptionStartOn;
                SubscriptionEndOn = community.SubscriptionEndOn;
                MaxUsersWithDefaultRole = community.MaxUsersWithDefaultRole;
                MaxOverDefaultSubscriptionsAllowed = community.MaxOverDefaultSubscriptionsAllowed;
                AllowUnsubscribe = community.AllowUnsubscribe;
                AllowSubscription = community.AllowSubscription;
                if (community.isArchived)
                    Status = Communities.CommunityStatus.Stored;
                else if (community.isClosedByAdministrator)
                    Status = Communities.CommunityStatus.Blocked;
                else
                    Status = Communities.CommunityStatus.Active;
                ConfirmSubscription = community.ConfirmSubscription;
            }
            else
            {
                Id = -idCommunity;
                Status = Communities.CommunityStatus.Blocked;
            }
        }

        public Boolean IsAvailable(DateTime date)
        {
            return (!SubscriptionStartOn.HasValue || SubscriptionStartOn.HasValue && SubscriptionStartOn.Value <= date)
                    &&
                    (!SubscriptionEndOn.HasValue || SubscriptionEndOn.HasValue && SubscriptionEndOn.Value > date);
        }
        public Boolean IsAvailableForSubscriptionStartOn(DateTime date)
        {
            return (!SubscriptionStartOn.HasValue || SubscriptionStartOn.HasValue && SubscriptionStartOn.Value <= date);
        }
        public Boolean IsAvailableForSubscriptionEndOn(DateTime date)
        {
            return (!SubscriptionEndOn.HasValue || SubscriptionEndOn.HasValue && SubscriptionEndOn.Value > date);
        }
    }
}