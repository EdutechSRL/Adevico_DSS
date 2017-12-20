using System;

namespace lm.Comol.Core.FileRepository.Domain
{
    public class RootObject
    {
        private readonly static String modulehome = "Modules/Repository/";

        #region "Repository"
            public static String BaseRepositoryUrl()
            {
                return modulehome + "List.aspx?";
            }
        public static String RepositoryItems(RepositoryType type, Int32 idCommunity, long idItem = -1, long idFolder = long.MinValue, FolderType folderType = FolderType.none, OrderBy orderBy = OrderBy.none, Boolean? ascending = null, String identifierPath = "")
            {
                return modulehome + "List.aspx?" + BaseParameters(type, idCommunity, idItem, idFolder, folderType, orderBy, ascending) + (String.IsNullOrWhiteSpace(identifierPath) ? "" : "&path=" + System.Web.HttpUtility.UrlEncode(identifierPath));
            }
        public static String Repository(RepositoryType type, Int32 idCommunity, long idItem = long.MinValue, long idFolder = long.MinValue, FolderType folderType = FolderType.none, OrderBy orderBy = OrderBy.none, Boolean? ascending = null, String identifierPath = "")
            {
                return modulehome + "List.aspx?" + BaseParameters(type, idCommunity, idItem, idFolder, folderType, orderBy, ascending) + (String.IsNullOrWhiteSpace(identifierPath) ? "" : "&path=" + System.Web.HttpUtility.UrlEncode(identifierPath));
            }
            private static String BaseParameters(RepositoryType type, Int32 idCommunity, long idItem = long.MinValue, long idFolder =long.MinValue, FolderType folderType = FolderType.none, OrderBy orderBy = OrderBy.none, Boolean? ascending = null, Boolean withPlaceHolders = false)
            {
                return "type=" + type.ToString() + ((idCommunity > -1) ? "&idCommunity=" + idCommunity.ToString() : "")
                    + BaseParameters(false,idItem, idFolder)
                    + ((folderType!= FolderType.none ) ? "&ftype=" + folderType.ToString() : "")
                    + BaseDisplayParameters(withPlaceHolders, orderBy, ascending);
            }
            private static String BaseParameters(Boolean isStartParameter=false,long idItem = long.MinValue, long idFolder = long.MinValue, Boolean setPrevious = false)
            {
                String parameters = ((idItem > long.MinValue) ? "&idItem=" + idItem.ToString() : "") + ((idFolder > long.MinValue) ? "&idFolder=" + idFolder.ToString() : "") + GetSetBackUrlParameter(setPrevious);
                if (isStartParameter && parameters.StartsWith("&"))
                    parameters = parameters.Substring(1);

                return parameters;
            }
            private static String BaseDisplayParameters(Boolean withPlaceHolders, OrderBy orderBy = OrderBy.none, Boolean? ascending = null)
            {
                return (withPlaceHolders) ? "&o=#OrderBy#&asc=#Boolean#"
                    : ((orderBy != OrderBy.none) ? "&o=" + orderBy.ToString() : "")
                    + ((ascending.HasValue) ? "&asc=" + ascending.ToString().ToLower() : "");
            }
            public static String FolderUrlTemplate(long idFolder, FolderType folderType, RepositoryType type, Int32 idCommunity)
            {
                return modulehome + "List.aspx?" + BaseParameters(type, idCommunity,  long.MinValue, idFolder, folderType, OrderBy.none, null, true);
            }
            public static String FolderUrlTemplate(long idFolder, FolderType folderType,String identifierPath, RepositoryType type, Int32 idCommunity)
            {
                return FolderUrlTemplate(idFolder, folderType, type, idCommunity) + "#folder_" + idFolder.ToString();
            }

           
        #endregion

