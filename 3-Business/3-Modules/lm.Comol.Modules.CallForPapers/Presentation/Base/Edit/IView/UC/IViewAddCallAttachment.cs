using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewAddCallAttachment : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions CurrentAction { get; set; }
        long IdCall { get; set; }
        CallForPaperType CallType { get; set; }
        Int32 IdCallCommunity { get; set; }
        Boolean isInitialized { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean DisplayCommands { get; set; }
        Boolean RaiseCommandEvents { get; set; }

        void InitializeControl(long idCall, CallForPaperType type, RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, String description = "", String dialogclass = "");
        void InitializeLinkRepositoryItems(Int32 idUser, ModuleRepository rPermissions, RepositoryIdentifier identifier, List<RepositoryItemLinkBase<long>> alreadyLinkedFiles);
        void InitializeCommunityUploader(RepositoryIdentifier identifier);
        void InitializeUploaderControl(RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier);

        void DisplayWorkingSessionExpired();
        void DisplayItemsAdded();
        void DisplayItemsNotAdded();
        void DisplayNoFilesToAdd();
        void DisplayCallNotFound(CallForPaperType type);

        List<dtoModuleUploadedItem> UploadFiles(String moduleCode, Int32 idObjectType, Int32 idAction,  Boolean addToRepository);
        void SendUserAction(int idCommunity, int idModule, long idCall,long idAttachment, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, long idAttachment, ModuleRequestForMembership.ActionType action);
    }
}