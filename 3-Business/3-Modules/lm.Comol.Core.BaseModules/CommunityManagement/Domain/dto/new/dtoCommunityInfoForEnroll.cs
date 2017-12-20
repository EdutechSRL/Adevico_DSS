using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable]
    public class dtoCommunityInfoForEnroll:dtoCommunityToEnroll 
    {
        public String Name { get; set; }
        public lm.Comol.Core.Communities.CommunityStatus Status { get; set; }
        public DateTime EnrolledOn {get;set;}

        public DateTime? SubscriptionStartOn {get;set;}
        public DateTime? SubscriptionEndOn {get;set;}
        public Int32 IdDefaultRole { get; set; }
        public Int32 MaxUsersWithDefaultRole  {get;set;}
        public Int32 MaxOverDefaultSubscriptionsAllowed  {get;set;}
        public Boolean AllowUnsubscribe {get;set;}
        public Boolean AllowSubscription { get; set; }
        public Boolean ConfirmSubscription { get; set; }
        
        public long EnrolledUsers { get; set; }
        public long AvailableSeats { get { return (MaxUsersWithDefaultRole <= 0) ? Int32.MaxValue : ((MaxUsersWithDefaultRole + MaxOverDefaultSubscriptionsAllowed) - EnrolledUsers); } }

        public Boolean AllowEnroll { get { return !NotAvailableFor.Any(); } }
        public Boolean HasConstraints { get { return Constraints != null && Constraints.Any(); } }
        public List<EnrollingStatus> NotAvailableFor { get; set; }
        public List<dtoCommunityConstraint> Constraints { get; set; }
        public dtoCommunityInfoForEnroll()
        {
            NotAvailableFor = new List<EnrollingStatus>();
            Constraints = new List<dtoCommunityConstraint>();
            EnrolledUsers = 0;
        }
        public dtoCommunityInfoForEnroll(Int32 idCommunity, String path, liteCommunityInfo community, Dictionary < Int32, Int32 > dRoles,Dictionary<Int32, long> enrolledUsers)
        {
            NotAvailableFor = new List<EnrollingStatus>();
            Constraints = new List<dtoCommunityConstraint>();
            AllowSubscription = !(community == null);
            Id = idCommunity;
            Path = path;
            EnrolledOn = DateTime.Now;
            EnrolledUsers = (enrolledUsers != null && enrolledUsers.ContainsKey(idCommunity)) ? enrolledUsers[idCommunity] : 0;
            IdDefaultRole = (dRoles != null && community != null && dRoles.ContainsKey(community.IdTypeOfCommunity)) ? dRoles[community.IdTypeOfCommunity] : 0;
            if (community != null)
            {
                if (community.isArchived)
                    Status = Communities.CommunityStatus.Stored;
                else if (community.isClosedByAdministrator)
                    Status = Communities.CommunityStatus.Blocked;
                else
                    Status = Communities.CommunityStatus.Active;
                Name = community.Name;
                AllowSubscription = community.AllowSubscription;
                AllowUnsubscribe = community.AllowUnsubscribe;
                ConfirmSubscription = community.ConfirmSubscription;
                MaxOverDefaultSubscriptionsAllowed = community.MaxOverDefaultSubscriptionsAllowed;
                MaxUsersWithDefaultRole = community.MaxUsersWithDefaultRole;
                SubscriptionEndOn = community.SubscriptionEndOn;
                SubscriptionStartOn = community.SubscriptionStartOn;

                if (!AllowSubscription)
                    NotAvailableFor.Add(EnrollingStatus.Unavailable);
                else
                {
                    if (!IsAvailableForSubscriptionStartOn(DateTime.Now))
                        NotAvailableFor.Add(EnrollingStatus.StartDate);
                    else if (!IsAvailableForSubscriptionEndOn(DateTime.Now))
                        NotAvailableFor.Add(EnrollingStatus.EndDate);
                }
            }
            else
                Id = -Id;
            if (AvailableSeats<=0)
                NotAvailableFor.Add(EnrollingStatus.Seats);
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

        public String PrintAvailableSeats(String delimeter)
        {
            if (MaxUsersWithDefaultRole <= 0)
                return "";
            else
                return EnrolledUsers.ToString() + delimeter + MaxUsersWithDefaultRole.ToString();
        }
    }
}