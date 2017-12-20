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
    public class EditCallSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditCallSettings View
            {
                get { return (IViewEditCallSettings)base.View; }
            }
            private ServiceRequestForMembership _ServiceRequest;
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
            private ServiceRequestForMembership RequestService
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
        public EditCallSettingsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditCallSettingsPresenter(iApplicationContext oContext, IViewEditCallSettings view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(Boolean allowUseOfDss)
        {
            CallStandardAction action = View.PreloadAction;
            long idCall = View.PreloadIdCall;

            dtoBaseForPaper call = null;
            CallForPaperType type = View.PreloadType;
            if (idCall==0)
                action = CallStandardAction.Add;
            else{
                type = CallService.GetCallType(idCall);
                call = CallService.GetDtoBaseCall(idCall);
                action = CallStandardAction.Edit;
            }

            View.CallType = type;
            View.CurrentAction = action;

            int idCommunity = SetCallCurrentCommunity(action,call);
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                litePerson currenUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                Boolean allowManage = false;
                Boolean allowView = false;
                Boolean allowSave = false;
                switch (type) { 
                    case CallForPaperType.CallForBids:
                        ModuleCallForPaper module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowView = (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers);
                        allowManage = module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper;
                        allowSave =  ( module.Administration || module.ManageCallForPapers || (module.CreateCallForPaper && idCall==0) || (call!= null && module.EditCallForPaper && currenUser== call.Owner));
                        break;
                    case CallForPaperType.RequestForMembership:
                        ModuleRequestForMembership moduleR = CallService.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowView = (moduleR.ViewBaseForPapers || moduleR.Administration || moduleR.ManageBaseForPapers);
                        allowManage = moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper;
                        allowSave = (moduleR.Administration || moduleR.ManageBaseForPapers || (moduleR.CreateBaseForPaper && idCall == 0) || (call != null && moduleR.EditBaseForPaper && currenUser == call.Owner));
                        break;
                    default:

                        break;

                }

                int idModule = (type == CallForPaperType.CallForBids) ? CallService.ServiceModuleID() : RequestService.ServiceModuleID();
                View.IdCallModule = idModule;
                if (call == null && action == CallStandardAction.Edit)
                {
                    View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                }
                else if (allowManage || allowSave)
                {
                    View.AllowSave = allowSave;
                    View.IdCall = idCall;
                    LoadCallStatus(idCall, call);

                    bool canEditAdvancedCommission = false;





                    if (action == CallStandardAction.Add)
                    {
                        View.LoadEmptyCall();
                        View.AllowStatusEdit = false;
                        View.ForPortal = (idCommunity == 0);
                    }
                    else
                    {
                        View.ForPortal = (call != null && call.IsPortal);
                        View.AllowStatusEdit = AllowStatusEdit(idCall, type,call,allowSave);
                        View.LoadCall(call, CallService.CommissionAdvanceCanSet(idCall));
                    }

                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps =CallService.GetAvailableSteps(idCall, WizardCallStep.GeneralSettings, type);
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    View.LoadWizardSteps(idCall, type, idCommunity, steps);
                    if (type == CallForPaperType.CallForBids)
                    {
                        View.LoadEvaluationSettings(CallService.GetEvaluationSettings(idCall, allowUseOfDss));
                        View.SendUserAction(idCommunity, idModule, idCall, (action == CallStandardAction.Add) ? ModuleCallForPaper.ActionType.StartCallCreation : ModuleCallForPaper.ActionType.StartCallEdit);
                    }
                    else
                        View.SendUserAction(idCommunity, idModule, idCall, (action == CallStandardAction.Add) ? ModuleRequestForMembership.ActionType.StartRequestCreation : ModuleRequestForMembership.ActionType.StartRequestEdit);

                    if (steps.Where(s => s.Id == WizardCallStep.SubmittersType && (s.Status == Core.Wizard.WizardItemStatus.valid || s.Status == Core.Wizard.WizardItemStatus.warning)).Any())
                        View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(type, idCall, idCommunity, View.PreloadView));
                }
                else
                    View.DisplayNoPermission(idCommunity, idModule);

            }
        }

        private Boolean AllowStatusEdit(long idCall, CallForPaperType type, dtoBaseForPaper call, Boolean allowSave)
        {
            switch (type) { 
                case CallForPaperType.CallForBids:
                    switch(call.Status){
                        case CallForPaperStatus.Draft:
                            return allowSave || CallService.CallHasSubmissions(idCall);
                        case CallForPaperStatus.SubmissionClosed:
                            return allowSave && (!CallService.CallHasSubmissions(idCall) || (call.EndDate.HasValue == false || call.EndDate.HasValue && call.EndDate <= DateTime.Now));
                        case CallForPaperStatus.SubmissionsLimitReached:
                            return allowSave && CallService.CallHasSubmissions(idCall);
                        case CallForPaperStatus.SubmissionOpened:
                            return allowSave && (!CallService.CallHasSubmissions(idCall) || (call.EndDate.HasValue == false || call.EndDate.HasValue && call.EndDate <= DateTime.Now));
                        default:
                            return allowSave && !CallService.CallHasSubmissions(idCall) ;

                    }
                default:
                    return allowSave;
            }
        }
        private int SetCallCurrentCommunity(CallStandardAction action, dtoBaseForPaper call)
        {
            int idCommunity = -1;
            if (action == CallStandardAction.Add)
            {
                if (View.PreloadIdCommunity > 0)
                    idCommunity = View.PreloadIdCommunity;
                if (idCommunity == 0)
                    idCommunity = this.UserContext.CurrentCommunityID;
            }
            dtoCallCommunityContext context = CallService.GetCallCommunityContext(call, View.Portalname, idCommunity);
            View.SetContainerName(action, context.CommunityName, context.CallName);
            View.IdCommunity = context.IdCommunity;
            return context.IdCommunity;

            //int idCommunity = 0;
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //if (action== CallStandardAction.Add){
            //    if (View.PreloadIdCommunity > 0)
            //        idCommunity = View.PreloadIdCommunity;
            //    if (idCommunity==0)
            //         idCommunity = this.UserContext.CurrentCommunityID;
            //}
            //else if (call!=null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community !=null) ? call.Community.Id : 0;


            //community = CurrentManager.GetCommunity(idCommunity);
            //if (community != null)
            //    View.SetContainerName(action, community.Name, (call!=null) ? call.Name : "");
            //else if (currentCommunity != null)
            //{
            //    idCommunity = this.UserContext.CurrentCommunityID;
            //    View.SetContainerName(action, currentCommunity.Name, (call != null) ? call.Name : "");
            //}
            //else
            //{
            //    idCommunity = 0;
            //    View.SetContainerName(action, View.Portalname, (call != null) ? call.Name : "");
            //}
            //View.IdCommunity = idCommunity;
            //return idCommunity;
        }

        private void LoadCallStatus(long idCall,dtoBaseForPaper call)
        {
            List<CallForPaperStatus> availableStatus = CallService.GetAvailableStatus(idCall);
            View.LoadStatus(availableStatus);
            View.CurrentStatus = ((idCall==0 || !availableStatus.Any() ||call==null)? CallForPaperStatus.Draft : (availableStatus.Contains(call.Status)? call.Status : availableStatus.FirstOrDefault()));
        }
        
        public void SaveSettings(dtoBaseForPaper dto, String submitterName, Boolean allowUseOfDss, Boolean validateStatus)
        {
            int idCommunity = View.IdCommunity;
            try
            {
                
                int idModule = (dto.Type == CallForPaperType.CallForBids) ? CallService.ServiceModuleID() : RequestService.ServiceModuleID();

                if (dto.Type == CallForPaperType.RequestForMembership && dto.EndDate.HasValue && dto.StartDate >= dto.EndDate.Value)
                    View.DisplayDateError(dto.StartDate, dto.EndDate.Value);
                else if (dto.Type == CallForPaperType.CallForBids && dto.EndDate.HasValue && dto.StartDate >= dto.EndDate.Value && ((dtoCall)dto).EndEvaluationOn.HasValue && dto.EndDate.Value >= ((dtoCall)dto).EndEvaluationOn.Value)
                    View.DisplayDateError(CallForPaperType.CallForBids);
                else if (dto.Type == CallForPaperType.CallForBids && dto.EndDate.HasValue && dto.StartDate >= dto.EndDate.Value)
                    View.DisplayDateError(dto.StartDate, dto.EndDate.Value);
                else if (dto.Type == CallForPaperType.CallForBids && dto.EndDate.HasValue && ((dtoCall)dto).EndEvaluationOn.HasValue && dto.EndDate.Value >= ((dtoCall)dto).EndEvaluationOn.Value)
                    View.DisplayEvaluationDateError(dto.EndDate.Value, ((dtoCall)dto).EndEvaluationOn.Value);
                else
                {
                    Boolean stepsToComplete = false;
                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> wizardSteps = null;
                    BaseForPaper call = null;
                    List<WizardCallStep> skipedSteps = new List<WizardCallStep>(); ;
                    long idCall = dto.Id;
                    Boolean hasSubmission = false;
                    if (idCall > 0)
                        hasSubmission = CallService.CallHasSubmissions(idCall);
                    if (dto.Status > CallForPaperStatus.Draft && idCall == 0) {
                        dto.Status = CallForPaperStatus.Draft;
                        stepsToComplete = true;
                        View.CurrentStatus = dto.Status;
                    }
                    else if (dto.Status > CallForPaperStatus.Draft && idCall > 0)
                    {
                        wizardSteps = CallService.GetAvailableSteps(idCall, WizardCallStep.GeneralSettings, View.CallType);
                        skipedSteps = CallService.GetSkippedSteps(wizardSteps, idCall, View.CallType);
   
                        stepsToComplete = (skipedSteps.Count > 0);

                        if (stepsToComplete && !hasSubmission)
                        {
                            dto.Status = CallForPaperStatus.Draft;
                            View.CurrentStatus = dto.Status;
                        }
                    }


                    if (dto.Type == CallForPaperType.CallForBids)
                        call = CallService.SaveCallSettings((dtoCall)dto, idCommunity, validateStatus);
                    else
                        call = RequestService.SaveCallSettings((dtoRequest)dto, idCommunity, validateStatus, submitterName);
                    if (call != null)
                    {
                        idCall = call.Id;
                        RefreshCallName(View.CurrentAction, dto.Name, idCommunity);
                    }
                    View.IdCall = idCall;
                    if (dto.Type == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, idModule, idCall, (View.CurrentAction == CallStandardAction.Add) ? ModuleCallForPaper.ActionType.AddCallSettings : ModuleCallForPaper.ActionType.SaveCallSettings);
                    else
                        View.SendUserAction(idCommunity, idModule, idCall, (View.CurrentAction == CallStandardAction.Add) ? ModuleRequestForMembership.ActionType.AddCallSettings : ModuleRequestForMembership.ActionType.SaveCallSettings);
                    if (wizardSteps == null)
                        wizardSteps = CallService.GetAvailableSteps(idCall, WizardCallStep.GeneralSettings, call.Type);
                    
                    if (stepsToComplete && skipedSteps.Count > 0)
                        View.DisplaySkippedRequiredSteps(skipedSteps);
                    else if (stepsToComplete && wizardSteps.Count > 0)
                        View.DisplaySkippedRequiredSteps(wizardSteps.Where(ws => ws.Status == Core.Wizard.WizardItemStatus.error || ws.Status == Core.Wizard.WizardItemStatus.disabled).Select(ws => ws.Id).ToList());
                    else if (call != null)
                        View.DisplaySettingsSaved();
                    if (View.InvalidStatusFound){
                        View.InvalidStatusFound = false;
                        View.LoadStatus(CallService.GetAvailableStatus(idCall), dto.Status);
                        dto = CallService.GetDtoBaseCall(idCall);
                        View.LoadCall(dto, CallService.CommissionAdvanceCanSet(idCall));
                        if (dto.Type == CallForPaperType.CallForBids)
                            View.LoadEvaluationSettings(CallService.GetEvaluationSettings(idCall, allowUseOfDss));
                        }
                    else
                        View.LoadStatus(CallService.GetAvailableStatus(idCall), dto.Status );

                    View.LoadWizardSteps(idCall, dto.Type, idCommunity, wizardSteps);
                }
            }
            catch (SkipRequiredSteps exc) {
                View.DisplaySkippedRequiredSteps(exc.Steps);
            }
            catch (CallForPaperInvalidStatus ex)
            {
                View.InvalidStatusFound = true;
                switch (dto.Status) { 
                    case CallForPaperStatus.Draft:
                        if (dto.StartDate>=DateTime.Now)
                            View.CurrentStatus = (!dto.EndDate.HasValue || (dto.EndDate.HasValue && dto.EndDate.Value<=DateTime.Now)) ? CallForPaperStatus.SubmissionOpened: CallForPaperStatus.SubmissionClosed;
                        else
                            View.CurrentStatus = CallForPaperStatus.SubmissionOpened;
                        break;
                }
                View.LoadInvalidStatus(dto.Status, dto.EndDate);
                RefreshCallName(View.CurrentAction, dto.Name, idCommunity);
            }
        }


        private void RefreshCallName(CallStandardAction action, String name, Int32 idCommunity)
        {
            Community community = CurrentManager.GetCommunity(idCommunity);
            if (community != null)
                View.SetContainerName(action, community.Name, name);
            else
                View.SetContainerName(action, View.Portalname, name);
        }
    }
}