        #region "Management"
            public static String EditItem(long idItem, long idFolder, String identifierPath, ItemAction action, Boolean setPrevious = true, String backUrl = "")
            {
                return modulehome + "Edit.aspx?" + BaseParameters(true,idItem, idFolder, setPrevious) + GetBackUrlParameter(backUrl) + (String.IsNullOrWhiteSpace(identifierPath) ? "" : "&path=" + System.Web.HttpUtility.UrlEncode(identifierPath)) + "#action_" + action.ToString();
            }
            public static String Details(long idItem, long idFolder, String identifierPath, ItemAction action, Boolean setPrevious = true, String backUrl = "")
            {
                return modulehome + "Details.aspx?" + BaseParameters(true, idItem, idFolder, setPrevious) + GetBackUrlParameter(backUrl) + (String.IsNullOrWhiteSpace(identifierPath) ? "" : "&path=" + System.Web.HttpUtility.UrlEncode(identifierPath)) + "#action_" + action.ToString();
            }

        
            public static String Statistics(long idFolder, long idItem, Boolean setPrevious = true, String backUrl = "")
            {
                return Statistics(ItemStatisticType.Download | ItemStatisticType.Play | ItemStatisticType.Scorm, idFolder, idItem, setPrevious, backUrl);
            }
         
            public static String Statistics(ItemStatisticType type, long idFolder, long idItem, Boolean setPrevious = true, String backUrl = "")
            {
                return modulehome + "Statistics.aspx?sType=" + type.ToString() + BaseParameters(false, idItem, idFolder, setPrevious) + GetBackUrlParameter(backUrl);
            }

         
            public static String MyStatistics(long idFolder, long idItem, Boolean setPrevious = true, String backUrl = "")
            {
                return Statistics(ItemStatisticType.Download | ItemStatisticType.Play | ItemStatisticType.Scorm, idFolder, idItem, setPrevious, backUrl);
            }
         
