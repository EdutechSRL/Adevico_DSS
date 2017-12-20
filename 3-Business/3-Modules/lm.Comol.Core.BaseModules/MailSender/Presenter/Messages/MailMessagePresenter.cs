using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Mail;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.MailCommons.Domain.Configurations;
using lm.Comol.Core.MailCommons.Domain.Messages;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public class MailMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService _ProfileService;

            protected virtual IViewMailMessage View
            {
                get { return (IViewMailMessage)base.View; }
            }
            private ProfileManagementService ProfileService
            {
                get
                {
                    if (_ProfileService == null)
                        _ProfileService = new ProfileManagementService(AppContext);
                    return _ProfileService;
                }
            }

            public MailMessagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MailMessagePresenter(iApplicationContext oContext, IViewMailMessage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(lm.Comol.Core.Mail.dtoMailMessagePreview item, String recipients, List<dtoBaseGenericFile> attachments = null) {
            View.DisplayOtherRecipients = false;
            InitView(PreviewMode.MailToSend, item, recipients, attachments);
        }
        public void InitView(PreviewMode pMode, dtoMailMessagePreview dtoContent, String recipients = "", List<dtoBaseGenericFile> attachments = null)
        {
            View.DisplayOptions = (pMode== PreviewMode.MailSent);
            Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (!UserContext.isAnonymous && person!=null)
            {
                View.isInitialized = true;
                LoadDisplayItems(pMode, attachments!=null && attachments.Any());
                
                switch (pMode) { 
                    case PreviewMode.MailToSend:
                        if (String.IsNullOrEmpty("recipients"))
                            recipients =person.Mail;
                        View.AllowSendMail = View.AllowSendMail && (View.EditAddressTo || !String.IsNullOrEmpty(recipients));
                        View.LoadPreviewMessage(pMode, dtoContent, recipients, attachments);
                        break;
                }
                
                //View.AllowSendMail = (pMode == PreviewMode.MailToSend && (View.EditAddressTo || !String.IsNullOrEmpty(addressTo)));
                //View.LoadMail(dtoContent.Settings.
            }
            else
                View.HideContent();
        }
        public void InitView(String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, SmtpServiceConfig smtpConfig, String recipients = "", List<String> modules = null, List<lm.Comol.Core.TemplateMessages.Domain.ChannelSettings> notifications = null, Int32 idCommunity = -1, ModuleObject obj = null)
        {
            View.DisplayOtherRecipients = false;
            InitView(PreviewMode.MailToSend, languageCode,content, smtpConfig, recipients, modules, notifications, idCommunity, obj);
        }

        public void InitInternalView(String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, SmtpServiceConfig smtpConfig, String recipients = "", List<String> modules = null, MessageSettings settings = null, Int32 idCommunity = -1, ModuleObject obj = null)
        {
            View.DisplayOtherRecipients = false;
            InitView(PreviewMode.MailToSend, languageCode, content, smtpConfig, recipients, modules, settings, idCommunity, obj);
        }
        public void InitView(PreviewMode pMode, String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, SmtpServiceConfig smtpConfig, String recipients = "", List<String> modules = null, List<lm.Comol.Core.TemplateMessages.Domain.ChannelSettings> notifications = null, Int32 idCommunity = -1, ModuleObject obj = null)
        {
            InitView(pMode, languageCode, content, smtpConfig, recipients, modules, (notifications != null && notifications.Where(n => n.Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail).Any()) ? notifications.Where(n => n.Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail).Select(n => n.MailSettings).FirstOrDefault() : null, idCommunity, obj);
        }
        public void InitView(PreviewMode pMode, String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, SmtpServiceConfig smtpConfig, String recipients = "", List<String> modules = null, MessageSettings settings = null, Int32 idCommunity = -1, ModuleObject obj = null)
        {
            View.DisplayOptions = (pMode == PreviewMode.MailSent);
            Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (!UserContext.isAnonymous && person != null)
            {
                View.isInitialized = true;
                switch (pMode)
                {
                    case PreviewMode.TemplateDisplay:
                    case PreviewMode.MailToSend:
                        if (String.IsNullOrEmpty("recipients"))
                            recipients = person.Mail;
                        View.AllowSendMail = settings != null && View.AllowSendMail && (View.EditAddressTo || !String.IsNullOrEmpty(recipients));
                        Language dLanguage = CurrentManager.GetDefaultLanguage();
                        Language language = CurrentManager.GetLanguageByCodeOrDefault(languageCode, true);
                        if (modules != null)
                        {
                            String organizationName = "";
                            Community community = null;
                            if (idCommunity>0){
                                community = CurrentManager.GetCommunity(idCommunity);
                                if (community!=null){
                                    organizationName= CurrentManager.GetOrganizationName(community.Id);
                                }
                            }
                            if (community!=null && idCommunity>0)
                                content = View.ParseContent((language == null) ? 0 : language.Id, languageCode, content, modules,  community,person, organizationName, obj);
                            else
                                content = View.ParseContent((language == null) ? 0 : language.Id, languageCode, content, modules, idCommunity, (community == null || idCommunity < 1) ? View.GetPortalName((language != null) ? language.Id : 0) : community.Name, person, organizationName, obj);
                        }
                        if (settings != null)
                        {
                            dtoMailMessagePreview dtoContent = new dtoMailMessagePreview((language!=null) ? language.Id : 0, dLanguage, new dtoMailMessage() { UserSubject = content.Subject, Body = content.Body }, settings, smtpConfig);
                            View.LoadPreviewTemplateMessage(pMode, dtoContent, recipients);
                        }
                        else
                        {
                            View.LoadPreviewTemplateMessage(pMode, content);
                        }
                        break;
                }
            }
            else
                View.HideContent();
        }

        private void LoadDisplayItems(PreviewMode pMode,Boolean hasAttachments) {
            List<DisplayItem> items = new List<DisplayItem>();
            List<DisplayItem> onItems = new List<DisplayItem>();
            switch (pMode) { 
                case PreviewMode.MailReceived:
                    break;
                case PreviewMode.MailSent:
                    items.Add(DisplayItem.Options);
                    items.Add(DisplayItem.Recipients);
                    onItems.Add(DisplayItem.Recipients);
                    break;
                case  PreviewMode.MailToSend:
                    break;
            }
            if (hasAttachments)
                items.Add(DisplayItem.Attachments);
            View.LoadDisplayItems(items, pMode != PreviewMode.MailToSend, onItems);
        }

        public Boolean SendMail(String senderName, String senderMail, String subject, String body, String recipients, MessageSettings settings, SmtpServiceConfig smtpConfig)
        {
            Boolean result = false;
            try
            {
                //List<System.Net.Mail.MailAddress> rItems = new List<System.Net.Mail.MailAddress>();
                //if (!String.IsNullOrEmpty(recipients)){
                //    rItems.AddRange(recipients.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList().Select(m => new System.Net.Mail.MailAddress(m)).ToList());
                //}
                //string separator,
                Language dLanguage = CurrentManager.GetDefaultLanguage();
                lm.Comol.Core.Mail.MailService mailService = new lm.Comol.Core.Mail.MailService(smtpConfig, settings);
                lm.Comol.Core.Mail.dtoMailMessage message = new lm.Comol.Core.Mail.dtoMailMessage(subject, body);
                message.FromUser = new System.Net.Mail.MailAddress(senderMail, senderMail);
                mailService.SendMail(UserContext.Language.Id, dLanguage, message, recipients, MailCommons.Domain.RecipientType.BCC);

                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
