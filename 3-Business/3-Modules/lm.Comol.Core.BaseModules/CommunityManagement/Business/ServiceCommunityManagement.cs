using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules;
using lm.Comol.Core.Business;
using lm.Comol.Core.Communities;
using NHibernate.Linq;
using lm.Comol.Core.BaseModules.CommunityManagement.Domain;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.CommunityManagement.Business
{
    public class ServiceCommunityManagement : CoreServices, iLinkedNHibernateService 
    {
        protected const int maxItemsForQuery = 100;
        #region "Standard"

            private const string UniqueCode = "SRVADMCMNT";
            //private BaseModuleManager Manager { get; set; }
            //private iUserContext UC { set; get; }
        #region initClass
        public ServiceCommunityManagement() { }
        public ServiceCommunityManagement(iApplicationContext oContext)
        {
            this.Manager = new BaseModuleManager(oContext.DataContext);
            this.UC = oContext.UserContext;
        }
        public ServiceCommunityManagement(iDataContext oDC)
        {
            this.Manager = new BaseModuleManager(oDC);
            this.UC = null;
        }


        #endregion
            public int ServiceModuleID()
            {
                return this.Manager.GetModuleID(UniqueCode);
            }


            public CoreModuleRepository ServicePermission(int personId, int communityId)
            {
                CoreModuleRepository module = new CoreModuleRepository();
                Person person = Manager.GetPerson(personId);
                if (communityId == 0)
                    module = CoreModuleRepository.CreatePortalmodule(person.TypeID);
                else
                {
                    module = new CoreModuleRepository(Manager.GetModulePermission(personId, communityId, ServiceModuleID()));
                }
                return module;
            }
        #endregion

        #region "LinkedNHibernateService"
            public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Int32 idCommunity, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                return new List<StandardActionType>();
            }
            public bool AllowActionExecution(ModuleLink link, Int32 idUser, Int32 idCommunity, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                return false;
            }
            public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                return false;
            }
            public dtoEvaluation EvaluateModuleLink(ModuleLink link, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                return new dtoEvaluation();
            }
            public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                return new List<dtoItemEvaluation<long>>();
            }
            public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, Int32 objectTypeId, Dictionary<Int32, string> translations, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                return new StatTreeNode<StatFileTreeLeaf>();
            }
            public void PhisicalDeleteCommunity(int idCommunity, int idUser, string baseFilePath, string baseThumbnailPath)
            {

            }
            public void PhisicalDeleteCommunity(string baseFileRepositoryPath, int idCommunity, int idUser)
            {
                //throw new NotImplementedException();
            }
            public void PhisicalDeleteRepositoryItem(long idFileItem, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
            }
            public void SaveActionExecution(ModuleLink link, Boolean isStarted, Boolean isPassed, short Completion, Boolean isCompleted, Int16 mark, Int32 idUser, bool alreadyCompleted, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
            }
            public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                throw new NotImplementedException();
            }
        #endregion


        #region "GetCommunities"
            public List<dtoRoleCommunityTypeTemplate> GetRoleCommunityTemplates(Int32 idType){
                List<Int32> idTypes = new List<Int32>();
                idTypes.Add(idType);
                return GetRoleCommunityTemplates(idTypes);
            }

            public List<dtoRoleCommunityTypeTemplate> GetRoleCommunityTemplates(List<Int32> idTypes)
            {
                Boolean isAlreadyOpen = Manager.IsInTransaction();

                List<dtoRoleCommunityTypeTemplate> results = null;
                try {
                    if (!isAlreadyOpen)
                        Manager.BeginTransaction();
                    results = (from t in Manager.GetIQ<_RoleCommunityTypeTemplate>() where idTypes.Contains(t.IdCommunityType) && t.IdRole>-1
                               select new dtoRoleCommunityTypeTemplate()
                                {
                                     IdCommunityType= t.IdCommunityType,
                                     IdRole= t.IdRole,
                                     isDefault =t.isDefault 
                                }).ToList();
                    if (!isAlreadyOpen) 
                        Manager.Commit();
                }
                catch (Exception ex) {
                    if (!isAlreadyOpen) 
                        Manager.RollBack();
                    results = new List<dtoRoleCommunityTypeTemplate>();
                }
                return results;
            }
            public List<dtoUserSubscription> GetUserSubscriptions(Int32 idProfile, List<dtoBaseCommunityNode> communityNodes)
            {
                List<dtoUserSubscription> results = null;
                try {
                    Manager.BeginTransaction();
                    Person person = Manager.GetPerson(idProfile);
                    List<Int32> communitiesId = communityNodes.Select(n => n.Id).Distinct().ToList();
                    var query = (from s in Manager.GetIQ<LazySubscription>()
                                 where communitiesId.Contains(s.IdCommunity) && s.IdPerson == idProfile
                               select new {
                                   Id = s.Id,
                                   IdCommunity = s.IdCommunity,
                                   IdPreviousRole = s.IdRole}
                               ).ToList();

                    var communityQuery = (from com in Manager.GetIQ<Community>() where communitiesId.Contains(com.Id) select new { Name = com.Name, Id = com.Id, IdType = com.IdTypeOfCommunity }).ToList();

                    results = (from i in communitiesId
                                  select new dtoUserSubscription()
                               {
                                   Id = (from t in query where t.IdCommunity==i select t.Id).FirstOrDefault(),
                                   IdCommunity = i,
                                   IdPreviousRole = (from t in query where t.IdCommunity == i select t.IdPreviousRole).FirstOrDefault(),
                                   Path= (from n in communityNodes where n.Id== i select n.Path).ToList() ,
                                   MostLikelyPath = GetMostLikelyPath(person,(from n in communityNodes where n.Id== i select n.Path).ToList()),
                                   IdCommunityType = (from com in communityQuery where com.Id == i select com.IdType).FirstOrDefault(),
                                   CommunityName = (from com in communityQuery where com.Id == i select com.Name).FirstOrDefault()
                               }).ToList();

                    Manager.Commit();
                }
                catch (Exception ex) {
                    Manager.RollBack();
                    results = new List<dtoUserSubscription>();
                }
                return results;
            }
            public List<dtoNewProfileSubscription> GetNewUserSubscriptions(List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> nodes)
            {
                List<dtoNewProfileSubscription> results = null;
                try
                {
                    List<Int32> idCommunities = nodes.Select(n => n.Community.Id).Distinct().ToList();
                    var query = (from c in Manager.GetIQ<liteCommunity>() select c);
                    if (idCommunities.Count > maxItemsForQuery)
                        query = query.ToList().Where(c => idCommunities.Contains(c.Id)).AsQueryable();
                    else
                        query = query.Where(c => idCommunities.Contains(c.Id));

                    results = query.ToList().Select(c => new dtoNewProfileSubscription() { IdRole = 0, Node = nodes.FirstOrDefault(n => n.Community.Id == c.Id), IdPreviousRole = 0, RoleName = "" }).ToList();
                }
                catch (Exception ex)
                {
                    results = new List<dtoNewProfileSubscription>();
                }
                return results;
            }
            public String GetMostLikelyPath(Person person, List<String> path)
            {
                return GetMostLikelyPath(person.Id, path);
            }
            public String GetMostLikelyPath(litePerson person, List<String> path)
            {
                return GetMostLikelyPath(person.Id, path);
            }
        
            public String GetMostLikelyPath(Int32 idPerson, List<String> path)
            {
                String result = "";
                if (path.Count == 1)
                    result = path[0];
                else
                {
                    List<dtoCommunityPath> items = (from p in path
                                                    select new dtoCommunityPath()
                                                    {
                                                        Path = p,
                                                        Idcommunities = (from pId in p.Split('.').ToList() where !String.IsNullOrEmpty(pId) select int.Parse(pId)).ToList()
                                                    }).ToList();

                    List<Int32> idCommunities = items.SelectMany(i => i.Idcommunities).Distinct().ToList();
                    List<LazySubscription> subscriptions = null;
                    if (idCommunities.Count > maxItemsForQuery)
                        subscriptions = (from s in Manager.GetIQ<LazySubscription>() where s.IdPerson == idPerson select s).ToList().Where(s => idCommunities.Contains(s.IdCommunity)).ToList();
                    else
                        subscriptions = (from s in Manager.GetIQ<LazySubscription>() where s.IdPerson == idPerson && idCommunities.Contains(s.IdCommunity) select s).ToList();

                    foreach (dtoCommunityPath item in items)
                    {
                        item.Subscriptions = subscriptions.Where(s =>s.Enabled && s.Accepted && item.Idcommunities.Contains(s.IdCommunity)).Count();
                        item.HiddenSubscriptions = subscriptions.Where(s => s.IdRole < 0 && item.Idcommunities.Contains(s.IdCommunity)).Count();
                    }
                    result = (from i in items where i.AllSubscriptions > 0 select i).OrderByDescending(s => s.Subscriptions).OrderByDescending(s => s.HiddenSubscriptions).Select(s => s.Path).FirstOrDefault();
                }
                return result;
            }
            public String GetMostLikelyPath(litePerson person, List<dtoCommunityPlainPath> items)
            {
                String result = "";
                if (items.Count == 1)
                    result = items[0].Path;
                else if (items != null)
                {
                    List<dtoCommunityPath> paths = (from i in items
                                                    select new dtoCommunityPath()
                                                    {
                                                        Path = i.Path,
                                                        Idcommunities = (from pId in i.Path.Split('.').ToList() where !String.IsNullOrEmpty(pId) select int.Parse(pId)).ToList()
                                                    }).ToList();
                    List<Int32> idCommunities = paths.SelectMany(i => i.Idcommunities).Distinct().ToList();
                    List<LazySubscription> subscriptions = null;
                    if (idCommunities.Count > maxItemsForQuery)
                        subscriptions = (from s in Manager.GetIQ<LazySubscription>() where s.Enabled && s.Accepted && s.IdPerson == person.Id select s).ToList().Where(s => idCommunities.Contains(s.IdCommunity)).ToList();
                    else
                        subscriptions = (from s in Manager.GetIQ<LazySubscription>() where s.Enabled && s.Accepted && s.IdPerson == person.Id && idCommunities.Contains(s.IdCommunity) select s).ToList();

                    foreach (dtoCommunityPath item in paths)
                    {
                        item.Subscriptions = subscriptions.Where(s => item.Idcommunities.Contains(s.IdCommunity)).Count();
                    }
                    result = (from i in paths where i.Subscriptions > 0 select i).OrderByDescending(s => s.Subscriptions).Select(s => s.Path).FirstOrDefault();
                }
                return result;
            }
            public String GetMostLikelyPath(Person  person, List<dtoCommunityPlainPath> items)
            {
                String result = "";
                if (items.Count == 1)
                    result = items[0].Path;
                else if (items != null)
                {
                    List<dtoCommunityPath> paths = (from i in items
                                                    select new dtoCommunityPath()
                                                    {
                                                        Path = i.Path,
                                                        Idcommunities = (from pId in i.Path.Split('.').ToList() where !String.IsNullOrEmpty(pId) select int.Parse(pId)).ToList()
                                                    }).ToList();
                    List<Int32> idCommunities = paths.SelectMany(i => i.Idcommunities).Distinct().ToList();
                    List<LazySubscription> subscriptions = null;
                    if (idCommunities.Count > maxItemsForQuery)
                        subscriptions = (from s in Manager.GetIQ<LazySubscription>() where s.Enabled && s.Accepted && s.IdPerson == person.Id select s).ToList().Where(s => idCommunities.Contains(s.IdCommunity)).ToList();
                    else
                        subscriptions = (from s in Manager.GetIQ<LazySubscription>() where s.Enabled && s.Accepted && s.IdPerson == person.Id && idCommunities.Contains(s.IdCommunity) select s).ToList();

                    foreach (dtoCommunityPath item in paths)
                    {
                        item.Subscriptions = subscriptions.Where(s => item.Idcommunities.Contains(s.IdCommunity)).Count();
                    }
                    result = (from i in paths where i.Subscriptions > 0 select i).OrderByDescending(s => s.Subscriptions).Select(s => s.Path).FirstOrDefault();
                }
                return result;
            }
        
            public String GetMostLikelyPath(Int32 idPerson, List<dtoCommunityPlainPath> items)
            {
                return GetMostLikelyPath(idPerson, items.Select(p => p.Path).ToList());
            }
            public String GetMostLikelyPath(Int32 idPerson, List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoPathItem> items)
            {
                return GetMostLikelyPath(idPerson, items.Select(p => p.Path).ToList());
            }
            public Boolean ProfileHasCommunityToUnsubscribe(Int32 idProfile,dtoCommunitiesFilters filter, List<Int32> subscribedCommunitiesId)
            {
                Person person = Manager.GetPerson(idProfile);
                Int32 IdDefaultOrganization = (from so in Manager.GetIQ<OrganizationProfiles>() 
                                               where so.Profile == person && so.isDefault == true select so.OrganizationID).Skip(0).Take(1).ToList().FirstOrDefault();
                Int32 IdCommunity = (from c in Manager.GetIQ<Community>() where c.IdFather == 0 && c.IdOrganization == IdDefaultOrganization && c.IdTypeOfCommunity == 0 select c.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                subscribedCommunitiesId.Add(IdCommunity);

                Int32 k = subscribedCommunitiesId.Count();

                k = (from s in Manager.GetIQ<LazySubscription>()
                     where s.IdRole > 0 && s.IdPerson == idProfile 
                     select s.IdCommunity).Count();
                List<Int32> idCommunities = (from s in Manager.GetIQ<LazySubscription>()
                                             where s.IdRole > 0 && s.IdPerson == idProfile && !subscribedCommunitiesId.Contains(s.IdCommunity)
                                             select s.IdCommunity).ToList();

                if (idCommunities.Count==0)
                    return false;

                var query = (from c in Manager.GetIQ<Community>() where idCommunities.Contains(c.Id) select c);
                k= query.Count();

                k = query.Where(c => idCommunities.Contains(c.Id) && c.isArchived == false && c.isClosedByAdministrator == false).Count();
                foreach (Community ct in query.Where(c =>  idCommunities.Contains(c.Id) && c.isArchived == false && c.isClosedByAdministrator == false).ToList()) {
                    k++;
                }
                switch (filter.Status) { 
                    case CommunityStatus.Active:
                        query = query.Where(c => c.isArchived == false && c.isClosedByAdministrator == false);
                        break;
                    case CommunityStatus.Blocked:
                        query = query.Where(c => c.isArchived == false && c.isClosedByAdministrator == true);
                        break;
                    case CommunityStatus.Stored:
                        query = query.Where(c => c.isArchived == true && c.isClosedByAdministrator == false);
                        break;
                }
                if (filter.IdcommunityType>-1)
                    query = query.Where(c => c.IdTypeOfCommunity == filter.IdcommunityType);
                else if (filter.IdRemoveCommunityType > -1)
                    query = query.Where(c => c.IdTypeOfCommunity != filter.IdRemoveCommunityType);
                if (filter.IdOrganization > -1)
                    query = query.Where(c => c.IdOrganization == filter.IdOrganization);
                // TO CHECK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return query.Any();

            }

            public List<dtoBaseUserSubscription> ProfileIdCommunitiesToUnsubscribe(Int32 idProfile, dtoCommunitiesFilters filter, List<Int32> subscribedCommunitiesId)
            {
                Person person = Manager.GetPerson(idProfile);
                Int32 IdDefaultOrganization = (from so in Manager.GetIQ<OrganizationProfiles>()
                                               where so.Profile == person && so.isDefault == true
                                               select so.OrganizationID).Skip(0).Take(1).ToList().FirstOrDefault();
                Int32 IdCommunity = (from c in Manager.GetIQ<Community>() where c.IdFather == 0 && c.IdOrganization == IdDefaultOrganization && c.IdTypeOfCommunity == 0 select c.Id).Skip(0).Take(1).ToList().FirstOrDefault();

                subscribedCommunitiesId = subscribedCommunitiesId.Distinct().ToList();
                subscribedCommunitiesId.Add(IdCommunity);

                List<Int32> idCommunities = (from s in Manager.GetIQ<LazySubscription>()
                                             where s.IdRole > 0 && s.IdPerson == idProfile && !subscribedCommunitiesId.Contains(s.IdCommunity)
                                             select s.IdCommunity).ToList();

               
                var communityQuery = (from c in Manager.GetIQ<Community>() where idCommunities.Contains(c.Id) select c);

                switch (filter.Status)
                {
                    case CommunityStatus.Active:
                        communityQuery = communityQuery.Where(c => c.isArchived == false && c.isClosedByAdministrator == false);
                        break;
                    case CommunityStatus.Blocked:
                        communityQuery = communityQuery.Where(c => c.isArchived == false && c.isClosedByAdministrator == true);
                        break;
                    case CommunityStatus.Stored:
                        communityQuery = communityQuery.Where(c => c.isArchived == true && c.isClosedByAdministrator == false);
                        break;
                }
                if (filter.IdcommunityType > -1)
                    communityQuery = communityQuery.Where(c => c.IdTypeOfCommunity == filter.IdcommunityType);
                else if (filter.IdRemoveCommunityType > -1)
                    communityQuery = communityQuery.Where(c => c.IdTypeOfCommunity != filter.IdRemoveCommunityType);
                // TO CHECK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (filter.IdOrganization > -1)
                     communityQuery = communityQuery.Where(c => c.IdOrganization == filter.IdOrganization);
                List<Community> communities = communityQuery.ToList();
                idCommunities = communities.Select(c => c.Id).ToList();

                var query = (from s in Manager.GetIQ<LazySubscription>()
                             where s.IdRole > 0 && s.IdPerson == idProfile && idCommunities.Contains(s.IdCommunity)
                             select s).ToList();
                return query.Select(s => new dtoBaseUserSubscription() { Id = s.Id, CommunityName = communities.Where(c => c.Id == s.IdCommunity).Select(c => c.Name).FirstOrDefault(), IdCommunityType = communities.Where(c => c.Id == s.IdCommunity).Select(c => c.IdTypeOfCommunity).FirstOrDefault(), IdCommunity = s.IdCommunity, IdPreviousRole = s.IdRole, Path = new List<String>() }).ToList();

            }

       
            public List<CommunityStatus> GetCommunitiesAvailableStatus()
            {
                List<CommunityStatus> status = new List<CommunityStatus>();
                if ((from c in Manager.GetIQ<liteCommunityInfo>() where c.isArchived == false && c.isClosedByAdministrator == false select c.Id).Any())
                    status.Add(CommunityStatus.Active);
                if ((from c in Manager.GetIQ<liteCommunityInfo>() where c.isArchived && c.isClosedByAdministrator == false select c.Id).Any())
                    status.Add(CommunityStatus.Stored);
                if ((from c in Manager.GetIQ<liteCommunityInfo>() where c.isClosedByAdministrator select c.Id).Any())
                    status.Add(CommunityStatus.Blocked);
                //if (status.Count > 1)
                //    status.Insert(0, CommunityStatus.None);
                return status;
            }
            public List<CommunityStatus> GetCommunitiesAvailableStatus(Int32 idPerson)
            {
                List<CommunityStatus> status = new List<CommunityStatus>();
                var query = (from s in Manager.GetIQ<Subscription>() where s.Person.Id == idPerson && s.Role != null && s.Role.Id>0 select s.Community);
                List<iCommunity> st = query.ToList();
                if (st.Where(c => c.isArchived == false && c.isClosedByAdministrator == false).Any())
                    status.Add(CommunityStatus.Active);
                if (st.Where(c => c.isArchived && c.isClosedByAdministrator == false).Any())
                    status.Add(CommunityStatus.Stored);
                if (st.Where(c => c.isClosedByAdministrator).Select(c => c.Id).Any())
                    status.Add(CommunityStatus.Blocked);
                //if (status.Count > 1)
                //    status.Insert(0, CommunityStatus.None);
                return status;
            }
            public List<Int32> GetCommunitiesAvailableTypes(CommunityStatus status)
            {
                List<Int32> types = new List<Int32>();
                switch (status) { 
                    case CommunityStatus.Active:
                        types = (from c in Manager.GetIQ<Community>() where c.isArchived == false && c.isClosedByAdministrator == false select c.IdTypeOfCommunity).Distinct().ToList();
                        break;
                    case CommunityStatus.Blocked:
                        types = (from c in Manager.GetIQ<Community>() where c.isArchived && c.isClosedByAdministrator == false select c.IdTypeOfCommunity).Distinct().ToList();
                        break;
                    case CommunityStatus.Stored:
                        types = (from c in Manager.GetIQ<Community>() where c.isClosedByAdministrator select c.IdTypeOfCommunity).Distinct().ToList();
                        break;
                    default:
                        types = (from c in Manager.GetIQ<Community>() select c.IdTypeOfCommunity).Distinct().ToList();
                        break;
                }

                return types;
            }

            public dtoTreeCommunityNode GetGenericCommunitiesTree(dtoCommunitiesFilters filter, litePerson person)
            {
                dtoTreeCommunityNode root = GetAllCommunitiesTree(person);
                return root.Filter(filter,0);
            }
            public dtoTreeCommunityNode GetFilteredCommunitiesTree(dtoCommunitiesFilters filter, litePerson person)
            {
                dtoTreeCommunityNode root = GetAllCommunitiesTree(person);
                Int32 idDefaultOrganization = 0;
                if (person!=null)
                    idDefaultOrganization = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == person.Id  && o.isDefault select o.IdOrganization).Skip(0).Take(1).ToList().FirstOrDefault();
                else
                    idDefaultOrganization = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == UC.CurrentUserID && o.isDefault select o.IdOrganization).Skip(0).Take(1).ToList().FirstOrDefault();
                return root.Filter(filter, idDefaultOrganization);
            }

            public dtoTreeCommunityNode GetAllCommunitiesTree(litePerson person, Boolean useCache = true)
            {
                dtoTreeCommunityNode root = null;
                root = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<dtoTreeCommunityNode>(CacheKeys.UserCommunitiesTree((person==null)? -1 : person.Id)) : null;

                if (root == null )
                {
                    root = new dtoTreeCommunityNode() { Id = 0, Type = dtoCommunityNodeType.Root, Selected = false };

                    try
                    {
                        List<Int32> subscribed = new List<Int32>();
                        List<Int32> available = new List<Int32>();
                        if (person != null){
                            var query = (from s in Manager.GetIQ<LazySubscription>()
                                          where s.IdPerson == person.Id && s.IdRole > 0
                                          select s);
                            subscribed = query.Select(s=>s.IdCommunity).ToList();
                            available = query.Where(s=> s.Accepted).Select(s=>s.IdCommunity).ToList();
                        }
                        List<_OldCommunityRelation> relations = (from c in Manager.GetIQ<_OldCommunityRelation>()
                                                                 select c).ToList();

                        var allNodes = (from s in Manager.GetIQ<liteCommunityInfo>()
                                        select new dtoCommunityNode
                                        {
                                            Id = s.Id,
                                            Name = s.Name,
                                            IdFather = s.IdFather,
                                            ConfirmSubscription = s.ConfirmSubscription,
                                            isPrimary = true,
                                            Status = (s.isClosedByAdministrator) ? CommunityStatus.Blocked : (s.isArchived) ? CommunityStatus.Stored : CommunityStatus.Active,
                                            Type = (s.isClosedByAdministrator) ? dtoCommunityNodeType.Blocked : (s.isArchived) ? dtoCommunityNodeType.Stored : dtoCommunityNodeType.Active,
                                            IdOrganization = s.IdOrganization,
                                            IdCommunityType = s.IdTypeOfCommunity,
                                            Selected = (subscribed.Contains(s.Id)),
                                            AccessAvailable = (available.Contains(s.Id))
                                        }
                                              ).ToList();

                        var communitiesResponsible = (from s in Manager.GetIQ<LazySubscription>() where s.isResponsabile == true select new { IdCommunity = s.IdCommunity, idPerson = s.IdPerson }).ToList();
                        allNodes.ForEach(n => n.IdResponsible = (from cr in communitiesResponsible where cr.IdCommunity == n.Id select cr.idPerson).FirstOrDefault());

                        foreach (dtoCommunityNode n in allNodes.Where(n => n.IdFather == 0).ToList())
                        {
                            dtoTreeCommunityNode node = new dtoTreeCommunityNode(n);
                            node.Father = null;
                            node.Path = "." + node.Id.ToString() + ".";
                            node.Nodes = GetCommunitiesTree(node,node, allNodes, relations, subscribed);
                            root.Nodes.Add(node);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    if (useCache)
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.AddToCache<dtoTreeCommunityNode>(CacheKeys.UserCommunitiesTree((person == null) ? -1 : person.Id), root, CacheExpiration.Minutes(5));
                }
                return dtoTreeCommunityNode.FullClone(root, null);
            }
            private List<dtoTreeCommunityNode> GetCommunitiesTree(dtoTreeCommunityNode firstFather, dtoTreeCommunityNode fatherNode, List<dtoCommunityNode> allNodes, List<_OldCommunityRelation> relations, List<Int32> subscribed)
            {
                List<dtoTreeCommunityNode> nodes = new List<dtoTreeCommunityNode>();
                try
                {
                    List<Int32> childrens = (from c in relations
                                             where c.IdSource == fatherNode.Id
                                             select c.IdDestination).ToList();
                    var queryCommunity = (from s in allNodes
                                          where childrens.Contains(s.Id)
                                          select s
                                        );

                    foreach (dtoCommunityNode n in queryCommunity.ToList())
                    {
                        dtoTreeCommunityNode node = new dtoTreeCommunityNode(n);
                        node.IdFather = fatherNode.Id;
                        node.isPrimary = (n.IdFather == fatherNode.Id);
                        node.FathersName.AddRange(fatherNode.FathersName);
                        node.FathersName.Add(fatherNode.Name);
                        node.Father = fatherNode;
                        node.Path = fatherNode.Path + node.Id.ToString() + ".";
                        node.FirstFather = firstFather;
                        node.IdFirstFatherOrganization = firstFather.IdOrganization;
                        node.Nodes = GetCommunitiesTree(firstFather,node, allNodes, relations, subscribed);
                        nodes.Add(node);
                    }
                }
                catch (Exception ex)
                {

                }

                return nodes;
            }


            public dtoUnsubscribeTreeNode GetCommunitySubTree(Person person, Int32 idCommunity, String path)
            {
                dtoUnsubscribeTreeNode root = null;

                try
                {
                    List<liteSubscriptionInfo> subscriptions = null;
                    Int32 idDefaultOrganization = 0;

                    if (person != null)
                    {
                        subscriptions = (from s in Manager.GetIQ<liteSubscriptionInfo>()
                                         where s.IdPerson == person.Id && s.IdRole > 0
                                         select s).ToList();

                        idDefaultOrganization = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == person.Id && o.isDefault select o.IdOrganization).Skip(0).Take(1).ToList().FirstOrDefault();

                        List<_OldCommunityRelation> relations = (from c in Manager.GetIQ<_OldCommunityRelation>()
                                                                 select c).ToList();

                        liteCommunityInfo community = Manager.Get<liteCommunityInfo>(idCommunity);
                        if (community != null)
                        {
                            root = new dtoUnsubscribeTreeNode(community, (subscriptions == null) ? null : subscriptions.Where(s => s.IdCommunity == idCommunity).FirstOrDefault(), path);
                            if (community.IdOrganization == idDefaultOrganization && community.IdFather == 0)
                                root.AllowUnsubscriptionFromOrganization = false;

                            var allNodes = (from c in Manager.GetIQ<liteCommunityInfo>()
                                            select c).ToList();
                            foreach (liteCommunityInfo n in allNodes.Where(n => n.IdFather == idCommunity).ToList())
                            {
                                dtoUnsubscribeTreeNode node = new dtoUnsubscribeTreeNode(n, (subscriptions == null) ? null : subscriptions.Where(s => s.IdCommunity == n.Id).FirstOrDefault(),path + n.Id.ToString() + ".");
                                node.Father = root;
                                node.Nodes = GetCommunitySubTree( node, allNodes, relations, subscriptions);
                                root.Nodes.Add(node);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return root;
            }
            private List<dtoUnsubscribeTreeNode> GetCommunitySubTree(dtoUnsubscribeTreeNode fatherNode, List<liteCommunityInfo> allNodes, List<_OldCommunityRelation> relations, List<liteSubscriptionInfo> subscriptions)
            {
                List<dtoUnsubscribeTreeNode> nodes = new List<dtoUnsubscribeTreeNode>();
                try
                {
                    List<Int32> childrens = (from c in relations
                                             where c.IdSource == fatherNode.Id
                                             select c.IdDestination).ToList();
                    var queryCommunity = (from s in allNodes
                                          where childrens.Contains(s.Id)
                                          select s
                                        );

                    foreach (liteCommunityInfo c in queryCommunity.ToList())
                    {
                        dtoUnsubscribeTreeNode node = new dtoUnsubscribeTreeNode(c, (subscriptions == null) ? null : subscriptions.Where(s => s.IdCommunity == c.Id).FirstOrDefault(), fatherNode.Path + c.Id.ToString() + ".");
                        node.IdFather = fatherNode.Id;
                        node.isPrimary = (c.IdFather == fatherNode.Id);
                        node.Father = fatherNode;
                        node.Nodes = GetCommunitySubTree(node, allNodes, relations, subscriptions);
                        nodes.Add(node);
                    }
                }
                catch (Exception ex)
                {

                }

                return nodes;
            }

            public List<dtoTreeCommunityNode> GetAvailableNodes(List<dtoTreeCommunityNode> nodes, List<Int32> unloadIdCommunities, List<Int32> onlyFromOrganizations, Dictionary<Int32, long> requiredPermissions)
            {
                if (requiredPermissions != null )
                {
                    List<Int32> idCommunities = GetIdCommunityByModulePermissions(UC.CurrentUserID, requiredPermissions, unloadIdCommunities);
                    nodes.Where(n => !idCommunities.Contains(n.Id)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                }
                if (unloadIdCommunities != null)
                    unloadIdCommunities.ForEach(i => nodes.Where(n => n.Id == i).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable));
                if (onlyFromOrganizations != null && onlyFromOrganizations.Any())
                    nodes.Where(n => !onlyFromOrganizations.Contains(n.IdOrganization)).ToList().ForEach(n => n.Type = dtoCommunityNodeType.NotSelectable);
                return nodes;
            }
            public List<Int32> GetOrganizationIdChildrenCommunities(Int32 idOrganization, String searchBy = "")
            {
                return GetOrganizationsIdChildrenCommunities(new List<Int32>() { idOrganization }, searchBy);
            }
            public List<Int32> GetOrganizationsIdChildrenCommunities(List<Int32> idOrganizations, String searchBy = "")
            {
                return GetGenericIdChildrenCommunities(idOrganizations, -1, searchBy);
            }
            public List<Int32> GetCommunityIdChildren(Int32 idCommunity, String searchBy = "")
            {
                return GetGenericIdChildrenCommunities(new List<Int32>(), idCommunity, searchBy);
            }
            private List<Int32> GetGenericIdChildrenCommunities(List<Int32> idOrganizations, Int32 idCommunity, String searchBy ="")
            {
                List<Int32> result = new List<Int32>();
                dtoCommunitiesFilters filter = new dtoCommunitiesFilters();
                filter.Availability = CommunityAvailability.All;
                filter.IdcommunityType = -1;
                filter.SearchBy = (String.IsNullOrEmpty(searchBy) ? SearchCommunitiesBy.All : SearchCommunitiesBy.Contains);
                filter.Value = searchBy;
                filter.Status = CommunityStatus.None;
                if (idOrganizations == null)
                    filter.IdOrganization=-1;
                else if (idOrganizations.Count==1)
                     filter.IdOrganization=idOrganizations.FirstOrDefault();
                else{
                    filter.OnlyFromAvailableOrganizations= true;
                    filter.AvailableIdOrganizations=idOrganizations;
                }
                dtoTreeCommunityNode fullNode = GetFilteredCommunitiesTree(filter,null);
                if (fullNode != null) {
                    if (idCommunity < 0)
                    {
                        List<dtoTreeCommunityNode> nodes = fullNode.GetAllNodes().Where(n => (filter.IdOrganization== n.IdOrganization || (filter.OnlyFromAvailableOrganizations && filter.AvailableIdOrganizations.Contains(n.IdOrganization)) || (!filter.OnlyFromAvailableOrganizations && filter.IdOrganization==-1) ) && n.IdFather == 0).ToList();
                        nodes.ForEach(n => result.AddRange(n.GetAllNodes().Select(sn => sn.Id).ToList()));
                    }
                    else
                    {
                        List<dtoTreeCommunityNode> nodes = fullNode.GetAllNodes().Where(n => n.Id == idCommunity).ToList();
                        nodes.ForEach(n => result.AddRange(n.GetAllNodes().Select(sn => sn.Id).ToList()));
                    };
                }
                return result.Distinct().ToList();
            }
        #endregion 

        #region "Add communities filters"
            public Dictionary<Int32, String> GetDisplayOrganizations(litePerson person)
            {
                Dictionary<Int32, String> organizations = null;
                if (person != null)
                {
                    List<Int32> idOrganizations = null;
                    if (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator)
                    {
                        organizations = (from o in Manager.GetIQ<Organization>() orderby o.Name select o).ToDictionary(o => o.Id, o => o.Name);
                    }
                    else
                    {
                        idOrganizations = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == person.Id  select o.IdOrganization).ToList();
   
                        organizations = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) orderby o.Name select new { Id = o.Id, Name = o.Name }).ToList().ToDictionary(o => o.Id, o => o.Name);
                    }
                }
                else
                    organizations = new Dictionary<Int32, String>();
                return organizations;
            }
            public Dictionary<Int32, String> GetDisplayOrganizations(litePerson person, List<Int32> onlyFromIdCommunities)
            {
                Dictionary<Int32, String> organizations = null;
                if (person != null)
                {
                    List<Int32> idOrganizations = null;
                    if (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator)
                    {
                        organizations = (from o in Manager.GetIQ<Organization>() where onlyFromIdCommunities.Contains(o.Id) orderby o.Name select o).ToDictionary(o => o.Id, o => o.Name);
                    }
                    else
                    {
                        idOrganizations = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == person.Id select o.IdOrganization).ToList();

                        var p = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select o).OrderBy(o => o.Name).ToList();
                        organizations = p.ToDictionary(o => o.Id, o => o.Name);
                    }
                }
                else
                    organizations = new Dictionary<Int32, String>();
                return organizations;
            }
            public Dictionary<Int32, String> GetDisplayOrganizations(litePerson person, Int32 idProfile)
            {
                Dictionary<Int32, String> organizations = null;
                if (person != null)
                {
                    List<Int32> idOrganizations = null;
                    if (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator)
                        organizations = (from o in Manager.GetIQ<Organization>() select o).OrderBy(o => o.Name).ToList().ToDictionary(o => o.Id, o => o.Name);
                    else
                    {
                        idOrganizations = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == person.Id  || o.IdPerson == idProfile select o.IdOrganization).ToList();
                        organizations = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select o).OrderBy(o => o.Name).ToList().ToDictionary(o => o.Id, o => o.Name);
                    }
                }
                else
                    organizations = new Dictionary<Int32, String>();
                return organizations;
            }
            public List<Organization> GetAvailableOrganizations(litePerson person)
            {
                List<Organization> organizations = null;
                if (person != null)
                {
                    List<Int32> idOrganizations = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == person.Id select o.IdOrganization).ToList();
                    organizations = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select o).ToList();
                }
                else
                    organizations = new List<Organization>();
                return organizations;
            }
            public List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> GetOrganizationNodes(List<Int32> idOrganizations)
            {
                List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> organizations = (from c in Manager.GetIQ<liteCommunity>()
                                                                                                        where c.IdFather == 0 && idOrganizations.Contains(c.IdOrganization)
                                                                                                        select new lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem(c.Id, c.Name, c.IdType, c.IdOrganization, 0)).ToList();

                return organizations;
            }
            public List<Community> GetOrganizationsCommunity(List<Int32> idOrganizations)
            {
                List<Community> organizations = (from c in Manager.GetIQ<Community>()
                                                            where c.IdFather == 0 && idOrganizations.Contains(c.IdOrganization)
                                                            select c).ToList();

                return organizations;
            }
            /// <summary>
            /// Get id of organization community of specified organizations
            /// </summary>
            /// <param name="idOrganizations"></param>
            /// <returns></returns>
            public List<Int32> GetOrganizationsCommunityId(List<Int32> idOrganizations)
            {
                List<Int32> organizations = (from c in Manager.GetIQ<liteCommunity>()
                                                 where c.IdFather == 0 && idOrganizations.Contains(c.IdOrganization)
                                                 select c.Id).ToList();

                return organizations;
            }
            public List<litePerson> GetResponsibles(List<dtoTreeCommunityNode> nodes) {
                List<litePerson> result = null;
                try {
                    List<Int32> items = (from n in nodes where n.IdResponsible>0  select n.IdResponsible).Distinct().ToList();
                    //items.AddRange((from n in nodes where n.IdResponsible<0 select n.IdResponsible
                    if (items.Count > maxItemsForQuery)
                        result = (from p in Manager.GetIQ<litePerson>() select p).ToList().Where(p => items.Contains(p.Id)).ToList();
                    else
                        result = (from p in Manager.GetIQ<litePerson>() where items.Contains(p.Id) select p).ToList();
                }
                catch (Exception ex) {
                    result = new List<litePerson>();
                }
                return result;
            }
        #endregion

        #region "Manage Subscriptions"
            #region "ToReview
                public LazySubscription AddProfileToOrganization(Int32 idOrganization, Person person, Boolean isDefault)
                {
                    LazySubscription result = null;
                    try
                    {
                        List<dtoRoleCommunityTypeTemplate> roles = null;
                        liteCommunity community = (from c in Manager.GetIQ<liteCommunity>() where c.IdType == 0 && c.IdOrganization == idOrganization select c).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (community != null)
                            roles = GetRoleCommunityTemplates(community.IdType);


                        if (person != null && community != null && roles.Where(r => r.isDefault).Any())
                        {
                            liteOrganizationProfile orgProfile = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == person.Id  && o.IdOrganization == idOrganization select o).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (orgProfile == null)
                                orgProfile = new liteOrganizationProfile() { IdOrganization = idOrganization, IdPerson = person.Id, isDefault = isDefault };
                            else
                                orgProfile.isDefault = isDefault;

                            LazySubscription subscription = CreateEnrollment(person.Id, community, DateTime.Now,roles.Where(r => r.isDefault).Select(r => r.IdRole).FirstOrDefault(), true, GetUserEnrollment(person.Id, community.Id));
                            Manager.BeginTransaction();
                            Manager.SaveOrUpdate(orgProfile);
                            Manager.SaveOrUpdate(subscription);
                            Manager.Commit();
                            result = subscription;
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = null;
                    }
                    return result;
                }
                public List<LazySubscription> AddProfilesToOrganization(Int32 idOrganization, List<Int32> idPersons, dtoNewProfileSubscription enrollingItem, Boolean isDefault)
                {
                    List<LazySubscription> items = new List<LazySubscription>();
                    try
                    {
                        List<dtoRoleCommunityTypeTemplate> roles = null;
                        liteCommunity community = (from c in Manager.GetIQ<liteCommunity>() where c.IdType == 0 && c.IdOrganization == idOrganization select c).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (enrollingItem != null && idPersons != null && community!=null )
                        {
                            foreach (Int32 idPerson in idPersons)
                            {
                                liteOrganizationProfile orgProfile = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == idPerson && o.IdOrganization == idOrganization select o).Skip(0).Take(1).ToList().FirstOrDefault();
                                if (orgProfile == null)
                                    orgProfile = new liteOrganizationProfile() { IdOrganization = idOrganization, IdPerson = idPerson, isDefault = isDefault };
                                else
                                    orgProfile.isDefault = isDefault;

                                LazySubscription subscription = CreateEnrollment(idPerson, community, DateTime.Now, enrollingItem.IdRole, true, GetUserEnrollment(idPerson, community.Id));
                                Manager.BeginTransaction();
                                Manager.SaveOrUpdate(orgProfile);
                                Manager.SaveOrUpdate(subscription);
                                Manager.Commit();
                                items.Add(subscription);
                            }
                        }
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                        items = new List<LazySubscription>();
                    }
                    return items;
                }
                public List<LazySubscription> AddProfilesToCommunity(List<Int32> idPersons, dtoNewProfileSubscription enrollingItem)
                {
                    List<LazySubscription> items = new List<LazySubscription>();
                    try
                    {
                        if (enrollingItem != null && idPersons != null)
                        {
                            liteCommunity community = Manager.GetLiteCommunity(enrollingItem.Id);
                            if (community != null)
                            {
                                foreach (Int32 idPerson in idPersons)
                                {
                                    LazySubscription subscription = CreateEnrollment(idPerson, community, DateTime.Now, enrollingItem.IdRole, true, GetUserEnrollment(idPerson, community.Id));
                                    Manager.BeginTransaction();
                                    Manager.SaveOrUpdate(subscription);
                                    Manager.Commit();
                                    items.Add(subscription);
                                    if (community.IdType == 0 && !(from op in Manager.GetIQ<liteOrganizationProfile>() where op.IdPerson == idPerson && op.IdOrganization == community.IdOrganization select op.Id).Any())
                                    {
                                        liteOrganizationProfile op = new liteOrganizationProfile();
                                        op.IdOrganization = community.IdOrganization;
                                        op.IdPerson = idPerson;
                                        Manager.SaveOrUpdate(op);
                                    }
                                    if (community.IdType != 0)
                                        EnrollToCommunityFather(idPerson, community, GetMostLikelyPath(idPerson, enrollingItem.Node.Paths), subscription.SubscriptedOn);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        items = new List<LazySubscription>();
                    }
                    return items;
                }
            public Boolean UpdateUserSubscriptions(Person person, List<dtoUserSubscription> subscriptions, List<dtoBaseUserSubscription> unsubscriptions)
            {
                Boolean result = false;

                try
                {
                    Manager.BeginTransaction();
                    UnsubscribeUserFromCommunities(person, unsubscriptions);
                    EditUserSubscriptions(person, subscriptions.Where(s => s.isNew == false && s.isToUpdate).ToList());
                    AddUserSubscriptions(person, subscriptions.Where(s => s.isNew).ToList());

                    Manager.Commit();
                    CacheHelper.PurgeCacheItems(CacheKeys.UserCommunitiesTree(person.Id));
                    result = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }

                return result;
            }
            private void AddUserSubscriptions(Person person, List<dtoUserSubscription> subscriptions)
            {
                List<Int32> idCommunities = subscriptions.Select(s => s.IdCommunity).ToList();
                List<liteCommunity> communities = (from c in Manager.GetIQ<liteCommunity>() where idCommunities.Contains(c.Id)  select c).ToList();

                foreach (liteCommunity community in communities)
                {
                    LazySubscription userSubscription = CreateEnrollment(person.Id, community,DateTime.Now, subscriptions.Where(s => s.IdCommunity == community.Id).Select(s => s.IdRole).FirstOrDefault(), true, GetUserEnrollment(person.Id, community.Id));
                    Manager.SaveOrUpdate(userSubscription);
                    if (community.IdType == 0 && !(from op in Manager.GetIQ<OrganizationProfiles>() where op.Profile == person && op.OrganizationID == community.IdOrganization select op.Id).Any())
                    {
                        OrganizationProfiles op = new OrganizationProfiles();
                        op.OrganizationID = community.IdOrganization;
                        op.Profile = person;
                        Manager.SaveOrUpdate(op);
                    }
                    if (community.IdType != 0)
                        EnrollToCommunityFather(person.Id, community, subscriptions.Where(s => s.IdCommunity == community.Id).Select(s => s.MostLikelyPath).FirstOrDefault(),userSubscription.SubscriptedOn);
                }
            }
                private void EnrollToCommunityFather(Int32 idPerson, liteCommunity child, String MostLikelyPath, DateTime? enrolledOn = null)
                {
                    if (child != null)
                    {
                        if (string.IsNullOrEmpty(MostLikelyPath))
                            EnrollToCommunityFather(idPerson, child, enrolledOn);
                        else
                        {
                            List<Int32> idcommunities = (from pId in MostLikelyPath.Split('.').ToList() where !String.IsNullOrEmpty(pId) select int.Parse(pId)).ToList();
                            List<Int32> subscribed = (from s in Manager.GetIQ<LazySubscription>()
                                                      where idcommunities.Contains(s.IdCommunity) && s.IdPerson == idPerson
                                                      select s.IdCommunity).ToList();
                            idcommunities = idcommunities.Except(subscribed).ToList();
                            foreach (liteCommunity community in (from c in Manager.GetIQ<liteCommunity>() where idcommunities.Contains(c.Id) select c).ToList())
                            {
                                LazySubscription userSubscription = CreateEnrollment(idPerson, community, enrolledOn);
                                Manager.SaveOrUpdate(userSubscription);
                                if (community.IdType == 0 && !(from op in Manager.GetIQ<liteOrganizationProfile>() where op.IdPerson == idPerson && op.IdOrganization == community.IdOrganization select op.Id).Any())
                                {
                                    liteOrganizationProfile op = new liteOrganizationProfile();
                                    op.IdOrganization = community.IdOrganization;
                                    op.IdPerson = idPerson;
                                    Manager.SaveOrUpdate(op);
                                }
                            }
                        }
                    }
                }
                private void EnrollToCommunityFather(Int32 idPerson, liteCommunity child,DateTime? enrolledOn = null)
                {
                    if (child != null)
                    {
                        liteCommunity community = Manager.GetLiteCommunity(child.IdFather);
                        List<Int32> idFathers = (from or in Manager.GetIQ<_OldCommunityRelation>()
                                                 where or.IdDestination == child.Id
                                                 select or.IdSource).ToList();
                        if (community != null && !(from s in Manager.GetIQ<LazySubscription>() where idFathers.Contains(s.IdCommunity) && s.IdPerson == idPerson select s.Id).Any())
                        {
                            LazySubscription userSubscription = CreateEnrollment(idPerson, community, enrolledOn);
                            Manager.SaveOrUpdate(userSubscription);
                            if (community.IdType == 0 && !(from op in Manager.GetIQ<liteOrganizationProfile>() where op.IdPerson == idPerson && op.IdOrganization == community.IdOrganization select op.Id).Any())
                            {
                                liteOrganizationProfile op = new liteOrganizationProfile();
                                op.IdOrganization = community.IdOrganization;
                                op.IdPerson = idPerson;
                                Manager.SaveOrUpdate(op);
                            }
                            if (community.IdType != 0)
                                EnrollToCommunityFather(idPerson, community, enrolledOn);
                        }
                    }
                }
            private void EditUserSubscriptions(Person person, List<dtoUserSubscription> subscriptions)
            {
                List<Int32> idCommunities = subscriptions.Select(s => s.IdCommunity).ToList();
                List<LazySubscription> toEdit = (from s in Manager.GetIQ<LazySubscription>()
                                                 where s.IdPerson == person.Id && idCommunities.Contains(s.IdCommunity)
                                                 select s).ToList();
                foreach (LazySubscription subscription in toEdit)
                {
                    subscription.IdRole = subscriptions.Where(s => s.IdCommunity == subscription.IdCommunity).Select(s => s.IdRole).FirstOrDefault();
                    subscription.Accepted = true;
                    subscription.Enabled = true;
                }
                Manager.SaveOrUpdateList(toEdit);
            }
            private void UnsubscribeUserFromCommunities(Person person, List<dtoBaseUserSubscription> unsubscriptions)
            {
                List<Int32> idCommunities = unsubscriptions.Select(s => s.IdCommunity).ToList();
                List<LazySubscription> toRemove = (from s in Manager.GetIQ<LazySubscription>()
                                                   where s.IdPerson == person.Id && idCommunities.Contains(s.IdCommunity)
                                                   select s).ToList();
                List<Int32> created = (from c in Manager.GetIQ<Community>() where idCommunities.Contains(c.Id) && c.Creator == person select c.Id).ToList();
                foreach (LazySubscription unsubscription in toRemove)
                {
                    if (created.Contains(unsubscription.IdCommunity))
                        // creator
                        unsubscription.IdRole = -2;
                    else
                        // passante
                        unsubscription.IdRole = -3;
                }
                Manager.SaveOrUpdateList(toRemove);
            }
                private LazySubscription CreateEnrollment(Int32 idPerson, liteCommunity community, DateTime? enrolledOn = null,Int32 idRole=-1,Boolean setActive = false, LazySubscription source =null)
                {
                    LazySubscription enrollment = source;
                    if (enrollment == null)
                        enrollment = new LazySubscription();
                    if (idRole == -1 || idRole == 0 || idRole < (int)RoleTypeStandard.Guest)
                        idRole = (community.IdCreatedBy == idPerson) ? (int)RoleTypeStandard.HiddenCreator : (int)RoleTypeStandard.HiddenEnrollment;
                    if (!enrolledOn.HasValue)
                        enrolledOn = DateTime.Now;
                    enrollment.LastAccessOn = enrolledOn;
                    enrollment.SubscriptedOn= enrolledOn;
                    enrollment.IdPerson = idPerson;
                    enrollment.IdCommunity = community.Id;
                    enrollment.IdRole = idRole;
                    enrollment.Accepted = setActive || !community.ConfirmSubscription && (community.IdType != (int)CommunityTypeStandard.Organization || enrollment.IdRole == (int)RoleTypeStandard.HiddenCreator);
                    enrollment.Enabled = setActive || !community.ConfirmSubscription && (community.IdType != (int)CommunityTypeStandard.Organization || enrollment.IdRole == (int)RoleTypeStandard.HiddenCreator);

                    return enrollment;
                }
                private LazySubscription GetUserEnrollment(Int32 idPerson, Int32 idCommunity)
                {
                    return (from s in Manager.GetIQ<LazySubscription>() where s.IdCommunity == idCommunity && s.IdPerson == idPerson select s).Skip(0).Take(1).ToList().FirstOrDefault();
                }
            #endregion

            public dtoEnrollment EnrollTo(Int32 idPerson, dtoCommunityInfoForEnroll item) //, List<dtoCommunityConstraint> constraints
            {
                return EnrollTo(idPerson,new List<dtoCommunityInfoForEnroll>() { item}).FirstOrDefault();
            }
            public List<dtoEnrollment> EnrollTo(Int32 idPerson, List<dtoCommunityInfoForEnroll> items)
            {
                List<dtoEnrollment> results = new List<dtoEnrollment>();
                List<Int32> idCommunities = items.Select(s => s.Id).ToList();
                var query = (from c in Manager.GetIQ<liteCommunityInfo>() select c);
                if (idCommunities.Count() <= maxItemsForQuery)
                    query = query.Where(c => idCommunities.Contains(c.Id));
                else
                    query = query.ToList().Where(c => idCommunities.Contains(c.Id)).AsQueryable();

                List<liteCommunityInfo> communities = query.ToList();
                litePerson person = Manager.GetLitePerson(idPerson);
                if (person != null && person.Id > 0 && communities != null && communities.Any())
                {
                    List<Int32> idUserOrganizations = Manager.GetUserOrganizations(idPerson);
                    List<dtoUserEnrollments> responsibles = GetResponsibles(communities.Select(r=> r.Id).ToList());
                    Dictionary<Int32,List<dtoTranslatedRoleType>> roles = responsibles.Where(r=> r.IsValid()).Select(r=> r.Person.LanguageID).Distinct().ToDictionary(i=> i, i=> Manager.GetTranslatedItem<dtoTranslatedRoleType>(i));
                    IList<Language> languages = Manager.GetAllLanguages();

                    foreach (liteCommunityInfo community in communities)
                    {
                        DateTime enrollOn = DateTime.Now;
                        dtoCommunityInfoForEnroll dto = items.Where(i => i.Id == community.Id).FirstOrDefault();
                        List<dtoCommunityConstraint> constraints = GetDtoCommunityConstraints(idPerson, community.Id , GetConstraints(new List<Int32>() { community.Id}));
                        dtoEnrollment item = new dtoEnrollment(enrollOn, community, constraints.Where(c=> c.IdSource==community.Id || c.IdDestinationCommunity== community.Id).ToList());
                        if (community.MaxUsersWithDefaultRole > 0)
                        {
                            long enrolledIn = GetEnrolledUsers(item.IdCommunity, dto.IdDefaultRole);
                            if (enrolledIn == -1 && dto.AvailableSeats == 0)
                                item.NotAvailableFor.Add(EnrollingStatus.Seats);
                            else {
                                dto.EnrolledUsers = enrolledIn;
                                if (dto.AvailableSeats<=0)
                                    item.NotAvailableFor.Add(EnrollingStatus.Seats);
                            }
                        }
                        if (item.IsEnrollAvailable)
                        {
                            LazySubscription subscription = AddSubscription(idPerson, idUserOrganizations, community, dto.IdDefaultRole, dto.Path, enrollOn);
                            if (subscription == null)
                                item.Status = EnrolledStatus.UnableToEnroll;
                            else if (subscription.SubscriptedOn.Equals(enrollOn))
                                item.Status = (subscription.Accepted) ? EnrolledStatus.Available : EnrolledStatus.NeedConfirm;
                            else
                            {
                                item.Status = EnrolledStatus.PreviousEnrolled;
                                if (subscription.SubscriptedOn.HasValue)
                                    item.EnrolledOn = subscription.SubscriptedOn.Value;
                            }
                             if (subscription != null){
                                 item.ExtendedInfo.IdRole = subscription.IdRole;
                                 item.ExtendedInfo.Responsible = responsibles.Where(r => r.IsValid() && r.IsEnrolled(item.IdCommunity)).Select(r=> r.Person).FirstOrDefault();
                                 item.ExtendedInfo.Language = languages.Where(l => item.ExtendedInfo.IsValid() && l.Id == item.ExtendedInfo.Responsible.LanguageID).FirstOrDefault();
                                 item.ExtendedInfo.RoleName= (roles.ContainsKey(item.ExtendedInfo.GetIdLanguage())) ? roles[item.ExtendedInfo.GetIdLanguage()].Where(r=> r.Id== subscription.IdRole).Select(r=> r.Name).FirstOrDefault() :"";
                             }
                        }
                        else
                            item.Status = EnrolledStatus.UnableToEnroll;
                        results.Add(item);
                    }
                }
                return results;
            }

            public List<dtoCommunityConstraint> GetDtoCommunityConstraints(Int32 idPerson, Int32 idCommunity, IEnumerable<liteCommunityConstraint> items, Boolean loadinfo = false, List<liteCommunityInfo> communities = null)
            {
                var query = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdRole > 0 && s.IdPerson == idPerson select s);
                List<Int32> idCommunities = items.Where(i => i.IdSource == idCommunity).Select(i => i.IdDestinationCommunity).Distinct().ToList();
                idCommunities.AddRange(items.Where(i => i.IdDestinationCommunity == idCommunity).Select(i => i.IdSource).Distinct().ToList());

                List<liteSubscriptionInfo> subscriptions = (idCommunities.Count() > maxItemsForQuery) ? query.ToList().Where(s => idCommunities.Contains(s.IdCommunity)).ToList() : query.Where(s => idCommunities.Contains(s.IdCommunity)).ToList();
                return GetDtoCommunityConstraints(subscriptions, idCommunity, items, loadinfo, communities);
            }
            public List<dtoCommunityConstraint> GetDtoCommunityConstraints(List<liteSubscriptionInfo> subscriptions, Int32 idCommunity, IEnumerable<liteCommunityConstraint> items, Boolean loadinfo = false, List<liteCommunityInfo> communities = null)
            {
                List<Int32> idCommunities = items.Where(i => i.IdSource == idCommunity).Select(i => i.IdDestinationCommunity).Distinct().ToList();
                idCommunities.AddRange(items.Where(i => i.IdDestinationCommunity == idCommunity).Select(i => i.IdSource).Distinct().ToList());

                if (loadinfo && communities == null)
                {
                    var queryCommunities = (from c in Manager.GetIQ<liteCommunityInfo>() select c);
                    if (idCommunities.Count() <= maxItemsForQuery)
                        queryCommunities = queryCommunities.Where(c => idCommunities.Contains(c.Id));
                    else
                        queryCommunities = queryCommunities.ToList().Where(c => idCommunities.Contains(c.Id)).AsQueryable();

                    communities = queryCommunities.ToList();
                }
                return items.Select(i => new dtoCommunityConstraint(i, subscriptions, i.IdSource != idCommunity, communities)).ToList();
            }
            public List<liteCommunityConstraint> GetConstraints(List<Int32> idCommunities)
            {
                var query = (from c in Manager.GetIQ<liteCommunityConstraint>() where c.Deleted == BaseStatusDeleted.None select c);
                if (idCommunities.Count() * 2 > maxItemsForQuery)
                    return query.ToList().Where(c => idCommunities.Contains(c.IdSource) || idCommunities.Contains(c.IdDestinationCommunity)).ToList();
                else
                    return query.Where(c => idCommunities.Contains(c.IdSource) || idCommunities.Contains(c.IdDestinationCommunity)).ToList();
            }
            private long GetEnrolledUsers(Int32 idCommunity, Int32 dRole)
            {
                if (dRole > 0)
                    return (from s in Manager.GetIQ<LazySubscription>() where s.IdRole == dRole && s.IdCommunity == idCommunity select s.Id).Count();
                else
                    return -1;
            }
            private List<dtoUserEnrollments> GetResponsibles(List<Int32> idCommunities)
            {
                List<dtoUserEnrollments> results = new List<dtoUserEnrollments>();
                List<Int32> idPersons = new List<Int32>();

                if (idCommunities.Count > maxItemsForQuery)
                    results = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.isResponsabile select s).ToList().Where(s => idCommunities.Contains(s.IdCommunity)).GroupBy(s => s.IdPerson).Select(s => new dtoUserEnrollments() { IdPerson = s.Key, IdCommunities = s.Select(ss => ss.IdCommunity).ToList() }).ToList();
                else
                    results = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.isResponsabile && idCommunities.Contains(s.IdCommunity) select s).ToList().GroupBy(s => s.IdPerson).Select(s => new dtoUserEnrollments() { IdPerson = s.Key, IdCommunities = s.Select(ss => ss.IdCommunity).ToList() }).ToList();
                idPersons = results.Select(r => r.IdPerson).Distinct().ToList();
                List<litePerson> users = null;
                var query = (from p in Manager.GetIQ<litePerson>() select p);
                if (idPersons.Count > maxItemsForQuery)
                    users = query.ToList().Where(s => idPersons.Contains(s.Id)).ToList();
                else
                    users = query.Where(s => idPersons.Contains(s.Id)).ToList();
                foreach (dtoUserEnrollments r in results)
                {
                    r.Person = users.Where(u => u.Id == r.IdPerson).FirstOrDefault();
                }

                return results;
            }
            private LazySubscription AddSubscription(Int32 idPerson, List<Int32> idUserOrganizations, liteCommunityInfo community, Int32 dRole, String path = "", DateTime? enrolledOn= null)
            {
                Boolean newSubscription = true;
                LazySubscription subscription = (from s in Manager.GetIQ<LazySubscription>() where s.IdPerson == idPerson && s.IdCommunity == community.Id select s).Skip(0).Take(1).ToList().FirstOrDefault();
                if (subscription == null)
                    subscription = new LazySubscription();
                else
                    newSubscription = false;
                if (newSubscription || subscription.IdRole < (int)RoleTypeStandard.Administrator)
                {
                    if (enrolledOn.HasValue)
                    {
                        subscription.LastAccessOn = enrolledOn.Value;
                        subscription.SubscriptedOn = enrolledOn.Value;
                    }
                    else
                    {
                        subscription.LastAccessOn = DateTime.Now;
                        subscription.SubscriptedOn = subscription.LastAccessOn;
                    }
                    subscription.Accepted = !community.ConfirmSubscription && (community.IdTypeOfCommunity != (int)CommunityTypeStandard.Organization || subscription.IdRole == (int)RoleTypeStandard.HiddenCreator);
                    subscription.Enabled = !community.ConfirmSubscription && (community.IdTypeOfCommunity != (int)CommunityTypeStandard.Organization || subscription.IdRole == (int)RoleTypeStandard.HiddenCreator);
                    subscription.IdCommunity = community.Id;

                    subscription.IdPerson = idPerson;
                    if (community.IdCreatedBy == idPerson && subscription.IdRole == (int)RoleTypeStandard.HiddenCreator && dRole > 0)
                        subscription.IdRole = (int)RoleTypeStandard.Administrator;
                    else
                        subscription.IdRole = dRole;

                    Manager.SaveOrUpdate(subscription);
                    // save organization link
                    if (community.IdTypeOfCommunity == (int)CommunityTypeStandard.Organization  && !idUserOrganizations.Contains(community.IdOrganization))
                    {
                        liteOrganizationProfile orgProfile = new liteOrganizationProfile();
                        orgProfile.IdOrganization = community.IdOrganization;
                        orgProfile.IdPerson = idPerson;
                        orgProfile.isDefault = false;
                        Manager.SaveOrUpdate(orgProfile);
                    }
                    if (community.IdTypeOfCommunity != (int)CommunityTypeStandard.Organization)
                        AddSubscriptionToFather(idPerson, idUserOrganizations, community, path,enrolledOn);
                }
                
                return subscription;
            }
            private void AddSubscriptionToFather(Int32 idPerson, List<Int32> idUserOrganizations, liteCommunityInfo child, String path, DateTime? enrolledOn)
            {
                if (child != null)
                {
                    if (string.IsNullOrEmpty(path))
                        AddSubscriptionToFather(idPerson, idUserOrganizations, child, enrolledOn);
                    else
                    {
                        List<Int32> idCommunities = (from pId in path.Split('.').ToList() where !String.IsNullOrEmpty(pId) select int.Parse(pId)).ToList();
                        List<Int32> subscribed = (from s in Manager.GetIQ<LazySubscription>()
                                                  where idCommunities.Contains(s.IdCommunity) && s.IdPerson == idPerson
                                                  select s.IdCommunity).ToList();
                        idCommunities = idCommunities.Except(subscribed).ToList();
                        foreach (liteCommunityInfo community in (from c in Manager.GetIQ<liteCommunityInfo>() where idCommunities.Contains(c.Id) select c).ToList())
                        {
                            AddSubscription(idPerson, idUserOrganizations, community, (community.IdCreatedBy == idPerson) ? (int)RoleTypeStandard.HiddenCreator : (int)RoleTypeStandard.HiddenEnrollment, path, enrolledOn);
                        }
                    }
                }
            }
            private void AddSubscriptionToFather(Int32 idPerson, List<Int32> idUserOrganizations, liteCommunityInfo child, DateTime? enrolledOn)
            {
                if (child!=null)
                {
                    List<Int32> idFathers = (from or in Manager.GetIQ<_OldCommunityRelation>()
                                             where or.IdDestination == child.Id 
                                             select or.IdSource).ToList();
                    if (!(from s in Manager.GetIQ<LazySubscription>() where idFathers.Contains(s.IdCommunity) && s.IdPerson == idPerson select s.Id).Any())
                    {
                        liteCommunityInfo father = Manager.GetLiteCommunityInfo(child.IdFather);
                        if (father != null)
                            AddSubscription(idPerson, idUserOrganizations, father, (father.IdCreatedBy == idPerson) ? (int)RoleTypeStandard.HiddenCreator : (int)RoleTypeStandard.HiddenEnrollment, "", enrolledOn); 
                      }
                }
            }

            public List<Int32> GetCommunityAvailableIdRoles(Community community)
            {
                List<Int32> roles = new List<Int32>();
                try
                {
                    if (community != null)
                    {
                        roles = (from t in Manager.GetIQ<RoleCommunityTypeTemplate>()
                                 where t.Type.Id == community.TypeOfCommunity.Id
                                 select t.Role.Id).ToList();
                    }
                }
                catch (Exception ex) {
                    roles = new List<Int32>();
                }
               
                return roles;
            }
        
        #endregion

        #region "Permissions"
            public lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement GetModulePermission(int personId, int communityId)
            {
                lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement module = new lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement();
                Person person = Manager.GetPerson(personId);
                if (communityId == 0)
                    module = lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.CreatePortalmodule(person.TypeID);
                else
                {
                    module = new lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement(Manager.GetModulePermission(personId, communityId, ServiceModuleID()));
                }
                return module;
            }
            public List<Organization> GetAvailableOrganizations(Int32 idUser, SearchCommunityFor type)
            {
                List<Organization> organizations = null;
                try
                {
                    Person user = Manager.GetPerson(idUser);
                    if (user != null)
                    {
                        if (user.TypeID == (int)UserTypeStandard.SysAdmin || user.TypeID == (int)UserTypeStandard.Administrator)
                            organizations = (from o in Manager.GetIQ<Organization>() select o).ToList();
                        else
                        {
                            organizations = new List<Organization>();
                            List<int> idOrganizations = (from po in Manager.GetIQ<OrganizationProfiles>() where po.Profile == user select po.OrganizationID).ToList();
                            List<Organization> sOrganizations = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select o).ToList();
                            List<Community> communities = (from c in Manager.GetIQ<Community>() where c.IdFather == 0 && idOrganizations.Contains(c.IdOrganization) select c).ToList();
                            List<int> idCommunities = communities.Select(c => c.Id).ToList();

                            Boolean isAdministrative = (user.TypeID == (int)UserTypeStandard.Administrative);

                            List<LazySubscription> subscriptions = (from s in Manager.GetIQ<LazySubscription>()
                                                                    where s.Accepted && s.Enabled && s.IdPerson == idUser && idCommunities.Contains(s.IdCommunity)
                                                                    select s).ToList();
                            switch (type)
                            {
                                case SearchCommunityFor.CommunityManagement:
                                case SearchCommunityFor.SystemManagement:
                                    Int32 idModule = Manager.GetModuleID(lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID);
                                    foreach (LazySubscription sub in subscriptions)
                                    {
                                        lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement module = new lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement(Manager.GetModulePermission(idUser, sub.IdCommunity, idModule));
                                        if (module.Administration || module.Manage)
                                            organizations.Add(sOrganizations.Where(o => o.Id == communities.Where(c => c.Id == sub.IdCommunity).Select(c => c.IdOrganization).FirstOrDefault()).FirstOrDefault());
                                    }

                                    break;
                                case SearchCommunityFor.Subscribe:
                                    organizations = sOrganizations.Where(o => !communities.Where(c => subscriptions.Select(s => s.IdCommunity).ToList().Contains(c.Id)).Select(c => c.IdOrganization).ToList().Contains(o.Id)).ToList();
                                    break;
                                case SearchCommunityFor.Subscribed:
                                    if (subscriptions.Any())
                                        organizations = sOrganizations.Where(o => communities.Where(c => subscriptions.Select(s => s.IdCommunity).ToList().Contains(c.Id)).Select(c => c.IdOrganization).ToList().Contains(o.Id)).ToList();
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    organizations = new List<Organization>();
                }
                return organizations;
            }
            public List<Int32> GetAvailableIdOrganizations(Int32 idUser, SearchCommunityFor type)
            {
                List<Int32> organizations = null;
                try
                {
                    litePerson user = Manager.GetLitePerson(idUser);
                    if (user != null)
                    {
                        if (user.TypeID == (int)UserTypeStandard.SysAdmin || user.TypeID == (int)UserTypeStandard.Administrator)
                            organizations = (from o in Manager.GetIQ<Organization>() select o.Id).ToList();
                        else
                        {
                            organizations = new List<Int32>();
                            List<Int32> idOrganizations = (from po in Manager.GetIQ<liteOrganizationProfile>() where po.IdPerson == idUser select po.IdOrganization).ToList();
                            Dictionary<Int32, Int32> orgCommunities = (from c in Manager.GetIQ<liteCommunity>() where c.IdFather == 0 select c).ToDictionary(c => c.Id, c => c.IdOrganization);
                            List<Int32> idSubscribedcommunities =  orgCommunities.Where(o=> idOrganizations.Contains(o.Value )).Select(o=> o.Key).ToList();
                            Boolean isAdministrative = (user.TypeID == (int)UserTypeStandard.Administrative);

                            var query = (from s in Manager.GetIQ<LazySubscription>()
                                         where s.Accepted && s.Enabled && s.IdPerson == idUser && idSubscribedcommunities.Contains(s.IdCommunity)
                                         select s);
                            switch (type)
                            {
                                case SearchCommunityFor.CommunityManagement:
                                case SearchCommunityFor.SystemManagement:
                                    Int32 idModule = Manager.GetModuleID(lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID);
                                    foreach (LazySubscription sub in query)
                                    {
                                        lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement module = new lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement(Manager.GetModulePermission(idUser, sub.IdCommunity, idModule));
                                        if (module.Administration || module.Manage)
                                            organizations.Add(orgCommunities[sub.IdCommunity]);
                                    }

                                    break;
                                case SearchCommunityFor.Subscribe:
                                    idSubscribedcommunities = query.Select(o => o.IdCommunity).ToList();
                                    organizations = orgCommunities.Where(o => !idOrganizations.Contains(o.Key)).Select(o=> o.Value).ToList();
                                    break;
                                case SearchCommunityFor.Subscribed:
                                    idSubscribedcommunities = query.Select(o => o.IdCommunity).ToList();
                                    organizations = orgCommunities.Where(o => idSubscribedcommunities.Contains(o.Key)).Select(o => o.Value).ToList();
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    organizations = new List<Int32>();
                }
                return organizations;
            }
            public Boolean HasPermissionToManageOtherCommunities(Int32 idProfile, List<Int32> excludeIdCommunities)
            {
                return HasAvailableCommunitiesByModule(idProfile, true, excludeIdCommunities, null);
            }
            public Boolean HasAvailableCommunitiesByModule(Int32 idProfile,Boolean forManage, List<Int32> excludeIdCommunities , Dictionary<String, long> otherModulePermissions = null)
            {
                Boolean result = false;
                Person person = Manager.GetPerson(idProfile);
                Int32 personType = person.TypeID;
                result = (personType == (Int32)UserTypeStandard.SysAdmin || personType == (Int32)UserTypeStandard.Administrative || personType == (Int32)UserTypeStandard.Administrator);
                if (!result)
                {
                    List<LazySubscription> subscriptions = (from s in Manager.GetIQ<LazySubscription>()
                                 where s.IdPerson == idProfile &&  s.Accepted && s.Enabled
                                 select s).ToList();
                    if (excludeIdCommunities.Any())
                        subscriptions = subscriptions.Where(s=> !excludeIdCommunities.Contains(s.IdCommunity)).ToList();

                    Int32 idModule = ServiceModuleID();
                    long managePermission = (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.Manage | (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.AdminService;
                    result = HasAvailableCommunitiesByModule(idProfile, subscriptions, idModule, managePermission);
                    if (!result && otherModulePermissions!=null)
                    {
                        foreach (var item in otherModulePermissions) {
                            result = HasAvailableCommunitiesByModule(idProfile, subscriptions, item.Key, item.Value);
                            if(result)
                                break;
                        }
                    }
                }
                return result;
            }
            public List<Int32> GetIdCommunityByModulePermissions(Int32 idProfile, Dictionary<Int32, long> permissions){
                return GetIdCommunityByModulePermissions(idProfile, permissions, new List<Int32>());
            }
            public List<Int32> GetIdCommunityByModulePermissions(Int32 idProfile, Dictionary<Int32, long> permissions, List<Int32> unloadIdCommunities)
            {
                List<Int32> results = new List<Int32>();
                try
                {
                    Person person = Manager.GetPerson(idProfile);
                    Int32 personType = person.TypeID;
                    List<LazySubscription> subscriptions = (from s in Manager.GetIQ<LazySubscription>()
                                            where s.IdPerson == idProfile && s.Accepted && s.Enabled
                                            select s).ToList();
                    subscriptions = subscriptions.Where(s => !unloadIdCommunities.Contains(s.IdCommunity)).ToList();

                    foreach(var permission in permissions){
                        List<LazyCommunityModulePermission> items = GetCommunitiesModulePermissions(idProfile, subscriptions, permission.Key, permission.Value);
                        if (items.Any()) {
                            results.AddRange(items.Select(i => i.CommunityID).ToList());
                            subscriptions = subscriptions.Where(s => !results.Contains(s.IdCommunity)).ToList();
                        }
                    } 
                }
                catch (Exception ex) { 
                
                }
                return results;
            }

            public List<Int32> GetAvailableOrganizationsByModulePermissions(Int32 idProfile, Dictionary<Int32, long> permissions, List<Int32> unloadIdCommunities, List<Int32> availableOrganizations)
            {
                List<Int32> results = new List<Int32>();
                try
                {
                    Person person = Manager.GetPerson(idProfile);
                    Int32 personType = person.TypeID;
                    List<LazySubscription> subscriptions = (from s in Manager.GetIQ<LazySubscription>()
                                                            where s.IdPerson == idProfile && s.Accepted && s.Enabled
                                                            select s).ToList();
                    subscriptions = subscriptions.Where(s => !unloadIdCommunities.Contains(s.IdCommunity)).ToList();
                    List<Int32> idCommunities = new List<Int32>();
                    foreach (var permission in permissions)
                    {
                        List<LazyCommunityModulePermission> items = GetCommunitiesModulePermissions(idProfile, subscriptions, permission.Key, permission.Value);
                        if (items.Any())
                        {
                            idCommunities.AddRange(items.Select(i => i.CommunityID).ToList());
                            subscriptions = subscriptions.Where(s => !results.Contains(s.IdCommunity)).ToList();
                        }
                    }
                    if (idCommunities.Count > 0 && idCommunities.Count < maxItemsForQuery)
                    {
                        results = (from c in Manager.GetIQ<Community>() where idCommunities.Contains(c.Id) && availableOrganizations.Contains(c.IdOrganization) select c.IdOrganization).Distinct().ToList();
                    }
                    else {
                        Int32 pageIndex = 0;
                        var idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        results = new List<Int32>();
                        while (idQuery.Any())
                        {
                            results.AddRange((from c in Manager.GetIQ<Community>() where idQuery.Contains(c.Id) && availableOrganizations.Contains(c.IdOrganization) select c.IdOrganization).Distinct().ToList());
                            pageIndex++;
                            idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        }
                        
                    }
                }
                catch (Exception ex)
                {

                }
                return results.Distinct().ToList();
            }

            private List<LazyCommunityModulePermission> GetCommunitiesModulePermissions(Int32 idProfile, List<LazySubscription> subscriptions, String moduleCode, long requiredPermissions)
            {
                return GetCommunitiesModulePermissions(idProfile, subscriptions, Manager.GetModuleID(moduleCode), requiredPermissions);
            }
            private List<LazyCommunityModulePermission> GetCommunitiesModulePermissions(Int32 idProfile, List<LazySubscription> subscriptions, Int32 idModule, long requiredPermissions)
            {
                List<LazyCommunityModulePermission> items = new List<LazyCommunityModulePermission>();
                List<Int32> idCommunities = subscriptions.Select(s => s.IdCommunity).ToList();
                if (idCommunities.Count <= maxItemsForQuery)
                    items = (from p in Manager.GetIQ<LazyCommunityModulePermission>()
                                    where idCommunities.Contains(p.CommunityID) && p.ModuleID == idModule
                                   select p).ToList().Where(p => subscriptions.Where(s => s.IdCommunity == p.CommunityID && s.IdRole == p.RoleID).Any()).ToList();
                else
                {
                    Int32 pageIndex = 0;
                    var idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idQuery.Any())
                    {
                        items.AddRange((from p in Manager.GetIQ<LazyCommunityModulePermission>()
                                                where idQuery.Contains(p.CommunityID) && p.ModuleID == idModule
                                        select p).ToList().Where(p => subscriptions.Where(s => s.IdCommunity == p.CommunityID && s.IdRole == p.RoleID).Any()).ToList());
                        pageIndex++;
                        idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                }
                return items.Where(p => PermissionHelper.CheckPermissionSoft(requiredPermissions, p.PermissionLong)).ToList();
            }

            private Boolean HasAvailableCommunitiesByModule(Int32 idProfile, List<LazySubscription> subscriptions, String moduleCode, long requiredPermissions)
            {
                return HasAvailableCommunitiesByModule(idProfile, subscriptions, Manager.GetModuleID(moduleCode), requiredPermissions);
            }
            private Boolean HasAvailableCommunitiesByModule(Int32 idProfile, List<LazySubscription> subscriptions, Int32 idModule, long requiredPermissions)
            {
                Boolean result = false;
                List<Int32> idCommunities = subscriptions.Select(s => s.IdCommunity).ToList();
                if (idCommunities.Count <= maxItemsForQuery)
                    return (from p in Manager.GetIQ<LazyCommunityModulePermission>()
                            where idCommunities.Contains(p.CommunityID) && p.ModuleID == idModule
                            select p).ToList().Where(p => subscriptions.Where(s => s.IdCommunity == p.CommunityID && s.IdRole == p.RoleID).Any() && PermissionHelper.CheckPermissionSoft(requiredPermissions, p.PermissionLong)).Any();
                else
                {
                    Int32 pageIndex = 0;
                    var idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idQuery.Any())
                    {
                        result = (from p in Manager.GetIQ<LazyCommunityModulePermission>()
                                        where idQuery.Contains(p.CommunityID) && p.ModuleID == idModule
                                        select p).ToList().Where(p => subscriptions.Where(s => s.IdCommunity == p.CommunityID && s.IdRole == p.RoleID).Any() && PermissionHelper.CheckPermissionSoft(requiredPermissions, p.PermissionLong)).Any();
                        if (result)
                            return result;
                        pageIndex++;
                        idQuery = idCommunities.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                }
                return result;
            }
        #endregion

        #region "utility"

        #endregion

        #region "My Subscriptions"

        #endregion
    }
}