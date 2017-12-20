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
    public class EditAttachmentsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditAttachments View
            {
                get { return (IViewEditAttachments)base.View; }
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
        public EditAttachmentsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditAttachmentsPresenter(iApplicationContext oContext, IViewEditAttachments view)
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
                    allowSave = allowSave && (type== CallForPaperType.RequestForMembership || (type != CallForPaperType.RequestForMembership && (!CallService.CallHasSubmissions(idCall))));
                    View.AllowSave = allowSave;
                    View.IdCall = idCall;
                    List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> availableActions = new List<Core.DomainModel.Repository.RepositoryAttachmentUploadActions>() { Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem };
                    lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier rIdentifier = call.GetRepositoryIdentifier();
                    if (!(rIdentifier.Type == Core.FileRepository.Domain.RepositoryType.Community && rIdentifier.IdCommunity==0))
                        View.InitializeAttachmentsControl(idCall, type,rIdentifier,  availableActions,  Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem);

                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps = CallService.GetAvailableSteps(idCall, WizardCallStep.Attachments, type);
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    View.LoadWizardSteps(idCall, type, idCommunity,steps);
                   
                    if (type == CallForPaperType.CallForBids)
                    {
                        LoadAttachments(idCall, module);
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.EditAttachments);
                    }
                    else
                    {
                        LoadAttachments(idCall, moduleR);
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.EditAttachments);
                    }
                    if (steps.Where(s=> s.Id == WizardCallStep.SubmittersType && (s.Status== Core.Wizard.WizardItemStatus.valid || s.Status== Core.Wizard.WizardItemStatus.warning)).Any())
                        View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(type, idCall,idCommunity, View.PreloadView));
                }
                else
                    View.DisplayNoPermission(idCommunity, idModule);
            }
        }

        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            //int idCommunity = 0;
            //Boolean forPortal = (call!=null) ? call.IsPortal : false;
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
            dtoCallCommunityContext context = CallService.GetCallCommunityContext(call, View.Portalname);
            View.SetContainerName(CallStandardAction.Edit, context.CommunityName, context.CallName);
            View.IdCommunity = context.IdCommunity;
            return context.IdCommunity;
        }
        private void LoadAttachments(long idCall, ModuleCallForPaper module)
        {
            View.LoadSubmitterTypes(CallService.GetCallAvailableSubmittersType(idCall));
            List<dtoAttachmentFilePermission> items = CallService.GetCallAttachments(idCall, module, false);
            if (items.Count == 0)
                View.DisplayNoAttachments();
            else
                View.LoadAttachments(items);
        }
        private void LoadAttachments(long idCall, ModuleRequestForMembership module)
        {
            View.LoadSubmitterTypes(CallService.GetCallAvailableSubmittersType(idCall));

            List<dtoAttachmentFilePermission> items = CallService.GetCallAttachments(idCall, module, false);
            if (items.Count == 0)
                View.DisplayNoAttachments();
            else
                View.LoadAttachments(items);
        }
        public  void LoadAttachments(long idCall)
        {
            switch (View.CallType)
            {
                case CallForPaperType.CallForBids:
                    ModuleCallForPaper module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, View.IdCommunity);
                    LoadAttachments(idCall, module);
                    break;
                case CallForPaperType.RequestForMembership:
                    ModuleRequestForMembership moduleR = CallService.RequestForMembershipServicePermission(UserContext.CurrentUserID, View.IdCommunity);
                    LoadAttachments(idCall, moduleR);
                    break;
                default:
                    break;
            }
        }

        //public void StartUpload() { 
        //    BaseForPaper call = CallService.GetCall(View.IdCall);
        //    if (call != null)
        //    {
        //        MultipleUploadResult<dtoModuleUploadedFile> files = null;
        //        String moduleCode = "";
        //        Int32 moduleAction = 0;
        //        Int32 idObjectOwnerType = 0;
        //        switch (call.Type)
        //        {
        //            case CallForPaperType.CallForBids:
        //                moduleCode = ModuleCallForPaper.UniqueCode;
        //                moduleAction = (Int32)ModuleCallForPaper.ActionType.DownloadCallForPaperFile;
        //                idObjectOwnerType = (Int32)ModuleCallForPaper.ObjectType.AttachmentFile;
        //                break;
        //            case CallForPaperType.RequestForMembership:
        //                moduleCode = ModuleRequestForMembership.UniqueCode;
        //                moduleAction = (Int32)ModuleRequestForMembership.ActionType.DownloadCallFile;
        //                idObjectOwnerType = (Int32)ModuleRequestForMembership.ObjectType.AttachmentFile;
        //                break;
        //        }
        //        files = View.UploadFiles(call, moduleCode, moduleAction, idObjectOwnerType);
        //        if (files != null && files.UploadedFile.Count > 0)
        //        {
        //            CallService.UploadAttachments(call, files.UploadedFile, moduleCode, View.IdCallModule, View.IdCommunity, idObjectOwnerType);
        //        }
        //        LoadAttachments(call.Id);
        //    }
        //}

        public void SaveSettings(List<dtoAttachmentFile> items)
        {
            if (!CallService.SaveAttachments(View.IdCall, items))
                View.DisplaySavingError();
            else
            {
                LoadAttachments(View.IdCall);
                View.DisplaySettingsSaved();
                if (View.CallType == CallForPaperType.CallForBids)
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.SaveAttachments);
                else
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.SaveAttachments);
            }
        }
        public void RemoveAttachment(long idAttachment)
        {
            try
            {
                if (!CallService.VirtualDeleteAttachment(idAttachment, true))
                    View.DisplayDeletingError();
                else {
                    if (View.CallType == CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.VirtualDeleteAttachment);
                    else
                        View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.VirtualDeleteAttachment);
                }
            }
            catch (SubmissionLinked ex)
            {
                View.DisplayDeletingError();
            }

            LoadAttachments(View.IdCall);
        }
    }
}