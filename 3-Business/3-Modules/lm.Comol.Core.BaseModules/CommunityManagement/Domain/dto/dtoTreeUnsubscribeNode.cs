
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Communities;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable(), CLSCompliant(true)]
    public class dtoUnsubscribeTreeNode : dtoBaseCommunityNode, ICloneable
	{
        public Int32 IdOrganization { get; set; }
        public Int32 IdRole { get; set; }
        public String Name { get; set; }
        public Boolean isPrimary { get; set; }
        public Boolean AllowUnsubscriptionFromOrganization { get; set; }
        public Boolean CommunityAllowUnsubscribe { get; set; }
        public DateTime? CommunitySubscriptionEndOn { get; set; }
        public Boolean CommunityAllowSubscription { get; set; }
        public Int32 MaxUsersWithDefaultRole { get; set; }
        
        
        public dtoUnsubscribeTreeNode Father { get; set; }
        public CommunityStatus CommunityStatus {get; set;}
        public List<dtoUnsubscribeTreeNode> Nodes { get; set; }
        public lm.Comol.Core.DomainModel.SubscriptionStatus SubscriptionStatus { get; set; }

        public dtoUnsubscribeTreeNode()
        {
            Nodes = new List<dtoUnsubscribeTreeNode>();
            AllowUnsubscriptionFromOrganization = true;
        }
        public dtoUnsubscribeTreeNode(liteCommunityInfo community, liteSubscriptionInfo subscription, String path)
        {
            Id = community.Id;
            IdFather = community.IdFather;
            IdCreatedBy = community.IdCreatedBy;
            IdOrganization = community.IdOrganization;
            this.isPrimary = true;
            Name = community.Name;
            Path = path;
            AllowUnsubscriptionFromOrganization = true;
            CommunityAllowUnsubscribe = community.AllowUnsubscribe;
            CommunitySubscriptionEndOn = community.SubscriptionEndOn;
            
            CommunityStatus = (community.isClosedByAdministrator) ? CommunityStatus.Blocked : ((community.isArchived) ? CommunityStatus.Stored : CommunityStatus.Active);
            Nodes = new List<dtoUnsubscribeTreeNode>();
            SubscriptionStatus = GetSubscriptionStatus(subscription);
            IdRole = (subscription != null) ? subscription.IdRole : -3;
            CommunityAllowSubscription = community.AllowSubscription;
            MaxUsersWithDefaultRole = community.MaxUsersWithDefaultRole;
        }

        private lm.Comol.Core.DomainModel.SubscriptionStatus GetSubscriptionStatus(liteSubscriptionInfo subscription)
        {
            if (subscription!=null){
                if (subscription.IdRole < 1)
                    return DomainModel.SubscriptionStatus.none;
                else if (subscription.Accepted && subscription.Enabled)
                    return DomainModel.SubscriptionStatus.activemember;
                else if (subscription.Accepted && !subscription.Enabled)
                    return DomainModel.SubscriptionStatus.blocked;
                else if (!subscription.Accepted && !subscription.Enabled)
                    return DomainModel.SubscriptionStatus.waiting;
                else
                    return DomainModel.SubscriptionStatus.none;
            }
            else
                return DomainModel.SubscriptionStatus.none;
        }


        public Boolean AllowUnsubscribe()
        {
            //SubscriptionStatus == lm.Comol.Core.DomainModel.SubscriptionStatus.waiting ||
            return SubscriptionStatus!= DomainModel.SubscriptionStatus.none && AllowUnsubscriptionFromOrganization && ( (SubscriptionStatus != DomainModel.SubscriptionStatus.blocked && CommunityAllowUnsubscribe)) && CommunityStatus != lm.Comol.Core.Communities.CommunityStatus.Blocked;
        }

        public List<dtoUnsubscribeTreeNode> GetAllNodes(Boolean addSelf = true )
        {
            List<dtoUnsubscribeTreeNode> results = new List<dtoUnsubscribeTreeNode>();
            if (addSelf)
                results.Add(this);
            foreach (dtoUnsubscribeTreeNode n in Nodes)
            {
                results.Add(n);
                results.AddRange(n.GetAllNodes(false));
            }
            return results;
        }

        public object Clone()
        {
            dtoUnsubscribeTreeNode item = new dtoUnsubscribeTreeNode();
            item.Id = Id;
            item.IdFather = IdFather;
            item.IdOrganization = IdOrganization;
            item.Name = Name;
            item.isPrimary = isPrimary;
            item.CommunityStatus = CommunityStatus;
            item.Nodes = new List<dtoUnsubscribeTreeNode>();
            item.Path = Path;
            item.SubscriptionStatus = SubscriptionStatus;
            item.AllowUnsubscriptionFromOrganization = AllowUnsubscriptionFromOrganization;
            item.CommunityAllowUnsubscribe = CommunityAllowUnsubscribe;
            item.IdRole = IdRole;
            item.CommunityAllowSubscription = CommunityAllowSubscription;
            item.MaxUsersWithDefaultRole = MaxUsersWithDefaultRole;
            item.CommunitySubscriptionEndOn = CommunitySubscriptionEndOn;
            item.IdCreatedBy = IdCreatedBy;
            return item;
        }

        public String ToString()
        {
            return "Id - " + Id.ToString() + " Path - " + Path + " Name - " + Name + " CommunityStatus - " + CommunityStatus.ToString() + " SubscriptionStatus - " + SubscriptionStatus.ToString();
        }
    }
}