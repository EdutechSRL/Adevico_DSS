using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class EditNotificationMailPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditNotificationMail View
            {
                get { return (IViewEditNotificationMail)base.View; }
            }
            private ServiceCallOfPapers _ServiceCall;
            private ServiceCallOfPapers CallService
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
                }
            }
            private ServiceRequestForMembership _ServiceRequest;
            private ServiceRequestForMembership RequestService
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
        public EditNotificationMailPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditNotificationMailPresenter(iApplicationContext oContext, IViewEditNotificationMail view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView()
        {
            long idCall = View.PreloadIdCall;

            dtoBaseForPaper call = null;
            CallForPaperType type = CallService.GetCallType(idCall);
            if (type == CallForPaperType.None)
                type = View.PreloadType;
            call = CallService.GetDtoBaseCall(idCall);

            View.CallType = type;
            int idCommunity = SetCallCurrentCommunity(call);
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                litePerson currenUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                Boolean allowManage = false;
                Boolean allowView = false;
                Boolean allowSave = false;
                ModuleCallForPaper module = null;
                ModuleRequestForMembership moduleR = null;
                switch (type)
                {
                    case CallForPaperType.CallForBids:
                        module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowView = (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers);
                        allowManage = module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper;
                        allowSave = (module.Administration || module.ManageCallForPapers || (module.CreateCallForPaper && idCall == 0) || (call != null && module.EditCallForPaper && currenUser == call.Owner));
                        break;
                    case CallForPaperType.RequestForMembership:
                        moduleR = CallService.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowView = (moduleR.ViewBaseForPapers || moduleR.Administration || moduleR.ManageBaseForPapers);
                        allowManage = moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper;
                        allowSave = (moduleR.Administration || moduleR.ManageBaseForPapers || (moduleR.CreateBaseForPaper && idCall == 0) || (call != null && moduleR.EditBaseForPaper && currenUser == call.Owner));
                        break;
                    default:
                        break;
                }
                int idModule = (type == CallForPaperType.CallForBids) ? CallService.ServiceModuleID() : RequestService.ServiceModuleID();
                View.IdCallModule = idModule;
                if (call == null)
                    View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                else if (allowManage || allowSave)
                {
                    View.AllowSave = allowSave;
                    View.IdCall = idCall;
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));

                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps =CallService.GetAvailableSteps(idCall, WizardCallStep.NotificationTemplateMail, type);
                    View.LoadWizardSteps(idCall, type, idCommunity, steps);

                    LoadTemplate(idCall);
                    if (type == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.EditManagerTemplate);
                    else
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.EditManagerTemplate);
                    if (steps.Where(s => s.Id == WizardCallStep.SubmittersType && (s.Status == Core.Wizard.WizardItemStatus.valid || s.Status == Core.Wizard.WizardItemStatus.warning)).Any())
                        View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(type, idCall, idCommunity, View.PreloadView));
                }
                else
                    View.DisplayNoPermission(idCommunity, idModule);
            }
        }

        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            dtoCallCommunityContext context = CallService.GetCallCommunityContext(call, View.Portalname);
            View.SetContainerName(CallStandardAction.Edit, context.CommunityName, context.CallName);
            View.IdCommunity = context.IdCommunity;
            return context.IdCommunity;
            //int idCommunity = 0;
            //Boolean forPortal = (call != null) ? call.IsPortal : false;
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //if (call != null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


            //community = CurrentManager.GetCommunity(idCommunity);
            //if (community != null)
            //    View.SetContainerName(CallStandardAction.Edit, community.Name, (call != null) ? call.Name : "");
            //else if (currentCommunity != null && !forPortal)
            //{
            //    idCommunity = this.UserContext.CurrentCommunityID;
            //    View.SetContainerName(CallStandardAction.Edit, currentCommunity.Name, (call != null) ? call.Name : "");
            //}
            //else
            //{
            //    idCommunity = 0;
            //    View.SetContainerName(CallStandardAction.Edit, View.Portalname, (call != null) ? call.Name : "");
            //}
            //View.IdCommunity = idCommunity;
            //return idCommunity;
        }
        private void LoadTemplate(long idCall)
        {
            BaseForPaper call = CallService.GetCall(idCall);
            if (call == null)
                View.LoadUnknowCall(View.IdCommunity, View.IdCallModule, idCall, View.CallType);
            else { 
                dtoManagerTemplateMail template = CallService.GetManagerTemplateMail(idCall);
                if (template == null)
                {
                    View.DisplayNoTemplate();
                    template = View.GetDefaultTemplate;
                    litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);

                    template.NotifyTo = (call.CreatedBy!=null) ? call.CreatedBy.Mail : "";

                    if (person!= null && call.CreatedBy !=null && person.Id != call.CreatedBy.Id)
                        template.NotifyTo = (string.IsNullOrEmpty(template.NotifyTo)) ? "" : ";" + person.Mail;
                }
                View.LoadTemplate(template);
            }
        }
        public void SaveSettings(dtoManagerTemplateMail template)
        {
            ManagerTemplateMail sTemplate = CallService.SaveManagerTemplate(View.IdCall, template);
            if (sTemplate == null)
                View.DisplayErrorSaving();
            else
            {
                View.IdTemplate = sTemplate.Id;
                View.DisplaySettingsSaved();
                if (View.CallType == CallForPaperType.CallForBids)
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.SaveManagerTemplate);
                else
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.SaveManagerTemplate);
                View.LoadWizardSteps(View.IdCall, View.CallType, View.IdCommunity, CallService.GetAvailableSteps(View.IdCall, WizardCallStep.NotificationTemplateMail, View.CallType));
            }
        }

        public void PreviewMessage(dtoManagerTemplateMail template, String fakeName, String fakeSurname, String fakeTaxCode, String fakeMail, lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, String websiteUrl, Dictionary<SubmissionTranslations, string> translations)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Language dLanguage = CurrentManager.GetDefaultLanguage();
                litePerson currentUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                litePerson fakeUser = new litePerson();
                fakeUser.TypeID = (int)UserTypeStandard.Student;
                fakeUser.Name = fakeName;
                fakeUser.Surname = fakeSurname;
                fakeUser.Mail = fakeMail;
                fakeUser.TaxCode = fakeTaxCode;
                fakeUser.LanguageID = currentUser.LanguageID;
                lm.Comol.Core.Mail.dtoMailMessage message = CallService.GetMailPreview(View.IdCall, template, currentUser, smtpConfig, websiteUrl, translations);
                String recipients = template.NotifyTo;
                if (String.IsNullOrEmpty(recipients))
                    recipients = currentUser.Mail;
                else if (recipients.Contains(","))
                    recipients = recipients.Replace(",", ";");
                View.DisplayMessagePreview(new lm.Comol.Core.Mail.dtoMailMessagePreview(currentUser.LanguageID,dLanguage, message, template.MailSettings, smtpConfig), recipients);
            }
        }
    }
}