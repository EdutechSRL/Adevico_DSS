using System.Runtime.Serialization;
using System;
using System.Linq;
using lm.Comol.Core.Communities;
using System.Collections.Generic;

namespace lm.Comol.Core.BaseModules.Dashboard.Domain
{
	[Serializable()]
    public class dtoCommunityPlainItem
	{
        public lm.Comol.Core.Dashboard.Domain.dtoCommunityItem Community { get; set; }
        public Int32 IdRole { get; set; }
        public DateTime? LastAccessOn { get; set; }
        public DateTime? SubscribedOn { get; set; }
        public DateTime? PreviousAccessOn { get; set; }
        public Boolean AllowUnsubscriptionFromOrganization { get; set; }
        public lm.Comol.Core.DomainModel.SubscriptionStatus Status { get; set; }
        public Boolean HasConstraints { get; set; }
        public Int32 PathDepth
        {
            get {
                return (Paths==null || !Paths.Any()) ? 0 :  (from p in Paths select p.PathDepth).Max();
            }
        }

        public List<dtoPathItem> Paths { get; set; }

        public dtoCommunityPlainItem()
        {
            Paths = new List<dtoPathItem>();
            Community = new Core.Dashboard.Domain.dtoCommunityItem();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCommunity"></param>
        /// <param name="name"></param>
        /// <param name="idType"></param>
        /// <param name="idOrganization"></param>
        /// <param name="idFather"></param>
        public dtoCommunityPlainItem(Int32 idCommunity,String name, Int32 idType,Int32 idOrganization, Int32 idFather)
        {
            Paths = new List<dtoPathItem>();
            Community = new Core.Dashboard.Domain.dtoCommunityItem() { Id = idCommunity,Name= name, IdOrganization = idOrganization, IdType = idType };
        }
#region "Path"
        public String PathsToString(String separator){
            string result ="";
            if (String.IsNullOrEmpty(separator))
                separator = "|";
            result = String.Join(separator, Paths.OrderBy(p=>p.isPrimary).Select(p => p.Path).ToArray());
            return result;
        }
        public List<String> PathsToList()
        {
            List<String> result = new List<String>();
            Paths.OrderBy(p=>p.isPrimary).ToList().ForEach(p=> result.Add(p.Path));
            return result;
        }
        public Boolean ContainsPath(String path)
        {
            return Paths.Where(p=>p.Path.Contains(path)).Any();
        }
        public Boolean ContainsPath(List<String> paths)
        {
            Boolean result = false;
            foreach (String path in paths) {
                result = ContainsPath(path);
                if (result)
                    break;
            }
            return result;
        }
#endregion


        public dtoCommunityPlainItem(lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node,List<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes, lm.Comol.Core.DomainModel.liteCommunityInfo community, Dictionary<Int32, List<long>> associations, Dictionary<Int32, String> responsibles, Dictionary<Int32, String> cTimes, Dictionary<Int32, String> degreesTypes, Dictionary<Int32, String> cTypes, lm.Comol.Core.DomainModel.liteSubscriptionInfo enrollment = null)
        {
            Paths = nodes.Select(i => new dtoPathItem() { IdFather = i.IdFather, FathersName = i.FathersName, isPrimary = i.isPrimary, Path = i.Path }).ToList();
            Community = CreateCommunity(node, community, associations, responsibles, cTimes, degreesTypes, cTypes, enrollment);
        }

        private lm.Comol.Core.Dashboard.Domain.dtoCommunityItem CreateCommunity(lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node, lm.Comol.Core.DomainModel.liteCommunityInfo community, Dictionary<Int32, List<long>> associations, Dictionary<Int32, String> responsibles, Dictionary<Int32, String> cTimes, Dictionary<Int32, String> degreesTypes, Dictionary<Int32, String> cTypes, lm.Comol.Core.DomainModel.liteSubscriptionInfo enrollment = null)
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
            if (cTimes != null && cTimes.ContainsKey(node.IdCourseTime))
                dto.CourseTime = cTimes[node.IdCourseTime];
            else
                dto.CourseTime = "";
            if (degreesTypes != null && degreesTypes.ContainsKey(node.IdDegreeType))
                dto.DegreeType = degreesTypes[node.IdDegreeType];
            else
                dto.DegreeType = "";
            if (cTypes.ContainsKey(node.IdCommunityType))
                dto.CommunityType= cTypes[node.IdCommunityType];

            if (enrollment != null)
            {
                IdRole = enrollment.IdRole;
                LastAccessOn = enrollment.LastAccessOn;
                SubscribedOn = enrollment.SubscribedOn;
                PreviousAccessOn = enrollment.PreviousAccessOn;
                if (enrollment.Accepted && enrollment.Enabled)
                {
                    if (Community != null && Community.Status != Communities.CommunityStatus.Blocked)
                        Status = DomainModel.SubscriptionStatus.activemember;
                    else
                        Status = DomainModel.SubscriptionStatus.communityblocked;
                }
                else if (!enrollment.Enabled && enrollment.Accepted)
                    Status = DomainModel.SubscriptionStatus.blocked;
                else if (!enrollment.Accepted)
                    Status = DomainModel.SubscriptionStatus.waiting;
            }
            return dto;
        }

        public String ToString()
        {
            return "Community - Id " + ((Community != null) ? Community.Id.ToString() : "0" )+ " IdRole: " + IdRole.ToString();
        }
    }
}