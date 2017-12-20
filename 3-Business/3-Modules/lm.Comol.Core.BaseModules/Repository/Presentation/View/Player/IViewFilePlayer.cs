using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewFilePlayer  : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        int PreloadedIdCommunity {get;}
        int PreloadedIdModule{get;}
        int PreloadedIdUser{get;}
        int PreloadedIdAction{get;}
        Boolean PreloadedSavingStatistics { get; }
        Boolean RedirectToPage(RepositoryItemType type);
        String PreloadedItemTypeID {get;}
        long PreloadedIdFile{get;}
        System.Guid PreloadedFileUniqueID{get;}
        String  PreloadedLanguage{get;}
        long PreloadedIdLink{get;}
        System.Guid PlayerWorkingSessionID { get; }

        int PlayerIdCommunity {get;set;}
        int PlayerIdModule  {get;set;}
        long PlayerIdFile {get;set;}
        System.Guid PlayerFileUniqueID {get;set;}
        String PlayerItemTypeID {get;set;}
        long PlayerIdLink {get;set;}
        String PlayerModuleCode {get;set;}
        Boolean PlayerSavingStatistics { get; set; }
        Guid PlayUniqueSessionId { get; set; }
        String PlaySessionId { get; set; }
        
        long GetPermissionToLink(int IdUser, long IdLink, BaseCommunityFile file, int IdCommunity);

        void LoadFileNotExist();
        void LoadFileNoPermission();
        void LoadFileNoReadyToPlay(RepositoryItemType type, TransferStatus status);
        void InvalidFileTypeToPlay(RepositoryItemType type);
        void LoadFileIntoPlayer(String playSessionId,System.Guid WorkingSessionID, System.Guid FileUniqueID, int IdUser, long IdLink, long IdFile, RepositoryItemType type);
        void LoadMultimediaFileIntoPlayer(String playSessionId, System.Guid WorkingSessionID, System.Guid FileUniqueID, int IdUser, long IdLink, long IdFile, string defaultUrl);

        void SendToSessionExpiredPage(int idCommunity, String languageCode);
        void SaveLinkEvaluation(long idLink, int IdUser);
    }
}