            public static String MyStatistics(ItemStatisticType type, long idFolder, long idItem, Boolean setPrevious = true, String backUrl = "")
            {
                return modulehome + "MyStatistics.aspx?sType=" + type.ToString() + BaseParameters(false, idItem, idFolder, setPrevious) + GetBackUrlParameter(backUrl);
            }

           
            public static String MyStatistics(ItemStatisticType type,long idItem, long idVersion, long idLink = 0, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "MyStatistics.aspx?sType=" + type.ToString() + UrlItemParameters(false,idItem, idVersion, idLink);
                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String UserStatistics(ItemStatisticType type, Int32 idUser, long idItem, long idVersion, long idLink = 0, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "UserStatistics.aspx?sType=" + type.ToString() + "&idUser=" + idUser.ToString() + UrlItemParameters(false, idItem, idVersion, idLink); 
                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String Statistics(ItemStatisticType type, long idItem, long idVersion, long idLink = 0, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "Statistics.aspx?sType=" + type.ToString() + UrlItemParameters(false, idItem, idVersion, idLink);
                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
        #endregion

        #region "Setting"
            public static String EditMultimediaSettings(long idItem, long idVersion, long idLink, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "MultimediaSettings.aspx?idLink=" + idLink.ToString() + UrlItemParameters(false,idItem, idVersion);
                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String EditMultimediaSettings(long idItem,long idVersion, long idFolder, String identifierPath, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "MultimediaSettings.aspx?" + UrlItemParameters(true, idItem, idVersion) + (idFolder > 0 ? "&idFolder=" + idFolder.ToString() : "") + (String.IsNullOrWhiteSpace(identifierPath) ? "" : "&path=" + System.Web.HttpUtility.UrlEncode(identifierPath));


                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String EditScormSettings(long idItem, long idVersion,long idLink, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "ScormSettings.aspx?idLink=" + idLink.ToString() + UrlItemParameters(false, idItem, idVersion);
                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String EditScormSettings(long idItem, long idVersion, long idFolder, String identifierPath, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "ScormSettings.aspx?" + UrlItemParameters(true, idItem, idVersion) + (idFolder > 0 ? "&idFolder=" + idFolder.ToString() : "") + (String.IsNullOrWhiteSpace(identifierPath) ? "" : "&path=" + System.Web.HttpUtility.UrlEncode(identifierPath));


                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String ViewScormSettings(long idItem, long idVersion, long idSettings, Boolean onModal = true)
            {
                return modulehome + "ViewScormSettingsOnModal.aspx?" + UrlItemParameters(true,idItem, idVersion) + (idSettings > 0 ? "&idSettings=" + idSettings.ToString() : "");
            }
        #endregion

         #region "File access"
            public static String OldDownload(String baseUrl, long idItem)
            {
                return baseUrl + "File.repository?FileID=" + idItem.ToString();
            }
            public static String Download(String baseUrl, long idItem, long idVersion, String name, Guid idNews)
            {
                return DownloadUrl(baseUrl, idItem, idVersion, name, idNews.ToString());
            }
            public static String Download(String baseUrl, long idItem, long idVersion, String name, Guid workingSessionId, Guid newsGuid , long idModule = -1, long idLink = -1)
            {
                return DownloadUrl(baseUrl, idItem, idVersion, name, (workingSessionId == Guid.Empty ? "" : workingSessionId.ToString()), (newsGuid == Guid.Empty ? "" : newsGuid.ToString()), idModule, idLink);
            }
            public static String Download(String baseUrl, long idItem, long idVersion, String name, String workingSessionId = "", String newsGuid = "", long idModule = -1, long idLink = -1)
            {
                return DownloadUrl(baseUrl, idItem, idVersion, name, workingSessionId, newsGuid, idModule, idLink);
            }
            private static String DownloadUrl(String baseUrl, long idItem, long idVersion, String name, String workingSessionId = "", String newsGuid = "", long idModule = -1, long idLink = -1)
            {
                String url = baseUrl + idItem.ToString() + "/" + idVersion.ToString()
                  + "/file.download" + GetDownloadQuery(workingSessionId, newsGuid, idModule, idLink); /*" + SanitizeFileName(name) + "*/

                return url;
                //return baseUrl + idItem.ToString() + "/" + idVersion.ToString() + "/" + (String.IsNullOrEmpty(newsGuid) ? "0" : newsGuid)
                //    + "/" + idModule.ToString() + "/" + idLink.ToString()
                //    +"/" + System.Web.HttpUtility.UrlPathEncode(name) + "/Download/";
            }
            
            /*
            private static string SanitizeFileName(string name)
            {
                name = name.Replace("&", "-");
                name = name.Replace("<", "-");
                name = name.Replace(">", "-");
                name = name.Replace("*", "-");
                name = name.Replace("%", "-");
                name = name.Replace(":", "-");
                name = name.Replace("\\", "-");
                return System.Web.HttpUtility.UrlPathEncode(name);
            }*/

            public static string DownloadFromModule(String baseUrl, long idItem, long idVersion, String name, DisplayMode mode,String workingSessionId, long idModule, long idLink, Boolean notSaveStat, Boolean? setPrevious = null, String backUrl = "")
            {
                String url = DownloadUrl(baseUrl, idItem, idVersion, name, workingSessionId, "", idModule, idLink) + ((notSaveStat) ? "&notSaveStat=" + (notSaveStat).ToString() : "");
                if (setPrevious == null)
                    setPrevious = !(mode == DisplayMode.downloadOrPlayOrModal || mode == DisplayMode.inModal);

                if (!String.IsNullOrEmpty(backUrl) && setPrevious.Value)
                    url += GetSetBackUrlParameter(setPrevious.Value) + GetBackUrlParameter(backUrl);
                return url;
            }

            public static String DownloadError(DownloadErrorType type, Boolean loggedUser, Boolean inModalWindow, long idItem, long idVersion, String name, Guid workingSessionId, Guid idNews, long idModule = -1, long idLink = -1)
            {
                String url = "Modules/Repository/ItemDownload" + ((loggedUser || inModalWindow) ? "" : "External") + (!inModalWindow ? "" : "Modal") + ".aspx?type=" + type.ToString() + "&idItem=" + idItem.ToString() + "&idVersion=" + idVersion.ToString() +
                    GetDownloadQuery((workingSessionId == Guid.Empty ? "" : workingSessionId.ToString()), (idNews == Guid.Empty ? "" : idNews.ToString()), idModule, idLink, true);
                return url;
            }
            private static String GetDownloadQuery(String workinkSessionId = "", String newsGuid = "", long idModule = -1, long idLink = -1, Boolean isAdded = false )
            {
                String url = "";
                if (!String.IsNullOrWhiteSpace(newsGuid))
                    url += (isAdded ? "&" : "?") + QueryKeyNames.NewsId.ToString() + "=" + newsGuid;
                if (idLink > 0)
                {
                    url += ((!isAdded && String.IsNullOrWhiteSpace(newsGuid)) ? "?" : "&");
                    url += QueryKeyNames.idModule.ToString() + "=" + idModule.ToString() + "&" + QueryKeyNames.idLink.ToString() + "=" + idLink.ToString();
                }
                if (!String.IsNullOrWhiteSpace(workinkSessionId))
                {
                    url += ((!isAdded && String.IsNullOrWhiteSpace(newsGuid) && idLink <= 0) ? "?" : "&");
                    url += QueryKeyNames.wSessionId.ToString() + "=" + workinkSessionId;
                }
                return url;
            }

            public static String PlayForRepository(lm.Comol.Core.FileRepository.Domain.liteRepositoryItem item, Boolean? setPrevious = null, String backUrl = "")
            {
                if (setPrevious == null)
                    setPrevious = !(item.DisplayMode == DisplayMode.downloadOrPlayOrModal || item.DisplayMode == DisplayMode.inModal);

                if (!setPrevious.Value)
                    backUrl = "";
                return FullPlay(item.Id, item.UniqueId, item.IdVersion, item.UniqueIdVersion, item.Type, 0, "", false, false, (item.DisplayMode == DisplayMode.downloadOrPlayOrModal || item.DisplayMode == DisplayMode.inModal), false, setPrevious.Value, backUrl);
            }
            public static String PlayForRepository(lm.Comol.Core.FileRepository.Domain.RepositoryItem item, Boolean? setPrevious = null, String backUrl = "")
            {
                if (setPrevious == null)
                    setPrevious = !(item.DisplayMode == DisplayMode.downloadOrPlayOrModal || item.DisplayMode == DisplayMode.inModal);

                if (!setPrevious.Value)
                    backUrl = "";
                return FullPlay(item.Id, item.UniqueId, item.IdVersion, item.UniqueIdVersion, item.Type, 0, "", false, false, (item.DisplayMode == DisplayMode.downloadOrPlayOrModal || item.DisplayMode == DisplayMode.inModal), false, setPrevious.Value, backUrl);
            }
            public static String PlayForRepository(lm.Comol.Core.FileRepository.Domain.dtoDisplayRepositoryItem item, Boolean? setPrevious = null, String backUrl = "")
            {
                if (setPrevious == null)
                    setPrevious = !(item.DisplayMode == DisplayMode.downloadOrPlayOrModal || item.DisplayMode == DisplayMode.inModal);

                if (!setPrevious.Value)
                    backUrl="";
                return FullPlay(item.Id, item.UniqueId, item.IdVersion, item.UniqueIdVersion, item.Type,0, "", false, false, (item.DisplayMode == DisplayMode.downloadOrPlayOrModal || item.DisplayMode == DisplayMode.inModal), false, setPrevious.Value, backUrl);
            }
            public static String PlayForRepository(long idItem, long idVersion, ItemType type, DisplayMode mode, Boolean setPrevious = true, String backUrl = "")
            {
                return Play(idItem, idVersion, type, "", false, false, (mode == DisplayMode.downloadOrPlayOrModal || mode == DisplayMode.inModal), false, setPrevious, backUrl);
            }

            public static String Play(long idItem, Guid uniqueIdItem, long idVersion, Guid uniqueIdVersion, ItemType type, String language, Boolean notSaveStat, Boolean refreshContainerPage, Boolean onModalPage, Boolean saveCompleteness, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "Player" + (onModalPage ? "OnModal" : "") + ".aspx?type=" + type.ToString() + "&idItem=" + idItem.ToString() + "&idVersion=" + idVersion.ToString();
                url += "&Language=" + language;
                url += PlayBaseParameters(notSaveStat, refreshContainerPage, onModalPage, saveCompleteness);
                url += UrlGuidParameters(uniqueIdItem, uniqueIdVersion);
                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String Play(long idItem, long idVersion, ItemType type, String language, Boolean notSaveStat, Boolean refreshContainerPage, Boolean onModalPage, Boolean saveCompleteness, Boolean setPrevious = true, String backUrl = "")
            {
                String url = modulehome + "Player.aspx?type=" + type.ToString() + "&idItem=" + idItem.ToString() + "&idVersion=" + idVersion.ToString();
                url += "&Language=" + language;
                url += PlayBaseParameters(notSaveStat, refreshContainerPage, onModalPage, saveCompleteness);
                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String PlayBaseParameters(Boolean notSaveStat, Boolean refreshContainerPage, Boolean onModalPage, Boolean saveCompleteness)
            {
                String url = "";

                if (refreshContainerPage)
                    url += "&refreshContainer=" + refreshContainerPage.ToString();
                if (onModalPage)
                    url += "&onModalPage=" + onModalPage.ToString();
                if (notSaveStat)
                    url += "&notSaveStat=" + notSaveStat.ToString();
                if (saveCompleteness)
                    url += "&saveCompleteness=" + saveCompleteness.ToString();

                return url;
            }
            public static String UrlGuidParameters(Guid uniqueId, Guid uniqueIdVersion)
            {
                String url = "&" + QueryKeyNames.uniqueId.ToString() + "=" +uniqueId.ToString();
                if (uniqueIdVersion != Guid.Empty)
                     url +="&" + QueryKeyNames.uniqueIdVersion.ToString() + "=" +uniqueIdVersion.ToString();

                return url;
            }
            public static String UrlItemParameters(Boolean isStartItem,long idItem, long idVersion, long idLink=0)
            {
                String url = (isStartItem ? "" : "&") + QueryKeyNames.idItem.ToString() + "=" + idItem.ToString();
                if (idVersion >0)
                    url += "&" + QueryKeyNames.idVersion.ToString() + "=" + idVersion.ToString();
                if (idLink > 0)
                    url += "&" + QueryKeyNames.idLink.ToString() + "=" + idLink.ToString();
                return url;
            }

            public static String PlayFromModule(long idItem, Guid uniqueIdItem, long idVersion, Guid uniqueIdVersion, ItemType type, long idLink, String language, Boolean notSaveStat, Boolean refreshContainerPage, Boolean onModalPage, Boolean saveCompleteness, Boolean setPrevious = true, String backUrl = "")
            {
                return FullPlay(idItem,uniqueIdItem,idVersion,uniqueIdVersion,type,idLink,language,notSaveStat,refreshContainerPage,onModalPage,saveCompleteness,setPrevious,backUrl);
            }

            public static String FullPlay(long idItem, Guid uniqueIdItem, long idVersion, Guid uniqueIdVersion, ItemType type, long idLink, String language, Boolean notSaveStat, Boolean refreshContainerPage, Boolean onModalPage, Boolean saveCompleteness, Boolean setPrevious, String backUrl )
            {
                String url = Play(idItem, uniqueIdItem, idVersion, uniqueIdVersion, type, language, notSaveStat, refreshContainerPage, onModalPage, saveCompleteness, false);
                
                url +=  (idLink==0 ? "" : "&idLink=" + idLink.ToString());
                url += GetSetBackUrlParameter(setPrevious) + GetBackUrlParameter(backUrl);
                return url;
            }
            public static String RenderPlayerFolder()
            {
                return modulehome + "Player/";
            }

            public static string AjaxAction(Int32 idCommunity, long idItem, Guid uniqueId, long idVersion, Guid uniqueIdVersion, ItemType type, long idLink, long idAction, Guid workingSessionId, String playSessionId, Boolean onModalPage)
            {
                return modulehome + "CommonActionSender.aspx?idCommunity=" + idCommunity.ToString() + "&idItem=" + idItem.ToString() + "&idVersion=" + idVersion.ToString()
                    + "&type=" + type.ToString()
                    + ((idAction == 0) ? "" : "&idAction=" + idAction.ToString()) + "&wSessionId=" + workingSessionId.ToString() + "&pSessionId=" + playSessionId
                    + ((idLink == 0) ? "" : "&idLink=" + idLink.ToString())
                    + ((onModalPage) ? "&onModalPage=" + onModalPage.ToString() : "")
                    + UrlGuidParameters(uniqueId, uniqueIdVersion);
            }
        #endregion

        public static String GetBackUrlPlaceHolder()
        {
            return "backUrl=";
        }
        public static String GetBackUrlParameter(String backUrl = "")
        {
            return (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + System.Web.HttpUtility.UrlEncode(backUrl));
        }
        public static String GetSetBackUrlParameter(Boolean setPrevious = true)
        {
            return (setPrevious ? "&setbackUrl=true" : "");
        }
    }
}