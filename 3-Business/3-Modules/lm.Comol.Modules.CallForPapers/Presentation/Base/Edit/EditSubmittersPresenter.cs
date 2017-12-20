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
    public class EditSubmittersPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditSubmittersType View
            {
                get { return (IViewEditSubmittersType)base.View; }
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
        public EditSubmittersPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditSubmittersPresenter(iApplicationContext oContext, IViewEditSubmittersType view)
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
                    View.IdCall = idCall;
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    if (type == CallForPaperType.CallForBids)
                    {
                        LoadSubmitters(idCall, module, type, idCommunity);
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.EditSubmittersType);
                    }
                    else
                    {
                        LoadSubmitters(idCall, moduleR, type, idCommunity);
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.EditSubmittersType);
                    }
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
        private void LoadSubmitters(long idCall, ModuleCallForPaper module, CallForPaperType type, Int32 idCommunity)
        {
            List<dtoSubmitterTypePermission> submitters = CallService.GetCallSubmittersType(idCall, module, false);
            if (submitters.Count == 0)
                View.DisplayNoSubmitters();
            else
                View.LoadSubmitters(submitters);
            List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps = CallService.GetAvailableSteps(idCall, WizardCallStep.SubmittersType, type);
            View.LoadWizardSteps(idCall, type, idCommunity, steps);
            if (steps.Where(s => (s.Id != WizardCallStep.CallAvailability) && submitters.Count > 0 && (s.Status == Core.Wizard.WizardItemStatus.valid || s.Status == Core.Wizard.WizardItemStatus.warning)).Any())
                View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(type, idCall, idCommunity, View.PreloadView));

        }
        private void LoadSubmitters(long idCall, ModuleRequestForMembership module, CallForPaperType type, Int32 idCommunity)
        {
            List<dtoSubmitterTypePermission> submitters = CallService.GetCallSubmittersType(idCall, module, false);
            if (submitters.Count == 0)
                View.DisplayNoSubmitters();
             else
                View.LoadSubmitters(submitters);
            List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps = CallService.GetAvailableSteps(idCall, WizardCallStep.SubmittersType, type);
            View.LoadWizardSteps(idCall, type, idCommunity, steps);
            if (steps.Where(s => (s.Id != WizardCallStep.CallAvailability) && submitters.Count >0 && (s.Status == Core.Wizard.WizardItemStatus.valid || s.Status == Core.Wizard.WizardItemStatus.warning)).Any())
                View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(type, idCall, idCommunity, View.PreloadView));
        }
        private void LoadSubmitters(long idCall)
        {
            switch (View.CallType)
            {
                case CallForPaperType.CallForBids:
                    ModuleCallForPaper module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, View.IdCommunity);
                    LoadSubmitters(idCall, module, CallForPaperType.CallForBids, View.IdCommunity);
                    break;
                case CallForPaperType.RequestForMembership:
                    ModuleRequestForMembership moduleR = CallService.RequestForMembershipServicePermission(UserContext.CurrentUserID, View.IdCommunity);
                    LoadSubmitters(idCall, moduleR, CallForPaperType.RequestForMembership, View.IdCommunity);
                    break;
                default:
                    break;
            }
        }
        public void SaveSettings(List<dtoSubmitterType> submitters)
        {
            if (!CallService.SaveSubmittersType(View.IdCall, submitters))
                View.DisplayError(SubmitterTypeError.ErrorSavingData);
            else
            {
                LoadSubmitters(View.IdCall);
                View.DisplaySettingsSaved();
                if (View.CallType == CallForPaperType.CallForBids)
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.SaveSubmittersType);
                else
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.SaveSubmittersType);
            }
        }
        public void RemoveSubmitter(long idSubmitter)
        {
            try
            {
                if (!CallService.VirtualDeleteSubmitterType(idSubmitter, true))
                {
                    LoadSubmitters(View.IdCall);
                    View.DisplayError(SubmitterTypeError.SubmitterUsed);
                }
                else
                {
                    LoadSubmitters(View.IdCall);
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.VirtualDeleteSubmitterType);
                    else
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.VirtualDeleteSubmitterType);
                }
            }
            catch (SubmissionLinked ex) {
                LoadSubmitters(View.IdCall);
                View.DisplayError(SubmitterTypeError.SubmitterUsed);
            }
            
           
        }
        public void AddSubmitter(List<dtoSubmitterType> submitters)
        {
            SaveSettings(submitters);
            if (CallService.AddSubmitterType(View.IdCall) != null) {
                if (View.CallType == CallForPaperType.CallForBids)
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.SaveSubmittersType);
                else
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.SaveSubmittersType);
            }
            LoadSubmitters(View.IdCall);
        }
    }
}