using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain; 
namespace lm.Comol.Core.BaseModules.CommunityDiary.Presentation
{
    public interface IViewCommunityDiary : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 PreloadIdCommunity { get; }
        Boolean PreloadAscending { get; }
        String UnknownUserTranslation { get; }

        Int32 IdModuleCommunityDiary { get; set; }
        Int32 IdModuleRepository { get; set; }
        Int32 IdCommunityDiary { get; set; }
        
        Boolean AllowAddItem{get;set;}
        Boolean AllowPrint{get;set;}
        Boolean AllowDeleteDiary{ get; set; }
        Boolean AllowMultipleDelete{get;set;}
        Boolean AllowItemsSelection{get;set;}
        Boolean AllowImport{get;set;}

        String BaseFolder{get;}
        Boolean DisplayOrderAscending{get;set;}
        List<long> SelectedItems{get;set;}

        void SetAddItemUrl(Int32 idCommunity);

        void DisplaySessionTimeout(Int32 idCommunity, long idItem = 0);
        void LoadItems(List<dtoDiaryItem> items, int idCommunity, int IdModule);
        void SetTitleName(String communityName);
        void HideItemsForNoPermission(int idCommunity, int IdModule);
        void ShowUnkownCommunityDiary(int idCommunity, int IdModule);

        String GetPortalNameTranslation();
        lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier RepositoryIdentifier { get; set; }

        void InitializeAttachmentsControl(List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction);
    }
}
