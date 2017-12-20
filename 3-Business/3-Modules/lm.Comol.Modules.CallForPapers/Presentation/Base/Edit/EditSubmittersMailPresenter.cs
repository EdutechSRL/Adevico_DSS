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
    public class EditSubmittersMailPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditSubmittersMail View
            {
                get { return (IViewEditSubmittersMail)base.View; }
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
        public EditSubmittersMailPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditSubmittersMailPresenter(iApplicationContext oContext, IViewEditSubmittersMail view)
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
                View.InEditing = new List<long>();
                int idModule = (type == CallForPaperType.CallForBids) ? CallService.ServiceModuleID() : RequestService.ServiceModuleID();
                View.IdCallModule = idModule;
                if (call == null)
                    View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                else if (allowManage || allowSave)
                {
                    View.AllowSave = allowSave;
                    View.IdCall = idCall;
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps = CallService.GetAvailableSteps(idCall, WizardCallStep.SubmitterTemplateMail, type);
                    View.LoadWizardSteps(idCall, type, idCommunity, steps);

                    LoadTemplates(idCall);
                    if (type == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewSubmittersTemplate);
                    else
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.ViewSubmittersTemplate);
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
        private void LoadTemplates(long idCall) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<dtoSubmitterType> submitters = CallService.GetCallAvailableSubmittersType(idCall);
                View.LoadSubmitterTypes(submitters);
                List<dtoSubmitterTemplateMail> templates = CallService.GetSubmittersTemplateMail(idCall);
                if (templates.Count == 0)
                {
                    View.DisplayNoTemplates();
                    if (View.AllowSave)
                        View.AddNewTemplate(1);
                }
                else
                {
                    View.AllowAdd = View.AllowSave && submitters.Count > 1;
                    View.LoadTemplates(templates);
                }
            }
        }

        public void AddTemplate(){
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                View.AddNewTemplate(CallService.GetSubmittersTemplateCount(View.IdCall) + 1);
        }
        public void SaveSettings(List<dtoSubmitterTemplateMail> templates)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<long> editing = View.InEditing;
                if (!CallService.SaveSubmittersTemplate(View.IdCall, templates, editing))
                    View.DisplayError(EditorErrors.SavingTemplateSettings);
                else
                {
                    LoadTemplates(View.IdCall);
                    DisplayMessage(View.IdCall, false);
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.SaveSubmittersTemplate);
                    else
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.SaveSubmittersTemplate);
                }
                View.LoadWizardSteps(View.IdCall, View.CallType, View.IdCommunity, CallService.GetAvailableSteps(View.IdCall, WizardCallStep.SubmitterTemplateMail, View.CallType));
            }
        }
        public void SaveSettings(dtoSubmitterTemplateMail template, List<dtoSubmitterTemplateMail> templates)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<long> editing = View.InEditing;
                if (template != null && template.Id == 0)
                {
                    SubmitterTemplateMail sTemplate = CallService.AddSubmitterTemplate(View.IdCall, template);
                    if (sTemplate != null)
                    {
                        template.Id = sTemplate.Id;
                        templates.Add(template);
                        editing.Add(sTemplate.Id);
                    }
                }

                View.InEditing = editing;
                if (!CallService.SaveSubmittersTemplate(View.IdCall, templates, editing))
                {
                    View.LoadTemplates(templates);
                    View.DisplayError(EditorErrors.SavingTemplateSettings);
                }
                else
                {
                    LoadTemplates(View.IdCall);
                    DisplayMessage(View.IdCall, false);
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.SaveSubmittersTemplate);
                    else
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.SaveSubmittersTemplate);
                }
                View.LoadWizardSteps(View.IdCall, View.CallType, View.IdCommunity, CallService.GetAvailableSteps(View.IdCall, WizardCallStep.SubmitterTemplateMail, View.CallType));
            }
        }
        public void SaveTemplate(dtoSubmitterTemplateMail template, List<dtoSubmitterTemplateMail> templates)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<long> editing = View.InEditing;
                if (template != null && template.Id == 0)
                {
                    SubmitterTemplateMail sTemplate = CallService.AddSubmitterTemplate(View.IdCall, template);
                    if (sTemplate != null)
                    {
                        template.Id = sTemplate.Id;
                        templates.Add(template);
                        editing.Add(sTemplate.Id);
                    }
                }

                if (!CallService.SaveSubmitterTemplate(template))
                {
                    View.InEditing = editing;
                    View.LoadTemplates(templates);
                    View.DisplayError(EditorErrors.SavingTemplateSettings);
                }
                else
                {
                    editing.Remove(template.Id);
                    View.InEditing = editing;
                    View.LoadTemplates(templates);
                    DisplayMessage(View.IdCall, false);
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.SaveSubmittersTemplate);
                    else
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.SaveSubmittersTemplate);
                }
                View.LoadWizardSteps(View.IdCall, View.CallType, View.IdCommunity, CallService.GetAvailableSteps(View.IdCall, WizardCallStep.SubmitterTemplateMail, View.CallType));
            }
        }
        public void RemoveTemplate(long idTemplate, List<dtoSubmitterTemplateMail> templates)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<long> editing = View.InEditing;
                try
                {
                    if (!CallService.VirtualDeleteSubmitterTemplate(idTemplate, true))
                        View.DisplayError(EditorErrors.RemovingTemplate);
                    else
                    {
                        View.DisplayTemplateRemoved();
                        editing.Remove(idTemplate);
                        templates = templates.Where(t => t.Id != idTemplate).ToList();
                        if (View.CallType == CallForPaperType.CallForBids)
                            View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.VirtualDeleteSubmitterTemplate);
                        else
                            View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.VirtualDeleteSubmitterTemplate);
                        View.LoadWizardSteps(View.IdCall, View.CallType, View.IdCommunity, CallService.GetAvailableSteps(View.IdCall, WizardCallStep.SubmitterTemplateMail, View.CallType));
                    }
                }
                catch (SubmissionLinked ex)
                {
                    View.DisplayError(EditorErrors.RemovingTemplate);
                }
                View.InEditing = editing;
                View.LoadTemplates(templates);
            }
        }
        public Boolean AddTemplate(dtoSubmitterTemplateMail template, List<dtoSubmitterTemplateMail> templates)
        {
            Boolean result = false;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                SubmitterTemplateMail sTemplate = CallService.AddSubmitterTemplate(View.IdCall, template);
                if (sTemplate != null)
                {
                    View.AllowAdd = View.AllowSave && View.Availablesubmitters.Count > 1;
                    template.Id = sTemplate.Id;
                    templates.Add(template);
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.AddSubmitterTemplate);
                    else
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.AddSubmitterTemplate);
                    DisplayMessage(View.IdCall, true);
                    View.LoadWizardSteps(View.IdCall, View.CallType, View.IdCommunity, CallService.GetAvailableSteps(View.IdCall, WizardCallStep.SubmitterTemplateMail, View.CallType));
                }
                View.LoadTemplates(templates);
            }
            return result;
        }

        public void StartEditingTemplate(long idTemplate, List<dtoSubmitterTemplateMail> templates)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<long> editing = View.InEditing;
                if (!editing.Contains(idTemplate))
                {
                    dtoSubmitterTemplateMail saved = CallService.GetSubmitterTemplateMail(idTemplate);
                    editing.Add(idTemplate);
                    View.InEditing = editing;
                    if (saved != null)
                        templates.Where(t => t.Id == idTemplate).ToList().ForEach(t => t.UpdateContent(saved));
                    else
                        View.DisplayError(EditorErrors.EditingTemplate);
                }
                View.LoadTemplates(templates);
            }
        }
        public void UndoEditingTemplate(long idTemplate, List<dtoSubmitterTemplateMail> templates)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<long> editing = View.InEditing;
                dtoSubmitterTemplateMail dto = templates.Where(t => t.Id == idTemplate).FirstOrDefault();

                if (dto != null)
                {
                    dtoSubmitterTemplateMail saved = CallService.GetSubmitterTemplateMail(idTemplate);
                    if (saved != null)
                    {
                        dto.MailSettings = saved.MailSettings;
                        dto.Subject = saved.Subject;
                        dto.Body = saved.Body;
                    }
                }
                editing.Remove(idTemplate);
                View.InEditing = editing;

                View.LoadTemplates(templates);
            }
        }

        public void PreviewMessage(dtoSubmitterTemplateMail template, String fakeName, String fakeSurname, String fakeTaxCode, String fakeMail, lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, String websiteUrl, Dictionary<SubmissionTranslations, string> translations)
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
                lm.Comol.Core.Mail.dtoMailMessage message = CallService.GetMailPreview(View.IdCall, template, currentUser, smtpConfig,websiteUrl, translations);
                View.DisplayMessagePreview(new lm.Comol.Core.Mail.dtoMailMessagePreview(currentUser.LanguageID,dLanguage, message, template.MailSettings, smtpConfig), currentUser.Mail);
            }
        }
        private void DisplayMessage(long idCall,Boolean forAdd)
        {
            Boolean missing = !(CallService.GetTemplateStatus(CallService.GetCall(idCall), WizardCallStep.SubmitterTemplateMail) == Core.Wizard.WizardItemStatus.valid); 
            if (forAdd)
                View.DisplayTemplateAdded(missing);
            else
                View.DisplaySettingsSaved(missing);
        }
    }
}