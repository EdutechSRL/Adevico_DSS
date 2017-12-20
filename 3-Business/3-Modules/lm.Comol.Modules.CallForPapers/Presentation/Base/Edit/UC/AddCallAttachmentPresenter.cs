using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class AddCallAttachmentPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdCallModule;
            private Int32 currentIdRequestModule;
            protected virtual IViewAddCallAttachment View
            {
                get { return (IViewAddCallAttachment)base.View; }
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
            private Int32 CurrentIdCallModule
            {
                get
                {
                    if (currentIdCallModule == 0)
                        currentIdCallModule = CurrentManager.GetModuleID(ModuleCallForPaper.UniqueCode);
                    return currentIdCallModule;
                }
            }
            private Int32 CurrentIdRequestModule
            {
                get
                {
                    if (currentIdRequestModule == 0)
                        currentIdRequestModule = CurrentManager.GetModuleID(ModuleRequestForMembership.UniqueCode);
                    return currentIdRequestModule;
                }
            }
            public AddCallAttachmentPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddCallAttachmentPresenter(iApplicationContext oContext, IViewAddCallAttachment view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idCall, CallForPaperType type, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions action, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, lm.Comol.Core.FileRepository.Domain.ModuleRepository rPermissions = null)
        {
            if (!UserContext.isAnonymous)
            {
                View.CurrentAction = action;
                View.IdCall = idCall;
                View.CallType = type;
                View.IdCallCommunity = identifier.IdCommunity;
                switch (action) { 
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem:
                        View.InitializeUploaderControl(action, identifier);
                        break;
                }
            }
            else
                View.CurrentAction = Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none;
        }

        public void AddFilesToItem(long idCall, CallForPaperType type)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired();
            else
            {
                BaseForPaper call = CallService.GetCall(idCall);
                Int32 idModule = (type== CallForPaperType.CallForBids ? CurrentIdCallModule : currentIdRequestModule);
                if (call== null){
                    View.DisplayCallNotFound(type);
                    if (type== CallForPaperType.CallForBids)
                        View.SendUserAction(View.IdCallCommunity, idModule,idCall,0, ModuleCallForPaper.ActionType.UploadFileToUnknownCall);
                    else
                        View.SendUserAction(View.IdCallCommunity, idModule,idCall,0, ModuleRequestForMembership.ActionType.UploadFileToUnknownCall);
                }
                else{
                    List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> files = null;
                    Int32 idObjectType = (Int32)ModuleCallForPaper.ObjectType.AttachmentFile;
                    Int32 idObjAction = (Int32)ModuleCallForPaper.ActionType.DownloadCallForPaperFile;
                    String moduleCode = ModuleCallForPaper.UniqueCode;
                    if (type!= CallForPaperType.CallForBids){
                        idObjectType = (Int32)ModuleRequestForMembership.ObjectType.AttachmentFile;
                        idObjAction = (Int32)ModuleRequestForMembership.ActionType.DownloadCallFile;
                        moduleCode = ModuleRequestForMembership.UniqueCode;
                    }
                    files = View.UploadFiles(moduleCode, idObjectType, idObjAction, false);
                    if (files != null && files.Any(f => f.IsAdded))
                    {
                        if (CallService.UploadAttachments(call, files.Where(f => f.IsAdded).ToList(), moduleCode, idModule, View.IdCallCommunity,  idObjectType))
                            View.DisplayItemsAdded();
                        else
                            View.DisplayItemsNotAdded();
                    }
                    else
                        View.DisplayNoFilesToAdd();
                }
            }
        }
    }
}