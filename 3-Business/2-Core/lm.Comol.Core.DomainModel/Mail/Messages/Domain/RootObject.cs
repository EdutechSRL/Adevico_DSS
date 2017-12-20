using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;

namespace lm.Comol.Core.Mail.Messages
{
    public class RootObject
    {
        private readonly static String modulehome = "Modules/Mail/";
        private static String ObjectOwnerQueryString(Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
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
        private static String BaseQueryString(String moduleCode, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null, long idTemplateMessage=0)
        {
            return "code=" + moduleCode + ((idTemplateMessage > 0) ? "&idT=" + idTemplateMessage.ToString() : "") + ObjectOwnerQueryString(idCommunity, idModule, obj);
        }

        public static String ViewMessageTemplate(long idTemplateMessage,String moduleCode, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null) {
            return modulehome + "MessageTemplate.aspx?" + BaseQueryString(moduleCode,idCommunity,idModule,obj,idTemplateMessage);
        }

        public static String ViewMessage(long idUserModule, String moduleCode, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null) {
            return ViewMessage(0, idUserModule, "", moduleCode, idCommunity, idModule, obj);
        }

        public static String ViewMessage(Int32 idPerson, String moduleCode, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            return ViewMessage(idPerson, 0, "", moduleCode, idCommunity, idModule, obj);
        }
        public static String ViewMessage(String mail, String moduleCode, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            return ViewMessage(0, 0, mail, moduleCode, idCommunity, idModule, obj);
        }

        public static String ViewMessage(Int32 idPerson, long idUserModule,String mail, String moduleCode, Int32 idCommunity = -1, Int32 idModule = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            String baseUrl = modulehome + "UserMessages.aspx?{0}{1}{2}{3}";
            return String.Format(baseUrl, "idP=" + idPerson.ToString(), (idUserModule > 0) ? "&idU=" + idUserModule.ToString() : "", String.IsNullOrEmpty(mail) ? "" : "&mail=" + mail, "&" + BaseQueryString(moduleCode, idCommunity, idModule, obj));
        }
    }
}