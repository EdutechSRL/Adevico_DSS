using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Business;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.Mail.Messages;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class MessagesSentPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService profileService;
            private lm.Comol.Core.BaseModules.TemplateMessages.Business.ModuleTemplateMessageService mailService;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewMessagesSent View
            {
                get { return (IViewMessagesSent)base.View; }
            }
            private TemplateMessagesService Service
            {
                get
                {
                    if (service == null)
                        service = new TemplateMessagesService(AppContext);
                    return service;
                }
            }
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (profileService == null)
                        profileService = new lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(AppContext);
                    return profileService;
                }
            }
            private lm.Comol.Core.BaseModules.TemplateMessages.Business.ModuleTemplateMessageService MailService
            {
                get
                {
                    if (mailService == null)
                        mailService = new lm.Comol.Core.BaseModules.TemplateMessages.Business.ModuleTemplateMessageService(AppContext);
                    return mailService;
                }
            }
            public MessagesSentPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MessagesSentPresenter(iApplicationContext oContext, IViewMessagesSent view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Int32 idCommunity = View.PreloadIdCommunity;
            if (idCommunity < 0)
                idCommunity = UserContext.CurrentCommunityID;
            dtoModuleMessagesContext context = GetContext();

            View.ContainerContext = context;
            if (UserContext.isAnonymous)
                SessionTimeout(idCommunity,View.PreloadSelectedTab);
            else
            {
                DisplayTab tabs = View.PreloadTabs;
                DisplayTab tab = View.PreloadSelectedTab;
                if (tabs ==  DisplayTab.None)
                    tab = DisplayTab.Sent;
              
                TemplateType type = View.PreloadTemplateType;
                if (type == TemplateType.None)
                    type = TemplateType.Module;

                SetCurrentItems(type, idCommunity, tab);
                LoadTabs(tabs, tab);
               
                Int32 idOtherModule = CurrentManager.GetModuleID(context.ModuleCode);
                Boolean otherModule = (context.ModuleCode != ModuleTemplateMessages.UniqueCode);
                switch (tab) { 
                    case DisplayTab.List:
                    case DisplayTab.Send:
                        View.GoToUrl(GetCurrentUrl(tab));
                        break;
                    case DisplayTab.Sent:
                        View.InitializeFilterSelector(new List<DisplayItems>() { DisplayItems.ByRecipient, DisplayItems.ByMessage }, DisplayItems.ByRecipient); 
                        if (HasPermission(context)) {
                            DisplayItems displayBy = View.PreloadDisplayBy;
                            View.CurrentDisplayBy = displayBy;

                            if (MailService.HasMessages(context.ModuleObject))
                            {
                                switch (displayBy)
                                {
                                    case DisplayItems.ByRecipient:
                                        InitializeByRecipients(context);
                                        break;
                                    case DisplayItems.ByMessage:
                                        IntializeByMessage(context);
                                        break;
                                }
                            }
                            else
                                View.DisplayObjectWithNoMessage();
                        }
                        else
                            View.DisplayNoPermission(idCommunity, idOtherModule, context.ModuleCode);
                        break;
                    default:
                        if (otherModule)
                            View.DisplayNoPermission(idCommunity, idOtherModule, context.ModuleCode);
                        else
                            View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                        break;
                }
            }
        }

