using System;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Linq;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Domain
{
    public class RootObject
    {
        private readonly static String modulehome = "Modules/Noticeboard/";

        public static String ModalMessagePage(long idMessage,Int32 idCommunity )
        {
            return modulehome + "RenderPage.aspx?idMessage=" + idMessage.ToString() + "&idCommunity=" + idCommunity.ToString();
        }
        public static String ViewMessage(long idMessage, Int32 idCommunity)
        {
            return modulehome + "ViewMessage.aspx?idMessage=" + idMessage.ToString() + "&idCommunity=" + idCommunity.ToString();

        }
        public static String DisplayMessage(long idMessage, Int32 idCommunity)
        {
            return modulehome + "DisplayMessage.aspx?idMessage=" + idMessage.ToString() + "&idCommunity=" + idCommunity.ToString();

        }
        public static String DisplayMessageWithoutSession(long idMessage, Int32 idCommunity, System.Guid waid)
        {
            return modulehome + "PublicDisplayMessage.aspx?idMessage=" + idMessage.ToString() + "&idCommunity=" + idCommunity.ToString() + "&waid=" + waid.ToString();

        }
        public static String DisplayMessageWithoutSession(String idMessagePlaceHolder, Int32 idCommunity, System.Guid waid)
        {
            return modulehome + "PublicDisplayMessage.aspx?idMessage=" + idMessagePlaceHolder + "&idCommunity=" + idCommunity.ToString() + "&waid=" + waid.ToString();

        }
        public static String GetThumbnail(long idMessage, Int32 idCommunity, Guid workingSessionId)
        {
            return modulehome + "CreateThumbnail.aspx?idMessage=" + idMessage.ToString()  + "&idCommunity=" + idCommunity.ToString() + "&wsid=" + workingSessionId.ToString();
        }
        public static String GetTemplate()
        {
            return modulehome + "DisplayMessageTemplate.html";
        }

        public static String AddMessageWithAdvancedEditor( Int32 idCommunity, Boolean isForPortal, Boolean setBackUrl)
        {
            return GetEditingurl(0, idCommunity, isForPortal, setBackUrl, true);
        }
        public static String AddMessageWithSimpleEditor(Int32 idCommunity, Boolean isForPortal, Boolean setBackUrl)
        {
            return GetEditingurl(0, idCommunity, isForPortal, setBackUrl, false);
        }
        public static String EditMessageWithAdvancedEditor(long idMessage, Int32 idCommunity, Boolean isForPortal, Boolean setBackUrl)
        {
            return GetEditingurl(idMessage, idCommunity, isForPortal, setBackUrl,true );
        }
        public static String EditMessageWithSimpleEditor(long idMessage, Int32 idCommunity, Boolean isForPortal, Boolean setBackUrl)
        {
            return GetEditingurl(idMessage, idCommunity, isForPortal, setBackUrl, false);
        }
        private static String GetEditingurl(long idMessage, Int32 idCommunity, Boolean isForPortal, Boolean setBackUrl, Boolean advanced)
        {
            return modulehome + "Edit" + (advanced? "" : "Simple") +".aspx?idMessage=" + idMessage.ToString() + "&idCommunity=" + idCommunity.ToString() + ((isForPortal) ? "&lfp=" + isForPortal.ToString() : "")
                + ((setBackUrl) ? "&lbu=" + setBackUrl.ToString() : "");

        }
        public static String NoticeboardDashboard(long idMessage, Int32 idCommunity, Boolean isForPortal, Boolean setBackUrl)
        {
            return modulehome + "NoticeboardDashboard.aspx?idMessage=" + idMessage.ToString() + "&idCommunity=" + idCommunity.ToString() + ((isForPortal) ? "&lfp=" + isForPortal.ToString() : "")
               + ((setBackUrl) ? "&lbu=" + setBackUrl.ToString() : "");
        }
        //private static String GetPortalPage(DashboardViewType view )
        //{
        //    switch(view){
        //        case DashboardViewType.List:
        //            return modulehome + "PortalDashboardList.aspx?";
        //        case DashboardViewType.Combined:
        //            return modulehome + "PortalDashboardCombined.aspx?";
        //        case DashboardViewType.Tile:
        //            return modulehome + "PortalDashboardTile.aspx?";
        //        case DashboardViewType.Search:
        //            return modulehome + "PortalDashboardSearch.aspx?";
        //    }
        //    return "";
        //}
     }
}