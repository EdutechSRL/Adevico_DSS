using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Mail.Messages;
using lm.Comol.Core.BaseModules.MailSender.Domain;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public class UserMessagesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private MailMessagesService service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewUserMessages View
            {
                get { return (IViewUserMessages)base.View; }
            }
            private MailMessagesService Service
            {
                get
                {
                    if (service == null)
                        service = new MailMessagesService(AppContext);
                    return service;
                }
            }
            public UserMessagesPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public UserMessagesPresenter(iApplicationContext oContext, IViewUserMessages view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            dtoModuleMessagesContext context = GetContext();
            View.ContainerContext = context;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (HasPermission(context))
                {
                    lm.Comol.Core.Mail.dtoRecipient recipient = new lm.Comol.Core.Mail.dtoRecipient();
                    if (context.IdPerson>0){
                        Person p = CurrentManager.GetPerson(context.IdPerson);
                        recipient.DisplayName= (p==null) ? View.UnknownUserName: p.SurnameAndName;
                        recipient.MailAddress= (p==null) ? "" : p.Mail;
                    }
                    else if (context.IdUserModule>0){
                        recipient = View.GetRecipient(context.ModuleCode, context.IdUserModule);
                        if (recipient==null)
                            recipient = new lm.Comol.Core.Mail.dtoRecipient() { DisplayName=View.RemovedUserName};
                    }
                    else if (!String.IsNullOrEmpty(context.MailAddress))
                    {
                        MailRecipient r = Service.GetRecipient(context.MailAddress, context);
                        if (r == null)
                            recipient = new lm.Comol.Core.Mail.dtoRecipient() { DisplayName = View.UnknownUserName };
                        else
                            recipient = new lm.Comol.Core.Mail.dtoRecipient() { DisplayName = r.MailAddress };
                    }
                    View.LoadRecipientName(recipient);
                    LoadUserMessages(context, 0, View.CurrentPageSize);
                }
                else
                    View.DisplayNoPermission(context.IdCommunity, context.IdModule, context.ModuleCode);
            }
        }

        private dtoModuleMessagesContext GetContext()
        {
            dtoModuleMessagesContext item = new dtoModuleMessagesContext();
            item.ModuleObject = View.PreloadModuleObject;
            item.IdCommunity = View.PreloadIdCommunity;
            item.IdModule = View.PreloadIdModule;
            item.IdPerson = View.PreloadIdPerson;
            item.IdUserModule = View.PreloadIdUserModule;
            item.MailAddress = View.PreloadMail;
            item.ModuleCode = View.PreloadModuleCode;

            if (item.IdCommunity == -1 && item.ModuleObject != null)
                item.IdCommunity = item.ModuleObject.CommunityID;
            if (item.IdModule > 0 && String.IsNullOrEmpty(item.ModuleCode))
                item.ModuleCode = CurrentManager.GetModuleCode(item.IdModule);
            else if (item.IdModule == 0 && !String.IsNullOrEmpty(item.ModuleCode))
                item.IdModule = CurrentManager.GetModuleID(item.ModuleCode);
            return item;
        }
        private Boolean HasPermission(dtoModuleMessagesContext context) {
            Int32 idUser = UserContext.CurrentUserID;
            if (idUser == context.IdPerson)
                return true;
            else{
                Person p = CurrentManager.GetPerson(idUser);
                return View.HasModulePermissions(context.ModuleCode, GetModulePermissions(context.IdModule, context.IdCommunity), context.IdCommunity, (p == null) ? (int)UserTypeStandard.Guest : p.TypeID, context.ModuleObject);
            }
        }
        public void LoadUserMessages(dtoModuleMessagesContext context,  int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                Int32 count = Service.GetUserMessagesCount(context);
                PagerBase pager = new PagerBase();
                pager.PageSize = pageSize;

                if (pageSize == 0)
                    pageSize = 50;
                pager.Count = count - 1;
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;

                View.CurrentPageSize = pageSize;
                List<dtoDisplayUserMessage> items = Service.GetUserMessages(context,View.RemovedUserName, View.UnknownUserName, pageIndex,pageSize);
                if (items == null || !items.Any() || items.Count <= 0)
                    View.DisplayNoMessagesFound();
                else
                    View.LoadItems(items);
            }
        }
        public void LoadMessage(long idMessage) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else{
                dtoModuleMessagesContext context = View.ContainerContext;
                MessageTranslation tMessage = Service.GetMessageTranslation(idMessage);
                if (tMessage == null)
                    LoadUserMessages(View.ContainerContext, View.Pager.PageIndex, View.CurrentPageSize);
                else
                    View.DisplayMessagePreview(tMessage.LanguageCode, new DomainModel.Languages.ItemObjectTranslation() { Body = tMessage.Body, Subject = tMessage.Subject, IsHtml = tMessage.Message.MailSettings.IsBodyHtml }, tMessage.Message.MailSettings, context.IdCommunity, context.ModuleObject);
            }
        }

        private long GetModulePermissions(Int32 idModule, Int32 idCommunity)
        {
            return CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule);
        }
    }
}