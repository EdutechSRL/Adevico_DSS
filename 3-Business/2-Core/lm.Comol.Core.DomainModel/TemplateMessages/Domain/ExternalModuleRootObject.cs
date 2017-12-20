using System;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Linq;

namespace lm.Comol.Core.TemplateMessages
{
    public class ExternalModuleRootObject
    {
        private readonly static String modulehome = "Modules/Templates/";
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
        //    query += "&ownType=" + template.Type.ToString();
        //    query += ((template.IdPerson > 0) ? "&idPerson=" + template.IdPerson.ToString() : "");
        //    query += ((template.IdCommunity > 0) ? "&idCommunity=" + template.IdCommunity.ToString() : "");
        //    query += ((template.IdModule > 0) ? "&idModule=" + template.IdModule.ToString() : "");
        //    query += ((!String.IsNullOrEmpty(template.ModuleCode)) ? "&moduleCode=" + template.ModuleCode : "");
        //    query += ((template.ModulePermission > 0) ? "&idModuleP=" + template.ModulePermission.ToString() : "");
        //    query += ((template.IdObject > 0) ? "&idObj=" + template.IdObject.ToString() : "");
        //    query += ((template.IdObjectType > 0) ? "&idObjType=" + template.IdObjectType.ToString() : "");
        //    query += ((template.IdObjectModule > 0) ? "&idObjModule=" + template.IdObjectModule.ToString() : "");
        //    query += ((template.IdObjectCommunity > 0) ? "&idObjCommunity=" + template.IdObjectCommunity.ToString() : "");

        //    return query;
        //}
        //public static String List(Int32 idContainerCommunity, TemplateType type, dtoBaseTemplateOwner ownerInfo, Boolean setFromCookies = false, String moduleCode = "", long idPermissions = 0, long idTemplate = 0, long idVersion = 0)
        //{
        //    String url = modulehome + "List.aspx?" + BaseQueryString(type, ownerInfo) + BaseContainerString(idContainerCommunity, moduleCode, idPermissions, setFromCookies);
        //    url += (idTemplate > 0) + "&idTemplate=" + idTemplate.ToString();
        //    url += (idVersion > 0) ? "#ver_" + idVersion.ToString() : (idTemplate > 0) ? "#tmp_" + idTemplate.ToString() : "";
        //    return url;
        //}

