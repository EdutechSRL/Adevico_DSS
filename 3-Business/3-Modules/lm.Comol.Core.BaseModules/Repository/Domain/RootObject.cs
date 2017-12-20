using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Domain
{
    public class RootObject
    {

        public static string PublishIntoRepositoryFromModule()
        {
            return "Modules/Common/PublishIntoRepositoryFromModuleItem.ascx";
        }
        public static string DisplayModuleActionControl()
        {
            return "Modules/Repository/UC/UC_ModuleRepositoryAction.ascx";
        }

        //public static string DisplayActionControl()
        //{
        //    return "Modules/Repository/UC/UC_ModuleToRepositoryAction.ascx";
        //}
        public static string CreateActionControl()
        {
            return "Modules/Repository/UC/UC_ModuleToRepository.ascx";
        }  
        public static string DisplayActionControlQuiz()
        {
            return "Modules/EduPath/UC/UC_ModuleToQuizAction.ascx";
        }
        public static string DownloadFile(long idFile, long idUser, string languageCode, System.Guid wSessionId)
        {
            return "Modules/Repository/File.repository?FileID=" + idFile.ToString() + "&ForUserID=" + idUser.ToString() + "&Language=" + languageCode + "&wSessionId=" + wSessionId.ToString();
        }
        public static string DownloadFileFromModule(long idFile, long idUser, string languageCode, System.Guid wSessionId, int moduleId, int communityId, long linkId, Boolean notSaveStat)
        {
            return DownloadFile(idFile, idUser, languageCode, wSessionId) + "&LinkID=" + linkId.ToString() + "&ModuleID=" + moduleId.ToString() + "&CommunityID=" + communityId.ToString() + ((notSaveStat) ? "&notSaveStat=" + (notSaveStat).ToString() : "");
        }

        public static string ModuleEditingSingleItemPermission(long fileId, long folderId, int communityId, string backUrl, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" + fileId.ToString() + "&FolderID=" + folderId.ToString() + "&CommunityID=" + communityId.ToString() + "&Action=SingleUpload&BackUrl=" + backUrl + "&PreserveUrl=True&ForService=true" + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }

         public static string RepositorySingleItemPermission(long fileId, long folderId, int communityId, string currentView, string previousPage, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" + fileId.ToString() + "&FolderID=" + folderId.ToString() + "&CommunityID=" + communityId.ToString() + "&View=" + currentView + "&PreviousPage=" + previousPage  + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }

        public static string AjaxScormAction(long fileId,  int communityId, int IdModule, System.Guid workingSessionID, int IdAction)
        {
            return "Modules/Common/CommonActionSenderAjax.aspx?&CommunityID=" + communityId.ToString() + "&ModuleID=" + IdModule.ToString() + "&WorkingSessionID=" + workingSessionID.ToString() + "&ActionID=" + IdAction.ToString() + "&fileId=" + fileId.ToString();
        }
        public static string AjaxScormStatistics(long fileId, int communityId, int IdModule, System.Guid workingSessionID, int IdAction, long IdLink, System.Guid uniqueId)
        {
            return "Modules/Common/CommonActionSenderAjax.aspx?ForScorm=true&CommunityID=" + communityId.ToString() + "&ModuleID=" + IdModule.ToString() + "&WorkingSessionID=" + workingSessionID.ToString() + "&ActionID=" + IdAction.ToString() + "&LinkID=" + IdLink.ToString() + "&Id=" + uniqueId.ToString() + "&fileId=" + fileId.ToString();
        }
      

        public static string RepositoryUploadFile(long IdFolder, int IdCommunity,string previousView, string previousPage, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryUpload.aspx?Create=" + ItemRepositoryToCreate.File.ToString() + "&FolderID=" + IdFolder.ToString() + "&PreviousView=" + previousView + "&CommunityID=" + IdCommunity.ToString() + "&PreviousPage=" + previousPage + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryCreateFolder(long IdFolder, int IdCommunity, string previousView, string previousPage, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryUpload.aspx?Create=" + ItemRepositoryToCreate.Folder.ToString() + "&FolderID=" + IdFolder.ToString() + "&PreviousView=" + previousView + "&CommunityID=" + IdCommunity.ToString() + "&PreviousPage=" + previousPage + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }

        public static string RepositoryUploadMultipleFile(long IdFolder, int IdCommunity,string previousView, string previousPage, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryMultipleUpload.aspx?Create=" + ItemRepositoryToCreate.File.ToString() + "&FolderID=" + IdFolder.ToString() + "&PreviousView=" + previousView + "&CommunityID=" + IdCommunity.ToString() + "&PreviousPage=" + previousPage + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryCreateItem(long IdFolder, int IdCommunity, ItemRepositoryToCreate type, string previousView, string previousPage, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryUpload.aspx?Create=" + type.ToString() + "&FolderID=" + IdFolder.ToString() + "&PreviousView=" + previousView + "&CommunityID=" + IdCommunity.ToString() + "&PreviousPage=" + previousPage + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryMultipleDelete(long IdFolder, int IdCommunity,string previousView, int pageIndex, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryMultipleDelete.aspx?FolderID=" + IdFolder.ToString() + "&View=" + previousView + "&CommunityID=" + IdCommunity.ToString() + "&Page=" + pageIndex.ToString() + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryImport(long IdFolder, int IdCommunity,string previousView, int pageIndex, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryImport.aspx?FolderID=" + IdFolder.ToString() + "&PreviousView=" + previousView + "&CommunityID=" + IdCommunity.ToString() + "&Page=" + pageIndex.ToString() + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }

        public static string RepositoryCurrentList(long IdFolder, int IdCommunity, string currentView, int pageIndex, ContentView view)
        {
            return "Modules/Repository/CommunityRepository.aspx?FolderID=" + IdFolder.ToString() + "&View=" + currentView + "&CommunityID=" + IdCommunity.ToString() + "&Page=" + pageIndex.ToString() + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryList(long IdFolder, int IdCommunity,string previousView, int pageIndex, ContentView view)
        {
            return "Modules/Repository/CommunityRepository.aspx?FolderID=" + IdFolder.ToString() + "&PreviousView=" + previousView + "&CommunityID=" + IdCommunity.ToString() + "&Page=" + pageIndex.ToString() + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }

        public static string ManagementScormStatistics(long IdFile, string backUrl, ContentView view)
        {
            return "Modules/Scorm/ScormStatisticheMain.aspx?FileID=" + IdFile.ToString() + "&BackUrl=" + backUrl + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string ManagementScormStatistics(long IdFile, Int32 idUser, string backUrl, ContentView view)
        {
            return "Modules/Scorm/ScormStatisticheMain.aspx?UserID=" + idUser.ToString()+  "&FileID=" + IdFile.ToString() + "&BackUrl=" + backUrl + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string UserScormStatistics(long IdFile, string backUrl, ContentView view)
        {
            return "Modules/Scorm/ScormStatisticheUtente.aspx?FileID=" + IdFile.ToString() + "&BackUrl=" + backUrl + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
       
        public static string RepositoryEdit(long IdItem, long IdFolder, int IdCommunity, string currentView, string previousPage, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryEdit.aspx?ItemID=" + IdItem.ToString() + "&FolderID=" + IdFolder.ToString() + "&CommunityID=" + IdCommunity.ToString() + "&View=" + currentView + "&PreviousPage=" + previousPage + "&CommunityID=" + IdCommunity.ToString() + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }

        public static string RepositoryListFormatUrl(int IdCommunity, string currentView, ContentView view)
        {
            return "{0}Modules/Repository/CommunityRepository.aspx?Page=0&FolderID={1}&CommunityID=" + IdCommunity.ToString() + "&View=" + currentView + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryManagementFormatUrl(int IdCommunity, string currentView, ContentView view)
        {
            return "{0}Modules/Repository/CommunityRepositoryManagement.aspx?Page=0&FolderID={1}&CommunityID=" + IdCommunity.ToString() + "&View=" + currentView + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryManagement(long IdFolder, int IdCommunity,string currentView, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryManagement.aspx?Page=0&FolderID=" + IdFolder.ToString() + "&CommunityID=" + IdCommunity.ToString() + "&View=" + currentView + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryManagement(long IdFolder, int IdCommunity, string currentView, int pageIndex, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryManagement.aspx?Page=0&FolderID=" + IdFolder.ToString() + "&CommunityID=" + IdCommunity.ToString() + "&View=" + currentView + "&Page=" + pageIndex.ToString() + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string RepositoryError(string pageSender, string previousView, string previousPage, ContentView view)
        {
            return "Modules/Repository/CommunityRepositoryItemError.aspx?PageSender=" + pageSender + "&PreviousView=" + previousView + "&PreviousPage=" + previousPage + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }

        public static string EditScormPackageSettings(long IdFile, string backUrl, ContentView view)
        {
            return "Modules/Scorm/ScormPackageSettings.aspx?FileID=" + IdFile.ToString() + "&BackUrl=" + backUrl + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string EditScormPackageSettings(long IdFile, long IdLink, string backUrl, ContentView view)
        {
            return "Modules/Scorm/ScormPackageSettings.aspx?FileID=" + IdFile.ToString() + "&LinkID=" + IdLink.ToString() +  "&BackUrl=" + backUrl + ((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string EditMultimediaFileSettings(long IdFile, string backUrl, ContentView view)
        {
            return "Modules/Repository/MultimediaFileSettings.aspx?FileID=" + IdFile.ToString() + "&BackUrl=" + backUrl + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }
        public static string EditMultimediaFileSettings(long IdFile, long IdLink, string backUrl, ContentView view)
        {
            return "Modules/Repository/MultimediaFileSettings.aspx?FileID=" + IdFile.ToString() + "&LinkID=" + IdLink.ToString() + "&BackUrl=" + backUrl + ((view == ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
        }

        public static string PlayMultimediaFile(long IdFile, System.Guid uniqueId, int idUser, string language, Boolean notSaveStat, ContentView view)
        {
            String url= "Modules/Repository/MultimediaFileLoader.aspx";
            url += "?FileID=" + IdFile.ToString();
            url += "&FileUniqueID=" + uniqueId.ToString();
            url += "&ForUserID=" + idUser.ToString();
            url += "&Language=" + language;
            if (notSaveStat)
                url += "&notSaveStat=" + notSaveStat.ToString();
            url +=((view== ContentView.viewAll) ? "" : "&CV=" + ((int)view).ToString());
            return url;
        }
        public static string PlayMultimediaFile(long idLink, long idFile, System.Guid uniqueId, int idUser, string language, Boolean notSaveStat, ContentView view)
        {
            String url = PlayMultimediaFile(idFile, uniqueId, idUser, language,notSaveStat, view);
            url += "&LinkId=" + idLink.ToString();
            return url;
        }
        public static string PlayMultimediaFileFromModule(long idFile, System.Guid uniqueId, int idUser, string language, Boolean notSaveStat, int idModule, int idCommunity, long idLink, int Idaction, ContentView view)
        {
            String url = PlayMultimediaFile(idFile, uniqueId, idUser, language,notSaveStat, view);
            url += "&ModuleID=" + idModule.ToString();
            url += "&CommunityID=" + idCommunity.ToString();
            url += "&ActionID=" + Idaction.ToString();
            url += "&LinkId=" + idLink.ToString();
            return url;
        }

    }
}
