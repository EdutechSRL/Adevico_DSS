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
    public class TagsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            private ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTagsList View
            {
                get { return (IViewTagsList)base.View; }
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
            public TagsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TagsListPresenter(iApplicationContext oContext, IViewTagsList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(ModuleTags permissions, Int32 idCommunity, Boolean fromRecycleBin = false, Boolean fromOrganization = false)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                View.IdTagsCommunity = idCommunity;
                dtoFilters filters = new dtoFilters(fromOrganization);
                filters.FromRecycleBin = fromRecycleBin;
                InitializeView(filters, idCommunity);
            }
        }

        private void InitializeView(dtoFilters filters, Int32 idCommunity)
        {
            View.IdSelectedTagLanguage = -1;
            List<lm.Comol.Core.Dashboard.Domain.dtoItemFilter<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem>> languages = ServiceTags.GetLanguageSelectorItems(View.GetDefaultLanguageName(), View.GetDefaultLanguageCode());
            View.LoadLanguages(languages);
            View.FirstLoad = true;
            View.FirstLoadForLanguages = languages.ToDictionary(l => l.Value.IdLanguage, l => true);
            View.CurrentFilters = filters;
            LoadTags(filters, idCommunity,0, View.CurrentPageSize);
        }
        public void LoadTags(dtoFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                SetTagsInfo(filters, idCommunity);
                List<dtoTagItem> items = ServiceTags.GetTags(UserContext.CurrentUserID, TagType.Community, filters, idCommunity, View.GetUnknownUserName());
                if (items == null)
                    View.DisplayErrorLoadingFromDB();
                else
                {
                    Int32 itemsCount = items.Count();
                    PagerBase pager = new PagerBase();
                    pager.PageSize = pageSize;
                    pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;
                    pager.PageIndex = pageIndex;
                    View.Pager = pager;
                    String dCode = View.GetDefaultLanguageCode();
                    String dLanguage = View.GetDefaultLanguageName();
                    items = items.Skip(pager.PageIndex * pageSize).Take(pageSize).ToList();
                    
                    View.AllowApplyFilters(!(View.FirstLoad && !items.Any()));
                    items.ForEach(i => i.Translations.Insert(0, new lm.Comol.Core.DomainModel.Languages.dtoLanguageItem() { IdLanguage = -1, IsMultiLanguage = true, LanguageCode = dCode, LanguageName = dLanguage }));
                    View.LoadTags(items, filters.IdSelectedLanguage);
                    View.SendUserAction((filters.ForOrganization) ? idCommunity : 0, CurrentIdModule, (filters.IdOrganization>-3) ?  ModuleTags.ActionType.OrganizationListTags : ModuleTags.ActionType.PortalListTags);
                    View.FirstLoad = false;
                    View.FirstLoadForLanguages[filters.IdSelectedLanguage] = false;
                }
            }
        }
        public void ApplyFilters(dtoFilters filters, Int32 idCommunity, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                LoadTags(filters, idCommunity, 0, pageSize);
            }
        }
        public void BulkInsert(dtoFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize, List<Dictionary<Int32, String>> toInsert)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleTags.ActionType action = ModuleTags.ActionType.GenericError;
                List<Dictionary<Int32, String>> notInserted = new List<Dictionary<int, string>>();
                List<TagItem> tags = ServiceTags.BulkInsert(TagType.Community, (idCommunity==0), idCommunity, toInsert, notInserted);
                if (tags.Any())
                {
                    if (notInserted.Any())
                    {
                        action = (filters.ForOrganization) ? ModuleTags.ActionType.AddedBulkTagsToOrganizationWithDuplicates : ModuleTags.ActionType.AddedBulkTagsToPortalWithDuplicates;
                        View.DisplayMessage(action, notInserted.Select(i => i[0]).ToList());
                    }
                    else
                    {
                        action = (filters.ForOrganization) ? ModuleTags.ActionType.AddedBulkTagsToOrganization : ModuleTags.ActionType.AddedBulkTagsToPortal;
                        View.DisplayMessage(action);
                    }
                }
                else
                {
                    action = (filters.ForOrganization) ? ModuleTags.ActionType.UnableToAddBulkTagsToOrganization : ModuleTags.ActionType.UnableToAddBulkTagsToPortal;
                    View.DisplayMessage(action);
                }
                View.SendUserAction((filters.ForOrganization) ? idCommunity : 0, CurrentIdModule, action);
                LoadTags(filters, idCommunity, 0, pageSize);
            }
        }
        public void SetStatus(long idTag, lm.Comol.Core.Dashboard.Domain.AvailableStatus status, dtoFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleTags.ActionType action = (status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available) ? ModuleTags.ActionType.EnableTag : ModuleTags.ActionType.DisableTag;
                TagItem item = ServiceTags.SetStatus(idTag, status);
                if (item == null || item.Status != status)
                    action = (status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available) ? ModuleTags.ActionType.UnableToEnableTag : ModuleTags.ActionType.UnableToDisableTag;
                View.DisplayMessage(action);
                View.SendUserAction((filters.ForOrganization) ? idCommunity : 0, CurrentIdModule,idTag, action);
                LoadTags(filters, idCommunity, 0, pageSize);
            }
        }
        public void VirtualDelete(long idTag, Boolean delete, dtoFilters filters, Int32 idCommunity, Int32 pageIndex, Int32 pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleTags.ActionType action = (delete) ? ModuleTags.ActionType.VirtualDelete : ModuleTags.ActionType.VirtualUndelete;
                TagItem item = ServiceTags.VirtualDelete(idTag, delete);
                if (item == null)
                    action = (delete) ? ModuleTags.ActionType.UnableToDelete : ModuleTags.ActionType.UnableToUndelete;
                View.DisplayMessage(action);
                View.SendUserAction((filters.ForOrganization) ? idCommunity : 0, CurrentIdModule,idTag, action);
                LoadTags(filters, idCommunity, 0, pageSize);
            }
        }
        private void SetTagsInfo(dtoFilters filters, Int32 idCommunity)
        {
            Int32 idOrganization=0;
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement permissions = null;
            if (filters.ForOrganization)
            {
                if (idCommunity == 0)
                {
                    idCommunity = UserContext.CurrentCommunityID;
                    idOrganization = CurrentManager.GetIdOrganizationFromCommunity(idCommunity);
                }
                if (idOrganization == 0)
                {
                    idOrganization = CurrentManager.GetUserDefaultIdOrganization(UserContext.CurrentUserID);
                    idCommunity = CurrentManager.GetIdCommunityFromOrganization(idOrganization);
                }
                permissions = new DomainModel.Domain.ModuleCommunityManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID));
            }
            else
                permissions = DomainModel.Domain.ModuleCommunityManagement.CreatePortalmodule((p==null) ? (Int32)UserTypeStandard.Guest : p.TypeID);

            Int32 noTags = ServiceTags.GetCommunitiesWithNoTags();
            String url = "";
            if (permissions.Administration || permissions.Manage)
                url = RootObject.BulkTagsAssignment(idOrganization, !filters.ForOrganization, true);
            View.LoadTagsInfo(noTags, ServiceTags.GetUntranslatedTagsCount(filters.IdSelectedLanguage, false),0,url);
        }
    }
}