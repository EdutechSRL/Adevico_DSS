using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class RootObject
    {
        private const String _BaseService = "Modules/SkinManagement/";

         public static String ManagementModuleSkin(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType,String backUrl)
        {
            return ManagementModuleSkin((long)0,idModule,idCommunity,idModuleItem,idItemType,backUrl);
        }
        public static String ManagementModuleSkin(long idLink, Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String backUrl)
        {
            return _BaseService + "SkinModuleManagement.aspx?idLink=" + idLink.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idModule=" + idModule.ToString() + "&idModuleItem=" + idModuleItem.ToString() + "&idItemType=" + idItemType.ToString() + "&backUrl=" + backUrl;
        }
       
        public static String AddModuleSkin(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String backUrl)
        {
            return AddModuleSkin((long)0, idModule, idCommunity, idModuleItem, idItemType, backUrl);
        }
        public static String AddModuleSkin(long idLink, Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String backUrl)
        {
            return AddSkin(idCommunity,"&idLink=" + idLink.ToString() + "&idModule=" + idModule.ToString() + "&idModuleItem=" + idModuleItem.ToString() + "&idItemType=" + idItemType.ToString(),backUrl,SkinType.Module) ;
        }
        public static String AddSkin(Int32 idCommunity,String query, String backUrl, SkinType type)
        {
            return _BaseService + "Add.aspx?idCommunity=" + idCommunity.ToString() + "&type=" + type.ToString() + query + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl);
        }
        public static String EditModuleSkin(long idSkin,Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String backUrl)
        {
            return EditModuleSkin((long)0, idSkin,idModule,idCommunity,idModuleItem,idItemType, backUrl);
        }
        public static String EditModuleSkin(long idLink, long idSkin, Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String backUrl)
        {
            return EditSkin(idSkin, "&idLink=" + idLink.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idModule=" + idModule.ToString() + "&idModuleItem=" + idModuleItem.ToString() + "&idItemType=" + idItemType.ToString(), backUrl, SkinType.Module);
        }
        public static String EditSkin(long idSkin, String query, String backUrl, SkinType type)
        {
            return _BaseService + "Edit.aspx?type=" + type.ToString() + "&idSkin=" + idSkin.ToString() + query + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl);
        }

        public static String DeleteModuleSkin(long idSkin, Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String backUrl)
        {
            return DeleteModuleSkin((long)0, idSkin, idModule, idCommunity, idModuleItem, idItemType, backUrl);
        }
        public static String DeleteModuleSkin(long idLink, long idSkin, Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String backUrl)
        {
            return DeleteSkin(idSkin, "&idLink=" + idLink.ToString() + "&idModule=" + idModule.ToString() + "&idModuleItem=" + idModuleItem.ToString() + "&idItemType=" + idItemType.ToString(), backUrl, SkinType.Module);
        }
        public static String DeleteSkin(long idSkin, String query, String backUrl, SkinType type)
        {
            return _BaseService + "Delete.aspx?type=" + type.ToString() + "&idSkin=" + idSkin.ToString() + query + (String.IsNullOrEmpty(backUrl) ? "" : "&backUrl=" + backUrl);
        }
        public static String Preview(DtoDisplaySkin dto, Int32 idCommunity, Int32 idModule)
        {
            return _BaseService + "Preview.aspx?idItem=" + dto.Id.ToString() + "&itemType=" + dto.Type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idModule=" + idModule.ToString();
        }
    }
}