using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using NHibernate.Linq;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.BaseModules.Dashboard.Domain;
using lm.Comol.Core.DomainModel.Filters;

namespace lm.Comol.Core.BaseModules.Dashboard.Business
{
    public partial class ServiceDashboardCommunities : lm.Comol.Core.Dashboard.Business.ServiceDashboard
    {
        private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement _serviceCommunityManagement;
        protected lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement ServiceCommunityManagement { get { return (_serviceCommunityManagement == null) ? new lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(_Context) : _serviceCommunityManagement; } }
        private lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles _serviceTiles;
        protected lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles ServiceTiles { get { return (_serviceTiles == null) ? new lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles(_Context) : _serviceTiles; } }

        #region initClass
            public ServiceDashboardCommunities() :base() { }
            public ServiceDashboardCommunities(iApplicationContext oContext)
                : base(oContext)
            {

            }
            public ServiceDashboardCommunities(iDataContext oDC)
                : base(oDC)
            {

            }
        #endregion


        #region "Get Tree"
            public List<dtoCommunityNodeItem> GetTree(Int32 idPerson, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean advancedMode, Int32 idReferenceCommunity = 0, Boolean useCache = true)
            {
                return GetTree(Manager.GetLitePerson(idPerson), filters, advancedMode, idReferenceCommunity, useCache);
            }
            public List<dtoCommunityNodeItem> GetTree(litePerson p, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean advancedMode, Int32 idReferenceCommunity = 0, Boolean useCache = true)
            {
                if (p == null)
                    return new List<dtoCommunityNodeItem>();
                return GetInternalTree(p, filters, advancedMode, idReferenceCommunity, useCache);
            }
            private List<dtoCommunityNodeItem> GetInternalTree(litePerson p, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean advancedMode, Int32 idReferenceCommunity = 0, Boolean useCache =true)
            {
                List<dtoCommunityNodeItem> nodes = new List<dtoCommunityNodeItem>();
                lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode tree = ServiceCommunityManagement.GetAllCommunitiesTree(p, useCache);
                if (tree != null)
                {
                    Int32 idOrganization = Manager.GetUserDefaultIdOrganization(p.Id);
                    if (filters.IdTags.Any())
                    {
                        Dictionary<Int32, List<long>> associations = Service.CacheGetCommunityAssociation(false).GroupBy(t => t.IdCommunity).ToDictionary(t => t.Key, t => t.Select(tg => tg.IdTag).ToList());
                        tree.SetTags(associations);
                    }
                    tree = tree.Filter(filters, idOrganization);
                    if (filters.IdOrganization > 0 && tree.Nodes.Any())
                        tree.Nodes = tree.Nodes.Where(n => n.IdOrganization == filters.IdOrganization && n.IdCommunityType == (int)CommunityTypeStandard.Organization).ToList();
                    if (tree.Nodes.Any())
                    {
                        #region "Base info"
                        Dictionary<Int32, String> cType = Manager.GetTranslatedItem<dtoTranslatedCommunityType>(UC.Language.Id).ToDictionary(c => c.Id, c => c.Name);

                        List<Int32> idResponsibles = tree.GetAllIdResponsibles().Distinct().ToList();
                        List<Int32> idCommunities = tree.GetAllIdCommunities().Distinct().ToList();

                        IEnumerable<liteSubscriptionInfo> subscriptions = (idCommunities.Count> maxItemsForQuery)? (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                                                                                            where s.IdPerson == p.Id && s.IdRole > -1
                                                                                                            select s).ToList().Where(s=> idCommunities.Contains(s.IdCommunity)).ToList():
                                                                                                             (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                                                                                              where s.IdPerson == p.Id && s.IdRole > -1 && idCommunities.Contains(s.IdCommunity)
                                                                                                              select s).ToList();

                        List<liteCommunityInfo> communities = (idCommunities.Count > maxItemsForQuery) ? (from c in Manager.GetIQ<liteCommunityInfo>() select c).ToList().Where(c => idCommunities.Contains(c.Id)).ToList() : (from c in Manager.GetIQ<liteCommunityInfo>() where idCommunities.Contains(c.Id) select c).ToList();
                        Dictionary<Int32, String> responsibles = (idResponsibles.Count > maxItemsForQuery) ? (from r in Manager.GetIQ<litePerson>() select r).ToList().Where(r => idResponsibles.Contains(r.Id)).ToDictionary(r => r.Id, r => r.SurnameAndName) : (from r in Manager.GetIQ<litePerson>() where idResponsibles.Contains(r.Id) select r).ToDictionary(r => r.Id, r => r.SurnameAndName);
                        #endregion

                        #region "Enroll To Info"
                        Dictionary<Int32, Int32> dRoles = null;
                        Dictionary<Int32, long> enrolledUsers = null;
                        List<liteCommunityConstraint> constraints = null;
                        Dictionary<Int32, Int32> toEnroll = communities.Where(c => !subscriptions.Where(s => s.IdCommunity == c.Id).Any()).ToDictionary(c => c.Id, c=> c.IdTypeOfCommunity);
                        if (toEnroll.Any())
                        {
                            dRoles = GetDefaultRoles(communities.Where(r => toEnroll.Keys.Contains(r.Id) && r.MaxUsersWithDefaultRole > 0).Select(r => r.IdTypeOfCommunity).Distinct().ToList());
                            constraints = ServiceCommunityManagement.GetConstraints(toEnroll.Keys.ToList());
                            List<Int32> rUsers = GetUsersIdByType((Int32)UserTypeStandard.TypingOffice);
                            enrolledUsers = toEnroll.Keys.Where(k=> dRoles.ContainsKey(k)).ToDictionary(i => i, i => GetEnrolledUsersCount(dRoles[toEnroll[i]], i, rUsers));
                        }
                        #endregion

                        if (advancedMode)
                            tree.Nodes.Where(n => cType.ContainsKey(n.IdCommunityType)).GroupBy(n => n.IdCommunityType).OrderBy(n => cType[n.Key]).ToList().ForEach(n => nodes.AddRange(GenerateAdvancedNodes(null, n.Key, n.ToList(), communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType, null, null, true)));
                        else
                            tree.Nodes.ForEach(n => nodes.AddRange(GenerateNodes(null, advancedMode, n, communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType)));
                    }
                    if (nodes !=null && nodes.Where(n=> n.IdCommunity>0 && n.Details.Community.IdType== (Int32)CommunityTypeStandard.Organization && n.Details.Community.IdOrganization == idOrganization ).Any()){
                        nodes.Where(n => n.IdCommunity > 0 && n.Details.Community.IdType == (Int32)CommunityTypeStandard.Organization && n.Details.Community.IdOrganization == idOrganization).ToList().ForEach(n => n.Details.Permissions.UnsubscribeFrom = false);
                    }
                }
                
                return nodes;
            }
            private List<dtoCommunityNodeItem> GenerateAdvancedNodes(dtoCommunityNodeItem father, Int32 idCommunityType, List<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes, List<liteCommunityInfo> communities, IEnumerable<liteSubscriptionInfo> subscriptions, Dictionary<Int32, long> enrolledUsers = null, List<liteCommunityConstraint> constraints = null, Int32 idReferenceCommunity = 0, Dictionary<Int32, String> cType = null, Dictionary<Int32, String> cTimes = null, Dictionary<Int32, String> degreesTypes = null, Boolean setOpen = false)
            {
                List<dtoCommunityNodeItem> results = new List<dtoCommunityNodeItem>();
                dtoCommunityNodeItem node = new dtoCommunityNodeItem() { Displayname = (cType.ContainsKey(idCommunityType) ? cType[idCommunityType] : ""), Type = NodeType.Virtual, HasCurrent = nodes.Where(n => n.ContaisCommunity(idReferenceCommunity)).Any() || setOpen };
                dtoCommunityNodeItem openNode = GenerateOpenNode(NodeType.OpenVirtualNode, node.HasCurrent,father, idCommunityType);
                results.Add(openNode);
                results.Add(node);
                results.Add(new dtoCommunityNodeItem() { Type = NodeType.OpenChildren });
                switch (idCommunityType)
                {
                    case (Int32)CommunityTypeStandard.Degree:
                        if (degreesTypes != null)
                            nodes.Where(n => degreesTypes.ContainsKey(n.IdDegreeType)).GroupBy(n => n.IdDegreeType).OrderBy(n => degreesTypes[n.Key]).ToList().ForEach(n => results.AddRange(GenerateAdvancedNodes(openNode,n.Key, idCommunityType, n.ToList(), communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType, cTimes, degreesTypes)));
                        else
                            nodes.ForEach(n => results.AddRange(GenerateNodes(openNode,true, n, communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType, cTimes, degreesTypes)));
                        break;
                    default:
                        nodes.ForEach(n => results.AddRange(GenerateNodes(openNode, true, n, communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType, cTimes, degreesTypes)));
                        break;
                }

                results.Add(new dtoCommunityNodeItem() { Type = NodeType.CloseChildren });
                results.Add(new dtoCommunityNodeItem() { Type = NodeType.CloseNode });
                return results;
            }
            private List<dtoCommunityNodeItem> GenerateAdvancedNodes(dtoCommunityNodeItem father, Int32 idDegreeType, Int32 idCommunityType, List<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes, List<liteCommunityInfo> communities, IEnumerable<liteSubscriptionInfo> subscriptions, Dictionary<Int32, long> enrolledUsers = null, List<liteCommunityConstraint> constraints = null, Int32 idReferenceCommunity = 0, Dictionary<Int32, String> cType = null, Dictionary<Int32, String> cTimes = null, Dictionary<Int32, String> degreesTypes = null)
            {
                List<dtoCommunityNodeItem> results = new List<dtoCommunityNodeItem>();
                dtoCommunityNodeItem node = new dtoCommunityNodeItem() { Displayname = (degreesTypes.ContainsKey(idDegreeType) ? degreesTypes[idDegreeType] : ""), Type = NodeType.Virtual, HasCurrent = nodes.Where(n => n.ContaisCommunity(idReferenceCommunity)).Any()  };
                dtoCommunityNodeItem openNode = GenerateOpenNode(NodeType.OpenVirtualNode, node.HasCurrent,father, idCommunityType,idDegreeType);
                results.Add(openNode);
                results.Add(node);
                results.Add(new dtoCommunityNodeItem() { Type = NodeType.OpenChildren });

                nodes.ForEach(n => results.AddRange(GenerateNodes(openNode,true, n, communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType, cTimes, degreesTypes)));

                results.Add(new dtoCommunityNodeItem() { Type = NodeType.CloseChildren });
                results.Add(new dtoCommunityNodeItem() { Type = NodeType.CloseNode });
                return results;
            }
            private List<dtoCommunityNodeItem> GenerateNodes(dtoCommunityNodeItem father, Boolean advancedMode, lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node, List<liteCommunityInfo> communities, IEnumerable<liteSubscriptionInfo> subscriptions, Dictionary<Int32, long> enrolledUsers = null, List<liteCommunityConstraint> constraints = null, Int32 idReferenceCommunity = 0, Dictionary<Int32, String> cType = null, Dictionary<Int32, String> cTimes = null, Dictionary<Int32, String> degreesTypes = null)
            {
                List<dtoCommunityNodeItem> results = new List<dtoCommunityNodeItem>();

                dtoCommunityNodeItem cNode = GenerateCommunityNode(advancedMode,node, communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType, cTimes, degreesTypes);
                dtoCommunityNodeItem openNode = GenerateOpenNode(NodeType.OpenCommunityNode, cNode.HasCurrent, father, cNode, node);
               
                results.Add(openNode);
                results.Add(cNode);
                if (node.Nodes.Any())
                {
                    results.Add(new dtoCommunityNodeItem() { Type = NodeType.OpenChildren });
                    if (advancedMode)
                        node.Nodes.Where(n => cType.ContainsKey(n.IdCommunityType)).GroupBy(n => n.IdCommunityType).OrderBy(n => cType[n.Key]).ToList().ForEach(n => results.AddRange(GenerateAdvancedNodes(openNode,n.Key, n.ToList(), communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType, cTimes, degreesTypes)));
                    else
                        node.Nodes.ForEach(n => results.AddRange(GenerateNodes(openNode,advancedMode, n, communities, subscriptions, enrolledUsers, constraints, idReferenceCommunity, cType, cTimes, degreesTypes)));
                    results.Add(new dtoCommunityNodeItem() { Type = NodeType.CloseChildren });
                }
                else
                    results.Add(new dtoCommunityNodeItem() { Type = NodeType.NoChildren });

                results.Add(new dtoCommunityNodeItem() { Type = NodeType.CloseNode });
                return results;
            }

            private dtoCommunityNodeItem GenerateOpenNode(NodeType type, Boolean hasCurrent, dtoCommunityNodeItem father, Int32 idCommunityType, Int32 idDegreeType=0)
            {
                return new dtoCommunityNodeItem() { Type = type, HasCurrent = hasCurrent, UniqueId = GenerateNodeUniqueId(father,idCommunityType,idDegreeType)};
            }
            private dtoCommunityNodeItem GenerateOpenNode(NodeType type, Boolean hasCurrent, dtoCommunityNodeItem father, dtoCommunityNodeItem cNode, lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node)
            {
                return new dtoCommunityNodeItem() { Type = type, HasCurrent = hasCurrent, UniqueId = GenerateNodeUniqueId(father, cNode, node) };
            }
            private dtoCommunityNodeItem GenerateCommunityNode(Boolean advancedMode,lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node, List<liteCommunityInfo> communities, IEnumerable<liteSubscriptionInfo> subscriptions, Dictionary<Int32, long> enrolledUsers = null, List<liteCommunityConstraint> constraints = null, Int32 idReferenceCommunity = 0, Dictionary<Int32, String> cType = null, Dictionary<Int32, String> cTimes = null, Dictionary<Int32, String> degreesTypes = null)
            {
                dtoCommunityNodeItem nNode = new dtoCommunityNodeItem() { ForAdvanced=advancedMode, Displayname = node.Name, CurrentPath = node.Path, Type = NodeType.Community, IsCurrent = (idReferenceCommunity == node.Id), HasCurrent = node.ContaisCommunity(idReferenceCommunity), Details = new dtoNodeDetails(communities.Where(c => c.Id == node.Id).FirstOrDefault(), node.Id, enrolledUsers, constraints, subscriptions.Where(s => s.IdCommunity == node.Id).FirstOrDefault()) };
                nNode.Details.Permissions.ViewDetails = true;
                nNode.Details.Permissions.UnsubscribeFrom = (node.Selected && nNode.Details.Community.AllowUnsubscribe) && node.Status != Communities.CommunityStatus.Blocked;
                nNode.Details.Permissions.AccessTo = subscriptions.Where(s => s.IdCommunity == node.Id && s.Enabled && s.Accepted).Any() && node.Status != Communities.CommunityStatus.Blocked;
                if (constraints != null && constraints.Any() && !nNode.Details.Permissions.AccessTo && nNode.Details.AllowSubscribe)
                    nNode.LoadConstraints(ServiceCommunityManagement.GetDtoCommunityConstraints(subscriptions.ToList(), node.Id, constraints));
                nNode.Details.Permissions.EnrollTo = !nNode.Details.Permissions.AccessTo && !node.Selected && nNode.Details.AllowSubscribe && node.Status != Communities.CommunityStatus.Blocked;
                return nNode;
            }
            private String GenerateNodeUniqueId(dtoCommunityNodeItem father, dtoCommunityNodeItem current, lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node)
            {
                return current.IdCommunity.ToString() + "_" + (String.IsNullOrEmpty(current.CurrentPath) ? "--" : current.CurrentPath) + "_Father_" + (father == null ? "--" : father.UniqueId);
            }
            private String GenerateNodeUniqueId(dtoCommunityNodeItem father, Int32 idCommunityType, Int32 idDegreeType=0)
            {
                return NodeType.Virtual.ToString() + idCommunityType.ToString() + "_" + idDegreeType.ToString() + "_" + (father == null ? "--" : father.UniqueId);
            }
        #endregion