#region "Initializers"
    #region "Base"
        private void SetCurrentItems(TemplateType type, Int32 idCommunity, DisplayTab current)
        {
            View.CurrentModuleCode = View.PreloadModuleCode;
            View.AvailableTabs = View.PreloadTabs;
            View.CurrentIdCommunity = idCommunity;
            View.CurrentIdModule = View.PreloadIdModule;
            View.CurrentModuleObject = View.PreloadModuleObject;
            System.Guid sessionId = Guid.NewGuid();
            View.CurrentSessionId = sessionId;
            View.SetBackUrl(View.PreloadBackUrl, sessionId, GetCurrentUrl(current));
        }
        private void LoadTabs(DisplayTab tabs,DisplayTab tab)
        {
            List<DisplayTab> items = new List<DisplayTab>();
            if (tabs != DisplayTab.None) {
                if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)tabs, (long)DisplayTab.List))
                    items.Add(DisplayTab.List);
                if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)tabs, (long)DisplayTab.Send))
                    items.Add(DisplayTab.Send);
                if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)tabs, (long)DisplayTab.Sent))
                    items.Add(DisplayTab.Sent);
                if (!items.Contains(tab))
                    tab = items.Where(t => t != DisplayTab.Sent).FirstOrDefault();
            }
        
            View.LoadTabs(items, tab);
        }  
        public void LoadTab(DisplayTab current) {
            if (UserContext.isAnonymous)
                SessionTimeout(View.CurrentIdCommunity,current);
            else {
                //switch (current) { 
                //    case DisplayTab.List:
                //        if (!View.IsInitializedList)
                //            InitializeList(null, View.CurrentIdCommunity, View.CurrentTemplateType, View.ModulePermissions);
                //        View.DisplayList();
                //        break;
                //    case DisplayTab.Send:
                //        if (!View.IsInitializedSender)
                //        {
                //            //GetAvailableSaveAs(),
                //            View.InitializeSendMessage(false, View.CurrentIdCommunity, View.CurrentModuleCode);
                //        }
                //        View.DisplaySendMessage();
                //        break;
                //}
                View.SelectedTab = current;
            }    
        }
    #endregion
    #region "Common Filters"
        private void InitializeSearchFilter(lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter = null)
        {
            List<lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy> items = new List<Core.BaseModules.ProfileManagement.SearchProfilesBy>();
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.All);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Name);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail);
            View.LoadSearchProfilesBy(items, (filter==null) ?  Core.BaseModules.ProfileManagement.SearchProfilesBy.All : filter.SearchBy);
        }
    #endregion
    #region "Recipients"
        public void IntializeByRecipients()
        {
            View.CurrentDisplayBy = DisplayItems.ByRecipient;
            View.CurrentStartWith = "";
            View.CurrentSearchValue = "";
            InitializeByRecipients(View.ContainerContext);
        }
        private void InitializeByRecipients(dtoModuleMessagesContext context)
        {
            View.UserCurrentOrderBy = MailSender.UserByMessagesOrder.ByUser;
            View.Ascending = true;
            lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter = InitializeFilter(context.ModuleObject);
            View.CurrentFilter = filter;
            View.InitializeWordSelector(new List<String>());

            LoadColumns(false,false,false);
            View.DisplayNoUsersFound();
        }
        private lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters InitializeFilter(ModuleObject obj)
        {
            lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter = new lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters() { Ascending = true, OrderBy =  MailSender.UserByMessagesOrder.ByUser };
            filter.IdCommunity = obj.CommunityID;
            InitializeAgencyFilter(filter, obj);
            InitializeSearchFilter(filter);
            InitializeCommunityFilters(obj, filter.IdCommunity,filter );

            return filter;
        }
        private void InitializeAgencyFilter(lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter, ModuleObject obj, long idDefaultAgency = 0)
        {
            Boolean hasAgencies = false;
            Dictionary<long, String> agencies = null;
            hasAgencies = MailService.HasUsersWithProfileType(obj, (int)UserTypeStandard.Employee);
            if (hasAgencies)
                agencies = MailService.GetAgenciesForUsers(obj);

            View.IsAgencyColumnVisible = (hasAgencies && agencies != null && agencies.Count > 0);
            if (hasAgencies && agencies != null && agencies.Count > 0)
                View.LoadAgencies(agencies, idDefaultAgency);
            else
                View.UnLoadAgencies();
        }
        private void InitializeCommunityFilters(ModuleObject obj, Int32 idCommunity, lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter)
        {
            if (idCommunity>0)
            {
                filter.IdCommunity = idCommunity;
                filter.IdRole = -1;
                filter.IdProfileType = -1;
                View.LoadAvailableRoles(MailService.GetAvailableSubscriptionsIdRoles(obj,idCommunity, false), -1);
            }
            else
            {
                filter.IdRole = -1;
                filter.IdCommunity = idCommunity;
                View.LoadAvailableProfileType(MailService.GetAvailableProfileTypes(obj), -1);
            }
        }
        //public void ChangeUserTypeFilter(UserTypeFilter userType)
        //{
        //    dtoUsersByMessageFilter filter = View.CurrentFilter;
        //    ModuleObject obj = View.CurrentObject;
        //    if (obj != null)
        //    {
        //        List<UserStatus> items = Service.GetAvailableUserStatus(obj.ObjectLongID, userType, View.RemoveUsers);
        //        UserStatus dStatus = (!items.Any() ? UserStatus.All : ((items.Contains(View.CurrentUserStatus)) ? View.CurrentUserStatus : items[0]));
        //        View.LoadAvailableUserStatus(items, dStatus);
        //        filter.UserStatus = dStatus;
        //        InitializeAgencyFilter(filter, obj, View.SelectedIdAgency);
        //        InitializeMailStatusFilter(filter, filter.UserType, dStatus, obj);

        //        if (filter.UserType == UserTypeFilter.None || (filter.UserType == UserTypeFilter.All && (filter.UserStatus == UserStatus.All || filter.UserStatus == UserStatus.NotSubscribed)))
        //            LoadCommunityFilters(obj.ObjectLongID, filter);
        //        else
        //            View.HideCommunityFilters();
        //    }
        //}
    #endregion
    #region "Messages"
        public  void IntializeByMessage()
        {
            View.CurrentDisplayBy = DisplayItems.ByMessage;
            View.CurrentStartWith = "";
            View.CurrentSearchValue = "";
            IntializeByMessage(View.ContainerContext);
        }
        private void IntializeByMessage(dtoModuleMessagesContext context)
        {
            View.MessageCurrentOrderBy = MessageOrder.ByDate;
            View.Ascending = false;
            View.InitializeWordSelector(new List<String>());

            LoadMessages(0, View.PageSize, true);
        }
    #endregion
