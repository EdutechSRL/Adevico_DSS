using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewModuleRepositoryAction : IGenericModuleDisplayAction
    {
        Boolean InsideOtherModule { get; set; }
        String ServiceCode {get;set;}
        Int32 ServiceID {get;set;}
        Boolean IsReadyToPlay { get; set; }
        System.Guid WorkingSessionId { get; }
        ContentView PreLoadedContentView { get; }
        String DestinationUrl { get; }
        String ItemIdentifier { get; set; }
        String GetBaseUrl { get; }
       // String BaseUrl { get; }
        Int32 ForUserId { get; set; }
        Boolean AllowDownload(lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
        lm.Comol.Core.DomainModel.Repository.RepositoryItemType ItemType { get; set; }

        void DisplayFolder(String folderName);
        void DisplayCreateFolder(String folderName);
        void DisplayUploadFile(String folderName);
        void DisplayFolderAction(long idFolder, String folderName);
        void DisplayUploadAction(long idFolder, String folderName);

        void DisplayItem(String name, String extension,long size, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
        void DisplayFolder(String name, String url);
        void DisplayItem(String name, String url, String extension, long size, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
        void DisplayPlayItem(String name, String url, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
        void DisplayActions(List<dtoModuleActionControl> actions);

        String GetUrlForPlayScorm(long idLink, long idItem, Guid uniqueId, int idCommunity, int idModule, int idAction, Boolean noSaveStat);
        String GetUrlForPlayScorm(long idLink, long idItem, Guid uniqueId, int idCommunity, int idModule, Boolean noSaveStat);
        String GetUrlForMultimediaFile();
        void DisplayEmptyAction();
        void DisplayEmptyActions();

        String GetDisplayItemDescription(String name, String extension, long size, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
        String CreateFolderDescription(String name);
        String UploadFileDescription(String name);
        String FolderDescription(String name);
        //Dictionary<StandardActionType, String> GetActionsUrl(long idLink, long idItem, Int32 idCommunity, System.Guid uniqueId, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
        //Dictionary<StandardActionType, String> GetActionsUrl(long idLink, long idItem, Int32 idCommunity, System.Guid uniqueId, Int32 idAction, lm.Comol.Core.DomainModel.Repository.RepositoryItemType type);
     
        //String GetUrlForUpload(long idLink, long idFolder, Int32 idCommunity);
        //String GetUrlForCreateFolder(long idLink, long idFolder, Int32 idCommunity);
        //String GetUrlForFolder(long idLink, long idFolder, Int32 idCommunity);

        void DisplayPlaceHolders(List<dtoPlaceHolder> items);
        //void DisplayItemActions(Dictionary<StandardActionType, String> actions);
    }
}

// <CLSCompliant(True)> Public Interface ImoduleToRepositoryAction
//        Inherits iModuleActionView

//        Sub DisplayNoAction()
//        Sub DisplayAction()

//        Sub ActionForDownload(ByVal IdLink As Long, ByVal IdCommunity As Integer, ByVal item As BaseCommunityFile)
//        Sub ActionForPlay(ByVal IdLink As Long, ByVal IdCommunity As Integer, ByVal item As BaseCommunityFile)
//        Sub ActionForPlayInternal(ByVal IdLink As Long, ByVal IdCommunity As Integer, ByVal item As BaseCommunityFile, ByVal idAction As Integer)
//        Sub ActionForUpload(ByVal descriptionOnly As Boolean, ByVal IdLink As Long, ByVal idFolder As Long, ByVal FolderName As String, ByVal IdCommunity As Integer)
//        Sub ActionForCreateFolder(ByVal descriptionOnly As Boolean, ByVal IdLink As Long, ByVal idFolder As Long, ByVal FolderName As String, ByVal IdCommunity As Integer)

//        Sub DisplayItemAction(ByVal fileName As String, ByVal extension As String, ByVal size As Long, ByVal type As Repository.RepositoryItemType)

//        ReadOnly Person GetUrlForDownload(ByVal IdLink As Long, ByVal Idfile As Long, ByVal IdCommunity As Integer) As String
//        ReadOnly Person GetUrlForPlay(ByVal IdLink As Long, ByVal Idfile As Long, ByVal UniqueID As System.Guid, ByVal CommunityID As Integer, ByVal type As Repository.RepositoryItemType) As String
//        ReadOnly Person GetUrlForPlayInternal(ByVal IdLink As Long, ByVal Idfile As Long, ByVal UniqueID As System.Guid, ByVal CommunityID As Integer, ByVal ServiceActionID As Integer, ByVal type As Repository.RepositoryItemType) As String
//        ReadOnly Person GetUrlForUpload(ByVal IdLink As Long, ByVal FolderID As Long, ByVal IdCommunity As String) As String
//        ReadOnly Person GetUrlForCreateFolder(ByVal IdLink As Long, ByVal FolderID As Long, ByVal IdCommunity As Integer) As String
//        ReadOnly Person GetUrlForSettings(ByVal Idfile As Long, ByVal type As Repository.RepositoryItemType) As String
//        ReadOnly Person GetUrlForPersonalStatistics(ByVal Idfile As Long, ByVal type As Repository.RepositoryItemType) As String
//        ReadOnly Person GetUrlForAdvancedStatistics(ByVal Idfile As Long, ByVal type As Repository.RepositoryItemType) As String
//    End Interface
//End Namespace