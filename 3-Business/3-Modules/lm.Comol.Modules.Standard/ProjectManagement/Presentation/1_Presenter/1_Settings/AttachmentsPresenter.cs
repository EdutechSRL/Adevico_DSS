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
    public class AttachmentsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewAttachments View
            {
                get { return (IViewAttachments)base.View; }
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
            public AttachmentsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AttachmentsPresenter(iApplicationContext oContext, IViewAttachments view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(String unknownUser)
        {
            long idProject = View.PreloadIdProject;
            dtoProjectContext cContext = new dtoProjectContext() { IdCommunity = View.PreloadIdCommunity, isPersonal = View.PreloadIsPersonal, isForPortal = View.PreloadForPortal };
            dtoProject project = InitializeContext(idProject, ref cContext);
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                litePerson p = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                SetBackUrls(View.PreloadFromPage, View.PreloadIdContainerCommunity, project, cContext);
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>> steps = Service.GetAvailableSteps(WizardProjectStep.Documents, idProject);
                View.LoadWizardSteps(idProject, cContext.IdCommunity, cContext.isPersonal, cContext.isForPortal, steps);
                if (project == null)
                    View.DisplayUnknownProject();
                else {
                    ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, cContext.IdCommunity, CurrentIdModule));
                    PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                    View.RepositoryIdentifier = CreateRepositoryIdentifier(project);
                    if ((mPermission.Administration && !project.isPersonal) || (pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject)
                        LoadAttachments(unknownUser,project.Id, project.isPersonal, project.IdCommunity, mPermission, pPermissions, true);
                    else{
                        View.AllowSave = false;
                        ProjectResource resource = Service.GetResource(idProject, UserContext.CurrentUserID);
                        if (resource != null && resource.ProjectRole != ActivityRole.Visitor && resource.ProjectRole != ActivityRole.None)
                            LoadAttachments(unknownUser, project.Id, project.isPersonal, project.IdCommunity, mPermission, pPermissions, false);
                        else
                            View.DisplayNoPermission(cContext.IdCommunity, CurrentIdModule);
                    }
                }
            }
        }
        private lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier CreateRepositoryIdentifier(dtoProject project)
        {
            if (project.isPortal)
                return lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal, 0);
            else
                return lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(lm.Comol.Core.FileRepository.Domain.RepositoryType.Community, project.IdCommunity);
        }
        private void SetBackUrls(PageListType fromPage, Int32 idContainerCommunity, dtoProject project, dtoProjectContext cContext)
        {
            switch (fromPage)
            {
                case PageListType.ListAdministrator:
                case PageListType.ListManager:
                case PageListType.ListResource:
                    View.SetProjectsUrl(Service.GetBackUrl(fromPage, idContainerCommunity, (project ==null) ? 0 : project.Id));
                    break;
                case PageListType.DashboardManager:
                case PageListType.DashboardResource:
                case PageListType.DashboardAdministrator:
                    if (project != null) 
                        View.SetDashboardUrl(RootObject.DashboardFromCookies(cContext, idContainerCommunity, fromPage, project.Id), fromPage);
                    break;
                case PageListType.ProjectDashboardManager:
                case PageListType.ProjectDashboardResource:
                    if (project != null)
                        View.SetDashboardUrl(RootObject.ProjectDashboardFromCookies(cContext, idContainerCommunity, fromPage, project.Id), fromPage);
                    break;
            }
            if (project != null)
                View.SetProjectMapUrl(RootObject.ProjectMap(project.Id, project.IdCommunity, project.isPortal, project.isPersonal, fromPage, idContainerCommunity));
        }
        private dtoProject InitializeContext(long idProject, ref dtoProjectContext cContex)
        {
            dtoProject project = Service.GetdtoProject(idProject);
            View.IdProject = idProject;
            if (project == null)
            {
                Int32 idCommunity = (!cContex.isForPortal && cContex.IdCommunity < 1) ? UserContext.CurrentCommunityID : cContex.IdCommunity;
                liteCommunity community = (idCommunity > 0) ? CurrentManager.GetLiteCommunity(idCommunity) : null;
                cContex.IdCommunity = (community != null) ? community.Id : 0;
            }
            else
            {
                cContex.IdCommunity = project.IdCommunity;
                View.forPortal = project.isPortal;
                View.isPersonal = project.isPersonal;
            }

            View.ProjectIdCommunity = cContex.IdCommunity;
            View.forPortal = cContex.isForPortal;
            View.isPersonal = cContex.isPersonal;
            return project;
        }

        //private void LoadAttachments(long idProject, ModuleProjectManagement mPermission, PmActivityPermission pPermissions) { 
        //    LoadAttachments(idProject,mPermission,pPermissions,(mPermission.Administration && !project.isPersonal) || (pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject) || (pPermissions & PmActivityPermission.AddAttachments) == PmActivityPermission.AddAttachments);
        //}
        private void LoadAttachments(String unknownUser,long idProject, Boolean isPersonal, Int32 idCommunity, ModuleProjectManagement mPermission, PmActivityPermission pPermissions, Boolean allowAdd, ModuleProjectManagement.ActionType dAction = ModuleProjectManagement.ActionType.ProjectAttachmentsLoad)
        {
            View.SendUserAction(idCommunity, CurrentIdModule, idProject, dAction);

            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = Service.GetRepositoryPermissions(idCommunity);
            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> availableActions = Service.AttachmentsGetAvailableUploadActions((mPermission.Administration && !isPersonal), pPermissions, cRepository);
            View.AllowSave = allowAdd && availableActions.Any();
            View.InitializeAttachmentsControl(View.RepositoryIdentifier,cRepository, availableActions, Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem);
            View.LoadAttachments(Service.GetProjectAttachments(idProject, 0, false, unknownUser));
        }

        public void LoadAttachments(String unknownUser,long idProject,Boolean isPortal,Boolean isPersonal,Int32 idCommunity)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                ModuleProjectManagement mPermission = (isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, CurrentIdModule));
                PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                Boolean allowSave = (mPermission.Administration && !isPersonal) || ((pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject);
                View.AllowSave = allowSave;
                LoadAttachments(unknownUser,idProject, isPersonal,idCommunity, mPermission,pPermissions,allowSave);
            }
        }

        public void EditUrl(long idAttachmentLink, long idAttachment, String unknownUser) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean reloadItems = true;
                Boolean isPersonal = View.isPersonal;
                Int32 idCommunity = View.ProjectIdCommunity;
                Boolean isPortal = View.forPortal;
                long idProject = View.IdProject;
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                ModuleProjectManagement mPermission = (isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, CurrentIdModule));
                PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                Boolean allowSave = (mPermission.Administration && !isPersonal) || ((pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject);

                if (allowSave)
                {
                    dtoAttachment attachment = Service.AttachmentGetByIdLink(idAttachmentLink,unknownUser);
                    if (attachment != null && attachment.Type == AttachmentType.url)
                    {
                        View.DisplayUrlForEditing(new dtoUrl() { Id = attachment.IdAttachment, Address = attachment.Url, Name = attachment.DisplayName });
                        reloadItems = false;
                    }
                    else
                    {
                        View.SendUserAction(idCommunity, CurrentIdModule, idProject, idAttachment, ModuleProjectManagement.ActionType.ProjectAttachmentsUrlUnknown);
                        View.DisplayUrlRemoved();
                    }
                }
                else
                {
                    View.SendUserAction(idCommunity, CurrentIdModule, idProject, idAttachment, ModuleProjectManagement.ActionType.ProjectAttachmentsNoPermissionToEditUrl);
                    View.DisplayNoPermissionToEditUrl();
                }
                if (reloadItems)
                    LoadAttachments(unknownUser,idProject, isPersonal, idCommunity, mPermission, pPermissions, allowSave, ModuleProjectManagement.ActionType.ProjectAttachmentsReload);
            }
        }
        public void SaveUrl(dtoUrl item, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean isPersonal = View.isPersonal;
                Int32 idCommunity = View.ProjectIdCommunity;
                Boolean isPortal = View.forPortal;
                long idProject = View.IdProject;
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                ModuleProjectManagement mPermission = (isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, CurrentIdModule));
                PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                Boolean allowSave = (mPermission.Administration && !isPersonal) || ((pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject);

                if (allowSave && item !=null)
                {
                    if (String.IsNullOrEmpty(item.Address))
                    {
                        View.SendUserAction(idCommunity, CurrentIdModule, idProject, item.Id, ModuleProjectManagement.ActionType.ProjectAttachmentsUnableToEditUrl);
                        View.DisplayUnableToSaveEmptyUrl(item);
                    }
                    else {
                        ProjectAttachment attachment = Service.AttachmentSaveUrl(item);
                        if (attachment != null)
                        {
                            View.DisplaySavedUrl(item);
                            View.SendUserAction(idCommunity, CurrentIdModule, idProject, item.Id, ModuleProjectManagement.ActionType.ProjectAttachmentsUrlModified);
                        }
                        else
                        {
                            View.SendUserAction(idCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectAttachmentsUnableToEditUrl);
                            View.DisplayUnableToSaveUrl(item);
                        }
                    }
                }
                else
                {
                    View.SendUserAction(idCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectAttachmentsNoPermissionToEditUrl);
                    View.DisplayNoPermissionToEditUrl();
                }
                LoadAttachments(unknownUser,idProject, isPersonal, idCommunity, mPermission, pPermissions, allowSave, ModuleProjectManagement.ActionType.ProjectAttachmentsReload);
            }
        }

        public void VirtualDeleteAttachment(long idAttachmentLink, String unknownUser)
        {
            VirtualDeleteAttachments(new List<long> { idAttachmentLink }, unknownUser);
        }
        public void VirtualDeleteAttachments(List<long> idAttachmentLinks, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean isPersonal = View.isPersonal;
                Int32 idCommunity = View.ProjectIdCommunity;
                Boolean isPortal = View.forPortal;
                long idProject = View.IdProject;
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                ModuleProjectManagement mPermission = (isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, CurrentIdModule));
                PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                Boolean allowSave = (mPermission.Administration && !isPersonal) || ((pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject);
                if (idAttachmentLinks.Any())
                {
                    if (allowSave)
                    {
                        if (Service.AttachmentLinksSetVirtualDelete(idAttachmentLinks, true))
                        {
                            View.DisplayDeletedItems(idAttachmentLinks.Count);
                            View.SendUserAction(idCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectAttachmentsVirtualDeletedItems);
                        }
                        else {
                            View.SendUserAction(idCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectAttachmentsUnableToVirtualDeleteItems);
                            View.DisplayUnableToDeleteItems(idAttachmentLinks.Count);
                        }
                    }
                    else
                    {
                        View.SendUserAction(idCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ProjectAttachmentsNoPermissionToDeleteItems);
                        View.DisplayNoPermissionToDeleteItems(idAttachmentLinks.Count);
                    }
                }
                LoadAttachments(unknownUser, idProject, isPersonal, idCommunity, mPermission, pPermissions, allowSave, ModuleProjectManagement.ActionType.ProjectAttachmentsReload);
            }
        }
    }
}