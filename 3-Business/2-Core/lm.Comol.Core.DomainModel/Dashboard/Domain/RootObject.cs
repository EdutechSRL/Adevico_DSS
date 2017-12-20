using System;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Linq;

namespace lm.Comol.Core.Dashboard.Domain
{
    public class RootObject
    {
        private readonly static String modulehome = "Modules/DashBoard/";

        #region "Dashboard Use"
            private static String GetPortalPage(Int32 iduser, DashboardViewType view)
            {
                switch (view)
                {
                    case DashboardViewType.List:
                        return modulehome + "PortalDashboardList.aspx?" + ((iduser > 0) ? "iduser=" + iduser.ToString() +"&" : "");
                    case DashboardViewType.Combined:
                        return modulehome + "PortalDashboardCombined.aspx?" + ((iduser > 0) ? "iduser=" + iduser.ToString() +"&" : "");
                    case DashboardViewType.Tile:
                        return modulehome + "PortalDashboardTile.aspx?" + ((iduser > 0) ? "iduser=" + iduser.ToString() +"&" : "");
                    case DashboardViewType.Search:
                        return modulehome + "Search.aspx?" + ((iduser > 0) ? "iduser=" + iduser.ToString() +"&" : "");
                    case DashboardViewType.Subscribe:
                        return modulehome + "EnrollTo.aspx?" + ((iduser > 0) ? "iduser=" + iduser.ToString() +"&" : "");
                }
                return "";
            }
            private static String GetCommunityPage(Int32 iduser, DashboardViewType view)
            {
                switch (view)
                {
                    case DashboardViewType.List:
                        return modulehome + "CommunityDashboardList.aspx?" + ((iduser > 0) ? "&iduser=" + iduser.ToString() +"&" : "");
                    case DashboardViewType.Combined:
                        return modulehome + "CommunityDashboardCombined.aspx?" + ((iduser > 0) ? "&iduser=" + iduser.ToString() + "&" : "");
                    case DashboardViewType.Tile:
                        return modulehome + "CommunityDashboardTile.aspx?" + ((iduser > 0) ? "&iduser=" + iduser.ToString() + "&" : "");
                    case DashboardViewType.Search:
                        return modulehome + "CommunityDashboardSearch.aspx?" + ((iduser > 0) ? "&iduser=" + iduser.ToString() + "&" : "");
                }
                return "";
            }
            public static String LoadPortalView(Int32 iduser, ISettingsBase settings)
            {
                return GetPortalPage(iduser,settings.View) + "g=" + settings.GroupBy.ToString() + "&n=" + settings.GetNoticeboard(settings.View).ToString() + "&o=" + settings.OrderBy.ToString();
            }
            public static String LoadPortalView(Int32 iduser, DashboardViewType view, GroupItemsBy groupBy, OrderItemsBy orderItemsBy, DisplayNoticeboard dNoticeboard, long idTile = -1, long idTag = -1, Boolean loadFromUrl = false, Boolean moreTiles = false, Boolean moreCommunities = false)
            {
                return GetPortalPage(iduser, view) + "g=" + groupBy.ToString() + "&n=" + dNoticeboard.ToString() + "&o=" + orderItemsBy.ToString() + ((idTile == -1) ? "" : "&idTile=" + idTile.ToString()) + ((idTag == -1) ? "" : "&idTag=" + idTag.ToString()) + ((!loadFromUrl) ? "" : "&lfu=" + loadFromUrl.ToString()) + ((!moreTiles) ? "" : "&mt=" + moreTiles.ToString()) + ((!moreCommunities) ? "" : "&mc=" + moreCommunities.ToString());
            }
            public static String LoadCommunityView(Int32 iduser, DashboardViewType view, Int32 idCommunity)
            {
                return GetPortalPage(iduser, view) + "idCommuniy=" + idCommunity.ToString();
            }
            public static String Search(Int32 iduser, DisplaySearchItems search, String text, Boolean forSubscription = false, Int32 idCommunityType = -1, Boolean myCommunities = false)
            {
                return GetPortalPage(iduser, DashboardViewType.Search) + "s=" + search.ToString() + "&t=" + text + (forSubscription ? "&subscribe=" + forSubscription.ToString() : "") + (myCommunities ? "&my=" + myCommunities.ToString() : "") + (idCommunityType > -1 ? "&idType=" + idCommunityType.ToString() : "");
            }
            public static String EnrollTo(Int32 iduser, String text, Int32 idCommunityType = -1, Boolean preload = true)
            {
                return modulehome + "EnrollTo.aspx?t=" + text + "&subscribe=true" + (idCommunityType > -1 ? "&idType=" + idCommunityType.ToString() : "") + (preload ? "&load=" + preload.ToString() : "");
            }
            public static String GroupByParameter(GroupItemsBy groupBy)
            {
                return "g=" + groupBy.ToString();
            }
        #endregion

