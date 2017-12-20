using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement
{
    public class RootObject
    {
        public static string SystemOutOforder()
        {
            return "Authentication/SystemOutOforder.aspx";
        }
        public static string InternalShibbolethAuthenticationPage(Boolean allowAdmin)
        {
            return "ISAuthenticationPage.aspx" + ((allowAdmin == true) ? "?AdminAccess=true" : "");
        }
        public static string InternalLoginControl() 
        {
            return "Authentication/UC/UC_InternalLogin.ascx";
        }
        public static string InternalLogin(Boolean allowAdmin)
        {
            return "Authentication/InternalLogin.aspx" + ((allowAdmin == true) ? "?AdminAccess=true" : "");
        }
        public static string InternalRetrievePassword(Boolean allowAdmin)
        {
            return "Authentication/RetrievePassword.aspx" + ((allowAdmin == true) ? "?AdminAccess=true" : "");
        }
        public static string ShibbolethLogin(Boolean allowAdmin) 
        {
            return "Authentication/SHBlogin/Index.aspx" + ((allowAdmin == true) ? "?AdminAccess=true" : "");
        }
        //public static string ShibbolethLogout()
        //{
        //    return "Modules/Common/LogoutForSh.aspx";
        //}
        public static string LogoutMessage(Authentication.LogoutMode mode, Authentication.AuthenticationProviderType type, String destinationUrl)
        {
            return "Modules/Common/LogoutMessage.aspx?mode=" + mode.ToString() + "&type=" + type.ToString() + "&url=" + destinationUrl;
        }
        public static string InvalidToken(int idPerson, long idProvider, lm.Comol.Core.Authentication.UrlProviderResult message)
        {
            return "Authentication/InvalidToken.aspx?idPerson=" + idPerson.ToString() + "&idProvider=" + idProvider.ToString() + "&token=" + message.ToString();
        }
        public static string TokenValidation(Boolean allowAdmin)
        {
            return "Authentication/TokenValidation.aspx?idPerson=" + ((allowAdmin == true) ? "?AdminAccess=true" : "");
        }
    }
}