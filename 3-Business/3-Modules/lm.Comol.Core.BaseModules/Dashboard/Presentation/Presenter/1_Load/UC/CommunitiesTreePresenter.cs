using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Dashboard.Domain;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class CommunitiesTreePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities service;
        private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;
        protected virtual IViewCommunitiesTree View
        {
            get { return (IViewCommunitiesTree)base.View; }
        }
        private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities Service
        {
            get
            {
                if (service == null)
                    service = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                return service;
            }
        }
        private lm.Comol.Core.Tag.Business.ServiceTags ServiceTags
        {
            get
            {
                if (servicetag == null)
                    servicetag = new lm.Comol.Core.Tag.Business.ServiceTags(AppContext);
                return servicetag;
            }
        }
        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleDashboard.UniqueCode);
                return currentIdModule;
            }
        }
        public CommunitiesTreePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public CommunitiesTreePresenter(iApplicationContext oContext, IViewCommunitiesTree view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(Boolean advancedMode, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability availability = CommunityManagement.CommunityAvailability.Subscribed , Int32 idReferenceCommunity = 0, String referencePath = "")
        {
            View.AdvancedMode = advancedMode;
            View.ReferenceIdCommunity = idReferenceCommunity;
            View.IsFirstLoad = true;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = (idReferenceCommunity>0) ? ModuleDashboard.ActionType.TreeNoChildrenToLoad : ModuleDashboard.ActionType.TreeUnableToLoad;
                List<lm.Comol.Core.DomainModel.Filters.Filter> fToLoad = null;
                switch (availability)
                {
                    case CommunityManagement.CommunityAvailability.Subscribed:
                        fToLoad = Service.GetDefaultFilters(UserContext.CurrentUserID, "", -1,-1,null, null, availability).OrderBy(f => f.DisplayOrder).ToList();
                        View.LoadDefaultFilters(fToLoad);
                        break;
                }
                if (fToLoad != null && fToLoad.Any())
                {
                    lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters = new lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters(fToLoad, availability);
                    View.CurrentFilters =filters;
                    action = (idReferenceCommunity > 0) ? ModuleDashboard.ActionType.TreeLoadChildren : ModuleDashboard.ActionType.TreeLoad;
                    LoadTree(filters, advancedMode, idReferenceCommunity,false );
                }
                else
                    View.DisplayNoTreeToLoad((idReferenceCommunity > 0) ? CurrentManager.GetCommunityName(idReferenceCommunity) : "");
                View.SendUserAction(idReferenceCommunity, CurrentIdModule, action);
            }
        }

        #region "Load methods"
            private void LoadTree(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean advancedMode, Int32 idReferenceCommunity = 0, Boolean useCache = true)
            {
                litePerson p = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                List<dtoCommunityNodeItem> nodes = Service.GetTree(p, filters, advancedMode, idReferenceCommunity, useCache);

                List<Int32> idCommunities = View.GetIdcommunitiesWithNews(nodes.Where(n => n.Type == NodeType.Community && n.Details.Permissions.AccessTo).Select(n => n.IdCommunity).Distinct().ToList(), UserContext.CurrentUserID);
                nodes.Where(n=> idCommunities.Contains(n.IdCommunity)).ToList().ForEach(n=> n.Details.Permissions.ViewNews=true);
                View.LoadTree(nodes);
            }
            public void ApplyFilters(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean advancedMode, Int32 idReferenceCommunity)
            {
                View.CurrentFilters = filters;
                LoadTree(filters, advancedMode, idReferenceCommunity, true);
            }
        #endregion

        #region "Remove Enrollment"
            public void UnsubscribeFromCommunity(Int32 idCommunity, String path, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode node = Service.UnsubscribeInfo(UserContext.CurrentUserID, idCommunity, path);
                    if (node != null)
                    {
                        ModuleDashboard.ActionType dAction = ModuleDashboard.ActionType.None;
                        List<lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode> nodes = node.GetAllNodes();
                        if (!nodes.Where(n => n.AllowUnsubscribe()).Any())
                        {
                            View.DisplayUnableToUnsubscribe(CurrentManager.GetCommunityName(idCommunity));
                            dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunity;
                        }
                        else
                        {
                            List<RemoveAction> actions = new List<RemoveAction>();
                            actions.Add(RemoveAction.None);
                            actions.Add(RemoveAction.FromCommunity);
                            if (nodes.Where(n => n.AllowUnsubscribe()).Count() > 1)
                                actions.Add(RemoveAction.FromAllSubCommunities);

                            if (node == null)
                            {
                                View.DisplayUnableToUnsubscribe(CurrentManager.GetCommunityName(idCommunity));
                                dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunity;
                            }
                            else if (!node.AllowUnsubscribe())
                            {
                                View.DisplayUnsubscribeNotAllowed(node.Name);
                                dAction = ModuleDashboard.ActionType.UnsubscribeNotallowed;
                            }
                            else if (node.AllowUnsubscribe() && (!node.CommunityAllowSubscription || node.MaxUsersWithDefaultRole > 0 || (node.CommunitySubscriptionEndOn.HasValue && DateTime.Now.AddDays(30) > node.CommunitySubscriptionEndOn.Value)))
                            {
                                View.DisplayConfirmMessage(idCommunity, path, node, actions, RemoveAction.None, nodes.Where(n => n.AllowUnsubscribe() && n.Id != idCommunity).ToList());
                                dAction = ModuleDashboard.ActionType.RequireUnsubscribeConfirm;
                            }
                            else
                            {
                                if (nodes.Where(n => n.AllowUnsubscribe()).Count() > 1)
                                {
                                    View.DisplayConfirmMessage(idCommunity, path, node, actions, RemoveAction.FromCommunity, nodes.Where(n => n.AllowUnsubscribe() && n.Id != idCommunity).ToList());
                                    dAction = ModuleDashboard.ActionType.RequireUnsubscribeConfirmFromSubCommunities;
                                }
                                else
                                {
                                    List<liteSubscriptionInfo> subscriptions = Service.UnsubscribeFromCommunity(UserContext.CurrentUserID, node, RemoveAction.FromCommunity);
                                    if (subscriptions != null && subscriptions.Any() && subscriptions.Count == 1 && subscriptions[0].IdRole < 1)
                                    {
                                        View.DisplayUnsubscribedFrom(node.Name);
                                        dAction = ModuleDashboard.ActionType.UnsubscribeFromCommunity;
                                    }
                                    else
                                    {
                                        View.DisplayUnableToUnsubscribe(node.Name);
                                        dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunity;
                                    }
                                }
                            }
                        }
                        View.SendUserAction(0, CurrentIdModule, idCommunity, dAction);
                        if (dAction == ModuleDashboard.ActionType.UnsubscribeFromCommunity)
                            LoadTree(filters, View.AdvancedMode, View.ReferenceIdCommunity,false);
                    }
                    else
                    {
                        String name = CurrentManager.GetCommunityName(idCommunity);
                        if (!String.IsNullOrEmpty(name))
                            View.DisplayUnableToUnsubscribe(CurrentManager.GetCommunityName(idCommunity));
                        View.SendUserAction(0, CurrentIdModule, idCommunity, ModuleDashboard.ActionType.UnableToUnsubscribe);
                    }
              
                }
            }
            public void UnsubscribeFromCommunity(Int32 idCommunity, String path,RemoveAction action,  lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode node = Service.UnsubscribeInfo(UserContext.CurrentUserID, idCommunity, path);
                    if (node != null)
                    {
                        switch (action)
                        {
                            case RemoveAction.None:
                                break;
                            default:
                                ModuleDashboard.ActionType dAction = ModuleDashboard.ActionType.UnableToUnsubscribe;
                                List<liteSubscriptionInfo> subscriptions = Service.UnsubscribeFromCommunity(UserContext.CurrentUserID, node, action);
                                if (subscriptions == null && !subscriptions.Any())
                                {
                                    switch (action)
                                    {
                                        case RemoveAction.FromCommunity:
                                            dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunity;
                                            View.DisplayUnableToUnsubscribe(node.Name);
                                            break;
                                        case RemoveAction.FromAllSubCommunities:
                                            dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunities;
                                            View.DisplayUnsubscriptionMessage(new List<String>(), node.GetAllNodes().Where(n => n.AllowUnsubscribe()).Select(n => n.Name).ToList());
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (action)
                                    {
                                        case RemoveAction.FromCommunity:
                                            dAction = ModuleDashboard.ActionType.UnsubscribeFromCommunity;
                                            View.DisplayUnsubscribedFrom(node.Name);
                                            break;
                                        case RemoveAction.FromAllSubCommunities:
                                            dAction = ModuleDashboard.ActionType.UnsubscribeFromCommunities;
                                            View.DisplayUnsubscriptionMessage(node.GetAllNodes().Where(n => n.AllowUnsubscribe() && subscriptions.Where(s => s.IdCommunity == n.Id && s.IdRole < 1).Any()).Select(n => n.Name).ToList(),
                                                node.GetAllNodes().Where(n => n.AllowUnsubscribe() && subscriptions.Where(s => s.IdCommunity == n.Id && s.IdRole > 0).Any()).Select(n => n.Name).ToList()
                                                );
                                            break;
                                    }
                                }
                                View.SendUserAction(0, CurrentIdModule, idCommunity, dAction);
                                switch (dAction)
                                {
                                    case ModuleDashboard.ActionType.UnsubscribeFromCommunity:
                                    case ModuleDashboard.ActionType.UnsubscribeFromCommunities:
                                        LoadTree(filters, View.AdvancedMode, View.ReferenceIdCommunity, false);
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        String name = CurrentManager.GetCommunityName(idCommunity);
                        if (!String.IsNullOrEmpty(name))
                            View.DisplayUnableToUnsubscribe(name);
                        View.SendUserAction(0, CurrentIdModule, idCommunity, ModuleDashboard.ActionType.UnableToUnsubscribe);
                    }
                    
                }
            }
        #endregion

        #region "Enroll To"
            public void EnrollTo(Int32 idCommunity, String name, String path, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters)
            {
                Int32 idPerson = UserContext.CurrentUserID;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    Boolean reloadCommunities = false;
                    ModuleDashboard.ActionType dAction = ModuleDashboard.ActionType.None;
                    dtoCommunityInfoForEnroll item = Service.GetEnrollingItem(idPerson, idCommunity, path);
                    litePerson person = CurrentManager.GetLitePerson(idPerson);
                    if (item != null && item.Id > 0 && person != null && person.Id > 0)
                    {
                        if (!item.AllowEnroll)
                        {
                            dAction = ModuleDashboard.ActionType.EnrollNotAllowed;
                            View.DisplayEnrollMessage(item, dAction);
                        }
                        else
                        {
                            if (!item.HasConstraints && item.AllowUnsubscribe)
                            {
                                dtoEnrollment enrollment = Service.EnrollTo(idPerson, item);
                                if (enrollment == null)
                                {
                                    dAction = ModuleDashboard.ActionType.UnableToEnroll;
                                    View.DisplayEnrollMessage(item, dAction);
                                }
                                else
                                {
                                    View.DisplayEnrollMessage(enrollment, enrollment.IdCommunity, person, Service.GetTranslatedProfileType(person), CurrentManager.GetUserDefaultOrganizationName(idPerson));
                                    reloadCommunities = true;
                                    dAction = (enrollment.Status == EnrolledStatus.NeedConfirm) ? ModuleDashboard.ActionType.EnrollToCommunityWaitingConfirm : ModuleDashboard.ActionType.EnrollToCommunity;
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(idPerson));
                                }
                            }
                            else
                            {
                                dAction = ModuleDashboard.ActionType.RequireEnrollConfirm;
                                View.DisplayConfirmMessage(item);
                            }
                        }
                    }
                    else
                    {
                        dAction = ModuleDashboard.ActionType.UnableToEnroll;
                        View.DisplayUnknownCommunity(name);
                    }
                    View.SendUserAction(0, CurrentIdModule, idCommunity, dAction);
                    if (reloadCommunities)
                        LoadTree(filters, View.AdvancedMode, View.ReferenceIdCommunity, false);
                }
            }
            public void EnrollTo(dtoCommunityInfoForEnroll community, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Boolean advancedMode, Int32 idReferenceCommunity)
            {
                Int32 idPerson = UserContext.CurrentUserID;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    Boolean reloadCommunities = false;
                    if (community!=null)
                    {
                        reloadCommunities = true;
                        litePerson person = CurrentManager.GetLitePerson(idPerson);
                        String profileType = Service.GetTranslatedProfileType(person);
                        String organizationName = CurrentManager.GetUserDefaultOrganizationName(idPerson);

                        dtoEnrollment enrollment = Service.EnrollTo(idPerson, community);
                        if (enrollment!=null)
                        {
                            switch (enrollment.Status)
                            {
                                case EnrolledStatus.Available:
                                case EnrolledStatus.NeedConfirm:
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(idPerson));
                                    //View.NotifyEnrollment(enrollment, person, profileType, organizationName);

                                    View.SendUserAction(0, CurrentIdModule, community.Id,(enrollment.Status == EnrolledStatus.Available) ? ModuleDashboard.ActionType.EnrollToCommunity : ModuleDashboard.ActionType.EnrollToCommunityWaitingConfirm);
                                    reloadCommunities = true;
                                    break;
                            }
                            View.DisplayEnrollMessage(enrollment, enrollment.IdCommunity, person, Service.GetTranslatedProfileType(person), CurrentManager.GetUserDefaultOrganizationName(idPerson));
                        }
                        if (reloadCommunities)
                            LoadTree(filters, advancedMode, idReferenceCommunity, false);
                    }
                    else
                        View.SendUserAction(0, CurrentIdModule, ModuleDashboard.ActionType.NoSelectedCommunitiesToEnroll);
                }
            }
        #endregion 
    }
}