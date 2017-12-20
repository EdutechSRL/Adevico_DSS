using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    public static class rootObject
    {
        private static String _BaseUrl = "Modules/DocTemplate/";

        public static String AddTemplate()
        {
            return EditTemplate(0) + "&add=true";
        }
        public static String EditTemplate(long idTemplate)
        {
            return _BaseUrl + "Edit.aspx?idTemplate=" + idTemplate.ToString();
        }

        ////OK!
        //public static String PreviewTemplate(long idTemplate)
        //{
        //    return PreviewTemplate(idTemplate, 0);
        //}
        //public static String PreviewTemplate(long idTemplate, long idVersion)
        //{
        //    return _BaseUrl + "Preview.aspx?IdTemplate=" + idTemplate.ToString() + ((idVersion == 0) ? "" : "&idVersion=" + idVersion.ToString());
        //}

        public static String PreviewTemplate(long idTemplate, Boolean fromList)
        {
            return PreviewTemplate(idTemplate, 0, fromList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <param name="idVersion"></param>
        /// <param name="fromList"></param>
        /// <returns></returns>
        public static String PreviewTemplate(long idTemplate, long idVersion, Boolean fromList)
        {
            return _BaseUrl + "Preview.aspx?IdTemplate=" + idTemplate.ToString() + ((idVersion == 0) ? "" : "&idVersion=" + idVersion.ToString()) + "&fromList=" + fromList.ToString();
        }
        /// <summary>
        /// DEPRECATA!
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <param name="idVersion"></param>
        /// <param name="fromList"></param>
        /// <param name="backUrl"></param>
        /// <returns></returns>
        public static String PreviewTemplate(long idTemplate, long idVersion, Boolean fromList, String backUrl)
        {
            return PreviewTemplate(idTemplate, idVersion,fromList) + (String.IsNullOrEmpty(backUrl) ? "" : "&BackUrl=" + backUrl);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static String PreviewTemplate(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_sTemplateVersion item, lm.Comol.Core.DomainModel.ModuleObject source)
        {
            return PreviewTemplate(item.IdTemplate, item.Id, false, "") + ((source == null || (source != null && source.ObjectLongID == 0)) ? "" : "&idCommunity=" + source.CommunityID.ToString() + "&idModule=" + source.ServiceID.ToString() + "&idModuleItem=" + source.ObjectLongID.ToString() + "&idItemType=" + source.ObjectTypeID.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="source"></param>
        /// <param name="backUrl"></param>
        /// <returns></returns>
        public static String PreviewTemplate(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_sTemplateVersion item, lm.Comol.Core.DomainModel.ModuleObject source, String backUrl)
        {
            return PreviewTemplate(item.IdTemplate, item.Id, false, backUrl) + ((source == null || (source != null && source.ObjectLongID == 0)) ? "" : "&idCommunity=" + source.CommunityID.ToString() + "&idModule=" + source.ServiceID.ToString() + "&idModuleItem=" + source.ObjectLongID.ToString() + "&idItemType=" + source.ObjectTypeID.ToString());
        }

        public static String List()
        {
            return _BaseUrl+ "List.aspx";
        }
        /// <summary>
        /// Inutilizzata!
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        public static String List(long idTemplate)
        {
            string url = List();
            if(idTemplate > 0)
                url += "?idTemplate=" + idTemplate.ToString();
            return url;
        }
    }
}
