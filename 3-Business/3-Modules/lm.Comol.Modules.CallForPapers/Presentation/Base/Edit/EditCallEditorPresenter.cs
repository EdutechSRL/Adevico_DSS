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
    public class EditCallEditorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCallEditor View
            {
                get { return (IViewCallEditor)base.View; }
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
        public EditCallEditorPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditCallEditorPresenter(iApplicationContext oContext, IViewCallEditor view)
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
                    View.AllowSave = allowSave && (!CallService.CallHasSubmissions(idCall));
                    View.AllowUpdateTags = !View.AllowSave;


                    View.IdCall = idCall;
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));

                    View.TagCurrent = call.Tags;

                    LoadSections(idCall);
                    if (type == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.LoadCallEditor);
                    else
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.LoadCallEditor);
                 
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
        private void LoadSections(long idCall)
        {
            BaseForPaper call = CallService.GetCall(idCall);
            if (call != null)
                LoadSections(call);
        }
        private void LoadSections(BaseForPaper call)
        {
            View.LoadSubmitterTypes(CallService.GetCallAvailableSubmittersType(call));
            List<dtoCallSection<dtoCallField>> sections = CallService.GetEditorSections(call);
            List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps = RequestService.GetAvailableSteps(call.Id, WizardCallStep.SubmissionEditor, call.Type);
            //View.InitializeAddFieldControl(call.Id);
            if (sections == null || sections.Count == 0)
            {
                if (!CallService.isNewCall(call))
                    View.DisplayError(EditorErrors.NoSections);
                else
                {
                    FieldsSection section = CallService.AddSection(call, View.DefaultSectionName, View.DefaultSectionDescription);
                    if (section != null)
                    {
                        sections = new List<dtoCallSection<dtoCallField>>();
                        sections.Add(new dtoCallSection<dtoCallField>() { Id = section.Id, Description = section.Description, DisplayOrder = section.DisplayOrder, Name = section.Name });
                        View.LoadSections(sections);
                    }
                }
                View.LoadWizardSteps(call.Id, call.Type, View.IdCommunity, steps);
            }
            else
            {
                View.LoadWizardSteps(call.Id, call.Type, View.IdCommunity, steps);
                View.LoadSections(sections);
            }

               
            if (steps.Where(s => s.Id == WizardCallStep.SubmittersType && (s.Status == Core.Wizard.WizardItemStatus.valid || s.Status == Core.Wizard.WizardItemStatus.warning)).Any())
                View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(call.Type, call.Id, View.IdCommunity, View.PreloadView));
        }
      
        public void SaveSettings(List<dtoCallSection <dtoCallField>> sections)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                if (!CallService.SaveSections(idCall, sections, View.TagCurrent))
                    View.DisplayError(EditorErrors.Saving);
                else
                {


                    LoadSections(View.IdCall);
                    View.DisplaySettingsSaved();
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveCallSections);
                    else
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.SaveCallSections);
                }
            }
        }

        public void AddSection(List<dtoCallSection<dtoCallField>> sections, String name, String description)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                FieldsSection section = CallService.AddSectionToCall(idCall, sections, name, description);
                if (section == null)
                    View.DisplayError(EditorErrors.AddingSection);
                else
                {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddSectionToCall);
                    else
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.AddSectionToRequest);
                    View.ReloadEditor(RootObject.CallSubmissionEditorSectionAdded(section.Id, type, idCall, idCommunity, View.PreloadView));
                }
            }
        }
        public void CloneSection(List<dtoCallSection<dtoCallField>> sections, long idSection)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                FieldsSection section = CallService.CloneSection(idCall, sections, idSection);
                if (section == null)
                    View.DisplayError(EditorErrors.CloningSection);
                else
                {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddSectionToCall);
                    else
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.AddSectionToRequest);
                    View.ReloadEditor(RootObject.CallSubmissionEditorSectionAdded(section.Id, type, idCall, idCommunity, View.PreloadView));
                }
            }
        }
        public void CloneField(List<dtoCallSection<dtoCallField>> sections, long idField)
        {
            long pIdSection = 0;
            long pIdField = 0;
            CallForPaperType type = View.CallType;
            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;

            try
            {
                CallService.SaveSections(idCall, sections, View.TagCurrent);
                if (CallService.CloneField(idField, ref pIdSection, ref pIdField))
                {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCallField);
                    else
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.VirtualDeleteRequestField);
                    if (pIdField != 0)
                        View.ReloadEditor(RootObject.CallSubmissionEditorFieldRemoved(pIdField, type, idCall, idCommunity, View.PreloadView));
                    else
                        View.ReloadEditor(RootObject.CallSubmissionEditorSectionRemoved(pIdSection, type, idCall, idCommunity, View.PreloadView));
                }
                else
                    View.DisplayError(EditorErrors.CloningField);
            }
            catch (SubmissionLinked exSubmission)
            {
                View.DisplayError(EditorErrors.CloningField);
            }
            catch (Exception ex)
            {
                View.DisplayError(EditorErrors.CloningField);
            }
        }
        public void RemoveSection(List<dtoCallSection<dtoCallField>> sections, long idSection)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                long pIdSection = 0;
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;

                try
                {
                    CallService.SaveSections(idCall, sections, View.TagCurrent);
                    if (CallService.VirtualDeleteCallSection(idSection, true, ref pIdSection))
                    {
                        if (View.CallType == CallForPaperType.CallForBids)
                            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCallSection);
                        else
                            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.VirtualDeleteRequestSection);
                        View.ReloadEditor(RootObject.CallSubmissionEditorSectionRemoved(pIdSection, type, idCall, idCommunity, View.PreloadView));
                    }
                    else
                        View.DisplayError(EditorErrors.RemovingSection);
                }
                catch (SubmissionLinked exSubmission)
                {
                    View.DisplayError(EditorErrors.RemovingSection);
                }
                catch (Exception ex)
                {
                    View.DisplayError(EditorErrors.RemovingSection);
                }
            }
        }
        public void RemoveField(List<dtoCallSection<dtoCallField>> sections, long idField)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                long pIdSection = 0;
                long pIdField = 0;
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;

                try
                {
                    CallService.SaveSections(idCall, sections, View.TagCurrent);
                    if (CallService.VirtualDeleteCallField(idField, true, ref pIdSection, ref pIdField))
                    {
                        if (View.CallType == CallForPaperType.CallForBids)
                            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCallField);
                        else
                            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.VirtualDeleteRequestField);
                        if (pIdField != 0)
                            View.ReloadEditor(RootObject.CallSubmissionEditorFieldRemoved(pIdField, type, idCall, idCommunity, View.PreloadView));
                        else
                            View.ReloadEditor(RootObject.CallSubmissionEditorSectionRemoved(pIdSection, type, idCall, idCommunity, View.PreloadView));
                    }
                    else
                        View.DisplayError(EditorErrors.RemovingField);
                }
                catch (SubmissionLinked exSubmission)
                {
                    View.DisplayError(EditorErrors.RemovingField);
                }
                catch (Exception ex)
                {
                    View.DisplayError(EditorErrors.RemovingField);
                }
            }
        }
        public void RemoveOption(List<dtoCallSection<dtoCallField>> sections, long idOption)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                long pIdOption = 0;
                long pIdSection = 0;
                long pIdField = 0;
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;

                try
                {
                    CallService.SaveSections(idCall, sections, View.TagCurrent);
                    if (CallService.VirtualDeleteFieldOption(idOption, true, ref pIdSection, ref pIdField, ref pIdOption))
                    {
                        if (View.CallType == CallForPaperType.CallForBids)
                            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteFieldOption);
                        else
                            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.VirtualDeleteFieldOption);
                        if (pIdOption != 0)
                            View.ReloadEditor(RootObject.CallSubmissionEditorOptionRemoved(pIdOption, type, idCall, idCommunity, View.PreloadView));
                        else if (pIdField != 0)
                            View.ReloadEditor(RootObject.CallSubmissionEditorFieldRemoved(pIdField, type, idCall, idCommunity, View.PreloadView));
                        else
                            View.ReloadEditor(RootObject.CallSubmissionEditorSectionRemoved(pIdSection, type, idCall, idCommunity, View.PreloadView));
                    }
                    else
                        View.DisplayError(EditorErrors.RemovingOption);
                }
                catch (SubmissionLinked exSubmission)
                {
                    View.DisplayError(EditorErrors.RemovingOption);
                }
                catch (Exception ex)
                {
                    View.DisplayError(EditorErrors.RemovingOption);
                }
            }
        }
        public void AddOption(List<dtoCallSection<dtoCallField>> sections, long idField, String name,Boolean isDefault, Boolean isFreeText)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                FieldOption option = CallService.AddOptionToField(idField, sections, name, isDefault, isFreeText);
                if (option == null)
                    View.DisplayError(EditorErrors.AddingOption);
                else
                {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddFieldOption);
                    else
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.AddFieldOption);
                    View.ReloadEditor(RootObject.CallSubmissionEditorOptionAdded(option.Id, type, idCall, idCommunity, View.PreloadView));
                }
            }
        }
        public void SetAsDefaultOption(List<dtoCallSection<dtoCallField>> sections, long idOption, Boolean isDefault)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                CallService.SaveSections(idCall, sections, View.TagCurrent);
                Boolean saved = CallService.SetAsDefaultOption(idOption, isDefault);
                if (!saved)
                    View.DisplayError(EditorErrors.SetAsDefaultOption);
                else
                {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SetAsDefaultOption);
                    else
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.SetAsDefaultOption);
                    View.ReloadEditor(RootObject.CallSubmissionEditorOptionRemoved(idOption, type, idCall, idCommunity, View.PreloadView));
                }
            }
        }
        public void SaveDisclaimerType(List<dtoCallSection<dtoCallField>> sections, long idField, DisclaimerType dType)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                CallService.SaveSections(idCall, sections, View.TagCurrent);
                Boolean saved = CallService.SaveDisclaimerType(idField, dType);
                if (!saved)
                    View.DisplayError(EditorErrors.EditingDisclaimerType);
                else
                {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.EditDisclaimerType);
                    else
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleRequestForMembership.ActionType.EditDisclaimerType);
                    View.ReloadEditor(RootObject.CallSubmissionEditorFieldRemoved(idField, type, idCall, idCommunity, View.PreloadView));
                }
            }
        }
        
        public void AddField(List<dtoCallSection<dtoCallField>> sections, dtoCallField field)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
            }
        }

        public void UpdateTags(string CallTags, IList<lm.Comol.Modules.CallForPapers.Advanced.dto.dtoTag> Tags)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return;
            }

            bool updated = CallService.CallTagUpdate(View.IdCall, CallTags, Tags);

            if (updated)
                InitView();

        }
    }
}