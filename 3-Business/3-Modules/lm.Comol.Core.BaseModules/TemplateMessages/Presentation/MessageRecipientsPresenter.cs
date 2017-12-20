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
    public class MessageRecipientsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService profileService;
            private lm.Comol.Core.BaseModules.TemplateMessages.Business.ModuleTemplateMessageService mailService;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewMessageRecipients View
            {
                get { return (IViewMessageRecipients)base.View; }
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
            public MessageRecipientsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MessageRecipientsPresenter(iApplicationContext oContext, IViewMessageRecipients view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            long idMessage = View.PreloadIdMessage;
            dtoModuleMessagesContext context = GetContext();
            View.ContainerContext = context;
            if (UserContext.isAnonymous)
                SessionTimeout();
            else
            {
                Int32 idCommunity = View.PreloadIdCommunity;
                if (idCommunity < 0)
                    idCommunity = UserContext.CurrentCommunityID;
                SetCurrentItems(idMessage,idCommunity);
               
                Int32 idOtherModule = CurrentManager.GetModuleID(context.ModuleCode);
                Boolean otherModule = (context.ModuleCode != ModuleTemplateMessages.UniqueCode);

                if (HasPermission(context)) {
                    if (MailService.IsMessageOf(idMessage, context.ModuleObject))
                    {
                        View.DisplayMessageInfo(MailService.GetMessage(idMessage));
                        InitializeByRecipients(idMessage, context);
                    }
                    else
                        View.DisplayUnknownMessage();
                }
                else
                    View.DisplayNoPermission(idCommunity, idOtherModule, context.ModuleCode);
            }
        }

#region "Initializers"
    #region "Base"
        private void SetCurrentItems(long idMessage, Int32 idCommunity)
        {
            View.CurrentModuleCode = View.PreloadModuleCode;
            View.CurrentIdCommunity = idCommunity;
            View.CurrentIdModule = View.PreloadIdModule;
            View.CurrentModuleObject = View.PreloadModuleObject;
            System.Guid sessionId = Guid.NewGuid();
            View.CurrentSessionId = sessionId;
            View.CurrentIdMessage = idMessage;
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
            View.CurrentStartWith = "";
            View.CurrentSearchValue = "";
            InitializeByRecipients(View.CurrentIdMessage,View.ContainerContext);
        }
        private void InitializeByRecipients(long idMessage,dtoModuleMessagesContext context)
        {
            View.UserCurrentOrderBy = MailSender.UserByMessagesOrder.ByUser;
            View.Ascending = true;
            lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter = InitializeFilter(idMessage,context.ModuleObject);
            View.CurrentFilter = filter;
            //View.InitializeWordSelector(new List<String>());

            LoadRecipients(idMessage,filter,0,View.PageSize,true);
            //View.DisplayNoUsersFound();
        }
        private lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters InitializeFilter(long idMessage, ModuleObject obj)
        {
            lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter = new lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters() { Ascending = true, OrderBy =  MailSender.UserByMessagesOrder.ByUser };
            filter.IdCommunity = obj.CommunityID;
            InitializeAgencyFilter(filter, idMessage,obj);
            InitializeSearchFilter(filter);
            InitializeCommunityFilters(idMessage,obj, filter.IdCommunity, filter);

            return filter;
        }
        private void InitializeAgencyFilter(lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter, long idMessage, ModuleObject obj, long idDefaultAgency = 0)
        {
            Boolean hasAgencies = false;
            Dictionary<long, String> agencies = null;
            hasAgencies = MailService.HasUsersWithProfileType(obj, (int)UserTypeStandard.Employee,idMessage);
            if (hasAgencies)
                agencies = MailService.GetAgenciesForUsers(obj,idMessage);

            View.IsAgencyColumnVisible = (hasAgencies && agencies != null && agencies.Count > 0);
            if (hasAgencies && agencies != null && agencies.Count > 0)
                View.LoadAgencies(agencies, idDefaultAgency);
            else
                View.UnLoadAgencies();
        }
        private void InitializeCommunityFilters(long idMessage,ModuleObject obj, Int32 idCommunity, lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter)
        {
            if (idCommunity>0)
            {
                filter.IdCommunity = idCommunity;
                filter.IdRole = -1;
                filter.IdProfileType = -1;
                View.LoadAvailableRoles(MailService.GetAvailableSubscriptionsIdRoles(obj, idCommunity, false, idMessage), -1);
            }
            else
            {
                filter.IdRole = -1;
                filter.IdCommunity = idCommunity;
                View.LoadAvailableProfileType(MailService.GetAvailableProfileTypes(obj, idMessage), -1);
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
#endregion
        public void LoadRecipients(long idMessage,lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter, Int32 pageIndex, Int32 pageSize, Boolean initialize)
        {
            if (UserContext.isAnonymous)
                SessionTimeout();
            else
            {
                filter.StartWith = View.CurrentStartWith;
                List<dtoModuleRecipientMessage> recipients = MailService.GetRecipientsForMessage(idMessage, View.UnknownUserTranslation, View.AnonymousUserTranslation, View.CurrentModuleObject, filter, ProfileService, false);
                if (initialize)
                    View.InitializeWordSelector(recipients.Select(r => r.FirstLetter).Distinct().ToList());
                else
                    View.InitializeWordSelector(recipients.Select(r => r.FirstLetter).Distinct().ToList(), filter.StartWith);

                recipients = MailService.GetParsedUsersForMessage(recipients, filter, ProfileService);


                PagerBase pager = new PagerBase();
                pager.PageSize = pageSize;//Me.View.CurrentPageSize
                pager.Count = (recipients.Count > 0) ? recipients.Count - 1 : 0;
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;

                recipients = recipients.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                LoadColumns(filter.IdCommunity > 0, filter.IdCommunity <= 0, recipients.Where(r => r.IdAgency > 0).Any());
                View.LoadedNoUsers = !recipients.Any();
                if (recipients.Any())
                    View.LoadRecipients(recipients);
                else
                    View.DisplayNoUsersFound();
            }
        }
        public void SelectMessage(long idRecipientMessage) {
            if (UserContext.isAnonymous)
                SessionTimeout();
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
                else
                    LoadRecipients(View.CurrentIdMessage, View.SelectedFilter,View.Pager.PageIndex, View.PageSize,false);
            }
        }
        private void LoadColumns(Boolean displayRole, Boolean displayProfileType, Boolean hasAgencies = true)
        {
            List<ColumnMessageGrid> columns = new List<ColumnMessageGrid>();
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
        public void SessionTimeout() {
            View.DisplaySessionTimeout(lm.Comol.Core.TemplateMessages.ExternalModuleRootObject.MessageRecipients(View.PreloadIdMessage, View.PreloadModuleCode, View.PreloadIdCommunity, View.PreloadIdModule,View.PreloadModuleObject));
        }
    }
}