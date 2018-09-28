using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.FileRepository.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewAddAttachment : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions CurrentAction { get; set; }
        long IdProject { get; set; }
        long IdActivity { get; set; }
        Int32 IdProjectCommunity { get; set; }
        Boolean isInitialized { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean DisplayCommands { get; set; }
        Boolean RaiseCommandEvents { get; set; }

        void InitializeControl(long idProject, RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, String description = "", String dialogclass = "");
        void InitializeControl(long idProject, long idActivity, RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, String description = "", String dialogclass = "");
        void InitializeControl(long idProject, RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, ModuleRepository rPermissions, String description = "", String dialogclass = "");
        void InitializeControl(long idProject, long idActivity, RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, ModuleRepository rPermissions, String description = "", String dialogclass = "");

        void InitializeLinkRepositoryItems(Int32 idUser, ModuleRepository rPermissions, RepositoryIdentifier identifier, List<RepositoryItemLinkBase<long>> alreadyLinkedFiles);
        void InitializeCommunityUploader(RepositoryIdentifier identifier);
        void InitializeUploaderControl(RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier);

        void DisplayWorkingSessionExpired();
        void DisplayItemsAdded();
        void DisplayItemsNotAdded();
        void DisplayProjectNotFound();
        void DisplayActivityNotFound();
        void DisplayNoFilesToAdd();
        List<dtoModuleUploadedItem> UploadFiles(Project project,  Boolean addToRepository);
        List<dtoModuleUploadedItem> UploadFiles(PmActivity activity, Boolean addToRepository);
        void SendUserAction(int idCommunity, int idModule, long idProject, long idActivity, ModuleProjectManagement.ActionType action);
    }
}