        #region "Dashboard"
            private readonly static String dashboardmodulehome = "Modules/DashBoard/Settings/";
            public static String DashboardList(DashboardType type, Boolean loadfromRecycleBin = false, Int32 idCommunity = -1)
            {
                String url = dashboardmodulehome;
                if (loadfromRecycleBin)
                    url = url + "RecycleBin.aspx?recycle=true";
                else
                    url = url + "List.aspx?recycle=false";
                url = url + (idCommunity > 0 ? "&idCommunity=" + idCommunity.ToString() : "");
                url = url + "&type=" + type.ToString();

                return url;
            }
            public static String DashboardAdd(DashboardType type, Int32 idCommunity = -1)
            {
                return dashboardmodulehome + "Add.aspx?" + DashboardUrl(0, type, idCommunity);
            }
            public static String DashboardEdit(long idDashboard, DashboardType type, Int32 idCommunity = -1, Boolean fromAdd = false)
            {
                return dashboardmodulehome + "Edit.aspx?" + DashboardUrl(idDashboard, type, idCommunity) + (fromAdd ? "&fromAdd=true" : "");
            }
            public static String DashboardEditViews(long idDashboard, DashboardType type, Int32 idCommunity = -1)
            {
                return dashboardmodulehome + "EditViews.aspx?" + DashboardUrl(idDashboard, type, idCommunity) ;
            }
            public static String DashboardTileReorder(WizardDashboardStep step, long idDashboard, DashboardType type, Int32 idCommunity = -1)
            {
                return dashboardmodulehome + "ReorderTiles.aspx?&step=" + step.ToString() + "&" + DashboardUrl(idDashboard, type, idCommunity);
            }
            public static String DashboardPreview(long idDashboard, DashboardType type, Int32 idCommunity = -1, WizardDashboardStep step = WizardDashboardStep.None)
            {
                return dashboardmodulehome + "Preview.aspx?" + DashboardUrl(idDashboard, type, idCommunity) + ((step == WizardDashboardStep.None) ? "" : "&step=" + step.ToString());
            }
            public static String DashboardPreview(long idDashboard, DashboardViewType vType, GroupItemsBy groupBy, OrderItemsBy orderItemsBy, long idTile = -1, long idTag = -1)
            {
                return dashboardmodulehome + "Preview.aspx?idDashboard=" + idDashboard.ToString() + "&vType=" + vType.ToString() + "&step=" + WizardDashboardStep.None.ToString() + "&g=" + groupBy.ToString() + "&o=" + orderItemsBy.ToString() + ((idTile == -1) ? "" : "&idTile=" + idTile.ToString()) + ((idTag == -1) ? "" : "&idTag=" + idTag.ToString());
            }
       
            public static String DashboardUrl(long idDashboard, DashboardType type, Int32 idCommunity = -1)
            {
                String url = "type=" + type.ToString();
                url = url + (idCommunity > 0 ? "&idCommunity=" + idCommunity.ToString() : "");
                url = url + (idDashboard > 0 ? "&idDashboard=" + idDashboard.ToString() : "");
                return url;
            }
        #endregion

