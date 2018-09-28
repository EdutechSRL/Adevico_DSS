using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Modules.Standard.ProjectManagement.Business;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public class AddAttachmentPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewAddAttachment View
            {
                get { return (IViewAddAttachment)base.View; }
            }
            private ServiceProjectManagement Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceProjectManagement(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleProjectManagement.UniqueCode);
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

        public void InitView(long idProject, long idActivity, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions action, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, lm.Comol.Core.FileRepository.Domain.ModuleRepository rPermissions = null)
        {
            if (!UserContext.isAnonymous)
            {
                View.CurrentAction = action;
                View.IdProject = idProject;
                View.IdActivity = idActivity;
                View.IdProjectCommunity = identifier.IdCommunity;
                switch (action) { 
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitem:

                        View.InitializeUploaderControl(action,identifier);
                        break;
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem:
                        View.InitializeUploaderControl(action, identifier);
                        break;
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity:
                        if (rPermissions != null && identifier.IdCommunity > 0)
                            View.InitializeLinkRepositoryItems(UserContext.CurrentUserID, rPermissions, identifier, Service.AttachmentsGetBaseLinkedFiles(idProject, idActivity));
                       // View.InitializeUploaderControl(idCommunity);
                        break;
                    case Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity:
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

        /// <summary>
        /// Add urls to project or activity
        /// </summary>
        /// <param name="idProject"></param>
        /// <param name="idActivity"></param>
        /// <param name="urls"></param>
        public void AddUrlToItem(long idProject, long idActivity, List<dtoUrl> urls) {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired();
            else {
                List<ProjectAttachment> attachments = Service.AttachmentsAddUrl(idProject, idActivity, urls);
                if (attachments == null)
                    View.DisplayItemsNotAdded();
                else
                    View.DisplayItemsAdded();
                View.SendUserAction(View.IdProjectCommunity, CurrentIdModule, idProject, idActivity, (attachments == null) ? ModuleProjectManagement.ActionType.AttachmentsNotAddedUrl : ModuleProjectManagement.ActionType.AttachmentsAddedUrl);

            }
        }
        public void AddFilesToItem(long idProject, long idActivity)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired();
            else
            {
                List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> files = null;
                PmActivity activity = (idActivity > 0) ? CurrentManager.Get<PmActivity>(idActivity) : null;
                Project project = (idProject>0) ? CurrentManager.Get<Project>(idProject) : ((activity != null) ? activity.Project : null);
                if (idActivity > 0) { 
                    if (activity==null){
                        View.DisplayActivityNotFound();
                        View.SendUserAction(View.IdProjectCommunity, CurrentIdModule, idProject, idActivity, ModuleProjectManagement.ActionType.AttachmentsActivityNotFound);
                        return;
                    }
                    else
                        files = View.UploadFiles(activity, false);
                }
                else if (idProject > 0 && project != null)
                    files = View.UploadFiles(project, false);
                else {
                    View.DisplayProjectNotFound();
                    View.SendUserAction(View.IdProjectCommunity, CurrentIdModule, idProject, idActivity, ModuleProjectManagement.ActionType.AttachmentsProjectNotFound);
                    return;
                }

                if (files != null && files.Any(f => f.IsAdded))
                {
                    List<ProjectAttachment> attachments = Service.AttachmentsAddFiles(project, activity, files.Where(f => f.IsAdded).ToList());
                    if (attachments == null)
                        View.DisplayItemsNotAdded();
                    else
                        View.DisplayItemsAdded();
                    View.SendUserAction(View.IdProjectCommunity, CurrentIdModule, idProject, idActivity, (attachments == null) ? ModuleProjectManagement.ActionType.AttachmentsNotAddedFiles : ModuleProjectManagement.ActionType.AttachmentsAddedFiles);
                }
                else
                    View.DisplayNoFilesToAdd();
            }
        }
        public void AddCommunityFilesToItem(long idProject, long idActivity)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired();
            else
            {
                List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> files = null;
                PmActivity activity = (idActivity > 0) ? CurrentManager.Get<PmActivity>(idActivity) : null;
                Project project = (idProject > 0) ? CurrentManager.Get<Project>(idProject) : ((activity != null) ? activity.Project : null);
                if (idActivity > 0)
                {
                    if (activity == null)
                    {
                        View.DisplayActivityNotFound();
                        View.SendUserAction(View.IdProjectCommunity, CurrentIdModule, idProject, idActivity, ModuleProjectManagement.ActionType.AttachmentsActivityNotFound);
                        return;
                    }
                    else
                        files = View.UploadFiles(activity,true);
                }
                else if (idProject > 0 && project != null)
                    files = View.UploadFiles(project,  true);
                else
                {
                    View.DisplayProjectNotFound();
                    View.SendUserAction(View.IdProjectCommunity, CurrentIdModule, idProject, idActivity, ModuleProjectManagement.ActionType.AttachmentsProjectNotFound);
                    return;
                }

                if (files != null && files.Any(f => f.IsAdded))
                {
                    AddCommunityFilesToItem(idProject, idActivity, (files == null) ? null : files.Where(f=> f.IsAdded).Select(f => f.Link).ToList());
                }
                else
                    View.DisplayNoFilesToAdd();
            }
        }
        public void AddCommunityFilesToItem(long idProject, long idActivity, List<ModuleActionLink> links)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired();
            else if (links != null && links.Count > 0)
            {
                List<ProjectAttachment> attachments = Service.AttachmentsLinkFiles(idProject, idActivity, links);
                if (attachments == null)
                    View.DisplayItemsNotAdded();
                else
                    View.DisplayItemsAdded();
                View.SendUserAction(View.IdProjectCommunity, CurrentIdModule, idProject, idActivity, (attachments == null) ? ModuleProjectManagement.ActionType.AttachmentsNotAddedFiles : ModuleProjectManagement.ActionType.AttachmentsAddedFiles);
            }
            else
                View.DisplayNoFilesToAdd();
        }
    }
}