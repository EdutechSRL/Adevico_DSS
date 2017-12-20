
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Communities;
namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable(), CLSCompliant(true)]
    public class dtoTreeCommunityNode : dtoCommunityNode, ICloneable
    {
        public List<dtoTreeCommunityNode> Nodes { get; set; }
        public List<String> FathersName { get; set; }
        public dtoTreeCommunityNode Father { get; set; }
        public dtoTreeCommunityNode FirstFather { get; set; }
        public Int32 IdFirstFatherOrganization { get; set; }
        public List<long> IdTags { get; set; }
        private Boolean IsTagsSet { get; set; }
        public dtoTreeCommunityNode()
        {
            IdTags = new List<long>();
            Nodes = new List<dtoTreeCommunityNode>();
            FathersName = new List<String>();
        }

        public List<dtoTreeCommunityNode> GetNodesByType(dtoCommunityNodeType type)
        {
            List<dtoTreeCommunityNode> oList = (from l in Nodes where (l.Type & type) >= 0 select l).ToList();

            foreach (dtoTreeCommunityNode node in Nodes)
            {
                oList.AddRange(node.GetNodesByType(type));
            }
            if ((oList == null))
            {
                oList = new List<dtoTreeCommunityNode>();
            }
            return oList;
        }
        public dtoTreeCommunityNode(dtoCommunityNode node)
        {
            Id = node.Id;
            IdFather = node.IdFather;
            IdOrganization = node.IdOrganization;
            Name = node.Name;
            ToolTip = node.ToolTip;
            isPrimary = node.isPrimary;
            IdCommunityType = node.IdCommunityType;
            Type = node.Type;
            Status = node.Status;
            Selected = node.Selected;
            AccessAvailable = node.AccessAvailable;
            IdResponsible = node.IdResponsible;
            IdDegreeType = node.IdDegreeType;
            IdCourseTime = node.IdCourseTime;
            CourseCode = node.CourseCode;
            Year = node.Year;
            FathersName = new List<String>();
            ConfirmSubscription = node.ConfirmSubscription;
            isClosedByAdministrator = node.isClosedByAdministrator;
            Nodes = new List<dtoTreeCommunityNode>();
            IdTags = new List<long>();
        }

        public dtoTreeCommunityNode Filter(dtoCommunitiesFilters filter, Int32 IdDefaultOrganization, List<Int32> onlyFromOrganizations = null)
        {
            //return FullClone(this, null).InternalFilter(filter, IdDefaultOrganization, onlyFromOrganizations);
            return InternalFilter(filter, IdDefaultOrganization, onlyFromOrganizations);
        }

        public dtoTreeCommunityNode InternalFilter(dtoCommunitiesFilters filter, Int32 IdDefaultOrganization, List<Int32> onlyFromOrganizations = null)
        {
            dtoTreeCommunityNode root = (dtoTreeCommunityNode)this.Clone();
            if (root.Type != dtoCommunityNodeType.Root && filter.Status != CommunityStatus.None && filter.Status != this.Status)
                root.Type = dtoCommunityNodeType.NotSelectable;

            if (root.Type != dtoCommunityNodeType.Root && root.Type != dtoCommunityNodeType.NotSelectable && filter.Availability != CommunityAvailability.All)
                root.Type = (filter.Availability == CommunityAvailability.NotSubscribed && this.Selected) ? dtoCommunityNodeType.NotSelectable
                    : this.Type;
            if (root.Type != dtoCommunityNodeType.NotSelectable && this.IdOrganization == IdDefaultOrganization && this.IdFather == 0)
                root.Type = dtoCommunityNodeType.NotSelectable;

            if (root.Type != dtoCommunityNodeType.Root && !HasNodes(filter))
                return root;
            else
            {
                foreach (dtoTreeCommunityNode n in this.Nodes.Where(n => onlyFromOrganizations == null || (onlyFromOrganizations != null && onlyFromOrganizations.Contains(n.IdOrganization))).OrderBy(nn => nn.Name).ToList())
                {
                    if (n.HasNodes(filter))
                    {
                        dtoTreeCommunityNode subNode = n.InternalFilter(filter, IdDefaultOrganization, onlyFromOrganizations);
                        if (subNode != null)
                            root.Nodes.Add(subNode);
                    }
                }
                if (root.Type == dtoCommunityNodeType.Root)
                    root.Type = dtoCommunityNodeType.Root;
                return root;
            }
        }
        public Boolean HasNodes(dtoCommunitiesFilters filter)
        {
            Boolean result = false;
            var query = (from n in this.Nodes select n);
            if (filter.IdOrganization > 0)
                query = query.Where(n => n.IdOrganization == filter.IdOrganization);
            else if (filter.OnlyFromAvailableOrganizations)
                query = query.Where(n => filter.AvailableIdOrganizations.Contains(n.IdOrganization));
            if (filter.IdcommunityType > -1)
                query = query.Where(n => n.IdCommunityType == filter.IdcommunityType);
            else if (filter.IdRemoveCommunityType >-1)
                query = query.Where(n => n.IdCommunityType != filter.IdRemoveCommunityType);
            if (filter.Status != CommunityStatus.None)
                query = query.Where(n => n.Status == filter.Status);
            if (filter.IdResponsible != -1)
                query = query.Where(n => n.IdResponsible == filter.IdResponsible);
            if (filter.Availability != CommunityAvailability.All)
                query = query.Where(n => (filter.Availability == CommunityAvailability.Subscribed && n.Selected) || (filter.Availability == CommunityAvailability.NotSubscribed && !n.Selected));
            if (filter.IdCourseTime >0)
                query = query.Where(n => n.IdCourseTime == filter.IdCourseTime);
            if (filter.IdDegreeType > 0)
                query = query.Where(n => n.IdCourseTime == filter.IdDegreeType);
            if (!String.IsNullOrEmpty(filter.CourseCode))
                query = query.Where(n => !String.IsNullOrEmpty(n.CourseCode) && n.CourseCode.ToLower().Contains(filter.CourseCode.Trim().ToLower()));
            if (filter.Year > 0)
                query = query.Where(n => n.Year == filter.Year);
            if (filter.IdTags.Any() && IsTagsSet)
                query = query.Where(n => n.IdTags.Where(t=> filter.IdTags.Contains(t)).Any());
            if (filter.SearchBy != SearchCommunitiesBy.All) {
                switch (filter.SearchBy) { 
                    case SearchCommunitiesBy.Contains:
                        if (!String.IsNullOrEmpty(filter.Value))
                            query = query.Where(n => !String.IsNullOrEmpty(n.Name) && n.Name.ToLower().Contains(filter.Value.ToLower()));
                        break;
                    case SearchCommunitiesBy.NameStartAs:
                        if (!String.IsNullOrEmpty(filter.Value))
                            query = query.Where(n => !String.IsNullOrEmpty(n.Name) && n.Name.ToLower().StartsWith(filter.Value.ToLower()));
                        break;
                    case SearchCommunitiesBy.StartAs:
                        if (filter.StartWith != "#")
                            query = query.Where(n => string.Compare(n.FirstLetter, filter.StartWith.ToLower(), true) == 0);
                        else
                            query = query.Where(n => DefaultOtherChars().Contains(n.FirstLetter));

                        break;
                }
            }
            Boolean hasSubNodes = query.Any();

            if (filter.IdOrganization > 0)
                result = (this.IdOrganization == filter.IdOrganization || hasSubNodes);
            else if (filter.OnlyFromAvailableOrganizations && filter.AvailableIdOrganizations.Any())
                result = (filter.AvailableIdOrganizations.Contains(this.IdOrganization) || hasSubNodes);
            else
                result = true;
            // TO CHECK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (filter.IdcommunityType >= 0)
                result = result && (((!filter.OnlyFromAvailableOrganizations && filter.IdOrganization < 1) || result)
                    && ((this.IdCommunityType == filter.IdcommunityType ) || hasSubNodes));
            else if (filter.IdRemoveCommunityType > -1)
                result = result && (((!filter.OnlyFromAvailableOrganizations && filter.IdOrganization < 1) || result)
                    && (this.IdCommunityType != filter.IdRemoveCommunityType || hasSubNodes));

            /* result = (((filter.OnlyFromAvailableOrganizations ) || (!filter.OnlyFromAvailableOrganizations && filter.IdOrganization < 1)) || result)
                    && (this.IdCommunityType == filter.IdcommunityType || hasSubNodes);*/
            if (filter.IdTags.Any() && IsTagsSet)
                result = (result && (IdTags.Where(t=> filter.IdTags.Contains(t)).Any() || hasSubNodes));

            if (filter.Status !=  CommunityStatus.None )
                result = result && (((filter.IdcommunityType < 1 && filter.IdcommunityType < 0) || result)
                    && (this.Status == filter.Status || hasSubNodes));
            if (filter.Availability !=  CommunityAvailability.All)
                result = result && (((filter.IdcommunityType < 1 && filter.IdcommunityType < 0 && filter.Status == CommunityStatus.None) || result)
                    && (
                    ((filter.Availability == CommunityAvailability.Subscribed && this.Selected)
                    || (filter.Availability == CommunityAvailability.NotSubscribed && !this.Selected)) || hasSubNodes
                    ));
            if (filter.SearchBy != SearchCommunitiesBy.All && !String.IsNullOrEmpty(this.Name))
            {
                switch (filter.SearchBy)
                {
                    case SearchCommunitiesBy.Contains:
                        if (!String.IsNullOrEmpty(filter.Value))
                            result = (result && Name.ToLower().Contains(filter.Value.ToLower()) || hasSubNodes);
                        break;
                    case SearchCommunitiesBy.NameStartAs:
                        if (!String.IsNullOrEmpty(filter.Value))
                            result = (result && Name.ToLower().StartsWith(filter.Value.ToLower()) || hasSubNodes);
                        break;
                    case SearchCommunitiesBy.StartAs:
                        if (filter.StartWith != "#")
                            result = ((result && string.Compare(FirstLetter, filter.StartWith.ToLower(), true) == 0) || hasSubNodes);
                        else
                            result = ((result && DefaultOtherChars().Contains(FirstLetter)) || hasSubNodes);
                        break;
                }
            }
            if (filter.IdResponsible != -1)
                result = ((result && filter.IdResponsible==IdResponsible) || hasSubNodes);

            if (result)
                return result;
            else if (Nodes.Count > 0)
            {
                foreach (dtoTreeCommunityNode n in this.Nodes)
                {
                    result = n.HasNodes(filter);
                    if (result == true)
                        break;
                }
            }
            return result;
        }
        //private Boolean VerifyFilters(dtoTreeCommunityNode n, dtoCommunitiesFilters filter)
        //{
        //    Boolean result = false;
        //    if (filter.IdOrganization > 0)
        //        result = (n.IdOrganization == filter.IdOrganization);
        //    else if (filter.OnlyFromAvailableOrganizations)
        //        result = (filter.AvailableIdOrganizations.Contains(n.IdOrganization));
        //    if (filter.IdcommunityType > -1)
        //        result = result && (n.IdCommunityType == filter.IdcommunityType);
        //    else if (filter.IdRemoveCommunityType > -1)
        //        result = result && (n.IdCommunityType != filter.IdRemoveCommunityType);

        //    if (filter.Status != CommunityStatus.None)
        //         result = result && (n.Status != filter.Status);
        //    if (filter.IdResponsible != -1)
        //        result = result && (n.IdResponsible != filter.IdResponsible);
        //    if (filter.Availability != CommunityAvailability.All)
        //         result = result && ((filter.Availability == CommunityAvailability.Subscribed && n.Selected) || (filter.Availability == CommunityAvailability.NotSubscribed && !n.Selected));
        //    if (filter.IdCourseTime > 0)
        //         result = result && (n.IdCourseTime != filter.IdCourseTime);
        //    if (filter.IdDegreeType > 0)
        //         result = result && (n.IdDegreeType != filter.IdDegreeType);
        //    if (!String.IsNullOrEmpty(filter.CourseCode))
        //         result = result && (!String.IsNullOrEmpty(n.CourseCode) && n.CourseCode.ToLower().Contains(filter.CourseCode.Trim().ToLower()));
        //    if (filter.Year > 0)
        //        result = result && (n.Year != filter.Year);

        //    if (filter.SearchBy != SearchCommunitiesBy.All)
        //    {
        //        switch (filter.SearchBy)
        //        {
        //            case SearchCommunitiesBy.Contains:
        //                if (!String.IsNullOrEmpty(filter.Value))
        //                    query = query.Where(n => !String.IsNullOrEmpty(n.Name) && n.Name.ToLower().Contains(filter.Value.ToLower()));
        //                break;
        //            case SearchCommunitiesBy.NameStartAs:
        //                if (!String.IsNullOrEmpty(filter.Value))
        //                    query = query.Where(n => !String.IsNullOrEmpty(n.Name) && n.Name.ToLower().StartsWith(filter.Value.ToLower()));
        //                break;
        //            case SearchCommunitiesBy.StartAs:
        //                if (filter.StartWith != "#")
        //                    query = query.Where(n => string.Compare(n.FirstLetter, filter.StartWith.ToLower(), true) == 0);
        //                else
        //                    query = query.Where(n => DefaultOtherChars().Contains(n.FirstLetter));

        //                break;
        //        }
        //    }
        //    Boolean hasSubNodes = query.Any();

        //    if (filter.IdOrganization > 0)
        //        result = (this.IdOrganization == filter.IdOrganization || hasSubNodes);
        //    else if (filter.OnlyFromAvailableOrganizations && filter.AvailableIdOrganizations.Any())
        //        result = (filter.AvailableIdOrganizations.Contains(this.IdOrganization) || hasSubNodes);

        //    // TO CHECK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //    if (filter.IdcommunityType >= 0)
        //        result = ((!filter.OnlyFromAvailableOrganizations && filter.IdOrganization < 1) || result)
        //            && ((this.IdCommunityType == filter.IdcommunityType) || hasSubNodes);
        //    else if (filter.IdRemoveCommunityType > -1)
        //        result = ((!filter.OnlyFromAvailableOrganizations && filter.IdOrganization < 1) || result)
        //            && (this.IdCommunityType != filter.IdRemoveCommunityType || hasSubNodes);

        //    /* result = (((filter.OnlyFromAvailableOrganizations ) || (!filter.OnlyFromAvailableOrganizations && filter.IdOrganization < 1)) || result)
        //            && (this.IdCommunityType == filter.IdcommunityType || hasSubNodes);*/
        //    if (filter.Status != CommunityStatus.None)
        //        result = ((filter.IdcommunityType < 1 && filter.IdcommunityType < 0) || result)
        //            && (this.Status == filter.Status || hasSubNodes);

        //    if (filter.Availability != CommunityAvailability.All)
        //        result = ((filter.IdcommunityType < 1 && filter.IdcommunityType < 0 && filter.Status == CommunityStatus.None) || result)
        //            && (
        //            ((filter.Availability == CommunityAvailability.Subscribed && this.Selected)
        //            || (filter.Availability == CommunityAvailability.NotSubscribed && !this.Selected)) || hasSubNodes
        //            );
        //    if (filter.SearchBy != SearchCommunitiesBy.All && !String.IsNullOrEmpty(this.Name))
        //    {
        //        switch (filter.SearchBy)
        //        {
        //            case SearchCommunitiesBy.Contains:
        //                if (!String.IsNullOrEmpty(filter.Value))
        //                    result = (result && Name.ToLower().Contains(filter.Value.ToLower()) || hasSubNodes);
        //                break;
        //            case SearchCommunitiesBy.NameStartAs:
        //                if (!String.IsNullOrEmpty(filter.Value))
        //                    result = (result && Name.ToLower().StartsWith(filter.Value.ToLower()) || hasSubNodes);
        //                break;
        //            case SearchCommunitiesBy.StartAs:
        //                if (filter.StartWith != "#")
        //                    result = ((result && string.Compare(FirstLetter, filter.StartWith.ToLower(), true) == 0) || hasSubNodes);
        //                else
        //                    result = ((result && DefaultOtherChars().Contains(FirstLetter)) || hasSubNodes);
        //                break;
        //        }
        //    }
        //    if (filter.IdResponsible != -1)
        //        result = ((result && filter.IdResponsible == IdResponsible) || hasSubNodes);

        //    if (result)
        //        return result;
        //    else if (Nodes.Count > 0)
        //    {
        //        foreach (dtoTreeCommunityNode n in this.Nodes)
        //        {
        //            result = n.HasNodes(filter);
        //            if (result == true)
        //                break;
        //        }
        //    }
        //    return result;
        //}

        public List<dtoTreeCommunityNode> GetAllNodes()
        {
            //return FullClone(this, null).GetAllClonedNodes();
            return GetAllClonedNodes();
        }
        private List<dtoTreeCommunityNode> GetAllClonedNodes()
        {
            List<dtoTreeCommunityNode> results = new List<dtoTreeCommunityNode>();
            foreach (dtoTreeCommunityNode n in Nodes)
            {
                results.Add(n);
                results.AddRange(n.GetAllClonedNodes());
            }
            return results;
        }
        public Boolean HasSelectableNodes()
        {
            Boolean result = (Type != dtoCommunityNodeType.NotSelectable && Type != dtoCommunityNodeType.None);
            if (!result)
                result = Nodes.Where(n => n.HasSelectableNodes()).Any();
            return result;
        }

        /// <summary>
        /// set nodes avaiability for subscription
        /// </summary>
        public void SetSubscriptionAvailability()
        {
            foreach (dtoTreeCommunityNode n in Nodes)
            {
                n.SetNodeSubscriptionAvailability();
                Type = (HasSelectableNodes() && !n.Selected  ? Type : dtoCommunityNodeType.NotSelectable);
            }
        }
        protected void SetNodeSubscriptionAvailability()
        {
            // hide children if organization father is not subscribed;
            if (IdFather == 0  && (!Selected || (Selected && !AccessAvailable))){
                Nodes.Clear();
                if (Selected && !AccessAvailable)
                    Type = dtoCommunityNodeType.NotSelectable;
            }
            //SetChildrenNotSelectableForSubscription();
            else
            {
                Boolean verifyChildren = true;
                // hide node if subscription is unavailable
                if (isClosedByAdministrator || Type == dtoCommunityNodeType.Stored || Type == dtoCommunityNodeType.Blocked)
                    Type = dtoCommunityNodeType.NotSelectable;
                // hide children in subscription must be confirmed
                else if (ConfirmSubscription && !Selected){
                    verifyChildren = false;
                    SetChildrenNotSelectableForSubscription();
                    foreach (dtoTreeCommunityNode n in Nodes.Where(n => n.HasAvailableNodesForSubscription()))
                    {
                        VerifyChildrenNotSelectableForSubscription(n);
                    }
                }
                if (verifyChildren)
                {
                    //verify children with some selectable nodes
                    foreach (dtoTreeCommunityNode n in Nodes.Where(n => n.HasSelectableNodes()))
                    {
                        n.SetNodeSubscriptionAvailability();
                    }
                }
            }
        }
        protected void SetChildrenNotSelectableForSubscription()
        {
            foreach (dtoTreeCommunityNode n in Nodes)
            {
                n.Type = (n.Selected) ? n.Type : dtoCommunityNodeType.NotSelectable;
                if (n.Nodes.Any())
                    n.SetChildrenNotSelectableForSubscription();
            }
        }
        protected Boolean HasAvailableNodesForSubscription()
        {
            Boolean result = (ConfirmSubscription && Selected && AccessAvailable && Status == CommunityStatus.Active);
            if (!result)
                return Nodes.Where(n => n.HasAvailableNodesForSubscription()).Any();
            return result;
        }

        protected void VerifyChildrenNotSelectableForSubscription(dtoTreeCommunityNode node)
        {
            if (node.ConfirmSubscription && node.Selected && AccessAvailable && node.Status == CommunityStatus.Active)
            {
                foreach (dtoTreeCommunityNode n in node.Nodes)
                {
                    if (n.Status == CommunityStatus.Active && !n.Selected)
                        n.Type = dtoCommunityNodeType.Active;
                    else if (n.HasAvailableNodesForSubscription())
                        VerifyChildrenNotSelectableForSubscription(n);
                }
            }
            else
            {
                foreach (dtoTreeCommunityNode n in node.Nodes.Where(i => i.HasAvailableNodesForSubscription()))
                {
                   VerifyChildrenNotSelectableForSubscription(n);
                }
            }
        }
        public object Clone()
        {
            dtoTreeCommunityNode item = new dtoTreeCommunityNode()
            {
                Id = Id,
                IdFather = IdFather,
                IdOrganization = IdOrganization,
                IdFirstFatherOrganization = IdFirstFatherOrganization,
                IdResponsible = IdResponsible,
                Name = Name,
                ToolTip = ToolTip,
                isPrimary = isPrimary,
                IdCommunityType = IdCommunityType,
                Type = Type,
                Status = Status,
                Selected = Selected,
                AccessAvailable = AccessAvailable,
                Nodes = new List<dtoTreeCommunityNode>(),
                IdDegreeType = IdDegreeType,
                IdCourseTime = IdCourseTime,
                CourseCode = CourseCode,
                Year = Year,
                Path = Path,
                FathersName = FathersName,
                IdCreatedBy= IdCreatedBy,
                ConfirmSubscription = ConfirmSubscription,
                isClosedByAdministrator = isClosedByAdministrator,
            };
            return item;
        }
        public static dtoTreeCommunityNode FullClone(dtoTreeCommunityNode node, dtoTreeCommunityNode father)
        {
            dtoTreeCommunityNode item = new dtoTreeCommunityNode()
            {
                Id = node.Id,
                IdFather = node.IdFather,
                IdOrganization = node.IdOrganization,
                IdFirstFatherOrganization = node.IdFirstFatherOrganization,
                IdResponsible = node.IdResponsible,
                Name = node.Name,
                ToolTip = node.ToolTip,
                isPrimary = node.isPrimary,
                IdCommunityType = node.IdCommunityType,
                Type = node.Type,
                Status = node.Status,
                Selected = node.Selected,
                AccessAvailable = node.AccessAvailable,
                IdDegreeType = node.IdDegreeType,
                IdCourseTime = node.IdCourseTime,
                CourseCode = node.CourseCode,
                Year = node.Year,
                Path = node.Path,
                FathersName = node.FathersName,
                IdCreatedBy = node.IdCreatedBy,
                Father = father,
                FirstFather = father,
                ConfirmSubscription = node.ConfirmSubscription,
                isClosedByAdministrator = node.isClosedByAdministrator,
            };
            item.Nodes = node.Nodes.Select(n => dtoTreeCommunityNode.FullClone(n, item)).ToList();
            return item;
        }
        public dtoCommunityNode ToCommunityNode()
        {
            dtoCommunityNode item = new dtoCommunityNode();
            item.Id = Id;
            item.IdFather = IdFather;
            item.IdOrganization = IdOrganization;
            item.IdResponsible = IdResponsible;
            item.Name = Name;
            item.ToolTip = ToolTip;
            item.isPrimary = isPrimary;
            item.IdCommunityType = IdCommunityType;
            item.Type = Type;
            item.Status = Status;
            item.Selected = Selected;
            item.AccessAvailable = AccessAvailable;
            item.Path = Path;
            return item;
        }

        private List<string> DefaultChars()
        {
            List<string> chars = new List<string>();
            for (int i = 48; i <= 57; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            // maiuscole
            for (int i = 65; i <= 90; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            // minuscole
            for (int i = 97; i <= 122; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            return chars;
        }
        private List<string> DefaultOtherChars()
        {
            List<string> chars = new List<string>();
            for (int i = 32; i <= 47; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            for (int i = 58; i <= 64; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            for (int i = 91; i <= 96; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            for (int i = 123; i <= 126; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            return chars;
        }

        public void SetTags(Dictionary<Int32, List<long>> associations){
            IdTags = (associations.ContainsKey(Id) ? associations[Id] : new List<long>());
            IsTagsSet = true;
            Nodes.ForEach(n => n.SetTags(associations));
        }
        public List<Int32> GetAllIdResponsibles()
        {
            List<Int32> results = new List<Int32>();
            if (IdResponsible > 0)
                results.Add(IdResponsible);
            if (Nodes.Any())
                Nodes.ForEach(n => results.AddRange(n.GetAllIdResponsibles()));
            return results.Distinct().ToList();
        }
        public List<Int32> GetAllIdCommunities()
        {
            List<Int32> results = new List<Int32>();
            if (Id > 0)
                results.Add(Id);
            if (Nodes.Any())
                Nodes.ForEach(n => results.AddRange(n.GetAllIdCommunities()));
            return results.Distinct().ToList();
        }
        public Boolean ContaisCommunity(Int32 idCommunity)
        {
            return (Id==idCommunity || (Nodes.Any() && Nodes.Where(n=> n.ContaisCommunity(idCommunity)).Any()));
        }
        private Boolean ContainsAllItems<T>(List<T> a, List<T> b)
        {
            return !b.Except(a).Any();
        }       


        public String ToString()
        {
            return "Id=" + Id + " IdFather=" + IdFather + " IdCommunityType=" + IdCommunityType + " Path=" + Path + " Name=" + Name + " Type=" + Type.ToString();
        }
    }
}