        #region "Tiles"
            private readonly static String tilemodulehome = "Modules/Tile/";
            public static String TileList(DashboardType type, Boolean loadfromRecycleBin = false, Int32 idCommunity = -1, long idTile = 0, long fromIdDashboard = 0, WizardDashboardStep step = WizardDashboardStep.None)
            {
                String url = tilemodulehome;
                if (loadfromRecycleBin)
                    url = url + "RecycleBin.aspx?recycle=true";
                else
                    url = url + "List.aspx?recycle=false";
                url = url + (idCommunity > 0 ? "&idCommunity=" + idCommunity.ToString() : "");
                url = url + "&type=" + type.ToString();
                if (fromIdDashboard > 0)
                    url = url + "&fromIdDashboard=" + fromIdDashboard.ToString() + ((step == WizardDashboardStep.None) ? "" : "&step=" + step.ToString());
                if (idTile > 0)
                    url = url + "&idTile=" + idTile.ToString() + "#tile" + idTile.ToString();

                return url;
            }
            public static String TileAdd(DashboardType type, Int32 idCommunity = -1, long fromIdDashboard = 0, WizardDashboardStep step = WizardDashboardStep.None)
            {
                return tilemodulehome + "Add.aspx?" + TileUrl(0, type, idCommunity, fromIdDashboard, step);
            }
            public static String TileView(long idTile, DashboardType type, Int32 idCommunity = -1, long fromIdDashboard = 0, WizardDashboardStep step = WizardDashboardStep.None)
            {
                return tilemodulehome + "View.aspx?" + TileUrl(idTile, type, idCommunity, fromIdDashboard, step);
            }
            public static String TileEdit(long idTile, DashboardType type, Int32 idCommunity = -1, Boolean fromAdd = false, long fromIdDashboard = 0, WizardDashboardStep step = WizardDashboardStep.None)
            {
                return tilemodulehome + "Edit.aspx?" + TileUrl(idTile, type, idCommunity, fromIdDashboard, step) + (fromAdd ? "&fromAdd=true" : "");
            }
            public static String TileUrl(long idTile, DashboardType type, Int32 idCommunity = -1, long fromIdDashboard = 0, WizardDashboardStep step = WizardDashboardStep.None)
            {
                String url = "type=" + type.ToString();
                url = url + (idCommunity > 0 ? "&idCommunity=" + idCommunity.ToString() : "");
                url = url + (fromIdDashboard > 0 ? "&fromIdDashboard=" + fromIdDashboard.ToString() : "") + ((step == WizardDashboardStep.None) ? "" : "&step=" + step.ToString());
                url = url + (idTile > 0 ? "&idTile=" + idTile.ToString() : "");
                return url;
            }
        #endregion
        #region "Community"
            //Int32 iduser,
            public static String CommunityDetails(Int32 idCommunity, Boolean popup = false, Boolean fromPage = false, Boolean loadFromSession= false)
            {
                return modulehome + (popup ? "ViewDetails" : "Details") + ".aspx?idCommunity=" + idCommunity.ToString() + ((fromPage && !popup) ? "&FromPage=" + fromPage.ToString() : "") + ((loadFromSession) ? "&FromSession=" + loadFromSession.ToString() : "");
                    //+ ((iduser > 0) ? "&iduser=" + iduser.ToString() : "");
            }
            public static String ViewTree(Boolean advancedMode, String loadMode, Int32 idCommunity, Boolean fromPage = false, Boolean loadFromSession = false)
            {
                return modulehome + "ViewTree.aspx?Advanced=" + advancedMode.ToString() + (String.IsNullOrEmpty(loadMode)? "" : "&mode=" + loadMode)  + "&idCommunity=" + idCommunity.ToString() + ((fromPage) ? "&FromPage=" + fromPage.ToString() : "") + ((loadFromSession) ? "&FromSession=" + loadFromSession.ToString() : "");
            }

            public static String ControlNodesTree()
            {
                return modulehome + "UC/UC_CommunityNodes.ascx";
            }

