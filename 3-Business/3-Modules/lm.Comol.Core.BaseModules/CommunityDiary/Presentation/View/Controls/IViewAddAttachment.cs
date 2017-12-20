using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Presentation
{
    public interface IViewAddAttachment : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions CurrentAction { get; set; }
        long IdEvent { get; set; }
        long IdEventItem { get; set; }
        Int32 IdEventCommunity { get; set; }
        Int32 LessonNumber { get; set; }
        Boolean isInitialized { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean DisplayCommands { get; set; }
        Boolean RaiseCommandEvents { get; set; }
        Boolean DisplayInfo { get; set; }

        void InitializeControl(long idEvent, long idEventItem, Int32 lessonNumber, RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, String description = "", String dialogclass = "");
        void InitializeControl(long idEvent, long idEventItem, Int32 lessonNumber, RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier, ModuleRepository rPermissions, String description = "", String dialogclass = "");

        void InitializeLinkRepositoryItems(Int32 idUser, ModuleRepository rPermissions, RepositoryIdentifier identifier, List<RepositoryItemLinkBase<long>> alreadyLinkedFiles);
        void InitializeCommunityUploader(RepositoryIdentifier identifier);
        void InitializeUploaderControl(RepositoryAttachmentUploadActions action, RepositoryIdentifier identifier);

        void DisplayWorkingSessionExpired();
        void DisplayItemsAdded();
        void DisplayItemsNotAdded();
        void DisplayEventItemNotFound();
        void DisplayNoFilesToAdd();
        List<dtoModuleUploadedItem> UploadFiles(String moduleCode , Int32 idObjectType , Int32 idAction,  Boolean addToRepository);
        void SendUserAction(int idCommunity, int idModule, long idEventItem, ModuleCommunityDiary.ActionType action);
        void DisplayEventItemInfo(String title, DateTime startOn, DateTime endOn, Boolean showDateInfo,Int32 minutesDuration,Int32 lessonNumber=0);
    }
}