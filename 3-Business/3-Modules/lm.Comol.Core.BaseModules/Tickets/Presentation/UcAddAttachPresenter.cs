using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using TK = lm.Comol.Core.BaseModules.Tickets;


namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class UcAddAttachPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewUcAddAttach View
        {
            get { return (View.iViewUcAddAttach)base.View; }
        }

        public UcAddAttachPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public UcAddAttachPresenter(iApplicationContext oContext, View.iViewUcAddAttach view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        //public bool AddAttachment(Int64 MessageId, bool IsVisible)
        //{
        //    Domain.Message msg = service.MessageGet(MessageId);
        //    if (msg == null || msg.Ticket == null) return false;

        //    List<lm.Comol.Core.BaseModules.Tickets.TicketService.TmpTkAttachment> attachment =
        //        service.AttachmentsAddFiles(msg, View.UploadFiles(MessageId, msg.Ticket.Id));

        //    //files = View.UploadFiles(project);
        //    return false;
        //}


        public void InitView(long idProject, long idActivity, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions action, Int32 idCommunity = -1, CoreModuleRepository rPermissions = null)
        {
            if (!UserContext.isAnonymous)
            {
                //View.CurrentAction = action;
                //View.IdProject = idProject;
                //View.IdActivity = idActivity;
                //View.IdProjectCommunity = idCommunity;
                switch (action)
                {
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitem:

                        //View.InitializeUploaderControl(action, idCommunity);
                        break;
                    //case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem:
                    //    if (idCommunity > 0)
                    //        //View.InitializeUploaderControl(action, idCommunity);
                    //    break;
                    //case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity:
                    //    if (rPermissions != null && idCommunity > 0)
                    //        //View.InitializeLinkRepositoryItems(rPermissions, idCommunity, Service.AttachmentsGetLinkedFiles(idProject, idActivity));
                    //    // View.InitializeUploaderControl(idCommunity);
                    //    break;
                    //case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity:
                    //    if (rPermissions != null && idCommunity > 0)
                    //        //View.InitializeCommunityUploader(rPermissions, idCommunity);
                    //    break;
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity:
                        break;
                }
            }
            else
            { }
                //View.CurrentAction = Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none;
        }
    }
}