#endregion
        public void LoadRecipients(lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter, Int32 pageIndex, Int32 pageSize, Boolean initialize)
        {
            if (UserContext.isAnonymous)
                SessionTimeout(View.CurrentIdCommunity, View.PreloadSelectedTab);
            else
            {
                filter.StartWith = View.CurrentStartWith;
                List<dtoGenericModuleMessageRecipient> recipients = MailService.GetAvailableRecipientsForObject(View.UnknownUserTranslation, View.AnonymousUserTranslation, View.CurrentModuleObject, filter, ProfileService);
                if (initialize)
                    View.InitializeWordSelector(recipients.Select(r => r.FirstLetter).Distinct().ToList());
                else
                    View.InitializeWordSelector(recipients.Select(r => r.FirstLetter).Distinct().ToList(), filter.StartWith);

                recipients = MailService.GetParsedUsersForMessages(recipients, filter, ProfileService);


                PagerBase pager = new PagerBase();
                pager.PageSize = pageSize;//Me.View.CurrentPageSize
                pager.Count = (recipients.Count > 0) ? recipients.Count - 1 : 0;
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;

                recipients = recipients.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                LoadColumns(recipients.Where(r => r.MessageNumber > 0).Any(), filter.IdCommunity > 0, filter.IdCommunity <= 0, recipients.Where(r => r.IdAgency > 0).Any());
                View.LoadedNoUsers = !recipients.Any();
                if (recipients.Any())
                    View.LoadRecipients(recipients);
                else
                    View.DisplayNoUsersFound();
            }
        }
        public void LoadMessages(Int32 pageIndex, Int32 pageSize, Boolean initialize)
        {
            if (UserContext.isAnonymous)
                SessionTimeout(View.CurrentIdCommunity, View.PreloadSelectedTab);
            else
            {
                String startWith = View.CurrentStartWith;
                List<dtoFilteredDisplayMessage> messages = MailService.GetObjectMessages(UserContext.CurrentUserID, View.CurrentModuleObject, View.SelectedFilter.IdCommunity);
                if (initialize)
                    View.InitializeWordSelector(messages.Select(r => r.FirstLetter).Distinct().ToList());
                else
                    View.InitializeWordSelector(messages.Select(r => r.FirstLetter).Distinct().ToList(), startWith);

                messages = MailService.ParseObjectMessages(messages,View.CurrentSearchValue,startWith, View.MessageCurrentOrderBy, View.Ascending, ProfileService);


                PagerBase pager = new PagerBase();
                pager.PageSize = pageSize;//Me.View.CurrentPageSize
                pager.Count = (messages.Count > 0) ? messages.Count - 1 : 0;
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;

                messages = messages.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                View.LoadedNoUsers = !messages.Any();
                if (messages.Any())
                    View.LoadMessages(messages);
                else
                    View.DisplayNoMessagesFound();
            }
        }
        public void SelectMessage(long idRecipientMessage) {
            if (UserContext.isAnonymous)
                SessionTimeout(View.CurrentIdCommunity, View.PreloadSelectedTab);
            else
            {
                MailRecipient recipient = CurrentManager.Get<MailRecipient>(idRecipientMessage);
                lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation translation = new lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation();
                if (recipient!= null){
                    if (recipient.Item!=null){
                        translation.Body= recipient.Item.Body;
                        translation.Subject= recipient.Item.Subject;
                    }
                    View.DisplayMessagePreview(false,recipient.LanguageCode,translation,new List<String>(), recipient.Message.MailSettings,View.SelectedFilter.IdCommunity, View.CurrentModuleObject);
                }
                else{
                    switch(View.CurrentDisplayBy){
                        case DisplayItems.ByRecipient:
                            LoadRecipients(View.SelectedFilter,View.Pager.PageIndex, View.PageSize,false);
                            break;
                        case DisplayItems.ByMessage:
                            break;
                    }
                }
            }
        }
        private void LoadColumns(Boolean hasMessage, Boolean displayRole, Boolean displayProfileType, Boolean hasAgencies = true)
        {
            List<ColumnMessageGrid> columns = new List<ColumnMessageGrid>();
            if (hasMessage)
                columns.Add(ColumnMessageGrid.Actions);
            if (View.IsAgencyColumnVisible && hasAgencies)
                columns.Add(ColumnMessageGrid.AgencyName);
            //if (displayProfileType)
            //    columns.Add(ColumnMessageGrid.ProfileType);
            //if (displayRole)
            //    columns.Add(ColumnMessageGrid.Role);
            View.AvailableColumns = columns;
        }

        #region "Permissions"
        private Boolean HasPermission(dtoModuleMessagesContext context)
        {
            Int32 idUser = UserContext.CurrentUserID;
            Person p = CurrentManager.GetPerson(idUser);
            return View.HasModulePermissions(context.ModuleCode, GetModulePermissions(context.IdModule, context.IdCommunity), context.IdCommunity, (p == null) ? (int)UserTypeStandard.Guest : p.TypeID, context.ModuleObject);
        }
        private lm.Comol.Core.Mail.Messages.dtoModuleMessagesContext GetContext()
        {
            dtoModuleMessagesContext item = new dtoModuleMessagesContext();
            item.ModuleObject = View.PreloadModuleObject;
            if (item.ModuleObject != null && !String.IsNullOrEmpty(item.ModuleObject.ServiceCode) && item.ModuleObject.ServiceID < 1)
            {
                item.ModuleObject.ServiceID = CurrentManager.GetModuleID(item.ModuleObject.ServiceCode);
                View.CurrentModuleObject = item.ModuleObject;
            }
            else if (item.ModuleObject != null && String.IsNullOrEmpty(item.ModuleObject.ServiceCode) && item.ModuleObject.ServiceID > 0){
                item.ModuleObject.ServiceID = CurrentManager.GetModuleID(item.ModuleObject.ServiceCode);
                View.CurrentModuleObject = item.ModuleObject;
            }
            item.IdCommunity = View.PreloadIdCommunity;
            item.IdModule = View.PreloadIdModule;
            item.ModuleCode = View.PreloadModuleCode;

            if (item.IdCommunity == -1 && item.ModuleObject != null)
                item.IdCommunity = item.ModuleObject.CommunityID;
            if (item.IdModule > 0 && String.IsNullOrEmpty(item.ModuleCode))
                item.ModuleCode = CurrentManager.GetModuleCode(item.IdModule);
            else if (item.IdModule == 0 && !String.IsNullOrEmpty(item.ModuleCode))
                item.IdModule = CurrentManager.GetModuleID(item.ModuleCode);
            return item;
        }
        public long GetModulePermissions(Int32 idModule, Int32 idCommunity)
        {
            return CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule);
        }
        private Person GetCurrentUser(ref Int32 idUser)
        {
            Person person = null;
            if (UserContext.isAnonymous)
            {
                person = (from p in CurrentManager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();//CurrentManager.GetPerson(UserContext.CurrentUserID);
                idUser = (person != null) ? person.Id : UserContext.CurrentUserID; //if(Person!=null) { IdUser = PersonId} else {IdUser = UserContext...}
            }
            else
                person = CurrentManager.GetPerson(idUser);
            return person;
        }
    #endregion     
        private void SessionTimeout(Int32 idCommunity, DisplayTab current) {
            View.DisplaySessionTimeout(idCommunity,GetCurrentUrl(current));
        }
        private String GetCurrentUrl(DisplayTab current)
        {
            switch (current)
            {
                case DisplayTab.List:
                    return ExternalModuleRootObject.ListAvailableTemplates(View.PreloadModuleCode, View.PreloadTabs, View.PreloadSelectionMode, View.PreloadAllowSelectTemplate, View.GetEncodedBackUrl(View.PreloadBackUrl), View.PreloadIdAction, View.PreloadWithEmptyActions, View.PreloadIdTemplate, View.PreloadIdVersion, View.PreloadIdCommunity, View.PreloadIdModule, View.PreloadModuleObject);
                case DisplayTab.Send:
                    return ExternalModuleRootObject.SendMessage(View.PreloadModuleCode, View.PreloadTabs, View.PreloadSelectionMode, View.PreloadAllowSelectTemplate, View.GetEncodedBackUrl(View.PreloadBackUrl), View.PreloadIdAction, View.PreloadWithEmptyActions, View.PreloadIdTemplate, View.PreloadIdVersion, View.PreloadIdCommunity, View.PreloadIdModule, View.PreloadModuleObject);
                case  DisplayTab.Sent:
                    return ExternalModuleRootObject.MessagesSent(View.PreloadModuleCode, View.PreloadTabs, View.PreloadSelectionMode, View.PreloadAllowSelectTemplate, View.GetEncodedBackUrl(View.PreloadBackUrl), View.PreloadIdAction, View.PreloadWithEmptyActions, View.PreloadIdTemplate, View.PreloadIdVersion, View.PreloadIdCommunity, View.PreloadIdModule, View.PreloadModuleObject);
                default:
                    return "";
            }
        }
    }
}