        #region "Get Enrolled"
            public long GetEnrolledUsersCount(Int32 idRole, Int32 idCommunity, List<Int32> removeUsers = null)
            {
                var query = (from s in Manager.GetIQ<LazySubscription>()
                             where s.IdRole == idRole && s.IdCommunity == idCommunity
                             select s);

                return (removeUsers == null) ? query.Select(s => s.Id).Count() : ((removeUsers.Count > maxItemsForQuery) ? query.ToList().Where(p => !removeUsers.Contains(p.Id)).Count() : query.Where(p => !removeUsers.Contains(p.Id)).Select(s=> s.Id).Count());
            }
            public List<Int32> GetUsersIdByType(Int32 idType)
            {
                return (from p in Manager.GetIQ<litePerson>()
                        where p.TypeID == idType
                        select p.Id).ToList();
            }
            public Int32 GetSubscribedCommunitiesCount(Int32 idPerson, DashboardViewType view, Int32 idCommunityType = -1, Int32 idRemoveCommunityType = -1, long idTile = -1, long idTag = -1, dtoTileDisplay tile = null)
            {
                Int32 count = 0;

                try
                {
                    litePerson p = Manager.GetLitePerson(idPerson);
                    switch(view){
                        case DashboardViewType.List:
                            count = GetSubscribedCommunitiesCount(ServiceCommunityManagement.GetAllCommunitiesTree(p), idCommunityType, idRemoveCommunityType);
                            break;
                        case DashboardViewType.Tile:
                        case DashboardViewType.Combined:
                            if (idCommunityType > -1)
                                count = GetSubscribedCommunitiesCount(ServiceCommunityManagement.GetAllCommunitiesTree(p), idCommunityType, idRemoveCommunityType);
                            else
                            {
                                List<Int32> idCommunities = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdRole > 0 && s.IdPerson == idPerson select s.IdCommunity).ToList().Distinct().ToList();

                                idTile = (tile == null) ? idTile : tile.Id;
                                if (tile != null && tile.Tags.Any())
                                    count = Service.CacheGetUserCommunitiesAssociation(idPerson, idCommunities, Tag.Domain.TagType.Community, true).Where(ta => ContainsAllItems<long>(ta.Tags, tile.Tags)).Count();
                                else if (idTile > 0)
                                {
                                    List<long> idTags = (from t in Manager.GetIQ<liteTileTagAssociation>() where t.IdTile == idTile select t.Tag.Id).ToList();
                                    count = Service.CacheGetUserCommunitiesAssociation(idPerson, idCommunities, Tag.Domain.TagType.Community, true).Where(ta => ContainsAllItems<long>(ta.Tags, idTags)).Count();
                                }
                                else if (idTag > 0)
                                    count = Service.CacheGetUserCommunitiesAssociation(idPerson, idCommunities, Tag.Domain.TagType.Community, true).Where(a => idCommunities.Contains(a.IdCommunity) && a.HasTag(idTag)).Count();
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return count;
            }
            private Int32 GetSubscribedCommunitiesCount(lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node, Int32 idCommunityType = -1, Int32 idRemoveCommunityType = -1)
            {
                Int32 count = 0;

                try
                {
                    if (node !=null)
                        count = node.GetAllNodes().Where(n => n.Selected && n.Id > 0 && (idCommunityType == -1 || (n.IdCommunityType == idCommunityType)) && (idRemoveCommunityType == -1 || idRemoveCommunityType != n.IdCommunityType)).Select(n => n.Id).Distinct().Count();
                }
                catch (Exception ex)
                {

                }
                return count;
            }
            public List<dtoSubscriptionItem> GetSubscribedCommunities(Int32 idPerson, DashboardViewType view, Int32 pageIndex, Int32 pageSize, OrderItemsBy orderBy, Boolean ascending, Int32 idCommunityType = -1, Int32 idRemoveCommunityType = -1, long idTile = -1, long idTag = -1, dtoTileDisplay tile = null)
            {
                List<dtoSubscriptionItem> items = null;
                try
                {
                    litePerson p = Manager.GetLitePerson(idPerson);
                    if (p != null)
                    {
                        switch (view)
                        {
                            case DashboardViewType.List:
                                items = GetSubscribedCommunities(p, pageIndex, pageSize, orderBy, ascending, idCommunityType, idRemoveCommunityType);
                                break;
                            case DashboardViewType.Combined:
                                if (idCommunityType>-1)
                                    items = GetSubscribedCommunities(p, pageIndex, pageSize, orderBy, ascending, idCommunityType, idRemoveCommunityType);
                                else
                                    items = GetSubscribedCommunities(p, pageIndex, pageSize, orderBy, ascending, idTile, idTag,tile );
                                break;
                        }
                    }
                }
                catch (Exception ex) { 
                
                }
                return items;
            }
            private List<dtoSubscriptionItem> GetSubscribedCommunities(litePerson p, Int32 pageIndex, Int32 pageSize, OrderItemsBy orderBy, Boolean ascending, Int32 idCommunityType, Int32 idRemoveCommunityType)
            {
                IEnumerable<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes = ServiceCommunityManagement.GetAllCommunitiesTree(p).GetAllNodes().Where(n => n.Selected && n.Id > 0 && (idCommunityType == -1 || (n.IdCommunityType == idCommunityType)) && (idRemoveCommunityType == -1 || idRemoveCommunityType != n.IdCommunityType));
                List<Int32> idCommunities = nodes.Select(c=> c.Id).Distinct().ToList();

                IEnumerable<liteSubscriptionInfo> subscriptions = (idCommunities.Count> maxItemsForQuery) ? (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                where s.IdPerson == p.Id && s.IdRole>-1
                                select s).ToList().Where(s=> idCommunities.Contains(s.IdCommunity)).ToList() :(from s in Manager.GetIQ<liteSubscriptionInfo>()
                                where idCommunities.Contains(s.IdCommunity) &&  s.IdPerson == p.Id && s.IdRole>-1
                                select s).ToList() ;

                List<dtoSubscriptionItem> items = subscriptions.Select(i => new dtoSubscriptionItem(i)).ToList();
                var query = (from i in items where i.Community.Id >0 select i);
                switch (orderBy)
                {
                    case OrderItemsBy.ActivatedOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.SubscriptionStartOn).ThenBy(s => s.Community.CreatedOn)
                            :
                            query.OrderByDescending(s => s.Community.SubscriptionEndOn).OrderByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.ClosedOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.ClosedOn).ThenBy(s => s.Community.CreatedOn)
                        :
                        query.OrderByDescending(s => s.Community.ClosedOn).OrderByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.CreatedOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.CreatedOn) : query.OrderByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.LastAccess:
                        query = (ascending) ? query.OrderBy(s => s.LastAccessOn) : query.OrderByDescending(s => s.LastAccessOn);
                        break;
                    case OrderItemsBy.Name:
                        query = (ascending) ? query.OrderBy(s => s.Community.Name) : query.OrderByDescending(s => s.Community.Name);
                        break;
                }

                items = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                List<liteCommunityConstraint> constraints = ServiceCommunityManagement.GetConstraints(items.Select(i => i.Community.Id).Distinct().ToList());
                foreach (dtoSubscriptionItem item in items){
                    item.HasConstraints = constraints.Where(c => c.IdSource == item.Id).Any();
                    item.Community.Path = ServiceCommunityManagement.GetMostLikelyPath(p, (from n in nodes where n.Id == item.Community.Id select n.Path).ToList());
                }

