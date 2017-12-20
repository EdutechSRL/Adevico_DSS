using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Tag.Domain;
using lm.Comol.Core.BaseModules.Tags.Business;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    public class BulkTagsAssignmentPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            private ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewBulkTagsAssignment View
            {
                get { return (IViewBulkTagsAssignment)base.View; }
            }
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceCommunities
            {
                get
                {
                    if (serviceCommunities == null)
                        serviceCommunities = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return serviceCommunities;
                }
            }
            private ServiceTags ServiceTags
            {
                get
                {
                    if (servicetag == null)
                        servicetag = new ServiceTags(AppContext);
                    return servicetag;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleTags.UniqueCode);
                    return currentIdModule;
                }
            }
            public BulkTagsAssignmentPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public BulkTagsAssignmentPresenter(iApplicationContext oContext, IViewBulkTagsAssignment view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 pageSize, Int32 idOrganization = 0, Boolean assigned = false, Boolean fromPortal = true, Boolean fromPage = false, String url = "")
        {
            litePerson p = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idCommunity = 0;
                lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement permissions = null;
                if (!fromPortal)
                {  
                    if (idOrganization>0)
                        idCommunity = CurrentManager.GetIdCommunityFromOrganization(idOrganization);
                    if (idCommunity==0){
                        idCommunity = UserContext.CurrentCommunityID;
                        idOrganization = CurrentManager.GetIdOrganizationFromCommunity(idCommunity);
                    }
                    if (idOrganization ==0){
                        idOrganization = CurrentManager.GetUserDefaultIdOrganization(UserContext.CurrentUserID);
                        idCommunity = CurrentManager.GetIdCommunityFromOrganization(idOrganization);
                    }
                    permissions = new DomainModel.Domain.ModuleCommunityManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID));
                }
                else
                    permissions = DomainModel.Domain.ModuleCommunityManagement.CreatePortalmodule(p.TypeID);
                View.CurrentIdOrganization = idOrganization;
                View.CurrentIdCommunity = idCommunity;
                View.CurrentAssignedTags = new List<dtoBulkCommunityForTags>();
                if (!String.IsNullOrEmpty(url) && fromPage)
                    View.SetBackUrl(url);
                if (permissions.Administration || permissions.Manage)
                {
                    View.AllowSave = true;
                    List<lm.Comol.Core.DomainModel.Filters.Filter> fToLoad = ServiceCommunities.GetDefaultFiltersForAssignments(p.Id, "", -1, null, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.All, assigned).OrderBy(f => f.DisplayOrder).ToList();

                    View.LoadDefaultFilters(fToLoad);
                    if (fToLoad != null && fToLoad.Any())
                    {
                        lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters = new lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters(fToLoad, CommunityManagement.CommunityAvailability.All);

                        View.CurrentFilters = filters;
                        LoadCommunities(idCommunity, filters, 0, pageSize, ModuleTags.ActionType.BulkTagsAssignLoad, true);
                    }
                    else
                    {
                        View.DisplayNoCommunitiesToLoad();
                        View.SendUserAction(idCommunity, CurrentIdModule, ModuleTags.ActionType.BulkTagsNoCommunitiesFound);
                    }
                }
                else
                    View.SendUserAction(idCommunity, CurrentIdModule, ModuleTags.ActionType.NoPermissionForBulkTagsAssign);
            }
        }

        private void LoadCommunities(Int32 idCommunity, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Int32 pageIndex, Int32 pageSize, ModuleTags.ActionType action, Boolean firstLoad = false)
        {
            List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags> items = ServiceCommunities.GetCommunitiesForBulkTagsManage(UserContext.CurrentUserID, filters);
            Int32 itemsCount = (items == null ? 0 : items.Count);
            
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;
            pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;
            pager.PageIndex = pageIndex;
            View.Pager = pager;
            if (items != null)
            {
                if (firstLoad && itemsCount == 0)
                    View.HideFilters();
                else if (itemsCount==0 && ServiceTags.GetCommunitiesWithNoTags() ==0)
                    View.HideFilters();
                View.LoadCommunities(GenerateItems(filters, items.Skip(pager.PageIndex * pageSize).Take(pageSize).ToList(), itemsCount > pageSize));
                View.SendUserAction(idCommunity, CurrentIdModule, action);
            }
            else
                View.DisplayErrorFromDB();
        }
        public void LoadCommunities(Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<dtoBulkCommunityForTags> current = View.CurrentAssignedTags;
                List<dtoBulkCommunityForTags> selected = View.GetSelectedItems();
                if (selected.Where(i => !i.Selected).Any())
                    current = current.Where(c => selected.Where(s => !s.Selected && s.IdCommunity == c.IdCommunity).Any()).ToList();
                if (selected.Where(i => i.Selected).Any())
                {
                    List<Int32> idCommunities = current.Select(c => c.IdCommunity).ToList();
                    current.AddRange(selected.Where(s => s.Selected && !idCommunities.Contains(s.IdCommunity)).ToList());
                    foreach (dtoBulkCommunityForTags c in current.Where(i => selected.Where(s => s.IdCommunity == i.IdCommunity && s.Selected).Any()))
                    {
                        c.IdSelectedTags = selected.Where(s => s.IdCommunity == c.IdCommunity && s.Selected).FirstOrDefault().IdSelectedTags;
                    }
                }
                View.CurrentAssignedTags = current;
                LoadCommunities(idCommunity, View.CurrentFilters, pageIndex, pageSize, ModuleTags.ActionType.BulkTagsAssignChangePage);
            }
        }
        public void ApplyFilters(Int32 idCommunity, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.CurrentAssignedTags = new List<dtoBulkCommunityForTags>();
                View.CurrentFilters = filters;
                LoadCommunities(idCommunity,filters, 0, pageSize, ModuleTags.ActionType.BulkTagsAssignApplyFilters);
            }
        }
        private List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags> GenerateItems(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags> items, Boolean hasMultiPage)
        {
            List<Int32> idOrganizations = (filters.IdOrganization > 0) ? new List<Int32>() { filters.IdOrganization } : new List<Int32>();
            List<dtoTagSelectItem> defaultTags = ServiceTags.GetTags(TagType.Community, 0, idOrganizations);
            View.LoadDefaultTags(defaultTags, hasMultiPage);
            foreach (lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags item in items.Where(i=> i.IdOrganization== filters.IdOrganization))
            {
                item.AvailableTags = defaultTags.Select(d => d.Copy(item.IdTags.Contains(d.Id))).ToList();
            }
            foreach (lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags item in items.Where(i => !i.AvailableTags.Any()))
            {
                item.AvailableTags = ServiceTags.GetTags(TagType.Community, item.Id, new List<Int32>() {item.IdOrganization}, -1, item.IdTags);
            }
            return items;
        }

        public void ApplyTags(List<Int32> idCommunities, List<long> tags, Boolean applyToAll,lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleTags.ActionType action = ModuleTags.ActionType.BulkTagsUnasigned;
                View.CurrentAssignedTags = new List<dtoBulkCommunityForTags>();
                pageIndex = (filters.WithoutTags) ? ((pageIndex > 0) ? pageIndex - 1 : pageIndex) : pageIndex;
                if (applyToAll)
                {
                    List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags> communities = ServiceCommunities.GetCommunitiesForBulkTagsManage(UserContext.CurrentUserID, filters);
                    if (communities != null)
                        idCommunities = communities.Select(c => c.Id).ToList();
                }

                if (idCommunities.Any())
                {
                    List<dtoBulkCommunityForTags> cTags = View.CurrentAssignedTags;
                    if (ServiceTags.ApplyTagsToCommunities(idCommunities, tags).Count>0)
                    {
                        action = ModuleTags.ActionType.BulkTagsAssigned;
                       
                        foreach (Int32 id in idCommunities)
                        {
                            dtoBulkCommunityForTags current = cTags.Where(c => c.IdCommunity == id).FirstOrDefault();
                            if (current != null)
                                current.IdSelectedTags = tags;
                        }
                        View.CurrentAssignedTags = cTags;
                        if (applyToAll)
                            filters = ServiceCommunities.GetDefaultFilters(CurrentManager.GetLitePerson(UserContext.CurrentUserID), CommunityManagement.CommunityAvailability.All, true);
                        else
                            filters = RecalcDefaultFilters(filters);
                        View.CurrentFilters = filters;
                        View.DeselectAll();
                        LoadCommunities(idCommunity, filters, pageIndex, pageSize, ModuleTags.ActionType.BulkTagsUpdateListAferTagsAssignment);
                    }
                    else
                        LoadCommunities(idCommunity, pageIndex, pageSize);
                }
                else
                    action = ModuleTags.ActionType.BulkTagsNoSelection;
                View.DisplayMessage(action);
                View.SendUserAction(idCommunity, CurrentIdModule, ModuleTags.ActionType.NoPermissionForBulkTagsAssign);
            }
        }
        public void ApplyTags(List<dtoBulkCommunityForTags> items,lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleTags.ActionType action = ModuleTags.ActionType.BulkTagsUnasigned;
                if (items.Any())
                {
                    List<dtoBulkCommunityForTags> cTags = View.CurrentAssignedTags;
                    if (ServiceTags.ApplyTagsToCommunities(items))
                    {
                        action = ModuleTags.ActionType.BulkTagsAssigned;
                        pageIndex = (filters.WithoutTags) ? ((pageIndex > 0) ? pageIndex - 1 : pageIndex) : pageIndex;
                        foreach (dtoBulkCommunityForTags item in items)
                        {
                            dtoBulkCommunityForTags current = cTags.Where(c => c.IdCommunity == item.IdCommunity).FirstOrDefault();
                            if (current != null)
                                current.IdSelectedTags = item.IdSelectedTags;
                        }
                       View.CurrentAssignedTags = cTags;
                       filters = RecalcDefaultFilters(filters);
                       View.CurrentFilters = filters;
                       LoadCommunities(idCommunity, filters, pageIndex, pageSize, ModuleTags.ActionType.BulkTagsUpdateListAferTagsAssignment);
                    }
                }
                else
                    action = ModuleTags.ActionType.BulkTagsNoSelection;
                View.DisplayMessage(action);
                View.SendUserAction(idCommunity, CurrentIdModule, action);
            }
        }
        private lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters RecalcDefaultFilters(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters)
        {
            List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags> communities = ServiceCommunities.GetCommunitiesForBulkTagsManage(UserContext.CurrentUserID, filters);
            if (communities != null && communities.Count == 0)
                return ServiceCommunities.GetDefaultFilters(CurrentManager.GetLitePerson(UserContext.CurrentUserID), CommunityManagement.CommunityAvailability.All, true);
            else
                return filters;
        }
    }
}