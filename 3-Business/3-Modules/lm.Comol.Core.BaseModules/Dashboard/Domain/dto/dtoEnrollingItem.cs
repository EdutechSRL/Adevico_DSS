using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Domain
{
    [Serializable]
    public class dtoEnrollingItem 
    {
        public String PrimaryPath { get; set; }
        public List<String> AvailablePath { get; set; }
        public lm.Comol.Core.Dashboard.Domain.dtoCommunityItem Community { get; set; }
        public long EnrolledUsers { get; set; }
        public long AvailableSeats { get { return (Community == null || Community.MaxUsersWithDefaultRole <= 0) ? Int32.MaxValue : ((Community.MaxUsersWithDefaultRole + Community.MaxOverDefaultSubscriptionsAllowed) - EnrolledUsers); } }

        public Boolean AllowSubscribe { get { return !NotAvailableFor.Any(); } }
        public Boolean HasConstraints { get { return Constraints != null && Constraints.Any(); } }
        public Boolean HasConstraintsInfo { get { return HasConstraints || Community.AllowSubscription || Community.AllowUnsubscribe || Community.ConfirmSubscription; } }
        public List<EnrollingStatus> NotAvailableFor { get; set; }
        public List<dtoCommunityConstraint> Constraints { get; set; }
        public dtoEnrollingItem()
        {
            NotAvailableFor = new List<EnrollingStatus>();
            Constraints = new List<dtoCommunityConstraint>();
            AvailablePath = new List<string>();
            EnrolledUsers = 0;
        }
        public dtoEnrollingItem(lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node, liteCommunityInfo community, Dictionary<Int32, List<long>> associations,Dictionary<Int32,String> responsibles, Dictionary<bool, List<string>> aPaths = null)
        {
            NotAvailableFor = new List<EnrollingStatus>();
            Constraints = new List<dtoCommunityConstraint>();
            if (aPaths == null)
            {
                PrimaryPath = node.Path;
                AvailablePath = new List<string>() { node.Path };
            }
            else
            {
                PrimaryPath = "";// (node.isPrimary) ? node.Path : "";
                AvailablePath = new List<string>();
                if (aPaths.ContainsKey(true))
                    AvailablePath.Add(aPaths[true].FirstOrDefault());
                if (aPaths.ContainsKey(false))
                    AvailablePath.AddRange(aPaths[false]);
            }
            Community = CreateCommunity(node, community, associations, responsibles);
        }

        private lm.Comol.Core.Dashboard.Domain.dtoCommunityItem CreateCommunity(lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node, liteCommunityInfo community, Dictionary<Int32, List<long>> associations,Dictionary<Int32,String> responsibles)
        {
            lm.Comol.Core.Dashboard.Domain.dtoCommunityItem dto = new lm.Comol.Core.Dashboard.Domain.dtoCommunityItem();
            dto.Id = node.Id;
            dto.IdOrganization = node.IdOrganization;
            dto.IdTags = (associations.ContainsKey(node.Id) ? associations[node.Id] : new List<long>());
            dto.IdType = node.IdCommunityType;
            dto.Name = node.Name;
            dto.Status = node.Status;
            if (community.isArchived)
                dto.Status = Communities.CommunityStatus.Stored;
            else if (community.isClosedByAdministrator)
                dto.Status = Communities.CommunityStatus.Blocked;
            else
                dto.Status = Communities.CommunityStatus.Active;
            dto.Tags = new List<string>();

            dto.AllowSubscription = community.AllowSubscription;
            dto.AllowUnsubscribe = community.AllowUnsubscribe;
            dto.ClosedOn= community.ClosedOn;
            dto.MaxOverDefaultSubscriptionsAllowed = community.MaxOverDefaultSubscriptionsAllowed;
            dto.MaxUsersWithDefaultRole = community.MaxUsersWithDefaultRole;
            dto.SubscriptionEndOn = community.SubscriptionEndOn;
            dto.ConfirmSubscription = community.ConfirmSubscription;
            dto.SubscriptionStartOn = community.SubscriptionStartOn;
            if (node.Year > 0)
                dto.Year = node.Year.ToString() + "/" + (node.Year + 1).ToString();
            else
                dto.Year = "";
            if (responsibles != null && responsibles.ContainsKey(node.IdResponsible))
                dto.Responsible = responsibles[node.IdResponsible];
            else
                dto.Responsible = "";
                dto.CourseTime = "";
                dto.DegreeType = "";
            return dto;
        }

        public static List<EnrollingStatus> GetEnrollingStatus(dtoEnrollingItem item){
            List<EnrollingStatus> status = new List<EnrollingStatus>();
            if (!item.Community.AllowSubscription)
                status.Add(EnrollingStatus.Unavailable);
            else
            {
                if (!item.Community.IsAvailableForSubscriptionStartOn(DateTime.Now))
                    status.Add(EnrollingStatus.StartDate);
                else if (!item.Community.IsAvailableForSubscriptionEndOn(DateTime.Now))
                    status.Add(EnrollingStatus.EndDate);
            }
            return status;
        }

        public String PrintAvailableSeats(String delimeter)
        {
            if (Community == null || Community.MaxUsersWithDefaultRole <= 0)
                return "";
            else
                return EnrolledUsers.ToString() + delimeter + Community.MaxUsersWithDefaultRole.ToString();
        }
    }
}