                return items;
            }
            private List<dtoSubscriptionItem> GetSubscribedCommunities(litePerson p, Int32 pageIndex, Int32 pageSize, OrderItemsBy orderBy, Boolean ascending, long idTile, long idTag, dtoTileDisplay tile)
            {
                IEnumerable<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes = ServiceCommunityManagement.GetAllCommunitiesTree(p).GetAllNodes().Where(n => n.Selected && n.Id > 0 );
                List<Int32> idCommunities = nodes.Select(c => c.Id).Distinct().ToList();

                idTile = (tile == null) ? idTile : tile.Id;
                if (tile != null && tile.Tags.Any())
                    idCommunities = Service.CacheGetUserCommunitiesAssociation(p.Id, idCommunities, Tag.Domain.TagType.Community, true).Where(ta => ContainsAllItems<long>(ta.Tags, tile.Tags) && idCommunities.Contains(ta.IdCommunity)).Select(t => t.IdCommunity).ToList();                                                                                                                           
                else if (idTile > 0)
                {
                    List<long> idTags = (from t in Manager.GetIQ<liteTileTagAssociation>() where t.IdTile == idTile select t.Tag.Id).ToList();
                    idCommunities = Service.CacheGetUserCommunitiesAssociation(p.Id, idCommunities, Tag.Domain.TagType.Community, true).Where(ta => ContainsAllItems<long>(ta.Tags, idTags) && idCommunities.Contains(ta.IdCommunity)).Select(t => t.IdCommunity).ToList();
                }
                else if (idTag > 0)
                    idCommunities = Service.CacheGetUserCommunitiesAssociation(p.Id, idCommunities, Tag.Domain.TagType.Community, true).Where(a => idCommunities.Contains(a.IdCommunity) && a.HasTag(idTag) && idCommunities.Contains(a.IdCommunity)).Select(t => t.IdCommunity).ToList();

                IEnumerable<liteSubscriptionInfo> subscriptions = (idCommunities.Count > maxItemsForQuery) ? (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                                                                                              where s.IdPerson == p.Id && s.IdRole > -1
                                                                                                              select s).ToList().Where(s => idCommunities.Contains(s.IdCommunity)).ToList() : (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                                                                                                                                                                               where idCommunities.Contains(s.IdCommunity) && s.IdPerson == p.Id && s.IdRole > -1
                                                                                                                                                                                               select s).ToList();
                                     
                List<dtoSubscriptionItem> items = subscriptions.Select(i => new dtoSubscriptionItem(i)).ToList();
                var query = (from i in items where i.Community.Id > 0 select i);
                switch (orderBy)
                {
                    case OrderItemsBy.ActivatedOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.SubscriptionStartOn).ThenBy(s => s.Community.CreatedOn)
                            :
                            query.OrderByDescending(s => s.Community.SubscriptionEndOn).OrderByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.ClosedOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.ClosedOn).ThenBy(s => s.Community.CreatedOn)
                        :
                        query.OrderByDescending(s => s.Community.ClosedOn).OrderByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.CreatedOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.CreatedOn) : query.OrderByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.LastAccess:
                        query = (ascending) ? query.OrderBy(s => s.LastAccessOn) : query.OrderByDescending(s => s.LastAccessOn);
                        break;
                    case OrderItemsBy.Name:
                        query = (ascending) ? query.OrderBy(s => s.Community.Name) : query.OrderByDescending(s => s.Community.Name);
                        break;
                }

                items = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                foreach (dtoSubscriptionItem item in items)
                {
                    item.Community.Path = ServiceCommunityManagement.GetMostLikelyPath(p, (from n in nodes where n.Id == item.Community.Id select n.Path).ToList());
                }

                return items;
            }
            public List<dtoSubscriptionItem> GetCommunities(Int32 idPerson, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean useCache = true, Boolean forTree = false)
            {
                List<dtoSubscriptionItem> items = null;
                try
                {
                    litePerson p = Manager.GetLitePerson(idPerson);

                    if (p!=null){
                        Int32 dOrganization = (p==null) ? -3 : (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson== p.Id && o.isDefault  select o.IdOrganization).Skip(0).Take(1).ToList().FirstOrDefault();
                        IEnumerable<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes = GetAvailableNodes(p, dOrganization, filters, forTree);

                        List<Int32> idCommunities = nodes.Select(c => c.Id).Distinct().ToList();
                        if (filters.IdTags.Any())
                        {
                            if (filters.IdTile > 0)
                                idCommunities = Service.CacheGetUserCommunitiesAssociation(idPerson, idCommunities, Tag.Domain.TagType.Community, true).Where(ta => ContainsAllItems<long>(ta.Tags, filters.IdTags) && idCommunities.Contains(ta.IdCommunity)).Select(t => t.IdCommunity).ToList();
                            else
                                idCommunities = Service.CacheGetCommunityAssociation(useCache).Where(t => filters.IdTags.Contains(t.IdTag)).Select(t => t.IdCommunity).ToList();
                            

                            if (forTree){
                               nodes.Where(n=> !idCommunities.Contains(n.Id)).ToList().ForEach(n=> n.Type= dtoCommunityNodeType.NotSelectable);
                               }
                            else
                                nodes = nodes.Where(n=> idCommunities.Contains(n.Id)).ToList();
                        }

                        idCommunities = nodes.Select(c => c.Id).Distinct().ToList();
                        switch(filters.Availability){
                            case CommunityAvailability.Subscribed:
                               IEnumerable<liteSubscriptionInfo> subscriptions = (idCommunities.Count > maxItemsForQuery) ? (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                                                                                              where s.IdPerson == p.Id && s.IdRole > -1
                                                                                                              select s).ToList().Where(s => idCommunities.Contains(s.IdCommunity)).ToList() : (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                                                                                                                                                                               where idCommunities.Contains(s.IdCommunity) && s.IdPerson == p.Id && s.IdRole > -1
                                                                                                                                                                                               select s).ToList();

                               items = subscriptions.Select(i => new dtoSubscriptionItem(i) { AllowUnsubscriptionFromOrganization = !(dOrganization == i.Community.IdOrganization && i.Community.IdTypeOfCommunity == (int)CommunityTypeStandard.Organization) }).ToList();

                               break;

                        }
                        foreach (dtoSubscriptionItem item in items)
                        {
                            item.Community.Path = ServiceCommunityManagement.GetMostLikelyPath(p, (from n in nodes where n.Id == item.Community.Id select n.Path).ToList());
                        }
                    }
                }
                catch(Exception ex){
                    return null;
                }
                return items;
            }
            public List<dtoCommunityPlainItem> GetPlainCommunities(Int32 idPerson, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters,List<Int32> unloadIdCommunities, Boolean useCache = true)
            {
                List<dtoCommunityPlainItem> items = null;
                try
                {
                    litePerson p = Manager.GetLitePerson(idPerson);

                    if (p != null)
                    {
                        Int32 dOrganization = (p == null) ? -3 : (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == p.Id && o.isDefault select o.IdOrganization).Skip(0).Take(1).ToList().FirstOrDefault();
                        IEnumerable<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes = GetAvailableNodes(p, dOrganization, filters, false);

                        List<Int32> idCommunities = null;
                        if (filters.RequiredPermissions != null && filters.RequiredPermissions.Any())
                        {
                            idCommunities = ServiceCommunityManagement.GetIdCommunityByModulePermissions(idPerson, filters.RequiredPermissions.ToDictionary(rp=> rp.IdModule, rp=> rp.Permissions), unloadIdCommunities);
                            nodes = nodes.Where(n => idCommunities.Contains(n.Id)).ToList();
                        }
                        if (unloadIdCommunities != null)
                            nodes = nodes.Where(n => !unloadIdCommunities.Contains(n.Id)).ToList(); 

                        idCommunities = nodes.Select(c => c.Id).Distinct().ToList();
                        if (filters.IdTags.Any())
                        {
                            if (filters.IdTile > 0)
                                idCommunities = Service.CacheGetUserCommunitiesAssociation(idPerson, idCommunities, Tag.Domain.TagType.Community, true).Where(ta => ContainsAllItems<long>(ta.Tags, filters.IdTags) && idCommunities.Contains(ta.IdCommunity)).Select(t => t.IdCommunity).ToList();
                            else
                                idCommunities = Service.CacheGetCommunityAssociation(useCache).Where(t => filters.IdTags.Contains(t.IdTag)).Select(t => t.IdCommunity).ToList();
                            nodes = nodes.Where(n => idCommunities.Contains(n.Id)).ToList();
                        }

                        idCommunities = nodes.Select(c => c.Id).Distinct().ToList();
                        IEnumerable<liteSubscriptionInfo> subscriptions = null;
                        switch (filters.Availability)
                        {
                            case CommunityAvailability.Subscribed:
                                subscriptions = (idCommunities.Count > maxItemsForQuery) ? (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                                                                        where s.IdPerson == p.Id && s.IdRole > -1
                                                                                        select s).ToList().Where(s => idCommunities.Contains(s.IdCommunity)).ToList() 
                                                                                        : 
                                                                                        (from s in Manager.GetIQ<liteSubscriptionInfo>() where idCommunities.Contains(s.IdCommunity) 
                                                                                             && s.IdPerson == p.Id && s.IdRole > -1 select s).ToList();
                                break;
                        }

                        List<liteCommunityInfo> communities = (idCommunities.Count > maxItemsForQuery) ? (from c in Manager.GetIQ<liteCommunityInfo>() select c).ToList().Where(c => idCommunities.Contains(c.Id)).ToList() : (from c in Manager.GetIQ<liteCommunityInfo>() where idCommunities.Contains(c.Id) select c).ToList();
                        Dictionary<Int32, String> types = Manager.GetTranslatedCommunityTypes(UC.Language.Id).ToDictionary(c => c.Id, c => c.Name);

                        List<Int32> idResponsibles = nodes.Select(n => n.IdResponsible).Distinct().Distinct().ToList();
                        Dictionary<Int32, String> responsibles = (idResponsibles.Count > maxItemsForQuery) ? (from r in Manager.GetIQ<litePerson>() select r).ToList().Where(r => idResponsibles.Contains(r.Id)).ToDictionary(r => r.Id, r => r.SurnameAndName) : (from r in Manager.GetIQ<litePerson>() where idResponsibles.Contains(r.Id) select r).ToDictionary(r => r.Id, r => r.SurnameAndName);

                        Dictionary<Int32, List<long>> associations = Service.CacheGetCommunityAssociation(false).GroupBy(t => t.IdCommunity).ToDictionary(t => t.Key, t => t.Select(tg=> tg.IdTag).ToList());

                        items = nodes.GroupBy(n => n.Id).Select(n =>
                                new dtoCommunityPlainItem(n.FirstOrDefault(), n.ToList(), communities.FirstOrDefault(c => c.Id == n.Key), associations, responsibles,null,null, types,(subscriptions==null) ? null: subscriptions.FirstOrDefault(s => s.IdCommunity == n.Key))).ToList();
                   }
                }
                catch (Exception ex)
                {
                    return null;
                }
                return items;
            }

            public lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode UnsubscribeInfo(Int32 idPerson, Int32 idCommunity, String path)
            {
                lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode result = null;
                try
                {
                    Person p = Manager.GetPerson(idPerson);
                    if (p != null && p.TypeID != (int)UserTypeStandard.Guest && p.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        result = ServiceCommunityManagement.GetCommunitySubTree(p, idCommunity, path);
                    }
                }
                catch (Exception ex) { 
                
                }
                return result;
            }
            private IEnumerable<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> GetAvailableNodes(litePerson p, Int32 dOrganization, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean forTree)
            {
                dtoTreeCommunityNode root = ServiceCommunityManagement.GetAllCommunitiesTree(p);
                if (filters.Availability == CommunityAvailability.NotSubscribed)
                    root.SetSubscriptionAvailability();
                IEnumerable<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes = root.Filter(filters, dOrganization).GetAllNodes();
                if (forTree)
                    return nodes;
                else
                {
                    String path = "";
                    if (nodes.Where(n => n.IdOrganization == filters.IdOrganization && n.IdFather == 0).Any())
                        path = nodes.Where(n => n.IdOrganization == filters.IdOrganization && n.IdFather == 0).FirstOrDefault().Path;

                    if (filters.IdResponsible > -1)
                        nodes = nodes.Where(n => n.IdResponsible == filters.IdResponsible);
                    if (filters.IdCourseTime > -1)
                        nodes = nodes.Where(n => n.IdCourseTime == filters.IdCourseTime);
                    if (filters.IdDegreeType > -1)
                        nodes = nodes.Where(n => n.IdCourseTime == filters.IdDegreeType);
                    if (filters.Year > -1)
                        nodes = nodes.Where(n => n.Year == filters.Year);
                    if (filters.Status != Communities.CommunityStatus.None)
                        nodes = nodes.Where(n => n.Status == filters.Status);
                    if (filters.IdcommunityType > -1)
                        nodes = nodes.Where(n => n.IdCommunityType == filters.IdcommunityType);
                    if (filters.IdOrganization > -1)
                        nodes = nodes.Where(n => n.IdOrganization == filters.IdOrganization || n.Path.StartsWith(path));
                    switch (filters.Availability)
                    {
                        case CommunityAvailability.NotSubscribed:
                            nodes = nodes.Where(n => !n.Selected && n.Type!= dtoCommunityNodeType.NotSelectable);
                            break;
                    }
                    if (filters.SearchBy != SearchCommunitiesBy.All)
                    {
                        switch (filters.SearchBy)
                        {
                            case SearchCommunitiesBy.Contains:
                                if (!String.IsNullOrEmpty(filters.Value))
                                    nodes = nodes.Where(n => !String.IsNullOrEmpty(n.Name) && n.Name.ToLower().Contains(filters.Value.ToLower()));
                                if (filters.StartWith=="#")
                                    nodes = nodes.Where(n => DefaultOtherChars().Contains(n.FirstLetter));
                                else if (!String.IsNullOrEmpty(filters.StartWith))
                                    nodes = nodes.Where(n => string.Compare(n.FirstLetter, filters.StartWith.ToLower(), true) == 0);
                                break;
                            case SearchCommunitiesBy.NameStartAs:
                                if (!String.IsNullOrEmpty(filters.Value))
                                    nodes = nodes.Where(n => !String.IsNullOrEmpty(n.Name) && n.Name.ToLower().StartsWith(filters.Value.ToLower()));
                                break;
                            case SearchCommunitiesBy.StartAs:
                                if (filters.StartWith != "#")
                                    nodes = nodes.Where(n => string.Compare(n.FirstLetter, filters.StartWith.ToLower(), true) == 0);
                                else
                                    nodes = nodes.Where(n => DefaultOtherChars().Contains(n.FirstLetter));

                                break;
                        }
                    }
                    return nodes;
                }
            }
            public List<dtoSubscriptionItem> GetCommunities(Int32 idLanguage,List<dtoSubscriptionItem> items, Int32 pageIndex, Int32 pageSize, OrderItemsBy orderBy, Boolean ascending)
            {
                var query = (from i in items where i.Community.Id > 0 select i);
                switch (orderBy)
                {
                    case OrderItemsBy.ActivatedOn:
                        query = (ascending) ? query.OrderByDescending(s => s.Community.SubscriptionStartOn.HasValue).ThenBy(s => s.Community.SubscriptionStartOn).ThenBy(s => s.Community.CreatedOn)
                            :
                            query.OrderByDescending(s => s.Community.SubscriptionStartOn.HasValue).ThenByDescending(s => s.Community.SubscriptionEndOn).ThenByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.ClosedOn:
                        query = (ascending) ? query.OrderByDescending(s => s.Community.ClosedOn.HasValue).ThenBy(s => s.Community.ClosedOn).ThenBy(s => s.Community.CreatedOn)
                        :
                        query.OrderByDescending(s => s.Community.ClosedOn.HasValue).ThenByDescending(s => s.Community.ClosedOn).ThenByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.CreatedOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.CreatedOn) : query.OrderByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.LastAccess:
                        query = (ascending) ? query.OrderBy(s => s.LastAccessOn) : query.OrderByDescending(s => s.LastAccessOn);
                        break;
                    case OrderItemsBy.Name:
                        query = (ascending) ? query.OrderBy(s => s.Community.Name) : query.OrderByDescending(s => s.Community.Name);
                        break;
                }
                List<dtoSubscriptionItem> results =query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                List<lm.Comol.Core.DomainModel.dtoTranslatedCommunityType> types = Manager.GetTranslatedCommunityTypes(idLanguage);
                foreach(dtoSubscriptionItem item in results){
                    item.Community.CommunityType= types.Where(t=> t.Id== item.Community.IdType).Select(t=> t.Name).FirstOrDefault();
                }

                return results;
             
            }
            public List<dtoCommunityPlainItem> GetCommunities(List<dtoCommunityPlainItem> items, Int32 pageIndex, Int32 pageSize, OrderItemsBy orderBy, Boolean ascending)
            {
                var query = (from i in items where i.Community.Id > 0 select i);
                switch (orderBy)
                {
                    case OrderItemsBy.ActivatedOn:
                        query = (ascending) ? query.OrderByDescending(s => s.Community.SubscriptionStartOn.HasValue).ThenBy(s => s.Community.SubscriptionStartOn).ThenBy(s => s.Community.CreatedOn)
                            :
                            query.OrderByDescending(s => s.Community.SubscriptionStartOn.HasValue).ThenByDescending(s => s.Community.SubscriptionEndOn).ThenByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.ClosedOn:
                        query = (ascending) ? query.OrderByDescending(s => s.Community.ClosedOn.HasValue).ThenBy(s => s.Community.ClosedOn).ThenBy(s => s.Community.CreatedOn)
                        :
                        query.OrderByDescending(s => s.Community.ClosedOn.HasValue).ThenByDescending(s => s.Community.ClosedOn).ThenByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.CreatedOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.CreatedOn) : query.OrderByDescending(s => s.Community.CreatedOn);
                        break;
                    case OrderItemsBy.LastAccess:
                        query = (ascending) ? query.OrderBy(s => s.LastAccessOn) : query.OrderByDescending(s => s.LastAccessOn);
                        break;
                    case OrderItemsBy.Name:
                        query = (ascending) ? query.OrderBy(s => s.Community.Name) : query.OrderByDescending(s => s.Community.Name);
                        break;
                }
                return query.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            }
            public List<liteSubscriptionInfo> UnsubscribeFromCommunity(Int32 idUser,lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode node, RemoveAction action){
                List<liteSubscriptionInfo> subscriptions = null;
                try
                {
                    List<Int32> idCommunities = new List<Int32>();
                    idCommunities.Add(node.Id);
                    if (action == RemoveAction.FromAllSubCommunities)
                        idCommunities.AddRange(node.GetAllNodes().Where(n => n.AllowUnsubscribe()).Select(n => n.Id).ToList());
                    idCommunities = idCommunities.Distinct().ToList();
                    if (idCommunities.Count() < maxItemsForQuery)
                        subscriptions = (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                         where idCommunities.Contains(s.IdCommunity) && s.IdPerson == idUser
                                         select s).ToList();
                    else
                        subscriptions = (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                         where s.IdPerson == idUser
                                         select s).ToList().Where(s => idCommunities.Contains(s.IdCommunity)).ToList();
                    foreach (liteSubscriptionInfo s in subscriptions)
                    {
                        if (s.Community.IdCreatedBy == idUser)
                            s.IdRole = -2;
                        else
                            s.IdRole = -3;
                    }
                    if (subscriptions!=null && subscriptions.Any())
                        Manager.SaveOrUpdateList(subscriptions);

                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.UserCommunitiesTags(idUser, Tag.Domain.TagType.Community));
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.DashboardUserTiles(idUser));
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(idUser));
                }
                catch (Exception ex)
                {
                    subscriptions = null;
                }
    
                return subscriptions;
            }
        #endregion

        #region "ToEnroll"
            public List<dtoEnrollingItem> GetCommunitiesToEnroll(Int32 idPerson, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean useCache = true, Boolean forTree = false)
            {
                List<dtoEnrollingItem> items = null;
                try
                {
                    litePerson p = Manager.GetLitePerson(idPerson);

                    if (p != null)
                    {
                        Int32 dOrganization = (p == null) ? -3 : (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == idPerson && o.isDefault select o.IdOrganization).Skip(0).Take(1).ToList().FirstOrDefault();
                        IEnumerable<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes = GetAvailableNodes(p, dOrganization, filters, forTree);


                        Dictionary<Int32, List<long>> associations = Service.CacheGetCommunityAssociation(false).GroupBy(t => t.IdCommunity).ToDictionary(t => t.Key, t => t.Select(tg=> tg.IdTag).ToList());

                        List<Int32> idCommunities = null;
                        if (filters.IdTags.Any())
                        {
                            idCommunities = associations.Where(v => filters.IdTags.Where(t => v.Value.Contains(t)).Any()).Select(v => v.Key).Distinct().ToList();
                            //idCommunities = Service.CacheGetCommunityAssociation(useCache).Where(t => filters.IdTags.Contains(t.IdTag)).Select(t => t.IdCommunity).ToList();
                            if (forTree)
                            {
                                nodes.Where(n => !idCommunities.Contains(n.Id)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                            }
                            else
                                nodes = nodes.Where(n => idCommunities.Contains(n.Id)).ToList();
                        }

                        idCommunities = nodes.Select(c => c.Id).Distinct().ToList();
                        List<liteCommunityInfo> communities = (idCommunities.Count > maxItemsForQuery) ? (from c in Manager.GetIQ<liteCommunityInfo>() select c).ToList().Where(c => idCommunities.Contains(c.Id)).ToList() : (from c in Manager.GetIQ<liteCommunityInfo>() where idCommunities.Contains(c.Id) select c).ToList();

                        List<Int32> idResponsibles = nodes.Select(n=> n.IdResponsible).Distinct().Distinct().ToList();
                        Dictionary<Int32, String> responsibles = (idResponsibles.Count >maxItemsForQuery) ? (from r in Manager.GetIQ<litePerson>() select r).ToList().Where(r=> idResponsibles.Contains(r.Id)).ToDictionary(r=> r.Id,r=> r.SurnameAndName): (from r in Manager.GetIQ<litePerson>() where idResponsibles.Contains(r.Id) select r).ToDictionary(r=> r.Id,r=> r.SurnameAndName);

                        items = nodes.GroupBy(n => n.Id).Where(n => n.Count() == 1).Select(n => new dtoEnrollingItem(n.FirstOrDefault(), communities.Where(c => c.Id == n.Key).FirstOrDefault(), associations, responsibles)).ToList();
                        items.AddRange(nodes.GroupBy(n => n.Id).Where(n => n.Count() > 1).Select(n => new dtoEnrollingItem(n.OrderByDescending(c => c.isPrimary).FirstOrDefault(), communities.Where(c => c.Id == n.Key).FirstOrDefault(), associations, responsibles, n.GroupBy(nn => nn.isPrimary).ToDictionary(nn => nn.Key, nn => nn.Select(np => np.Path).ToList()))).ToList());
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                return items;
            }
            public List<dtoEnrollingItem> GetCommunities(Int32 idPerson, List<dtoEnrollingItem> items, Int32 pageIndex, Int32 pageSize, OrderItemsToSubscribeBy orderBy, Boolean ascending)
            {
                var query = (from i in items where i.Community.Id > 0 select i);
                switch (orderBy)
                {
                    case OrderItemsToSubscribeBy.Name:
                        query = (ascending) ? query.OrderBy(s => s.Community.Name) : query.OrderByDescending(s => s.Community.Name);
                        break;
                    case OrderItemsToSubscribeBy.SubscriptionOpenOn:
                        query = (ascending) ? query.OrderBy(s => s.Community.SubscriptionStartOn).ThenBy(s => s.Community.Name)
                        :
                        query.OrderByDescending(s => s.Community.SubscriptionStartOn).ThenByDescending(s => s.Community.Name);
                        break;
                    case OrderItemsToSubscribeBy.SubscriptionClosedOn:
                        query = (ascending) ? query.OrderBy(s=> s.Community.SubscriptionEndOn.HasValue).ThenBy(s => s.Community.SubscriptionEndOn).ThenBy(s => s.Community.Name)
                            : query.OrderBy(s => s.Community.SubscriptionEndOn.HasValue).ThenByDescending(s => s.Community.SubscriptionEndOn).ThenByDescending(s => s.Community.Name);
                        break;
                    case OrderItemsToSubscribeBy.MaxUsers:
                        query = (ascending) ? query.OrderBy(s => s.AvailableSeats).ThenBy(s => s.Community.Name)
                            : query.OrderByDescending(s => s.AvailableSeats).ThenByDescending(s => s.Community.Name);
                        break;
                    case OrderItemsToSubscribeBy.DegreeType:
                        query = (ascending) ? query.OrderBy(s => s.Community.SubscriptionStartOn).ThenBy(s => s.Community.CreatedOn)
                            :
                            query.OrderByDescending(s => s.Community.SubscriptionEndOn).ThenByDescending(s => s.Community.CreatedOn);
                        break;
                   
                    case OrderItemsToSubscribeBy.Responsible:
                        query = (ascending) ? query.OrderBy(s => s.Community.Responsible).ThenBy(s => s.Community.Name) : query.OrderByDescending(s => s.Community.Responsible).ThenByDescending(s => s.Community.Name);
                        break;
                    case OrderItemsToSubscribeBy.Timespan:
                        query = (ascending) ? query.OrderBy(s => s.Community.CourseTime).ThenBy(s => s.Community.Name) : query.OrderByDescending(s => s.Community.CourseTime).ThenByDescending(s => s.Community.Name);
                        break;
                    case OrderItemsToSubscribeBy.Year:
                        query = (ascending) ? query.OrderBy(s => s.Community.Year).ThenBy(s => s.Community.Name) : query.OrderByDescending(s => s.Community.Year).ThenBy(s => s.Community.Name);
                        break;
                   
                }

                List<dtoEnrollingItem> results = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                if (results!=null){
                    LoadEnrollingInfo(idPerson,results.AsEnumerable());
                }
                return results;
             
            }
            private void LoadEnrollingInfo(Int32 idPerson, IEnumerable<dtoEnrollingItem> items)
            {
                Dictionary<Int32, Int32> dRoles = GetDefaultRoles(items.Where(r => r.Community.MaxUsersWithDefaultRole > 0).Select(r => r.Community.IdType).Distinct().ToList());
                List<liteCommunityConstraint> constraints = ServiceCommunityManagement.GetConstraints(items.Select(i => i.Community.Id).Distinct().ToList());
                foreach (dtoEnrollingItem item in items)
                {
                    if (item.Community.MaxUsersWithDefaultRole > 0)
                        item.EnrolledUsers = (from s in Manager.GetIQ<LazySubscription>()
                                              where s.IdRole ==dRoles[item.Community.IdType] && s.IdCommunity == item.Community.Id
                                              select s.Id).Count();
                    if (String.IsNullOrEmpty(item.PrimaryPath))
                        item.PrimaryPath = ServiceCommunityManagement.GetMostLikelyPath(idPerson, item.AvailablePath);

                    if (item.AvailableSeats<=0)
                        item.NotAvailableFor.Add(EnrollingStatus.Seats);
                    item.NotAvailableFor.AddRange(dtoEnrollingItem.GetEnrollingStatus(item));
                    LoadConstraintsInfo(idPerson,item, constraints.Where(c => c.IdSource == item.Community.Id || c.IdDestinationCommunity == item.Community.Id));
                }
            }
            private void LoadConstraintsInfo(Int32 idPerson, dtoEnrollingItem item ,IEnumerable<liteCommunityConstraint> items)
            {
                var query =  (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdRole>0 && s.IdPerson ==idPerson select s);
                List<Int32> idCommunities = items.Where(i=> i.IdSource==item.Community.Id).Select(i=> i.IdDestinationCommunity).Distinct().ToList();
                idCommunities.AddRange(items.Where(i=> i.IdDestinationCommunity==item.Community.Id).Select(i=> i.IdSource).Distinct().ToList());
                List<liteSubscriptionInfo> subscriptions = (idCommunities.Count()> maxItemsForQuery) ? query.ToList().Where(s=> idCommunities.Contains(s.IdCommunity)).ToList() : query.Where(s=> idCommunities.Contains(s.IdCommunity)).ToList();

                item.Constraints = items.Select(i => new dtoCommunityConstraint(i, subscriptions, i.IdSource!=item.Community.Id)).ToList();
                if (item.Constraints.Where(c => !c.IsRespected).Any())
                    item.NotAvailableFor.Add(EnrollingStatus.Constraints);

            }
           
            public dtoCommunityInfoForEnroll GetEnrollingItem(Int32 idPerson, Int32 idCommunity, String path)
            {
                return GetEnrollingItems(idPerson, new List<dtoCommunityToEnroll>() { new dtoCommunityInfoForEnroll() { Id = idCommunity, Path = path } }).FirstOrDefault();
            }
            public List<dtoCommunityInfoForEnroll> GetEnrollingItems(Int32 idPerson, List<dtoCommunityToEnroll> items)
            {
                List<dtoCommunityInfoForEnroll> results = new List<dtoCommunityInfoForEnroll>();
                try
                {
                    litePerson p = Manager.GetLitePerson(idPerson);
                    if (p != null && p.TypeID != (int)UserTypeStandard.Guest && p.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        List<Int32> idEnrollingCommunities= items.Select(i=> i.Id).Distinct().ToList();
                        List<Int32> idCommunities = new List<int>();
                        idCommunities.AddRange(idEnrollingCommunities);

                         /// find constraints
                        List<liteCommunityConstraint> constraints = ServiceCommunityManagement.GetConstraints(idCommunities);
                        if (constraints.Any()){
                            idCommunities.AddRange(constraints.Where(c=> !idCommunities.Contains(c.IdSource)).Select(c=>  c.IdSource).ToList());
                            idCommunities.AddRange(constraints.Where(c=> !idCommunities.Contains(c.IdDestinationCommunity)).Select(c=>  c.IdDestinationCommunity).ToList());
                        }
                        idCommunities = idCommunities.Where(c=> c>0).Distinct().ToList();

                        var query = (from c in Manager.GetIQ<liteCommunityInfo>() select c);
                        if (idCommunities.Count() <= maxItemsForQuery)
                            query = query.Where(c => idCommunities.Contains(c.Id));
                        else
                            query = query.ToList().Where(c => idCommunities.Contains(c.Id)).AsQueryable();

                        List<liteCommunityInfo> communities = query.ToList();
                        Dictionary<Int32, Int32> dRoles = GetDefaultRoles(communities.Where(c=> idEnrollingCommunities.Contains(c.Id)).Select(c=>c.IdTypeOfCommunity).Distinct().ToList());
                        Dictionary<Int32, long> enrolledUsers = GetEnrolledUsers(communities.Where(c=> c.MaxUsersWithDefaultRole>0 && idEnrollingCommunities.Contains(c.Id)).GroupBy(c=> c.IdTypeOfCommunity).ToDictionary(c=> c.Key, c=> c.Select(cm=> cm.Id).ToList()),dRoles);

                        results = items.Select(i => new dtoCommunityInfoForEnroll(i.Id, i.Path, communities.Where(c => c.Id == i.Id).FirstOrDefault(), dRoles,enrolledUsers)).ToList();
                        
                        foreach(dtoCommunityInfoForEnroll item in results.Where(r=> constraints.Where(c => c.IdSource == r.Id || c.IdDestinationCommunity == r.Id).Any())){
                            LoadConstraintsInfo(idPerson, item, constraints.Where(c => c.IdSource == item.Id || c.IdDestinationCommunity == item.Id), communities);
                        }
                    }
                    
                }
                catch (Exception ex)
                {

                }
                return results;
            }
            private Dictionary<Int32, Int32> GetDefaultRoles(List<Int32> idTypes)
            {
                if (idTypes == null || !idTypes.Any())
                    return new Dictionary<Int32, Int32>();
                else
                    return (from r in Manager.GetIQ<liteRoleCommunityTypeTemplate>()
                            where idTypes.Contains(r.IdCommunityType) && r.isDefault
                            select r).ToDictionary(r => r.IdCommunityType, r => r.IdRole);
            }
            public Int32 GetDefaultRoleForEnrolling(Int32 idCommunityType)
            {
                return (from r in Manager.GetIQ<liteRoleCommunityTypeTemplate>()
                        where idCommunityType == r.IdCommunityType && r.isDefault
                        select r.IdRole).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            private Dictionary<Int32, long> GetEnrolledUsers(Dictionary<Int32, List<Int32>> items, Dictionary<Int32, Int32> dRoles)
            {
                Dictionary<Int32, long> results = new Dictionary<Int32, long>();
                var query = (from s in Manager.GetIQ<LazySubscription>() select s);

                items.Where(i => dRoles.ContainsKey(i.Key)).ToList().ForEach(i => i.Value.ForEach(v => results.Add(v, query.Where(s => s.IdRole == dRoles[i.Key] && s.IdCommunity == v).Count())));

                //(long)
                return results;
            }
          
            private void LoadConstraintsInfo(Int32 idPerson, dtoCommunityInfoForEnroll item, IEnumerable<liteCommunityConstraint> items, List<liteCommunityInfo> communities)
            {
                item.Constraints = GetInternalConstraints(idPerson, item.Id, items, communities);
                if (item.Constraints.Where(c => !c.IsRespected).Any())
                    item.NotAvailableFor.Add(EnrollingStatus.Constraints);
            }

            private List<dtoCommunityConstraint> GetInternalConstraints(Int32 idPerson, Int32 idCommunity, IEnumerable<liteCommunityConstraint> constraints, List<liteCommunityInfo> communities =null)
            {
                return ServiceCommunityManagement.GetDtoCommunityConstraints(idPerson, idCommunity, constraints, (communities != null), communities);
            }
        
            public String GetTranslatedProfileType(litePerson person)
            {
                if (person == null || person.Id == 0)
                    return "";
                else
                    return Manager.GetTranslatedItem<dtoTranslatedProfileType>(person.LanguageID).Where(t => t.Id == person.TypeID).Skip(0).Take(1).ToList().Select(t => t.Name).FirstOrDefault();
            }
            public dtoEnrollment EnrollTo(Int32 idPerson, dtoCommunityInfoForEnroll item)
            {
                return ServiceCommunityManagement.EnrollTo(idPerson, item);
            }

            public litePerson GetResponsible(Int32 idCommunity)
            {
                Int32 idResponsible = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.isResponsabile && s.IdCommunity== idCommunity select s.IdPerson).Skip(0).Take(1).ToList().FirstOrDefault();
                litePerson person = (idResponsible > 0) ? Manager.GetLitePerson(idResponsible) : null;
                return (person == null || person.Id == 0) ? null : person;
            }
            public long GetWaitingEnrollments(Int32 idCommunity)
            {
                return (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdRole >0 && !s.Accepted && !s.Enabled  &&  s.IdCommunity == idCommunity select s.IdPerson).Count();
            }
            public List<dtoEnrollmentsDetailInfo> GetEnrollmentsInfo(Int32 idCommunity,Int32 idCommunityType, Int32 idLanguage)
            {
                List<dtoTranslatedRoleType> roles = Manager.GetTranslatedItem<dtoTranslatedRoleType>(idLanguage);
                Int32 idRole = GetDefaultRoleForEnrolling(idCommunityType);
                return (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                                          where s.IdRole > 0 && s.IdCommunity == idCommunity
                        select s).ToList().GroupBy(s => s.IdRole).Select(s => new dtoEnrollmentsDetailInfo { IdRole = s.Key, IsDefault= (idRole==s.Key), Count = s.Count(), Waiting = s.Where(sb => !sb.Accepted && !sb.Enabled).Count(), Blocked = s.Where(sb => sb.Accepted && !sb.Enabled).Count(), Name = roles.Where(r => r.Id == s.Key).Select(r => r.Name).FirstOrDefault() }).OrderBy(r => r.Name).ToList(); 

            }
            public List<dtoCommunityConstraint> GetDtoCommunityConstraints(Int32 idCommunity, Int32 idPerson)
            {
                List<Int32> idCommunities = new List<Int32> { idCommunity };
                List<liteCommunityConstraint> constraints = ServiceCommunityManagement.GetConstraints(idCommunities);
                if (constraints.Any())
                {
                    idCommunities.AddRange(constraints.Where(c => !idCommunities.Contains(c.IdSource)).Select(c => c.IdSource).ToList());
                    idCommunities.AddRange(constraints.Where(c => !idCommunities.Contains(c.IdDestinationCommunity)).Select(c => c.IdDestinationCommunity).ToList());
                }
                idCommunities = idCommunities.Where(c => c > 0).Distinct().ToList();

                var query = (from c in Manager.GetIQ<liteCommunityInfo>() select c);
                if (idCommunities.Count() <= maxItemsForQuery)
                    query = query.Where(c => idCommunities.Contains(c.Id));
                else
                    query = query.ToList().Where(c => idCommunities.Contains(c.Id)).AsQueryable();

                List<liteCommunityInfo> communities = query.ToList();
                return ServiceCommunityManagement.GetDtoCommunityConstraints(idPerson, idCommunity, constraints, true, communities);
            }
            public String GetDescription(Int32 idCommunity)
            {
                liteCommunityDetails details = Manager.Get<liteCommunityDetails>(idCommunity);
                return (details == null) ? "" : details.Description;
            }
            public String GetTranslatedCommunityType(Int32 idLanguage, Int32 idType)
            {
                return Manager.GetTranslatedItem<dtoTranslatedCommunityType>(idLanguage).Where(i => i.Id == idType).Select(i=> i.Name).FirstOrDefault();
            }
           
            public long GetEnrolledUsersWithDefaultRole(Int32 idCommunity,Int32 idCommunityType)
            {
                Int32 idRole = GetDefaultRoleForEnrolling(idCommunityType);
                return GetEnrolledUsersCount(idRole, idCommunity, GetUsersIdByType((Int32)UserTypeStandard.TypingOffice));
            }
        #endregion

        #region "Manage Filters"
            public List<Filter> GetDefaultFilters(Int32 idProfile, String searchBy = "", Int32 idCommunityTypeToLoad = -1, long idTile = -1, List<long>idTags =null, Dictionary<searchFilterType, long> defaultValues = null, CommunityAvailability availability = CommunityAvailability.Subscribed, Int32 idOrganization = -2, List<Int32> unloadIdCommunities = null, List<Int32> onlyFromOrganizations = null, Dictionary<Int32, long> requiredPermissions = null, Int32 idCommunityFather = 0)
            {
                if (defaultValues==null){
                    defaultValues = (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n !=  searchFilterType.none && n!= searchFilterType.tagassociation select n) .ToDictionary(t=> t, t=> (long)-1);
                    defaultValues[searchFilterType.organization] = idOrganization;
                    defaultValues[searchFilterType.name] = (!String.IsNullOrEmpty(searchBy) ? 0 : -1);
                    if (idTile > 0 && idTags !=null )
                    {
                        liteTile tile = GetTile(idTile);
                        if (tile != null)
                        {
                            switch (tile.Type)
                            {
                                case TileType.CommunityType:
                                    if (tile.CommunityTypes != null)
                                    {
                                        idCommunityTypeToLoad = tile.CommunityTypes.FirstOrDefault();
                                        idTile = -1;
                                    }
                                    break;
                                case TileType.CommunityTag:
                                case TileType.CombinedTags:
                                    if (idTags == null)
                                    {
                                        if (tile.Tags != null && tile.Tags.Any(t => t.Tag != null) && tile.Tags.Any(t => t.Deleted == BaseStatusDeleted.None))
                                        {
                                            defaultValues[searchFilterType.tag] = 0;
                                            idTags = new List<long>();
                                            idTags.AddRange(tile.Tags.Where(t => t.Tag != null && t.Deleted == BaseStatusDeleted.None).Select(t => t.Tag.Id).ToList());
                                        }
                                    }
                                    else
                                        defaultValues[searchFilterType.tag] = 0;
                                   break;
                            }
                        }
                    }
                    defaultValues[searchFilterType.communitytype] = idCommunityTypeToLoad;
                    defaultValues[searchFilterType.year] = -2;
                   
                }
                return GetFilters(idProfile, searchBy, idCommunityTypeToLoad, idTile, defaultValues, availability, idTags, unloadIdCommunities, onlyFromOrganizations, requiredPermissions, idCommunityFather);
            }
            public List<Filter> GetDefaultFiltersForAssignments(Int32 idProfile, String searchBy = "", Int32 idCommunityTypeToLoad = -1, Dictionary<searchFilterType, long> defaultValues = null, CommunityAvailability availability = CommunityAvailability.Subscribed,Boolean assigned = false, List<Int32> unloadIdCommunities = null, List<Int32> onlyFromOrganizations = null, Dictionary<Int32, long> requiredPermissions = null)
            {
                if (defaultValues == null)
                {
                    defaultValues = (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none select n).ToDictionary(t => t, t => (long)-1);
                    defaultValues[searchFilterType.organization] = -2;
                    defaultValues[searchFilterType.name] = (!String.IsNullOrEmpty(searchBy) ? 0 : -1);
                    defaultValues[searchFilterType.communitytype] = idCommunityTypeToLoad;
                    defaultValues[searchFilterType.tagassociation] = (assigned) ? 1 : 0;
                }
                return GetFilters(idProfile, searchBy, idCommunityTypeToLoad,-1, defaultValues, availability, null,  unloadIdCommunities, ServiceCommunityManagement.GetAvailableIdOrganizations(idProfile, SearchCommunityFor.SystemManagement), requiredPermissions);
            }
            public  lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters GetDefaultFilters(litePerson person, CommunityAvailability availability,Boolean forBulkAssignment,Int32 preloadIdCommunityType = -1, List<Int32> unloadIdCommunities = null,Dictionary<Int32, long> requiredPermissions = null){
                List<Int32> onlyFromOrganizations = ServiceCommunityManagement.GetAvailableIdOrganizations(person.Id, SearchCommunityFor.SystemManagement);
                lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters = new lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters();
                filters.Availability = availability;

                List<dtoTreeCommunityNode> nodes = null;
                if (availability == CommunityAvailability.NotSubscribed)
                {
                    dtoTreeCommunityNode root = ServiceCommunityManagement.GetAllCommunitiesTree(person);
                    root.SetSubscriptionAvailability();
                    nodes = ServiceCommunityManagement.GetAvailableNodes(root.GetAllNodes(), unloadIdCommunities, onlyFromOrganizations, requiredPermissions);
                }
                else
                    nodes = ServiceCommunityManagement.GetAvailableNodes(ServiceCommunityManagement.GetAllCommunitiesTree(person).GetAllNodes(), unloadIdCommunities, onlyFromOrganizations, requiredPermissions);

                switch (availability)
                {
                    case CommunityAvailability.Subscribed:
                        nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable && n.Selected).ToList();
                        break;
                    case CommunityAvailability.NotSubscribed:
                        nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable && !n.Selected).ToList();
                        //nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable && (!n.Selected || n.IdFather==0)).ToList();
                        break;
                    case CommunityAvailability.None:
                        nodes = new List<dtoTreeCommunityNode>();
                        break;
                    default:
                        nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).ToList();
                        break;
                }

                if (forBulkAssignment)
                {
                    filters.WithoutTags = true;
                    List<Int32> idCommunities = Service.CacheGetCommunityAssociation(false).Select(t => t.IdCommunity).Distinct().ToList();
                    nodes.Where(n => idCommunities.Contains(n.Id)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                    nodes.Where(n => n.IdFather == 0 && n.Type == dtoCommunityNodeType.NotSelectable && !n.HasSelectableNodes()).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                    nodes.Where(n => n.IdFather == 0 && n.Type == dtoCommunityNodeType.NotSelectable && n.HasSelectableNodes()).ToList().ForEach(n => n.Type = dtoCommunityNodeType.None);
                    nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).ToList();
                }
                liteOrganizationProfile dOrganization = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == person.Id && o.isDefault select o).Skip(0).Take(1).ToList().FirstOrDefault();

                if (!nodes.Any())
                    filters.IdOrganization = -1;
                else if (dOrganization != null && nodes.Where(n => n.IdOrganization == dOrganization.IdOrganization && (availability!= CommunityAvailability.NotSubscribed || n.HasSelectableNodes() )).Any())
                    filters.IdOrganization = dOrganization.IdOrganization;
                else if (availability== CommunityAvailability.NotSubscribed){
                    switch (nodes.Where(n => n.IdFather == 0 && (!n.Selected || (n.Selected && n.HasSelectableNodes()))).Count())
                    {
                        case 1:
                            filters.IdOrganization = nodes.Where(n => n.IdFather == 0 && (!n.Selected || (n.Selected && n.HasSelectableNodes()))).OrderBy(n => n.Name).Select(n => n.IdOrganization).FirstOrDefault();
                            break;
                        default:
                            filters.IdOrganization = -1;
                            break;
                    } 
                }
                else
                    filters.IdOrganization = nodes.Where(n => n.IdFather == 0).OrderBy(n => n.Name).Select(n => n.IdOrganization).FirstOrDefault();
              
               
                filters.SearchBy = CommunityManagement.SearchCommunitiesBy.Contains;
                filters.Value = "";
                filters.Availability = availability;
                String path = ".";
                if (filters.IdOrganization > 0)
                {
                    if (nodes.Where(n => n.IdOrganization == filters.IdOrganization && n.IdFather == 0).Any())
                        path = nodes.Where(n => n.IdOrganization == filters.IdOrganization && n.IdFather == 0).FirstOrDefault().Path;
                    nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.None && (String.IsNullOrEmpty(path) || n.Path.StartsWith(path))).ToList();
                }
                if (preloadIdCommunityType != -1)
                    filters.IdcommunityType = preloadIdCommunityType;
                else if (nodes.Select(n => n.IdCommunityType).Distinct().Count() == 1)
                    filters.IdcommunityType = nodes.Where(n => n.Type != dtoCommunityNodeType.None && (String.IsNullOrEmpty(path) || n.Path.StartsWith(path))).Select(n => n.IdCommunityType).Distinct().FirstOrDefault();
                else
                    filters.IdcommunityType = -1;

                nodes = nodes.Where(n => n.IdCommunityType == filters.IdcommunityType || filters.IdcommunityType == -1).ToList();
                switch (filters.IdcommunityType)
                {
                    case (int)CommunityTypeStandard.Degree:
                        if (nodes.Select(n => n.IdDegreeType).Distinct().Count() == 1)
                        {
                            filters.IdDegreeType = nodes.Select(n => n.IdDegreeType).Distinct().FirstOrDefault();
                            nodes = nodes.Where(n => n.IdDegreeType == filters.IdDegreeType).ToList();
                        }
                        else
                            filters.IdDegreeType = -1;
                        break;
                    case (int)CommunityTypeStandard.UniversityCourse:
                        if (nodes.Select(n => n.Year).Distinct().Count() == 1)
                        {
                            filters.Year = nodes.Select(n => n.Year).Distinct().FirstOrDefault();
                            nodes = nodes.Where(n => n.Year == filters.Year).ToList();
                        }
                        else
                            filters.Year = -1;
                        if (nodes.Select(n => n.IdCourseTime).Distinct().Count() == 1)
                        {
                            filters.IdCourseTime = nodes.Select(n => n.IdCourseTime).Distinct().FirstOrDefault();
                            nodes = nodes.Where(n => n.IdCourseTime == filters.IdCourseTime).ToList();
                        }
                        else
                            filters.IdCourseTime = -1;
                        break;
                }
                if (nodes.Select(n => n.IdResponsible).Distinct().Count() == 1)
                {
                    filters.IdResponsible = nodes.Select(n => n.IdResponsible).Distinct().FirstOrDefault();
                    nodes = nodes.Where(n => n.IdResponsible == filters.IdResponsible).ToList();
                }
                else
                    filters.IdResponsible = -1;
                return filters;
             }

            public List<Filter> ChangeFilters(Boolean alsoTagsAssociation, Int32 idProfile, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter filter, Int32 idCommunityTypeToLoad = -1,long idTile = -1, CommunityAvailability availability = CommunityAvailability.Subscribed, List<Int32> unloadIdCommunities = null, List<Int32> onlyFromOrganizations = null, Dictionary<Int32, long> requiredPermissions = null)
            {
                if (filter != null)
                {
                    Dictionary<searchFilterType, long> dValues = GetDefaultValues(filters, alsoTagsAssociation);
                  
                    List<long> selectedTags = null;

                    liteTile tile = null;
                    if (idTile > 0)
                    {
                        tile = ServiceTiles.GetTile(idTile);
                        if (tile != null)
                        {
                            switch (tile.Type)
                            {
                                case TileType.CommunityType:
                                    if (tile.CommunityTypes != null)
                                    {
                                        idCommunityTypeToLoad = tile.CommunityTypes.FirstOrDefault();
                                        idTile = -1;
                                    }
                                    break;
                                case TileType.CommunityTag:
                                case TileType.CombinedTags:
                                    if (tile.Tags != null && tile.Tags.Any(t => t.Tag != null) && tile.Tags.Any(t => t.Deleted == BaseStatusDeleted.None))
                                        selectedTags = tile.Tags.Where(t => t.Tag != null && t.Deleted == BaseStatusDeleted.None).Select(t => t.Tag.Id).ToList();
                                    break;
                            }
                        }
                    }
                    if (selectedTags == null)
                    {
                        if (filters.Where(f => f.Name == searchFilterType.tag.ToString()).Any() && (!dValues.ContainsKey(searchFilterType.tagassociation) || (dValues.ContainsKey(searchFilterType.tagassociation) && dValues[searchFilterType.tagassociation] != 0)))
                            selectedTags = filters.Where(f => f.Name == searchFilterType.tag.ToString()).FirstOrDefault().SelectedIds.Select(t => t.Id).ToList();
                        else
                            selectedTags = new List<long>();
                    }
                    if (idCommunityTypeToLoad > 0)
                        dValues[searchFilterType.communitytype] = idCommunityTypeToLoad;
                    lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType type = lm.Comol.Core.DomainModel.Helpers.EnumParser<searchFilterType>.GetByString(filter.Name, searchFilterType.none );
                    //switch (type)
                    //{
                    //    case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.organization:
                    //        List<lm.Comol.Core.DomainModel.Filters.Filter>  results = GetFilters(idProfile, filters.Where(f => f.Name == searchFilterType.name.ToString()).Select(f => f.Value).FirstOrDefault(), idCommunityTypeToLoad, dValues, availability, selectedTags, unloadIdCommunities, onlyFromOrganizations, requiredPermissions);
                    //        if (results.Any(r => r.Name == searchFilterType.organization.ToString()){
                    //            results.FirstOrDefault(r => r.Name == searchFilterType.organization.ToString()) = filter;
                    //            }
                    //        else {
                    //            results.Insert(0, filter);
                    //        }
                    //        return results;
                    //    default:
                            litePerson person = Manager.GetLitePerson(idProfile);
                            List<dtoTreeCommunityNode> nodes = GetFilteredItems(dValues,person, availability, unloadIdCommunities, onlyFromOrganizations, requiredPermissions, selectedTags, idTile );
                            String path = (dValues[searchFilterType.organization] < 1) ? "" : GetOrganizationPath((Int32)dValues[searchFilterType.organization], nodes);
                            if (availability == CommunityAvailability.NotSubscribed)
                                nodes = nodes.Where(n => !(n.IdCommunityType == (int)CommunityTypeStandard.Organization && n.Selected)).ToList();
                            
                            if (dValues.ContainsKey(searchFilterType.tagassociation))
                            {
                                List<Int32> idCommunities = Service.CacheGetCommunityAssociation(false).Select(t => t.IdCommunity).Distinct().ToList();
                                if (dValues[searchFilterType.tagassociation] == 0)
                                    nodes.Where(n => idCommunities.Contains(n.Id)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                                else
                                    nodes.Where(n => !idCommunities.Contains(n.Id)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                            }
                            var query = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable && (String.IsNullOrEmpty(path) || n.Path.StartsWith(path)));
                            switch (type)
                            {
                                case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.organization:
                                    filters = filters.Where(f => f.Name == searchFilterType.name.ToString() || f.Name == searchFilterType.tagassociation.ToString()).ToList();
                                    AddOrganizationFilters(idCommunityTypeToLoad, availability, nodes, dValues, filters, filter, selectedTags, idTile);
                                    break;
                                case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.communitytype:
                                    filters = filters.Where(f => f.Name == searchFilterType.organization.ToString() || f.Name == searchFilterType.name.ToString() || f.Name == searchFilterType.tagassociation.ToString()).ToList();
                                    AddCommunityTypeFilters(query, dValues, filters, filter, selectedTags, idTile);

                                    Filter tResponsible = filters.Where(f => f.Name == searchFilterType.responsible.ToString()).FirstOrDefault();
                                    //if (tResponsible != null && !(dValues[searchFilterType.communitytype] == (long)CommunityTypeStandard.Degree || dValues[searchFilterType.communitytype] == (long)CommunityTypeStandard.UniversityCourse))
                                    //    tResponsible.AutoUpdate = false;

                                    break;
                                case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.responsible:
                                    filters = filters.Where(f => f.Name == searchFilterType.organization.ToString() || f.Name == searchFilterType.name.ToString() || f.Name == searchFilterType.communitytype.ToString() || f.Name == searchFilterType.tagassociation.ToString()).ToList();

                                    query = query.Where(n => dValues[searchFilterType.communitytype] == -1 || (dValues[searchFilterType.communitytype] == n.IdCommunityType));

                                    AddResponsibleFilters(nodes, dValues, filters, filter, selectedTags,idTile);
                                    //if (filter != null && !(dValues[searchFilterType.communitytype] == (long)CommunityTypeStandard.Degree || dValues[searchFilterType.communitytype] == (long)CommunityTypeStandard.UniversityCourse))
                                    //    filter.AutoUpdate = false;

                                    break;
                                case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.year:
                                    filters = filters.Where(f => f.Name != searchFilterType.coursetime.ToString() && f.Name != searchFilterType.tag.ToString() && f.Name != searchFilterType.letters.ToString() && f.Name != searchFilterType.tag.ToString()).ToList();
                                    query = query.Where(n => dValues[searchFilterType.communitytype] == (long)CommunityTypeStandard.UniversityCourse);
                                    query = query.Where(n => dValues[searchFilterType.responsible] == -1 ||   dValues[searchFilterType.responsible] == n.IdResponsible);
                                    query = query.Where(n => dValues[searchFilterType.year] < 0 || dValues[searchFilterType.year] == n.Year);
                                    query = query.Where(n => dValues[searchFilterType.status] == -1 || dValues[searchFilterType.status] == (long)n.Status);

                                    if ((!dValues.ContainsKey(searchFilterType.tagassociation) && idTile < 1) || (dValues.ContainsKey(searchFilterType.tagassociation) && dValues[searchFilterType.tagassociation] > 0))
                                    {
                                        lm.Comol.Core.DomainModel.Filters.Filter tFilter = GetTagFilter(query, selectedTags, idTile );
                                        tFilter.GridSize = (!dValues.ContainsKey(searchFilterType.tagassociation) ? filter.GridSize : 12);
                                        filters.Add(tFilter);
                                    }

                                    filters.Add(GetLettersFilter(query, dValues));
                                    break;
                                case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tagassociation:
                                    filters = GetFilters(idProfile, filters.Where(f => f.Name == searchFilterType.name.ToString()).Select(f => f.Value).FirstOrDefault(), idCommunityTypeToLoad, -1, dValues, availability, selectedTags, unloadIdCommunities, onlyFromOrganizations, requiredPermissions);
                                    break;
                            }
                    //        break;
                    //}
                }
                return filters;
            }
            private List<dtoTreeCommunityNode> GetFilteredItems(Dictionary<searchFilterType, long> dValues, litePerson person, CommunityAvailability availability = CommunityAvailability.Subscribed, List<Int32> unloadIdCommunities = null, List<Int32> onlyFromOrganizations = null, Dictionary<Int32, long> requiredPermissions = null, List<long> tags = null, long idTile=-1)
            {
                List<dtoTreeCommunityNode> nodes = null;
                if (availability == CommunityAvailability.NotSubscribed)
                {
                    dtoTreeCommunityNode root = ServiceCommunityManagement.GetAllCommunitiesTree(person);
                    root.SetSubscriptionAvailability();
                    nodes = ServiceCommunityManagement.GetAvailableNodes(root.GetAllNodes(), unloadIdCommunities, onlyFromOrganizations, requiredPermissions);
                }
                else
                    nodes = ServiceCommunityManagement.GetAvailableNodes(ServiceCommunityManagement.GetAllCommunitiesTree(person).GetAllNodes(), unloadIdCommunities, onlyFromOrganizations, requiredPermissions);
                switch (availability)
                {
                    case CommunityAvailability.Subscribed:
                        nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable && n.Selected).ToList();
                        break;
                    case CommunityAvailability.NotSubscribed:
                        nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable  && (!n.Selected || n.IdFather==0)  ).ToList(); //&& (!n.Selected )|| n.IdFather==0
                        break;
                    case CommunityAvailability.None:
                        nodes = new List<dtoTreeCommunityNode>();
                        break;
                    default:
                        nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).ToList();
                        break;
                }

                List<Int32> idCommunities = null;
                if (dValues.ContainsKey(searchFilterType.tagassociation))
                {
                    idCommunities = Service.CacheGetCommunityAssociation(false).Select(t => t.IdCommunity).Distinct().ToList();
                    if (dValues[searchFilterType.tagassociation] == 0)
                        nodes.Where(n => idCommunities.Contains(n.Id)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                    else
                        nodes.Where(n => !idCommunities.Contains(n.Id)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                    //var q = nodes.Where(n => n.IdFather == 0).ToList();
                    UpdateUnselecatbleNodes(nodes, nodes.Where(n => n.Type == dtoCommunityNodeType.NotSelectable).Select(n => n.Id).ToList(), nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).Select(n => n.Id).ToList());
                    nodes.Where(n => n.IdFather == 0 && n.Type == dtoCommunityNodeType.NotSelectable && !n.HasSelectableNodes()).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                    UpdateUnselecatbleNodes(nodes, nodes.Where(n => n.Type == dtoCommunityNodeType.NotSelectable).Select(n => n.Id).ToList(), nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).Select(n => n.Id).ToList());

                    //var q1 = nodes.Where(n => n.IdFather == 0).ToList();
                    nodes.Where(n => n.IdFather == 0 && n.Type == dtoCommunityNodeType.NotSelectable && n.HasSelectableNodes()).ToList().ForEach(n => n.Type = dtoCommunityNodeType.None);
                    //var q3 = nodes.Where(n => n.IdFather == 0).ToList();
                    nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).ToList();
                }
                else if (idTile > 0 && tags != null && tags.Count > 0 && person!=null)
                {
                    idCommunities = nodes.Select(c => c.Id).Distinct().ToList();
                    idCommunities = Service.CacheGetUserCommunitiesAssociation(person.Id, idCommunities, Tag.Domain.TagType.Community, true).Where(ta => ContainsAllItems<long>(ta.Tags, tags) && idCommunities.Contains(ta.IdCommunity)).Select(t => t.IdCommunity).ToList();
                    //var q4 = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).ToList();
                    //var q5 = nodes.Where(n => n.Type == dtoCommunityNodeType.NotSelectable).ToList();
                    nodes.Where(n => !idCommunities.Contains(n.Id)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);

                    //var q6 = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).ToList();
                    //var q7 = nodes.Where(n => n.Type == dtoCommunityNodeType.NotSelectable).ToList();
                    UpdateUnselecatbleNodes(nodes, nodes.Where(n => n.Type == dtoCommunityNodeType.NotSelectable).Select(n => n.Id).ToList(), nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).Select(n => n.Id).ToList());
                    nodes.Where(n => n.IdFather == 0 && n.Type == dtoCommunityNodeType.NotSelectable && !n.HasSelectableNodes()).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);

                    //var q1 = nodes.Where(n => n.IdFather == 0).ToList();
                    UpdateUnselecatbleNodes(nodes, nodes.Where(n => n.Type == dtoCommunityNodeType.NotSelectable).Select(n => n.Id).ToList(), nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).Select(n => n.Id).ToList());
                    nodes.Where(n => n.IdFather == 0 && n.Type == dtoCommunityNodeType.NotSelectable && n.HasSelectableNodes()).ToList().ForEach(n => n.Type = dtoCommunityNodeType.None);
                   // var q3 = nodes.Where(n => n.IdFather == 0).ToList();
                    nodes = nodes.Where(n => n.Type != dtoCommunityNodeType.NotSelectable).ToList();
                }
                return nodes;
            }

            private void UpdateUnselecatbleNodes(List<dtoTreeCommunityNode> nodes, List<Int32> idToUnselect, List<Int32> idSelected)
            {
                foreach (dtoTreeCommunityNode n in nodes) {
                    if (idToUnselect.Contains(n.Id) || !idSelected.Contains(n.Id))
                        n.Type = dtoCommunityNodeType.NotSelectable;
                    if (n.Nodes.Any())
                        UpdateUnselecatbleNodes(n.Nodes, idToUnselect, idSelected);
                }
            }
            //private void UpdateOrganizationWithHasSelectableNodes(IEnumerable<dtoTreeCommunityNode> query,nodes)
            //{
            //    Boolean result = (Type != dtoCommunityNodeType.NotSelectable && Type != dtoCommunityNodeType.None);
            //    if (!result)
            //        return Nodes.Where(n => n.HasSelectableNodes()).Any();
            //    return result;
            //}
            private List<Filter> GetFilters(Int32 idProfile, String searchBy, Int32 idCommunityTypeToLoad,long idTile, Dictionary<searchFilterType, long> defaultValues, CommunityAvailability availability, List<long> tags, List<Int32> unloadIdCommunities = null, List<Int32> onlyFromOrganizations = null, Dictionary<Int32, long> requiredPermissions = null, Int32 idCommunityFather = 0)     
            {
                List<lm.Comol.Core.DomainModel.Filters.Filter> filters = new List<lm.Comol.Core.DomainModel.Filters.Filter>();
                litePerson person = Manager.GetLitePerson(idProfile);
                List<dtoTreeCommunityNode> nodes = GetFilteredItems(defaultValues, person, availability, unloadIdCommunities, onlyFromOrganizations, requiredPermissions, tags, idTile);
                if (defaultValues.ContainsKey(searchFilterType.tagassociation))
                    filters.Add(GetTagAssociationFilter(defaultValues[searchFilterType.tagassociation]));
                lm.Comol.Core.DomainModel.Filters.Filter organization = null;
              
                switch (idCommunityTypeToLoad)
                {
                    case (int)CommunityTypeStandard.UniversityCourse:
                        organization = GetOrganizationFilter(person, nodes, defaultValues, availability, idCommunityTypeToLoad, unloadIdCommunities, onlyFromOrganizations);
                        if (organization != null && organization.Values.Any())
                            AddOrganizationFilters(idCommunityTypeToLoad, availability, nodes, defaultValues, filters, organization, tags, idTile);

                        break;
                    case (int)CommunityTypeStandard.Degree:
                        organization = GetOrganizationFilter(person, nodes, defaultValues, availability, idCommunityTypeToLoad,unloadIdCommunities, onlyFromOrganizations);
                        if (organization != null && organization.Values.Any())
                            AddOrganizationFilters(idCommunityTypeToLoad, availability, nodes, defaultValues, filters, organization, tags, idTile);
                        break;
                    case (int)CommunityTypeStandard.Organization:
                        nodes = nodes.Where(n =>n.IdCommunityType == (int)CommunityTypeStandard.Organization && ((availability != CommunityAvailability.NotSubscribed && availability != CommunityAvailability.Subscribed ) || (availability== CommunityAvailability.NotSubscribed && !n.Selected) || (availability== CommunityAvailability.Subscribed && n.Selected))).ToList();
                        Filter responsible = GetResponsiblesFilter(nodes.Where(n => n.IdCommunityType == (int)CommunityTypeStandard.Organization), defaultValues);
                        responsible.AutoUpdate = !defaultValues.ContainsKey(searchFilterType.tagassociation);
                        if (responsible.Values.Any())
                            AddResponsibleFilters(nodes.Where(n => n.IdCommunityType == (int)CommunityTypeStandard.Organization), defaultValues, filters, responsible, tags, idTile);
                        break;
                    case -1:
                        organization = GetOrganizationFilter(person, nodes, defaultValues, availability, idCommunityTypeToLoad, unloadIdCommunities, onlyFromOrganizations);

                        if (organization != null && organization.Values.Any())
                            AddOrganizationFilters(idCommunityTypeToLoad, availability, nodes, defaultValues, filters, organization, tags, idTile);
                        Filter gResponsible = filters.Where(f => f.Name == searchFilterType.responsible.ToString()).FirstOrDefault();
                        if (gResponsible != null && gResponsible.Values.Any())
                            gResponsible.AutoUpdate = !defaultValues.ContainsKey(searchFilterType.tagassociation);
                        break;
                    default:
                        organization = GetOrganizationFilter(person, nodes, defaultValues, availability, idCommunityTypeToLoad, unloadIdCommunities, onlyFromOrganizations);
                        if (organization != null && organization.Values.Any())
                            AddOrganizationFilters(idCommunityTypeToLoad, availability, nodes, defaultValues, filters, organization, tags, idTile);
                        Filter dResponsible = filters.Where(f => f.Name == searchFilterType.responsible.ToString()).FirstOrDefault();
                        if (dResponsible != null && dResponsible.Values.Any())
                            dResponsible.AutoUpdate = !defaultValues.ContainsKey(searchFilterType.tagassociation);
                        break;
                }
                if (filters != null && filters.Any())
                    filters.Add(GetSimpleSearchFilter(searchBy));
                return filters;
            }

            private Dictionary<searchFilterType, long> GetDefaultValues(List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Boolean alsoTagsAssociation)
            {
                Dictionary<searchFilterType, long> values = (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none && (alsoTagsAssociation || (!alsoTagsAssociation && n!= searchFilterType.tagassociation)) select n).ToDictionary(t => t, t => (long)-1);
                foreach (searchFilterType key in (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none && (alsoTagsAssociation || (!alsoTagsAssociation && n!= searchFilterType.tagassociation)) select n)){
                    Filter filter = filters.Where(f=> f.Name == key.ToString()).FirstOrDefault();
                    if (filter == null)
                        values[key] = -1;
                    else if (filter.FilterType == FilterType.Checkbox)
                    {
                        if (filter.Values.Where(v => v.Checked).Any())
                            values[key] = filter.Values.Where(v => v.Checked).Select(v => v.Id).FirstOrDefault();
                        else
                            values[key] = -1;
                    }
                    else
                    {
                        switch (key)
                        {
                            case searchFilterType.name:
                                values[key] = (String.IsNullOrEmpty(filter.Value) ? -1 : 0);
                                break;
                            case searchFilterType.tag:
                                values[key] = (filter.SelectedIds.Any() ? 0 : -1);
                                break;
                            default:
                                values[key] = (filter.Selected != null) ? filter.Selected.Id : -1;
                                break;
                        }
                    }
                }
                return values;
            }
            private void AddOrganizationFilters(Int32 idCommunityTypeToLoad, CommunityAvailability availability, List<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter organization, List<long> selectedTags, long idTile)
            {
                String path = ".";
                if (organization.Selected.Id > 0)
                    path = GetOrganizationPath((Int32)organization.Selected.Id, nodes);
                    
                filters.Add(organization);
                if (availability == CommunityAvailability.NotSubscribed)
                    nodes = nodes.Where(n => !(n.IdCommunityType == (int)CommunityTypeStandard.Organization && n.Selected)).ToList();

                if (idCommunityTypeToLoad == -1)
                {
                    Filter communityType = GetCommunityTypeFilter(nodes.Where(n => n.Type!= dtoCommunityNodeType.None && (String.IsNullOrEmpty(path) || n.Path.StartsWith(path))), defaultValues);
                    AddCommunityTypeFilters(nodes.Where(n => String.IsNullOrEmpty(path) || n.Path.StartsWith(path)), defaultValues, filters, communityType, selectedTags, idTile);
                }
                else
                {
                    Filter responsible = GetResponsiblesFilter(nodes.Where(n => n.Type!= dtoCommunityNodeType.None && (String.IsNullOrEmpty(path) || n.Path.StartsWith(path))), defaultValues);
                    AddResponsibleFilters(nodes.Where(n => String.IsNullOrEmpty(path) || n.Path.StartsWith(path)), defaultValues, filters, responsible, selectedTags, idTile);
                }
            }

            private String GetOrganizationPath(Int32 idOrganization,List<dtoTreeCommunityNode> nodes)
            {
                if (nodes.Any(n => n.IdOrganization == idOrganization && n.IdFather == 0))
                    return nodes.Where(n => n.IdOrganization == idOrganization && n.IdFather == 0).FirstOrDefault().Path;
                else
                    return GetOrganizationPath(idOrganization);
            }

            private String GetOrganizationPath(Int32 idOrganization)
            {
                liteCommunity community = (from c in Manager.GetIQ<liteCommunity>() where c.IdFather==0 && c.IdOrganization==idOrganization && c.IdType==(int)CommunityTypeStandard.Organization select c).Skip(0).Take(1).ToList().FirstOrDefault();
                return (community == null) ? "." : "." + community.Id.ToString() + ".";
            }
            private void AddCommunityTypeFilters(IEnumerable<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter communityType, List<long> selectedTags,long idTile)
            {
                var query = nodes.Where(n=> defaultValues[searchFilterType.communitytype] == -1 || (defaultValues[searchFilterType.communitytype] == n.IdCommunityType));

                filters.Add(communityType);
                Filter responsible = GetResponsiblesFilter(query, defaultValues);
                AddResponsibleFilters(query, defaultValues, filters, responsible, selectedTags, idTile);
            }
            private void AddResponsibleFilters(IEnumerable<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter responsible, List<long> selectedTags, long idTile)
            {
                var query =nodes.Where(n => defaultValues[searchFilterType.responsible] == -1 || n.IdResponsible == defaultValues[searchFilterType.responsible]);
                filters.Add(responsible);

                lm.Comol.Core.DomainModel.Filters.Filter statusFilter = GetStatusFilter(query, defaultValues);
                if (statusFilter.Values.Count>1)
                    filters.Add(GetStatusFilter(query, defaultValues));

                if ((!defaultValues.ContainsKey(searchFilterType.tagassociation) && idTile<1 )|| (defaultValues.ContainsKey(searchFilterType.tagassociation) && (defaultValues[searchFilterType.tagassociation] > 0)))
                {
                    lm.Comol.Core.DomainModel.Filters.Filter filter = GetTagFilter(query, selectedTags, idTile );
                    filter.GridSize = (!defaultValues.ContainsKey(searchFilterType.tagassociation) ? filter.GridSize : 12);
                    filters.Add(filter);
                }
                   

                filters.Add(GetLettersFilter(query, defaultValues));

            }
            private void AddCourseYearFilters(IEnumerable<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter year)
            {
                var query = nodes.Where(n => ( n.Year>0 && defaultValues[searchFilterType.year] == -1) || (defaultValues[searchFilterType.year] == n.Year));

                filters.Add(year);
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetOrganizationFilter(litePerson person, List<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues, CommunityAvailability availability = CommunityAvailability.Subscribed, Int32 idCommunityTypeToLoad = -1, List<Int32> unloadIdCommunities = null, List<Int32> onlyFromOrganizations = null)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.organization.ToString();
                filter.DisplayOrder = (int)searchFilterType.organization;
                filter.AutoUpdate = true;

                liteOrganizationProfile dOrganization = (person==null) ? null : (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson== person.Id && o.isDefault select o).Skip(0).Take(1).ToList().FirstOrDefault();
                List<FilterListItem> items = null;
                switch(availability){
                    case CommunityAvailability.Subscribed:
                        items = nodes.Where(n => n.IdFather == 0 && (onlyFromOrganizations == null || !onlyFromOrganizations.Any() || onlyFromOrganizations.Contains(n.IdOrganization))).OrderBy(n => n.Name).Select(n => new FilterListItem() { Id = n.IdOrganization, Name = n.Name }).ToList();
                        break;
                    case CommunityAvailability.NotSubscribed:
                        items = nodes.Where(n => n.IdFather == 0 && (onlyFromOrganizations == null || !onlyFromOrganizations.Any() || onlyFromOrganizations.Contains(n.IdOrganization)) && n.HasNodes(GenerateFilters(idCommunityTypeToLoad, defaultValues, availability))).OrderBy(n => n.Name).Select(n => new FilterListItem() { Id = n.IdOrganization, Name = n.Name }).ToList();
                        break;
                    case CommunityAvailability.None:
                        return null;
                    case CommunityAvailability.All:
                        items = nodes.Where(n => n.IdFather == 0 && (onlyFromOrganizations == null || !onlyFromOrganizations.Any() || onlyFromOrganizations.Contains(n.IdOrganization))).OrderBy(n => n.Name).Select(n => new FilterListItem() { Id = n.IdOrganization, Name = n.Name }).ToList();
                        break;
                }
                List<long> idAddedOrganizations =items.Select(i=>i.Id).ToList();
                if (unloadIdCommunities != null && unloadIdCommunities.Any())
                {
                    List<Int32> idFathersOrganization = nodes.Where(n => n.IdFather > 0 && n.FirstFather !=null && !idAddedOrganizations.Contains((long)n.IdOrganization)).Select(n => n.IdFirstFatherOrganization).Distinct().ToList();
                    idFathersOrganization = idFathersOrganization.Where(i=> !idAddedOrganizations.Contains(i)).ToList();
                    if (idFathersOrganization.Any())
                    {
                        items.AddRange((from o in Manager.GetIQ<Organization>() where idFathersOrganization.Contains(o.Id) select new FilterListItem() { Id = o.Id, Name = o.Name }).ToList());
                        idAddedOrganizations = items.Select(i => i.Id).ToList();
                    }
                }
                if (onlyFromOrganizations != null && onlyFromOrganizations.Any() && onlyFromOrganizations.Any(i=> !items.Any(li=> li.Id== i))){
                    List<Int32> idOrganizations = onlyFromOrganizations.Where(i=> !items.Any(li=> li.Id== i)).ToList();
                    idOrganizations = idOrganizations.Where(o=> nodes.Any(n=> n.IdOrganization==o && n.Type!= dtoCommunityNodeType.NotSelectable)).ToList();
                    items.AddRange((from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select new FilterListItem() { Id = o.Id, Name = o.Name }).ToList());
                }
                filter.Values = items;
                if (filter.Values.Count>1)
                     filter.Values.Insert(0, new FilterListItem() { Id=-1});
                if (filter.Values.Where(v=> v.Id== defaultValues[searchFilterType.organization]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.organization]).Select(v => v).FirstOrDefault();
                else if (dOrganization != null && filter.Values.Where(v => v.Id == dOrganization.IdOrganization).Any())
                {
                    defaultValues[searchFilterType.organization] = dOrganization.IdOrganization;
                    filter.Selected = filter.Values.Where(v => v.Id == dOrganization.IdOrganization).FirstOrDefault();
                    filter.Values.Where(v => v.Id == dOrganization.IdOrganization).FirstOrDefault().Checked = true;
                }
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.organization] = filter.Selected.Id;
                }
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetCommunityTypeFilter(IEnumerable<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues)
            {
                List<dtoTranslatedCommunityType> translatedItems = Manager.GetTranslatedItem<dtoTranslatedCommunityType>(UC.Language.Id);
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.communitytype.ToString();
                filter.DisplayOrder = (int)searchFilterType.communitytype;
                filter.AutoUpdate = true;
                filter.Values = nodes.Select(n => n.IdCommunityType).Distinct().Select(n => new FilterListItem() { Id = n, Name = translatedItems.Where(t => t.Id == n).Select(t => t.Name).FirstOrDefault() }).OrderBy(t=> t.Name).ToList();
                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id =-1 });

                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.communitytype]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.communitytype]).FirstOrDefault();
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.communitytype] = filter.Selected.Id;
                }

                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetResponsiblesFilter(IEnumerable<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.responsible.ToString();
                filter.DisplayOrder = (int)searchFilterType.responsible;
                filter.AutoUpdate = true;
                filter.Values = ServiceCommunityManagement.GetResponsibles(nodes.ToList()).Select(p => new FilterListItem() { Id = p.Id, Name = p.SurnameAndName }).OrderBy(n=> n.Name).ToList();
                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id = -1 });

                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.responsible]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.responsible]).FirstOrDefault();
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.responsible] = filter.Selected.Id;
                }
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetStatusFilter(IEnumerable<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                List<lm.Comol.Core.Communities.CommunityStatus> items = nodes.Select(n => n.Status).Distinct().OrderBy(n=> n).ToList();
                filter.FilterType = DomainModel.Filters.FilterType.Radio;
                filter.Name = searchFilterType.status.ToString();
                filter.DisplayOrder = (int)searchFilterType.status;
                filter.AutoUpdate = false;
                filter.Values = items.Select(p => new FilterListItem() { Id = (long)p,  Name="" }).ToList();


                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.status]).Any())
                    filter.SelectedId = defaultValues[searchFilterType.status];
                else if (items.Contains(lm.Comol.Core.Communities.CommunityStatus.Active))
                {
                    filter.SelectedId = (long)lm.Comol.Core.Communities.CommunityStatus.Active;
                    defaultValues[searchFilterType.status] = (long)lm.Comol.Core.Communities.CommunityStatus.Active;
                }
                else if (filter.Values.Any())
                {
                    filter.SelectedId = filter.Values.FirstOrDefault().Id;
                    defaultValues[searchFilterType.status] = filter.SelectedId;
                }
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetYearFilter(IEnumerable<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.year.ToString();
                filter.DisplayOrder = (int)searchFilterType.year;
                filter.AutoUpdate = true;
                filter.Values = nodes.Where(n => n.Year > 0).Select(n => n.Year).Distinct().OrderByDescending(y=> y).Select(p => new FilterListItem() { Id = (long)p, Name= p.ToString() + "-" + (p+1).ToString() }).ToList();

                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id = -1 });
                if (defaultValues[searchFilterType.year] == -2 && filter.Values.Any() && filter.Values.Count > 0)
                {
                    filter.Selected = filter.Values.Where(v => v.Id != -1).FirstOrDefault();
                    defaultValues[searchFilterType.year] = (filter.Selected != null) ? filter.Selected.Id : -1;
                }
                else if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.year]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.year]).FirstOrDefault();
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.year] = filter.Selected.Id;
                }
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetTagFilter(IEnumerable<dtoTreeCommunityNode> nodes, List<long> values, long idTile)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.MultiSelect;
                filter.Name = searchFilterType.tag.ToString();
                filter.DisplayOrder = (int)searchFilterType.tag;
                List<lm.Comol.Core.Tag.Domain.liteCommunityTag> links = Service.CacheGetCommunityAssociation(true);
                List<Int32> idCommunities = nodes.Select(n => n.Id).Distinct().ToList();
                List<long> idTags = links.Where(t => idCommunities.Contains(t.IdCommunity)).Select(t => t.IdTag).Distinct().ToList();

                Language l = Manager.GetDefaultLanguage();

                filter.Values = Service.CacheGetTags(lm.Comol.Core.Tag.Domain.TagType.Community, true).Where(t => idTags.Contains(t.Id)).Select(t => new FilterListItem() { Id = t.Id, Disabled=(idTile>0) , Name = t.GetTitle(UC.Language.Id, l.Id), Checked =  (values!=null && values.Contains(t.Id))}).OrderBy(t=> t.Name).ToList();

                filter.SelectedIds = (filter.Values != null) ? filter.Values.Where(v => v.Checked).ToList() : new List<FilterListItem>();
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetSimpleSearchFilter(String searchBy){
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Text;
                filter.Name = searchFilterType.name.ToString();
                filter.DisplayOrder = (int)searchFilterType.name;
                filter.AutoUpdate = false;
                filter.Value= searchBy;
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetTagAssociationFilter(long value)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter(new FilterListItem(0, "", (value==0)));
                filter.FilterType = DomainModel.Filters.FilterType.Checkbox;
                filter.Name = searchFilterType.tagassociation.ToString();
                filter.DisplayOrder = (int)searchFilterType.tagassociation;
                filter.AutoUpdate = true;
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetLettersFilter(IEnumerable<dtoTreeCommunityNode> nodes, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.MaskedRadio;
                filter.Name = searchFilterType.letters.ToString();
                filter.DisplayOrder = (int)searchFilterType.letters;
                filter.AutoUpdate = false;
                filter.GridSize = 12;
                filter.Values = GenerateAlphabetItems(nodes.Select(n => n.FirstLetter).OrderBy(n => n).Distinct().ToList(), defaultValues[searchFilterType.letters]);
                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.letters]).Any())
                    filter.SelectedId = defaultValues[searchFilterType.letters];
                else
                {
                    filter.SelectedId = -1;
                    defaultValues[searchFilterType.letters] = -1;
                }
                return filter;
            }
            private List<FilterListItem> GenerateAlphabetItems(List<String> availableWords, long selected)
            {
                Boolean hasOtherChars = false;
                List<AlphabetItem> items = new List<AlphabetItem>();
                List<AlphabetItem> otherChars = new List<AlphabetItem>();

                //if (displayMode.IsFlagSet(AlphabetDisplayMode.commonletters) || displayMode.IsFlagSet(AlphabetDisplayMode.extendedletters))
                    items = (from n in Enumerable.Range(97, 26) select new AlphabetItem() { isEnabled = false, Value = Char.ConvertFromUtf32(n), DisplayName = Char.ConvertFromUtf32(n).ToString().ToUpper(), isSelected = false }).ToList();
                //if (displayMode.IsFlagSet(AlphabetDisplayMode.extendedletters))
                //    items.AddRange(GetOtherAlphabetItems(false));
                //if (displayMode.IsFlagSet(AlphabetDisplayMode.addUnmatchLetters))
                    //otherChars = GetOtherAlphabetItems(true);

                foreach (String l in availableWords)
                {
                    if (items.Where(i => i.Value == l).Any())
                        items.Where(i => i.Value == l).FirstOrDefault().isEnabled = true;
                    else if (System.Text.RegularExpressions.Regex.IsMatch(l, @"[^\w\.@-]", System.Text.RegularExpressions.RegexOptions.None))
                    {
                        String upper = "";
                        try
                        {
                            upper = l.ToUpper();
                        }
                        catch (Exception ex)
                        {
                            upper = l;
                        }
                        items.Add(new AlphabetItem() { isEnabled = true, Value = l, DisplayName = upper });
                    }
                    else if (otherChars.Where(i => i.Value == l).Any())
                        items.AddRange(otherChars.Where(i => i.Value == l).ToList());
                    else
                        hasOtherChars = true;
                }

                items = items.OrderBy(i => i.Value).ToList();

                items.Insert(0, new AlphabetItem() { Type = AlphabetItemType.otherChars, isEnabled = hasOtherChars, Value = "-9", DisplayName = "ALL" });
                items.Insert(0, new AlphabetItem() { DisplayAs = AlphabetItem.AlphabetItemDisplayAs.first, isEnabled = true, Type = AlphabetItemType.all, Value = "-1", DisplayName="#" });
                items.LastOrDefault().DisplayAs = AlphabetItem.AlphabetItemDisplayAs.last;

                switch(selected){
                    case -1:
                        items.Where(i => i.Type == DomainModel.Helpers.AlphabetItemType.all).FirstOrDefault().isSelected = true;
                        break;
                    case -9:
                        items.Where(i => i.Type == DomainModel.Helpers.AlphabetItemType.otherChars).FirstOrDefault().isSelected = true;
                        break;
                    default:
                        var query = items.Where(i =>  i.Type != DomainModel.Helpers.AlphabetItemType.otherChars && i.Type != DomainModel.Helpers.AlphabetItemType.all
                                && ! String.IsNullOrEmpty(i.Value)  && (long)i.Value[0] == selected);
                        if (query.Where(i=> i.isEnabled).Any())
                            query.Where(i=> i.isEnabled).FirstOrDefault().isSelected = true;
                        else if (query.Where(i=> !i.isEnabled).Any())
                            items.Where(i => i.Type == DomainModel.Helpers.AlphabetItemType.all).FirstOrDefault().isSelected = true;
                        break;
                }
                return items.Select(i => new FilterListItem() { Disabled  = !i.isEnabled , Checked = i.isSelected, Name = i.DisplayName, Id = (i.Type == AlphabetItemType.all || i.Type == AlphabetItemType.otherChars) ? long.Parse(i.Value) : (long)i.Value[0] }).ToList();
            }
            private static List<AlphabetItem> GetOtherAlphabetItems(Boolean defaultEnable)
            {
                return (from n in Enumerable.Range(222, 34) select new AlphabetItem() { isEnabled = defaultEnable, Value = Char.ConvertFromUtf32(n), DisplayName = Char.ConvertFromUtf32(n).ToString().ToUpper(), isSelected = false }).ToList();
            }

            private dtoCommunitiesFilters GenerateFilters(Int32 idCommunityTypeToLoad,Dictionary<searchFilterType, long> defaultValues, CommunityAvailability availability)
            {
                dtoCommunitiesFilters filter = new dtoCommunitiesFilters();
                filter.IdOrganization = (int)defaultValues[searchFilterType.organization];
                filter.IdcommunityType = (idCommunityTypeToLoad<0) ? idCommunityTypeToLoad : (int)defaultValues[searchFilterType.communitytype];
                filter.Availability = availability;
        //        filter.Status = defaultValues[searchFilterType.status];
        //        filter.Year = defaultValues[searchFilterType.year];
        //        filter.IdCourseTime = defaultValues[searchFilterType.coursetime];
        //        filter.IdDegreeType = defaultValues[searchFilterType.degreetype];
        //        filter.IdResponsible = defaultValues[searchFilterType.responsible];
        //tagassociation = 8,
        //tag = 9,
        //letters = 10,
        //public virtual int  { get; set; }
        //public virtual int  { get; set; }
        //public virtual int  { get; set; }
        //public virtual List<long> IdTags { get; set; }
        //public virtual Boolean WithoutTags { get; set; }


                return filter;
            }
        #endregion

        #region "Manage Communities"
            public List<dtoCommunityForTags> GetCommunitiesForBulkTagsManage(Int32 idPerson, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean useCache = true, Boolean setPath = false)
            {
                List<dtoCommunityForTags> items = null;
                try
                {
                    #region "ApplyFilters"
                    IEnumerable<lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode> nodes = ServiceCommunityManagement.GetAllCommunitiesTree(null).Filter(filters, 0, ServiceCommunityManagement.GetAvailableIdOrganizations(idPerson, SearchCommunityFor.SystemManagement)).GetAllNodes();
                    String path = "";
                    if (nodes.Where(n => n.IdOrganization == filters.IdOrganization && n.IdFather == 0).Any())
                        path = nodes.Where(n => n.IdOrganization == filters.IdOrganization && n.IdFather == 0).FirstOrDefault().Path;
                    if (filters.IdOrganization > -1)
                        nodes = nodes.Where(n => n.IdOrganization == filters.IdOrganization || n.Path.StartsWith(path));
                    if (filters.IdcommunityType > -1)
                        nodes = nodes.Where(n => n.IdCommunityType == filters.IdcommunityType);
                    if (filters.IdResponsible > -1)
                        nodes = nodes.Where(n => n.IdResponsible == filters.IdResponsible);
                    if (filters.IdCourseTime > -1)
                        nodes = nodes.Where(n => n.IdCourseTime == filters.IdCourseTime);
                    if (filters.IdDegreeType > -1)
                        nodes = nodes.Where(n => n.IdCourseTime == filters.IdDegreeType);
                    if (filters.Year > -1)
                        nodes = nodes.Where(n => n.Year == filters.Year);
                    if (filters.Status != Communities.CommunityStatus.None)
                        nodes = nodes.Where(n => n.Status == filters.Status);
                   
                 
                    if (filters.SearchBy != SearchCommunitiesBy.All)
                    {
                        switch (filters.SearchBy)
                        {
                            case SearchCommunitiesBy.Contains:
                                if (!String.IsNullOrEmpty(filters.Value))
                                    nodes = nodes.Where(n => !String.IsNullOrEmpty(n.Name) && n.Name.ToLower().Contains(filters.Value.ToLower()));
                                if (filters.StartWith == "#")
                                    nodes = nodes.Where(n => DefaultOtherChars().Contains(n.FirstLetter));
                                else if (!String.IsNullOrEmpty(filters.StartWith))
                                    nodes = nodes.Where(n => string.Compare(n.FirstLetter, filters.StartWith.ToLower(), true) == 0);
                                break;
                            case SearchCommunitiesBy.NameStartAs:
                                if (!String.IsNullOrEmpty(filters.Value))
                                    nodes = nodes.Where(n => !String.IsNullOrEmpty(n.Name) && n.Name.ToLower().StartsWith(filters.Value.ToLower()));
                                break;
                            case SearchCommunitiesBy.StartAs:
                                if (filters.StartWith != "#")
                                    nodes = nodes.Where(n => string.Compare(n.FirstLetter, filters.StartWith.ToLower(), true) == 0);
                                else
                                    nodes = nodes.Where(n => DefaultOtherChars().Contains(n.FirstLetter));
                                break;
                        }
                    }
                    Dictionary<Int32, List<long>> associations = Service.CacheGetCommunityAssociation(false).GroupBy(t => t.IdCommunity).ToDictionary(t => t.Key, t => t.Select(tg=> tg.IdTag).ToList());
                    if (filters.WithoutTags)
                        nodes = nodes.Where(n => !associations.ContainsKey(n.Id));
                    else
                    {
                        nodes = nodes.Where(n => associations.ContainsKey(n.Id));
                        if (filters.IdTags.Any())
                        {
                            List<Int32> idCommunities = associations.Where(v => filters.IdTags.Where(t => v.Value.Contains(t)).Any()).Select(v => v.Key).Distinct().ToList();
                            nodes = nodes.Where(n => idCommunities.Contains(n.Id));
                        }
                    }
                    #endregion
                    items = nodes.GroupBy(n => n.Id).Where(n => n.Count() == 1).Select(n => new dtoCommunityForTags(n.FirstOrDefault(), associations)).ToList();
                    items.AddRange(nodes.GroupBy(n => n.Id).Where(n => n.Count() >1 && n.Where(c=> c.isPrimary).Any()).Select(n => new dtoCommunityForTags(n.Where(c=>c.isPrimary).FirstOrDefault(), associations)).ToList());
                }
                catch (Exception ex)
                {
                    return null;
                }
                return items.OrderBy(n=> n.Name).ToList();
            }
        #endregion
            #region "Manage Tags"
            public List<Int32> GetAllAvailableOrganizations(Int32 idCommunity)
            {
                return ServiceCommunityManagement.GetAllCommunitiesTree(null).GetAllNodes().Where(n=> n.Id== idCommunity).Select(n=> n.IdOrganization).Distinct().ToList();
            }
        #endregion
    }
}