        //public static string Add(TemplateType type, dtoBaseTemplateOwner ownerInfo, Int32 idContainerCommunity, String moduleCode = "", long idPermissions = 0)
        //{
        //    return modulehome + "Add.aspx?" + BaseQueryString(type, ownerInfo) + BaseContainerString(idContainerCommunity, moduleCode, idPermissions);
        //}
        //public static String ListSentMessages() {
        //    return "";
        //}
        //public static String ListAvailableTemplates()
        //{
        //    return "";
        //}
        private static String ObjectOwnerQueryString(Int32 idCommunity=-1, Int32 idModule=0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            String query = "&type=" + TemplateType.Module.ToString();
            query += "&ownType=" + TemplateType.Module.ToString();
            query += ((idCommunity != -1) ? "&idCommunity=" + idCommunity.ToString() : "");
            query += ((idModule > 0) ? "&idModule=" + idModule.ToString() : "");
            if (obj != null)
            {
                query += ((obj.ObjectLongID > 0) ? "&oId=" + obj.ObjectLongID.ToString() : "");
                query += ((obj.ObjectTypeID > 0) ? "&oType=" + obj.ObjectTypeID.ToString() : "");
                query += ((obj.ServiceID > 0) ? "&oIdModule=" + obj.ServiceID.ToString() : "");
                query += ((!String.IsNullOrEmpty(obj.ServiceCode)) ? "&oMcode=" + obj.ServiceCode.ToString() : "");
                query += ((obj.CommunityID > 0) ? "&oCommunity=" + obj.CommunityID.ToString() : "");
            }

            return query;
        }
        private static String BaseQueryString(String moduleCode, DisplayTab current, DisplayTab tabs, UserSelection sMode, Boolean selectTemplates, String backUrl = "", long idModuleAction = 0, Boolean actionEmpty=false, long idTemplate = 0, long idVersion = 0, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            return "code=" + moduleCode + ((idModuleAction == 0) ? "" : "&action=" + idModuleAction.ToString()) + ((actionEmpty) ? "&aEmpty=true" : "" ) + "&tab=" + (int)current + "&tabs=" + (int)tabs + "&sl=" + ((int)sMode).ToString() + ((selectTemplates) ? "&dtsl=" + selectTemplates.ToString() : "")
                + ((idTemplate > 0) ? "&idTemplate=" + idTemplate.ToString() : "") + ((idVersion > 0) ? "&idVersion=" + idVersion.ToString() : "")
                + ObjectOwnerQueryString(idCommunity, idModule, obj)
                + ((String.IsNullOrEmpty(backUrl)) ? "" : "&back=" + backUrl);
        }
        //public static String SendMessage(String moduleCode, UserSelection sMode = (UserSelection.FromInputText | UserSelection.FromModule), Boolean selectTemplates = false, String backUrl = "", long idModuleAction = 0, Boolean actionEmpty = false, long idTemplate = 0, long idVersion = 0, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        //{
        //    return SendMessage(moduleCode, DisplayTab.None, sMode, selectTemplates, backUrl, idModuleAction, actionEmpty, idTemplate, idVersion, idCommunity, idModule, obj);
        //}
        public static String SendMessage(String moduleCode, DisplayTab tabs = (DisplayTab.List | DisplayTab.Send| DisplayTab.Sent), UserSelection sMode = (UserSelection.FromInputText | UserSelection.FromModule), Boolean selectTemplates = false, String backUrl = "", long idModuleAction = 0, Boolean actionEmpty = false, long idTemplate = 0, long idVersion = 0, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            String url = modulehome + "SendMessage.aspx?" + BaseQueryString(moduleCode, DisplayTab.Send, tabs, sMode, selectTemplates, backUrl, idModuleAction, actionEmpty, idTemplate, idVersion, idCommunity, idModule, obj);
            return url;
        }
        public static String ListAvailableTemplates(String moduleCode, DisplayTab tabs, UserSelection sMode, Boolean selectTemplates = false, String backUrl = "", long idModuleAction = 0, Boolean actionEmpty = false, long idTemplate = 0, long idVersion = 0, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            String url = modulehome + "SendMessage.aspx?" + BaseQueryString(moduleCode, DisplayTab.List, tabs, sMode, selectTemplates, backUrl, idModuleAction, actionEmpty, idTemplate, idVersion, idCommunity, idModule, obj);
            url += (idVersion > 0) ? "#ver_" + idVersion.ToString() : (idTemplate > 0) ? "#tmp_" + idTemplate.ToString() : "";
            return url;
        }
        public static String ListAvailableTemplates(String moduleCode, String backUrl, Int32 idCommunity = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null, long idModuleAction = 0, Boolean actionEmpty = false, long idTemplate = 0, long idVersion = 0)
        {
            String url = modulehome + "SendMessage.aspx?" + BaseQueryString(moduleCode, DisplayTab.List, (DisplayTab.List | DisplayTab.Send | DisplayTab.Sent), (UserSelection.FromInputText | UserSelection.FromModule), false, backUrl, idModuleAction, actionEmpty, idTemplate, idVersion, idCommunity, 0, obj);
            return url;
        }
        public static String MessagesSent(String moduleCode, DisplayTab tabs = (DisplayTab.List | DisplayTab.Send| DisplayTab.Sent), UserSelection sMode = (UserSelection.FromInputText | UserSelection.FromModule), Boolean selectTemplates = false, String backUrl = "", long idModuleAction = 0, Boolean actionEmpty = false, long idTemplate = 0, long idVersion = 0, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            String url = modulehome + "MessagesSent.aspx?" + BaseQueryString(moduleCode, DisplayTab.Sent, tabs, sMode, selectTemplates, backUrl, idModuleAction, actionEmpty, idTemplate, idVersion, idCommunity, idModule, obj);
            return url;
        }
        public static String MessageRecipients(long idMessage, String moduleCode, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            String url = modulehome + "MessageRecipients.aspx?idMessage=" + idMessage.ToString() + "&code=" + moduleCode + ObjectOwnerQueryString(idCommunity, idModule, obj);
            return url;
        }
        public static String SessionName(System.Guid sessionId)
        {
            return sessionId.ToString() + "_senderSentMessages_backUrl";
        }
    }
}