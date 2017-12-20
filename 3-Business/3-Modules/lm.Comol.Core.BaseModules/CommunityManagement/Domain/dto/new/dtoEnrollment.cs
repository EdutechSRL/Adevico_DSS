using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable]
    public class dtoEnrollment
    {
        public Int32 IdCommunity { get; set; }
        public String CommunityName { get; set; }
        public dtoEnrollmentInfo ExtendedInfo { get; set; }
     
        public EnrolledStatus Status { get; set; }
        public DateTime EnrollOn {get;set;}
        public DateTime EnrolledOn {get;set;}
        public DateTime? SubscriptionStartOn {get;set;}
        public DateTime? SubscriptionEndOn {get;set;}
        public List<EnrollingStatus> NotAvailableFor { get; set; }
        public Boolean IsEnrollAvailable { get { return NotAvailableFor == null || !NotAvailableFor.Any(); } }
        public List<dtoCommunityConstraint> Constraints { get; set; }
        public dtoEnrollment()
        {
            NotAvailableFor = new List<EnrollingStatus>();
            Constraints = new List<dtoCommunityConstraint>();
            ExtendedInfo = new dtoEnrollmentInfo(); 
        }

        public dtoEnrollment(DateTime enrollOn,  liteCommunityInfo community,List<dtoCommunityConstraint> constraints)
        {
            NotAvailableFor = new List<EnrollingStatus>();
            Constraints = constraints;

            IdCommunity = community.Id;
            CommunityName = community.Name;
            EnrollOn = enrollOn;
            if (!community.AllowSubscription)
                NotAvailableFor.Add(EnrollingStatus.Unavailable);
            else
            {
                if (!IsAvailableForSubscriptionStartOn(DateTime.Now))
                    NotAvailableFor.Add(EnrollingStatus.StartDate);
                else if (!IsAvailableForSubscriptionEndOn(DateTime.Now))
                    NotAvailableFor.Add(EnrollingStatus.EndDate);
            }
            if (constraints.Where(c => !c.IsRespected).Any())
                NotAvailableFor.Add(EnrollingStatus.Constraints);

            if (NotAvailableFor.Any())
                Status = EnrolledStatus.NotAvailable;
            else
                Status = EnrolledStatus.None;
            ExtendedInfo = new dtoEnrollmentInfo(); 
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

    [Serializable]
    public class dtoEnrollmentInfo {
        public litePerson Responsible { get; set; }
        public Language Language { get; set; }
        public Int32 IdRole { get; set; }
        public String RoleName { get; set; }
        public Boolean IsValid()
        {
            return (Responsible != null && Responsible.Id > 0);
        }
        public Int32 GetIdLanguage()
        {
            return (Language == null) ? 0 : Language.Id;
        }
    }
}