        #endregion
        #region "AutoLogon"
            public static String AutoLogonToCommunity(Int32 idCommunity)
            {
                return modulehome + "AutoLogon.aspx?idCommunity=" + idCommunity.ToString();
            }  
        #endregion
        //private static String BaseContainerString(Int32 idContainerCommunity, String moduleCode = "", long idPermissions = 0, Boolean setFromCookies = false)
        //{
        //    String query = ((setFromCookies) ? "&reload=true" : "");
        //    query += ((idContainerCommunity > -1) ? "&idCommunityCnt=" + idContainerCommunity.ToString() : "");
        //    query += ((!String.IsNullOrEmpty(moduleCode)) ? "&mCodeCnt=" + moduleCode : "");
        //    query += ((idPermissions > 0) ? "&mPrmCnt=" + idPermissions.ToString() : "");

        //    return query;
        //}
        //private static String BaseQueryString(TemplateType type, dtoBaseTemplateOwner template)
        //{
        //    String query = "type=" + type.ToString();
     
        //    if (template != null)
        //    {
        //        query += "&ownType=" + template.Type.ToString();
        //        query += ((template.IdPerson > 0) ? "&idPerson=" + template.IdPerson.ToString() : "");
        //        query += ((template.IdCommunity > 0) ? "&idCommunity=" + template.IdCommunity.ToString() : "");
        //        query += ((template.IdModule > 0) ? "&idModule=" + template.IdModule.ToString() : "");
        //        query += ((!String.IsNullOrEmpty(template.ModuleCode)) ? "&moduleCode=" + template.ModuleCode : "");
        //        query += ((template.ModulePermission > 0) ? "&idModuleP=" + template.ModulePermission.ToString() : "");
        //        query += ((template.IdObject > 0) ? "&idObj=" + template.IdObject.ToString() : "");
        //        query += ((template.IdObjectType > 0) ? "&idObjType=" + template.IdObjectType.ToString() : "");
        //        query += ((template.IdObjectModule > 0) ? "&idObjModule=" + template.IdObjectModule.ToString() : "");
        //        query += ((template.IdObjectCommunity > 0) ? "&idObjCommunity=" + template.IdObjectCommunity.ToString() : "");
        //    }
        //    return query;
        //}
        //public static String List(Int32 idContainerCommunity, TemplateType type, dtoBaseTemplateOwner ownerInfo, Boolean setFromCookies = false, String moduleCode = "", long idPermissions = 0, long idTemplate = 0, long idVersion = 0)
        //{
        //    Boolean fromContainer = ((!String.IsNullOrEmpty(moduleCode) || idPermissions>0) && idContainerCommunity >0);
        //    String url = modulehome + "List.aspx?" + BaseQueryString((fromContainer) ? TemplateType.Module : type, (fromContainer) ? null: ownerInfo) + BaseContainerString(idContainerCommunity, moduleCode, idPermissions, setFromCookies);
        //    url += (idTemplate > 0) ? "&idTemplate=" + idTemplate.ToString():"";
        //    url += (idVersion > 0) ? "#ver_" + idVersion.ToString() : (idTemplate > 0) ? "#tmp_" + idTemplate.ToString() : "";
        //    return url;
        //}

        //public static string Add(TemplateType type, dtoBaseTemplateOwner ownerInfo, Int32 idContainerCommunity, String moduleCode = "", long idPermissions = 0)
        //{
        //    return Add(Guid.Empty, type, ownerInfo, idContainerCommunity, moduleCode, idPermissions);
        //}
        //public static string Add(System.Guid sessionId,TemplateType type, dtoBaseTemplateOwner ownerInfo, Int32 idContainerCommunity, String moduleCode = "", long idPermissions = 0)
        //{
        //    return modulehome + "Add.aspx?" + BaseQueryString(type, ownerInfo) + BaseContainerString(idContainerCommunity, moduleCode, idPermissions) + ((sessionId == System.Guid.Empty) ? "" : "&sId=" + sessionId.ToString());
        //}
        //public static string EditByStep(TemplateType type, dtoBaseTemplateOwner ownerInfo, WizardTemplateStep step, Int32 idContainerCommunity, String moduleCode, long idPermissions, String backUrl, long idTemplate = 0, long idVersion = 0, Boolean preview = false, Boolean added = false, Boolean backToContainerModule = false )
        //{
        //    return EditByStep(Guid.Empty, type, ownerInfo, step, idContainerCommunity, moduleCode, idPermissions, backUrl, idTemplate, idVersion,preview, added);
        //}

