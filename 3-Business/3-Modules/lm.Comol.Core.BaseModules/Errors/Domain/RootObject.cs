using System;
using System.Collections.Generic;
namespace lm.Comol.Core.BaseModules.Errors.Domain
{
    public class RootObject
    {
        private readonly static String modulehome = "Modules/Errors/";
        public static String Internal(Boolean displayError = false)
        {
            return modulehome + "InternalError.aspx" + (displayError ? "?displayError=" +displayError.ToString():"");
        }
        public static String Authentication(Boolean displayError = false)
        {
            return modulehome + "AuthenticationError.aspx" + (displayError ? "?displayError=" +displayError.ToString():"");
        }
        public static String External(Boolean displayError = false)
        {
            return modulehome + "ExternalError.aspx" + (displayError ? "?displayError=" +displayError.ToString():"");
        }
        public static String Generic(Boolean displayError = false)
        {
            return modulehome + "GenericError.aspx" + (displayError ? "?displayError=" + displayError.ToString() : "");
        }
        public static String Popup(Boolean displayError = false)
        {
            return modulehome + "PopupError.aspx" + (displayError ? "?displayError=" + displayError.ToString() : "");
        }
        public static String Default(Boolean isAuthenticated, Boolean fromAuthenticationPage = false, Boolean fromExternalService = false, Boolean displayError = false,Boolean popup= false )
        {
            if (popup)
                return Popup(displayError);
            else if (isAuthenticated)
                return Internal(displayError);
            else if (fromAuthenticationPage)
                return Authentication(displayError);
            else if (fromExternalService)
                return External(displayError);
            else
                return Generic(displayError);
        }
        public static List<String> GetErrorPages()
        {
            List<String> pages = new List<string>();
            pages.Add("InternalError.aspx");
            pages.Add("AuthenticationError.aspx");
            pages.Add("ExternalError.aspx");
            pages.Add("GenericError.aspx");
            pages.Add("PopupError.aspx");

            return pages;
        }
     }
}