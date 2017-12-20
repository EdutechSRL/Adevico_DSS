using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    public class RootObject
    {
        public static String Management()
        {
            return "Modules/ProviderManagement/List.aspx";
        }
        public static String Management(long idProvider)
        {
            return "Modules/ProviderManagement/List.aspx#" + idProvider.ToString();
        }
        public static String AddProvider()
        {
            return "Modules/ProviderManagement/Add.aspx";
        }

        public static String EditProvider(long idProvider)
        {
            return "Modules/ProviderManagement/Edit.aspx?idProvider=" + idProvider.ToString();
        }
        public static String EditProviderSettings(long idProvider, Authentication.AuthenticationProviderType providerType) {
            switch (providerType) {
                case Authentication.AuthenticationProviderType.UrlMacProvider:
                    return EditUrlMacProviderSettings(idProvider);
                case Authentication.AuthenticationProviderType.Url:
                    return EditUrlProviderSettings(idProvider);
                default:
                    return EditProvider(idProvider);
            }
        }
        public static String EditUrlProviderSettings(long idProvider)
        {
            return "Modules/ProviderManagement/UrlSettings.aspx?idProvider=" + idProvider.ToString();
        }
        public static String EditShibbolethProviderSettings(long idProvider)
        {
            return "Modules/ProviderManagement/ShibbolethSettings.aspx?idProvider=" + idProvider.ToString();
        }
        public static String EditUrlMacProviderSettings(long idProvider)
        {
            return "Modules/ProviderManagement/UrlMacSettings.aspx?idProvider=" + idProvider.ToString();
        }
        public static String EditUrlMacProviderSettings(long idProvider,lm.Comol.Core.Authentication.UrlMacAttributeType cSelectedType,long idAttribute, Boolean editMode)
        {
            return EditUrlMacProviderSettings(idProvider) + "&sType=" + cSelectedType.ToString() + ((editMode) ? "&idEditAttribute=" + idAttribute.ToString() : "") + "#attribute_" + idAttribute.ToString();
        }
        public static String EditUrlMacProviderSettings(long idProvider,lm.Comol.Core.Authentication.UrlMacAttributeType cSelectedType, long idAttribute, long idAttributeItem, Boolean editMode)
        {
            return EditUrlMacProviderSettings(idProvider,cSelectedType, idAttribute, editMode) + ((idAttributeItem == 0) ? "" : "_" + idAttributeItem.ToString());
        }
    }
}