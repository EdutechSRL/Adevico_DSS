using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Domain
{
    [Serializable]
    public class dtoCommunityNodeItem 
    {
        public String UniqueId { get; set; }
        public Int32 IdCommunity { get { return Details == null ? 0 : Details.Community.Id; } }
        public String Displayname { get; set; }
        public String CurrentPath { get; set; }
        public dtoNodeDetails Details { get; set; }
        public NodeType Type { get; set; }
        public Boolean HasCurrent { get; set; }
        public Boolean IsCurrent { get; set; }
        public String Responsible { get { return (Details != null && Details.Community != null) ? Details.Community.Responsible : ""; } }
        public Boolean ForAdvanced { get; set; }
        
        public dtoCommunityNodeItem()
        {
        }

        public void LoadConstraints(List<dtoCommunityConstraint> items)
        {
            Details.LoadConstraints(items);
        }
       
        public String ToString()
        {
            return Type.ToString() + IdCommunity.ToString() + Displayname;
        }
    }

    [Serializable]
    public class dtoCommunityNodeItemPermission
    {
        public Boolean ViewDetails { get; set; }
        public Boolean ViewNews {get;set;}
        public Boolean EnrollTo { get; set; }
        public Boolean UnsubscribeFrom { get; set; }
        public Boolean AccessTo { get; set; }
    }

    [Serializable]
    public class dtoNodeDetails
    {
        public lm.Comol.Core.Dashboard.Domain.dtoCommunityItem Community { get; set; }
        public long EnrolledUsers { get; set; }
        public long AvailableSeats { get { return (Community == null || Community.MaxUsersWithDefaultRole <= 0) ? Int32.MaxValue : ((Community.MaxUsersWithDefaultRole + Community.MaxOverDefaultSubscriptionsAllowed) - EnrolledUsers); } }

        public Boolean AllowSubscribe { get { return !NotAvailableFor.Any(); } }
        public Boolean HasConstraints { get { return Constraints != null && Constraints.Any(); } }
        public Boolean HasConstraintsInfo { get { return Community != null && (HasConstraints || Community.AllowSubscription || Community.AllowUnsubscribe || Community.ConfirmSubscription); } }
        public List<EnrollingStatus> NotAvailableFor { get; set; }
        public List<dtoCommunityConstraint> Constraints { get; set; }
        public dtoCommunityNodeItemPermission Permissions { get; set; }
        public DateTime? LastAccessOn { get; set; }
        public DateTime? EnrolledOn { get; set; }

        public dtoNodeDetails()
        {
            Permissions = new dtoCommunityNodeItemPermission();
            NotAvailableFor = new List<EnrollingStatus>();
            Constraints = new List<dtoCommunityConstraint>();
        }

        public dtoNodeDetails(liteCommunityInfo community, Int32 idCommunity,Dictionary<Int32, long> enrolledUsers = null, List<liteCommunityConstraint> constraints = null,liteSubscriptionInfo enrollment = null)
        {
            Permissions = new dtoCommunityNodeItemPermission();
            NotAvailableFor = new List<EnrollingStatus>();
            Constraints = new List<dtoCommunityConstraint>();
            Community = new lm.Comol.Core.Dashboard.Domain.dtoCommunityItem(community, idCommunity);
            if (enrollment != null)
            {
                LastAccessOn  = enrollment.LastAccessOn;
                EnrolledOn  = enrollment.SubscribedOn;
            }
            if (AvailableSeats <= 0)
                NotAvailableFor.Add(EnrollingStatus.Seats);
            if (!Community.AllowSubscription)
                NotAvailableFor.Add(EnrollingStatus.Unavailable);
            else
            {
                if (!Community.IsAvailableForSubscriptionStartOn(DateTime.Now))
                    NotAvailableFor.Add(EnrollingStatus.StartDate);
                else if (!Community.IsAvailableForSubscriptionEndOn(DateTime.Now))
                    NotAvailableFor.Add(EnrollingStatus.EndDate);
            }

            if (enrolledUsers != null && enrolledUsers.ContainsKey(idCommunity))
            {
                EnrolledUsers = enrolledUsers[idCommunity];
                if(AvailableSeats<=0)
                    NotAvailableFor.Add(EnrollingStatus.Seats);
            }
        }
        public void LoadConstraints(List<dtoCommunityConstraint> items)
        {
            Constraints = items;
            if (Constraints.Where(c => !c.IsRespected).Any())
                NotAvailableFor.Add(EnrollingStatus.Constraints);
        }
    }


    [Serializable]
    public enum NodeType : int
    {
        Root = 0,
        Community = 1,
        Virtual = 2,
        OpenCommunityNode = 3,
        OpenVirtualNode = 4,
        CloseNode = 5,
        OpenChildren = 6,
        CloseChildren = 7,
        NoChildren = 8
    }
}