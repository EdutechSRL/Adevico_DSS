using System;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Linq;

namespace lm.Comol.Core.TemplateMessages
{
    public class RootObject
    {
        private readonly static String modulehome = "Modules/Templates/";
        private static String BaseContainerString(Int32 idContainerCommunity, String moduleCode = "", long idPermissions = 0, Boolean setFromCookies = false)
        {
            String query = ((setFromCookies) ? "&reload=true" : "");
            query += ((idContainerCommunity > -1) ? "&idCommunityCnt=" + idContainerCommunity.ToString() : "");
            query += ((!String.IsNullOrEmpty(moduleCode)) ? "&mCodeCnt=" + moduleCode : "");
            query += ((idPermissions > 0) ? "&mPrmCnt=" + idPermissions.ToString() : "");

            return query;
        }
        private static String BaseQueryString(TemplateType type, dtoBaseTemplateOwner template)
        {
            String query = "type=" + type.ToString();
     
            if (template != null)
            {
                query += "&ownType=" + template.Type.ToString();
                query += ((template.IdPerson > 0) ? "&idPerson=" + template.IdPerson.ToString() : "");
                query += ((template.IdCommunity > 0) ? "&idCommunity=" + template.IdCommunity.ToString() : "");
                query += ((template.IdModule > 0) ? "&idModule=" + template.IdModule.ToString() : "");
                query += ((!String.IsNullOrEmpty(template.ModuleCode)) ? "&moduleCode=" + template.ModuleCode : "");
                query += ((template.ModulePermission > 0) ? "&idModuleP=" + template.ModulePermission.ToString() : "");
                query += ((template.IdObject > 0) ? "&idObj=" + template.IdObject.ToString() : "");
                query += ((template.IdObjectType > 0) ? "&idObjType=" + template.IdObjectType.ToString() : "");
                query += ((template.IdObjectModule > 0) ? "&idObjModule=" + template.IdObjectModule.ToString() : "");
                query += ((template.IdObjectCommunity > 0) ? "&idObjCommunity=" + template.IdObjectCommunity.ToString() : "");
            }
            return query;
        }
        public static String List(Int32 idContainerCommunity, TemplateType type, dtoBaseTemplateOwner ownerInfo, Boolean setFromCookies = false, String moduleCode = "", long idPermissions = 0, long idTemplate = 0, long idVersion = 0)
        {
            Boolean fromContainer = ((!String.IsNullOrEmpty(moduleCode) || idPermissions>0) && idContainerCommunity >0);
            String url = modulehome + "List.aspx?" + BaseQueryString((fromContainer) ? TemplateType.Module : type, (fromContainer) ? null: ownerInfo) + BaseContainerString(idContainerCommunity, moduleCode, idPermissions, setFromCookies);
            url += (idTemplate > 0) ? "&idTemplate=" + idTemplate.ToString():"";
            url += (idVersion > 0) ? "#ver_" + idVersion.ToString() : (idTemplate > 0) ? "#tmp_" + idTemplate.ToString() : "";
            return url;
        }

        public static string Add(TemplateType type, dtoBaseTemplateOwner ownerInfo, Int32 idContainerCommunity, String moduleCode = "", long idPermissions = 0)
        {
            return Add(Guid.Empty, type, ownerInfo, idContainerCommunity, moduleCode, idPermissions);
        }
        public static string Add(System.Guid sessionId,TemplateType type, dtoBaseTemplateOwner ownerInfo, Int32 idContainerCommunity, String moduleCode = "", long idPermissions = 0)
        {
            return modulehome + "Add.aspx?" + BaseQueryString(type, ownerInfo) + BaseContainerString(idContainerCommunity, moduleCode, idPermissions) + ((sessionId == System.Guid.Empty) ? "" : "&sId=" + sessionId.ToString());
        }
        public static string EditByStep(TemplateType type, dtoBaseTemplateOwner ownerInfo, WizardTemplateStep step, Int32 idContainerCommunity, String moduleCode, long idPermissions, String backUrl, long idTemplate = 0, long idVersion = 0, Boolean preview = false, Boolean added = false, Boolean backToContainerModule = false )
        {
            return EditByStep(Guid.Empty, type, ownerInfo, step, idContainerCommunity, moduleCode, idPermissions, backUrl, idTemplate, idVersion,preview, added);
        }

        public static string EditByStep(System.Guid sessionId, TemplateType type, dtoBaseTemplateOwner ownerInfo, WizardTemplateStep step, Int32 idContainerCommunity, String moduleCode, long idPermissions, String backUrl, long idTemplate = 0, long idVersion = 0, Boolean preview = false, Boolean added = false, Boolean backToContainerModule = false )
        {
            string baseQuery = ((idTemplate > 0) ? "idTemplate=" + idTemplate.ToString() : "") + ((idVersion > 0) ? "&idVersion=" + idVersion.ToString() : "") + ((added) ? "&add=true" : "") + ((preview) ? "&preview=true" : "");
            if (!string.IsNullOrEmpty(baseQuery))
                baseQuery += "&";

            baseQuery += BaseQueryString(type, ownerInfo) + BaseContainerString(idContainerCommunity, moduleCode, idPermissions);
            switch (step)
            {
                case WizardTemplateStep.Settings:
                    return modulehome + "EditSettings.aspx?" + baseQuery + (backToContainerModule ? "&toCnt=true" : "") + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl) + ((sessionId== System.Guid.Empty) ? "" : "&sId=" + sessionId.ToString());
                case WizardTemplateStep.Translations:
                    return modulehome + "EditTemplate.aspx?" + baseQuery + (backToContainerModule ? "&toCnt=true" : "") + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl) + ((sessionId == System.Guid.Empty) ? "" : "&sId=" + sessionId.ToString());
                case WizardTemplateStep.Permission:
                    return modulehome + "EditPermission.aspx?" + baseQuery + (backToContainerModule ? "&toCnt=true" : "") + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl) + ((sessionId == System.Guid.Empty) ? "" : "&sId=" + sessionId.ToString());
                default:
                    return "";
            }
        }


        private static String ObjectOwnerQueryString(lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            String query = "";
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
        public static String Settings(Boolean forPortal=false,Int32 idCommunity=0, Int32 idOrganization= 0, lm.Comol.Core.DomainModel.ModuleObject obj = null) {
            return modulehome + String.Format("Settings.aspx?portal={0}&idCommunity={1}&idOrg={2}{3}", forPortal.ToString().ToLower(), idCommunity, idOrganization,
                (obj==null) ? "" :  ObjectOwnerQueryString(obj)); 
        }
        public static String PreviewTemplate(long idTemplate, long idVersion,Int32 idModule, String moduleCode, Boolean forPortal = false, Int32 idCommunity = 0, Int32 idOrganization = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            return modulehome + String.Format("Preview.aspx?idT={0}&idV={1}&idModule={2}&code={3}&portal={4}&idCommunity={5}&idOrg={6}{7}", idTemplate, idVersion,idModule,moduleCode,forPortal.ToString().ToLower(), idCommunity, idOrganization,
                (obj == null) ? "" : ObjectOwnerQueryString(obj));
        }
     }
}