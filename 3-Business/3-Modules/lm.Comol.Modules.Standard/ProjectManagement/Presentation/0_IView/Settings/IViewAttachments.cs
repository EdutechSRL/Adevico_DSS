using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Core.FileRepository.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewAttachments : IViewBaseEdit
    {
        Boolean AllowSave { get; set; }
        String UnknownUserTranslation { get; }
        RepositoryIdentifier RepositoryIdentifier { get; set; }
        void DisplayUnknownProject();
        void DisplayUnableToDeleteItems(long count);
        void DisplayNoPermissionToDeleteItems(long count);
        void DisplayDeletedItems(long count);
        void InitializeAttachmentsControl(RepositoryIdentifier identifier,ModuleRepository cRepository,List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction = Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none);
        void LoadAttachments(List<dtoAttachmentItem> items);
        void DisplayUrlForEditing(dtoUrl urlItem);
        void DisplayNoPermissionToEditUrl();
        void DisplayUrlRemoved();
        void DisplayUnableToSaveUrl(dtoUrl urlItem);
        void DisplaySavedUrl(dtoUrl urlItem);
        void DisplayUnableToSaveEmptyUrl(dtoUrl urlItem);
        
        void SendUserAction(int idCommunity, int idModule, long idProject,long idAttachment, ModuleProjectManagement.ActionType action);
    }
}