using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.WebConferencing.Domain;
namespace lm.Comol.Modules.Standard.WebConferencing.Presentation
{
    public class SelectUsersForMessageService : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {

        #region "Initializer"
            private int _idModule;
            private WbService service;
            private WbService Service
            {
                get
                {
                    if (service == null)
                    {
                        switch (View.SysParameter.CurrentSystem) { 
                            case wBImplementedSystem.eWorks:
                                service = new Domain.eWorks.eWService((Domain.eWorks.eWSystemParameter)View.SysParameter, this.AppContext);
                                break;
                            case  wBImplementedSystem.OpenMeetings:
                                service = new Domain.OpenMeetings.oMService((Domain.OpenMeetings.oMSystemParameter)View.SysParameter, this.AppContext);
                                break;
                        }
                    }
                    return service;
                }
            }

            private lm.Comol.Core.Mail.Messages.MailMessagesService messageservice;
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService profileservice;

            private int ModuleID
            {
                get
                {
                    if (_idModule <= 0)
                        _idModule = CurrentManager.GetModuleID(ModuleWebConferencing.UniqueCode);
                    return _idModule;
                }
            }
            public virtual lm.Comol.Core.Business.BaseModuleManager CurrentManager { get; set; }

            protected virtual IViewSelectUsersForMessageService View
            {
                get { return (IViewSelectUsersForMessageService)base.View; }
            }
            private lm.Comol.Core.Mail.Messages.MailMessagesService MessageService
            {
                get
                {
                    if (messageservice == null)
                        messageservice = new lm.Comol.Core.Mail.Messages.MailMessagesService(AppContext);
                    return messageservice;
                }
            }
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (profileservice == null)
                        profileservice = new lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(AppContext);
                    return profileservice;
                }
            }
            public SelectUsersForMessageService(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new lm.Comol.Core.Business.BaseModuleManager(oContext);
            }
            public SelectUsersForMessageService(iApplicationContext oContext, IViewSelectUsersForMessageService view)
                : base(oContext, view)
            {
                this.CurrentManager = new lm.Comol.Core.Business.BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(List<long> removeUsers, Boolean fromPortal, Int32 idCommunity, ModuleObject obj, long idTemplate, long idVersion, Boolean isTemplateCompliant, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = null)
        {
            View.RemoveUsers = removeUsers;
            View.LoadedNoUsers = false;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                InitializeFilters(removeUsers, fromPortal, idCommunity, obj, idTemplate, idVersion, isTemplateCompliant, translations);
            View.isInitialized = true;
        }
        public void InitializeFilters(List<long> removeUsers, Boolean fromPortal, Int32 idCommunity, ModuleObject obj, long idTemplate, long idVersion, Boolean isTemplateCompliant, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = null)
        {
            View.CurrentObject = obj;
            View.CurrentOrderBy = UserByMessagesOrder.BySurname;
            View.IdCommunityContainer = idCommunity;
            View.CodeModuleContainer = ModuleWebConferencing.UniqueCode;
            View.IdModuleContainer = Service.ServiceModuleID();

            View.Ascending = true;
            dtoUsersByMessageFilter filter = InitializeFilter(obj, translations);
            if (MessageService.HasMessages(obj))
                View.InitializeMessagesSelector(fromPortal, idCommunity, ModuleWebConferencing.UniqueCode,CurrentManager.GetModuleID(ModuleWebConferencing.UniqueCode), obj);
            
            View.CurrentFilter = filter;
            View.InitializeWordSelector(new List<String>());
            LoadColumns(false, filter.UserStatus != UserStatus.All);
            View.DisplayNoUsersFound();
        }
        private dtoUsersByMessageFilter InitializeFilter(ModuleObject obj, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = null)
        {
            dtoUsersByMessageFilter filter = new dtoUsersByMessageFilter() { Ascending = true, OrderBy = UserByMessagesOrder.BySurname };
            View.Ascending = true;
            View.CurrentOrderBy = UserByMessagesOrder.BySurname;
            InitializeUserTypeStatusFilter(filter, obj);
            InitializeUserStatusFilter(filter,filter.UserType, obj);
            InitializeMailStatusFilter(filter,filter.UserType,filter.UserStatus, obj);
            InitializeAgencyFilter(filter, obj);
            InitializeSearchFilter(filter);
            if (filter.UserType == UserTypeFilter.None || (filter.UserType == UserTypeFilter.All && (filter.UserStatus == UserStatus.All || filter.UserStatus == UserStatus.NotSubscribed)))
                LoadCommunityFilters(obj.ObjectLongID, filter);
            else
                View.HideCommunityFilters();
            return filter;
        }
        private void InitializeSearchFilter(dtoUsersByMessageFilter filter) {
            List<lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy> items = new List<Core.BaseModules.ProfileManagement.SearchProfilesBy>();
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.All);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Name);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail);
            View.LoadSearchProfilesBy(items, filter.SearchBy);
        }
        private void InitializeMailStatusFilter(dtoUsersByMessageFilter filter,UserTypeFilter userType, UserStatus status, ModuleObject obj)
        {
            List<MailStatus> items = Service.GetAvailableMailStatus(obj.ObjectLongID, userType,status, View.RemoveUsers);
            filter.MailStatus = (!items.Any() ? MailStatus.All : (items.Contains(MailStatus.Confirmed) ? MailStatus.Confirmed : items[0]));
            View.LoadAvailableStatus(items, filter.MailStatus);
        }
        private void InitializeUserTypeStatusFilter(dtoUsersByMessageFilter filter, ModuleObject obj)
        {
            List<UserTypeFilter> items = Service.GetAvailableUserTypes(obj, View.RemoveUsers,MessageService);
            filter.UserType = (!items.Any() ? UserTypeFilter.All : (items.Contains(UserTypeFilter.WithoutMembers) ? UserTypeFilter.WithoutMembers : items[0]));
            View.LoadAvailableUserType(items, filter.UserType);
        }
        private void InitializeUserStatusFilter(dtoUsersByMessageFilter filter,UserTypeFilter userType ,ModuleObject obj)
        {
            List<UserStatus> items = Service.GetAvailableUserStatus(obj.ObjectLongID,userType, View.RemoveUsers);
            filter.UserStatus = (!items.Any() ? UserStatus.All : (items.Contains(UserStatus.Unlocked) ? UserStatus.Unlocked : items[0]));
            View.LoadAvailableUserStatus(items, filter.UserStatus);
        }
        private void InitializeAgencyFilter(dtoUsersByMessageFilter filter, ModuleObject obj, long idDefaultAgency = 0)
        {
            Boolean hasAgencies = false;
            Dictionary<long, String> agencies = null;
            long idRoom = (obj == null) ? (long)0 : obj.ObjectLongID;
            hasAgencies = Service.HasUsersWithProfileType(idRoom, (int)UserTypeStandard.Employee, filter.UserType,filter.UserStatus);
            if (hasAgencies)
                agencies = Service.GetAgenciesForUsers(idRoom, filter.UserType, filter.UserStatus);
                 
            View.IsAgencyColumnVisible = (hasAgencies && agencies != null && agencies.Count > 0);
            if (hasAgencies && agencies != null && agencies.Count > 0)
                View.LoadAgencies(agencies, idDefaultAgency);
            else
                View.UnLoadAgencies();
        }
        private void LoadCommunityFilters(long idRoom, dtoUsersByMessageFilter filter)
        {
            WbRoom room = Service.RoomGet(idRoom);
            if (room != null && room.SubCommunity != SubscriptionType.None)
            {
                filter.IdCommunity = room.CommunityId;
                filter.IdRole = -1;
                filter.IdProfileType = -1;
                View.LoadAvailableRoles(ProfileService.GetAvailableSubscriptionsIdRoles(room.CommunityId, new List<Int32>(), false), -1);
            }
            else if (room != null && room.SubSystem != SubscriptionType.None)
            {
                filter.IdRole = -1;
                filter.IdCommunity = room.CommunityId;
                View.LoadAvailableProfileType(ProfileService.GetAvailableProfileTypes(-1), -1);
            }
            else
                View.HideCommunityFilters();
        }
        public void ChangeUserStatusFilter(UserStatus status) {
            dtoUsersByMessageFilter filter = View.CurrentFilter;
            ModuleObject obj = View.CurrentObject;
            if (obj != null)
            {
                if (filter.IdProfileType >= 0 && filter.IdProfileType != (int)UserTypeStandard.Employee)
                    View.UnLoadAgencies();
                else
                    InitializeAgencyFilter(filter, obj, View.SelectedIdAgency);
                InitializeMailStatusFilter(filter, filter.UserType, status, obj);
                if (filter.UserType == UserTypeFilter.None || (filter.UserType == UserTypeFilter.All && (filter.UserStatus == UserStatus.All || filter.UserStatus == UserStatus.NotSubscribed)))
                    LoadCommunityFilters(obj.ObjectLongID, filter);
                else
                    View.HideCommunityFilters();
            }
        }
        public void ChangeUserTypeFilter(UserTypeFilter userType)
        {
            dtoUsersByMessageFilter filter = View.CurrentFilter;
            ModuleObject obj = View.CurrentObject;
            if (obj != null) {
                List<UserStatus> items = Service.GetAvailableUserStatus(obj.ObjectLongID, userType, View.RemoveUsers);
                UserStatus dStatus = (!items.Any() ? UserStatus.All : ((items.Contains(View.CurrentUserStatus)) ? View.CurrentUserStatus : items[0]));
                View.LoadAvailableUserStatus(items, dStatus);
                filter.UserStatus = dStatus;
                InitializeAgencyFilter(filter, obj, View.SelectedIdAgency);
                InitializeMailStatusFilter(filter, filter.UserType, dStatus, obj);

                if (filter.UserType == UserTypeFilter.None || (filter.UserType == UserTypeFilter.All && (filter.UserStatus == UserStatus.All || filter.UserStatus == UserStatus.NotSubscribed)))
                    LoadCommunityFilters(obj.ObjectLongID, filter);
                else
                    View.HideCommunityFilters();
            }
        }
        public List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> GetSelectedRecipients()
        {
            List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> items = new List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient>();
            List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> sItems = UpdateItemsSelection();
            View.SelectedItems = sItems;

            dtoUsersByMessageFilter filter = View.CurrentFilter;
            filter.StartWith = View.CurrentStartWith;
            items = Service.GetSelectedUsers(View.CurrentObject, View.RemoveUsers, filter, View.SelectAll, sItems, MessageService, ProfileService);
            return items;
        }
        public void EditItemsSelection(Boolean selectAll)
        {
            View.SelectAll = selectAll;
            View.SelectedItems = (selectAll) ? UpdateItemsSelection().Distinct().ToList() : new List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem>();
        }
        public void LoadItems(dtoUsersByMessageFilter filter, Int32 pageIndex, Int32 pageSize, Boolean initialize)
        {
            
            filter.StartWith = View.CurrentStartWith;
            List<dtoWebConferenceMessageRecipient> recipients = Service.GetAvailableUsersForMessages(View.UnknownUserTranslation, View.AnonymousUserTranslation, View.CurrentObject, View.RemoveUsers, filter, MessageService, ProfileService);
            if (initialize)
                View.InitializeWordSelector(recipients.Select(r => r.FirstLetter).Distinct().ToList());
            else
                View.InitializeWordSelector(recipients.Select(r => r.FirstLetter).Distinct().ToList(), filter.StartWith);

            recipients = Service.GetUsersForMessages(ProfileService, recipients, filter, View.GetTranslatedProfileTypes, View.GetTranslatedRoles, true);


            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;//Me.View.CurrentPageSize
            pager.Count = (recipients.Count > 0) ? recipients.Count - 1 : 0;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            View.SelectedItems = UpdateItemsSelection();

            recipients = recipients.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            LoadColumns(recipients.Where(r=> r.MessageNumber>0).Any(), filter.UserStatus != UserStatus.All, recipients.Where(r=> r.IdAgency>0).Any());
            View.LoadedNoUsers = !recipients.Any();
            if (recipients.Any())
                View.LoadUsers(recipients);
            else
                View.DisplayNoUsersFound();
        }

        private List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> UpdateItemsSelection()
        {
            // SELECTED ITEMS
            List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> items = View.SelectedItems;
            List<dtoSelectItem<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem>> cSelectedItems = View.GetCurrentSelectedItems();

            // REMOVE ITEMS
            items = items.Where(i => !cSelectedItems.Where(si => !si.Selected && si.Id.IdModuleObject == i.IdModuleObject && si.Id.IdPerson == i.IdPerson && si.Id.IdUserModule == i.IdUserModule).Any()).ToList();
            // ADD ITEMS
            items.AddRange(cSelectedItems.Where(si => si.Selected && !items.Where(i => si.Id.IdModuleObject == i.IdModuleObject && si.Id.IdPerson == i.IdPerson && si.Id.IdUserModule == i.IdUserModule).Any()).Select(si => si.Id).Distinct().ToList());
            return items;
        }

        private void LoadColumns(Boolean hasMessage, Boolean hasStatus, Boolean hasAgencies = true)
        {
            List<ColumnMessageGrid> columns = new List<ColumnMessageGrid>();
            if (hasMessage)
                columns.Add(ColumnMessageGrid.Actions);
            if (View.IsAgencyColumnVisible && hasAgencies)
                columns.Add(ColumnMessageGrid.AgencyName);
            if (hasStatus)
                columns.Add(ColumnMessageGrid.UserStatus);
            View.AvailableColumns = columns;
        }
    }
}