using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class AddAttachmentPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private TicketService service;
            private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository _ServiceRepository;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewAddAttachment View
            {
                get { return (IViewAddAttachment)base.View; }
            }
            private TicketService Service
            {
                get
                {
                    if (service == null)
                        service = new TicketService(AppContext);
                    return service;
                }
            }
            private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository ServiceRepository
            {
                get
                {
                    if (_ServiceRepository == null)
                        _ServiceRepository = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(AppContext);
                    return _ServiceRepository;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleTicket.UniqueCode);
                    return currentIdModule;
                }
            }
            public AddAttachmentPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddAttachmentPresenter(iApplicationContext oContext, IViewAddAttachment view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(long idMessage,lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions action, Int32 idCommunity, Int32 idUploader = -1)
            {
                InitView(idMessage, action, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create((idCommunity <= 0 ? lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal : Core.FileRepository.Domain.RepositoryType.Community), idCommunity),null,idUploader);
            }
            public void InitView(long idMessage, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions action, Int32 idCommunity, lm.Comol.Core.FileRepository.Domain.ModuleRepository rPermissions, Int32 idUploader = -1)
            {
                InitView(idMessage, action, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create((idCommunity <= 0 ? lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal : Core.FileRepository.Domain.RepositoryType.Community), idCommunity), rPermissions, idUploader);
            }
            public void InitView(long idMessage, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions action, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, lm.Comol.Core.FileRepository.Domain.ModuleRepository rPermissions = null, Int32 idUploader = -1)
            {
                if (!UserContext.isAnonymous || (action == lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem &&  View.AllowAnonymousUpload))
                {
                    View.CurrentAction = action;
                    View.IdMessage = idMessage;
                    View.IdTicketCommunity = identifier.IdCommunity;
                    switch (action) { 
                        case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem:
                            View.InitializeUploaderControl(action, identifier,idUploader);
                            break;
                        case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity:
                            if (rPermissions == null)
                                rPermissions = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
                            if (rPermissions != null && identifier.IdCommunity > 0)
                                View.InitializeLinkRepositoryItems(UserContext.CurrentUserID, rPermissions, identifier, Service.AttachmentsGetBaseLinkedFiles(idMessage));
                            break;
                        case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity:
                            if (rPermissions == null)
                                rPermissions = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
                            if (rPermissions != null && identifier.IdCommunity > 0)
                                View.InitializeCommunityUploader(identifier);
                            break;
                        case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity:
                            break;
                    }
                }
                else
                    View.CurrentAction = Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none;
            }
    }
}