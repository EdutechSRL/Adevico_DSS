using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public interface IViewAddAttachment : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions CurrentAction { get; set; }
        long IdMessage { get; set; }
        long IdTicketUser { get; set; }
        Int32 IdTicketCommunity { get; set; }

        Boolean AllowAnonymousUpload { get; set; }
        Boolean isInitialized { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean DisplayCommands { get; set; }
        Boolean RaiseCommandEvents { get; set; }



        void InitializeControl(long idMessage, RepositoryAttachmentUploadActions action, Int32 idCommunity, Boolean allowAnonymousUpload, Int32 idUser = 0, String dialogclass = "");
        void InitializeControl(long idMessage,RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, ModuleRepository rPermissions);
        void InitializeControl(long idMessage,RepositoryAttachmentUploadActions action, Int32 idCommunity, ModuleRepository rPermissions);

        void InitializeLinkRepositoryItems(Int32 idUser, ModuleRepository rPermissions, RepositoryIdentifier identifier, List<RepositoryItemLinkBase<long>> alreadyLinkedFiles);
        //void InitializeLinkRepositoryItems(Int32 idUser, ModuleRepository rPermissions, RepositoryIdentifier identifier, List<RepositoryItemLinkBase<long>> alreadyLinkedFiles);
        void InitializeCommunityUploader(RepositoryIdentifier identifier);
        void InitializeUploaderControl(RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, Int32 idUploaderUser);

        void DisplayWorkingSessionExpired();
        void DisplayItemsAdded();
        void DisplayItemsNotAdded();
        void DisplayMessageNotFound();
        void DisplayNoFilesToAdd();
        List<dtoModuleUploadedItem> UploadFiles( Boolean addToRepository, Message message);
    }
}