        //public static string EditByStep(System.Guid sessionId, TemplateType type, dtoBaseTemplateOwner ownerInfo, WizardTemplateStep step, Int32 idContainerCommunity, String moduleCode, long idPermissions, String backUrl, long idTemplate = 0, long idVersion = 0, Boolean preview = false, Boolean added = false, Boolean backToContainerModule = false )
        //{
        //    string baseQuery = ((idTemplate > 0) ? "idTemplate=" + idTemplate.ToString() : "") + ((idVersion > 0) ? "&idVersion=" + idVersion.ToString() : "") + ((added) ? "&add=true" : "") + ((preview) ? "&preview=true" : "");
        //    if (!string.IsNullOrEmpty(baseQuery))
        //        baseQuery += "&";

        //    baseQuery += BaseQueryString(type, ownerInfo) + BaseContainerString(idContainerCommunity, moduleCode, idPermissions);
        //    switch (step)
        //    {
        //        case WizardTemplateStep.Settings:
        //            return modulehome + "EditSettings.aspx?" + baseQuery + (backToContainerModule ? "&toCnt=true" : "") + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl) + ((sessionId== System.Guid.Empty) ? "" : "&sId=" + sessionId.ToString());
        //        case WizardTemplateStep.Translations:
        //            return modulehome + "EditTemplate.aspx?" + baseQuery + (backToContainerModule ? "&toCnt=true" : "") + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl) + ((sessionId == System.Guid.Empty) ? "" : "&sId=" + sessionId.ToString());
        //        case WizardTemplateStep.Permission:
        //            return modulehome + "EditPermission.aspx?" + baseQuery + (backToContainerModule ? "&toCnt=true" : "") + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl) + ((sessionId == System.Guid.Empty) ? "" : "&sId=" + sessionId.ToString());
        //        default:
        //            return "";
        //    }
        //}


        //private static String ObjectOwnerQueryString(lm.Comol.Core.DomainModel.ModuleObject obj = null)
        //{
        //    String query = "";
        //    if (obj != null)
        //    {
        //        query += ((obj.ObjectLongID > 0) ? "&oId=" + obj.ObjectLongID.ToString() : "");
        //        query += ((obj.ObjectTypeID > 0) ? "&oType=" + obj.ObjectTypeID.ToString() : "");
        //        query += ((obj.ServiceID > 0) ? "&oIdModule=" + obj.ServiceID.ToString() : "");
        //        query += ((!String.IsNullOrEmpty(obj.ServiceCode)) ? "&oMcode=" + obj.ServiceCode.ToString() : "");
        //        query += ((obj.CommunityID > 0) ? "&oCommunity=" + obj.CommunityID.ToString() : "");
        //    }

        //    return query;
        //}
        //public static String Settings(Boolean forPortal=false,Int32 idCommunity=0, Int32 idOrganization= 0, lm.Comol.Core.DomainModel.ModuleObject obj = null) {
        //    return modulehome + String.Format("Settings.aspx?portal={0}&idCommunity={1}&idOrg={2}{3}", forPortal.ToString().ToLower(), idCommunity, idOrganization,
        //        (obj==null) ? "" :  ObjectOwnerQueryString(obj)); 
        //}
        //public static String PreviewTemplate(long idTemplate, long idVersion,Int32 idModule, String moduleCode, Boolean forPortal = false, Int32 idCommunity = 0, Int32 idOrganization = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        //{
        //    return modulehome + String.Format("Preview.aspx?idT={0}&idV={1}&idModule={2}&code={3}&portal={4}&idCommunity={5}&idOrg={6}{7}", idTemplate, idVersion,idModule,moduleCode,forPortal.ToString().ToLower(), idCommunity, idOrganization,
        //        (obj == null) ? "" : ObjectOwnerQueryString(obj));
        //}
     }
}