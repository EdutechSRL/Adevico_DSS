using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Presentation
{
    public interface IviewDiaryItem : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Int32 PreloadIdCommunity { get; }
        long PreloadIdItem { get; }
        Boolean PreloadIsInsertMode { get; }
        Boolean AllowEdit { set; }
        Boolean AllowFileManagement { set; }
        long CurrentIdItem { get; set; }
        long CurrentIdEvent { get; set; }
        Int32 IdCommunityDiary { get; set; }
        CommunityEventItem CurrentItem { get; }
        //Boolean AllowPublish { get; }
        String UnknownUserTranslation { get; }
        Int32 IdModuleCommunityDiary { get; set; }
        Int32 IdModuleRepository { get; set; }

        String GetPortalNameTranslation();


        void LoadItem(CommunityEventItem item, String description, String communityName, List<lm.Comol.Core.Events.Domain.dtoAttachmentItem> attachments);
        void LoadAttachments(List<lm.Comol.Core.Events.Domain.dtoAttachmentItem> attachments);
        void SendToItemsList(Int32 idCommunity, long goToIdItem);
        void NoPermission(Int32 idCommunity, Int32 idModule);
        void ShowNoItemWithThisID(Int32 idCommunity, Int32 idModule, long idItem);
        void SetBackToDiary(Int32 idCommunity, long idItem);
        void UpdateItemData(DateTime StartDate, DateTime EndDate);
        void DisplaySessionTimeout(Int32 idCommunity, long idItem = 0);
        lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier RepositoryIdentifier { get; set; }
        void InitializeAttachmentsControl(long idEvent, long idItem, List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction);
    }
}