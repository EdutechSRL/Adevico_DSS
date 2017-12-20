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
    public class EditFileToSubmitPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditFileToSubmit View
            {
                get { return (IViewEditFileToSubmit)base.View; }
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
            public EditFileToSubmitPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditFileToSubmitPresenter(iApplicationContext oContext, IViewEditFileToSubmit view)
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
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    if (type == CallForPaperType.CallForBids)
                    {
                        LoadRequiredFiles(idCall, module);
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewRequestedFiles);
                    }
                    else
                    {
                        LoadRequiredFiles(idCall, moduleR);
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.ViewRequestedFiles);
                    }

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
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //if (call!=null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community !=null) ? call.Community.Id : 0;


            //community = CurrentManager.GetCommunity(idCommunity);
            //if (community != null)
            //    View.SetContainerName(CallStandardAction.Edit, community.Name, (call != null) ? call.Name : "");
            //else if (currentCommunity != null)
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

        private void LoadRequiredFiles(long idCall, ModuleCallForPaper module)
        {
            dtoGenericPermission permission = CallService.GetPermissionForEditor(idCall, module);
            LoadRequiredFiles(idCall, permission);
        }
        private void LoadRequiredFiles(long idCall, ModuleRequestForMembership module)
        {
            dtoGenericPermission permission = CallService.GetPermissionForEditor(idCall, module);
            LoadRequiredFiles(idCall, permission);
        }
        private void LoadRequiredFiles(long idCall)
        {
            switch (View.CallType)
            {
                case CallForPaperType.CallForBids:
                    ModuleCallForPaper module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, View.IdCommunity);
                    LoadRequiredFiles(idCall, CallService.GetPermissionForEditor(idCall, module));
                    break;
                case CallForPaperType.RequestForMembership:
                    ModuleRequestForMembership moduleR = CallService.RequestForMembershipServicePermission(UserContext.CurrentUserID, View.IdCommunity);
                    LoadRequiredFiles(idCall, CallService.GetPermissionForEditor(idCall, moduleR));
                    break;
                default:
                    break;
            }
        }
        private void LoadRequiredFiles(long idCall, dtoGenericPermission permission)
        {
            View.LoadSubmitterTypes(CallService.GetCallAvailableSubmittersType(idCall));
            List<dtoRequestedFilePermission> items = CallService.GetRequestedFiles(idCall, permission,false);
            View.LoadWizardSteps(idCall, View.CallType, View.IdCommunity, CallService.GetAvailableSteps(idCall, WizardCallStep.FileToSubmit, View.CallType));
            if (items.Count == 0)
                View.DisplayNoFileToSubmit();
            else
                View.LoadFiles(items);
        }

        public void SaveSettings(List<dtoCallRequestedFile> items)
        {
            if (!CallService.SaveRequestedFiles(View.IdCall, items))
                View.DisplayError(EditorErrors.EditingRequestedFiles);
            else
            {
                LoadRequiredFiles(View.IdCall);
                View.DisplaySettingsSaved();
                if (View.CallType == CallForPaperType.CallForBids)
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.EditRequestedFile);
                else
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.EditRequestedFile);
            }
        }
        public void AddRequiredFile(dtoCallRequestedFile dto) {
            try
            {
                RequestedFile file = CallService.AddRequestedFile(View.IdCall, dto);
                if (file == null)
                    View.DisplayError(EditorErrors.AddingRequestedFile);
                else
                {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.AddRequestedFile);
                    else
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.AddRequestedFile);
                }
            }
            catch (SubmissionLinked ex)
            {
                View.DisplayError(EditorErrors.AddingRequestedFile);
            }
            LoadRequiredFiles(View.IdCall);
        }
        public void RemoveRequestedFile(long idRequestedFile)
        {
            try
            {
                if (!CallService.VirtualDeleteCallRequestedFile(idRequestedFile, true))
                    View.DisplayError(EditorErrors.RemovingRequestedFiles);
                else {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.VirtualDeleteRequestedFile);
                    else
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.VirtualDeleteRequestedFile);
                }
            }
            catch (SubmissionLinked ex)
            {
                View.DisplayError(EditorErrors.RemovingRequestedFiles);
            }
            LoadRequiredFiles(View.IdCall);
        }
    }
}