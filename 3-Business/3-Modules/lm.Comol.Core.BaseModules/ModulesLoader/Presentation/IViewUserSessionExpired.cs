using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ModulesLoader.Presentation
{
    public interface IViewUserSessionExpired : IViewGenericUserSessionExpired
    {
        Boolean PreservePreviousUrl { get; }
        Boolean PreloadedIsUnknownAccess  { get; }
        dtoExpiredAccessUrl PreloadedUnknownAccess { get; }
        dtoExpiredAccessUrl UnknownAccessTicket { get; set; }

        //void WriteLogonCookies(dtoExpiredAccessUrl item);
        //void GoToDefaultPage();
        //void LoadInternalLoginPage();
        //void LoadExternalProviderPage(String url);
        //void LoadShibbolethLoginPage();
        //void LoadOldAuthenticationPage(int idAuthenticationType);
        void LoadLanguage(Language language);
    }
    //Public Interface 
    

    //    ReadOnly Person PreloadedForUserID() As Integer
    //    ReadOnly Person PreloadedCommunityID() As Integer
    //    ReadOnly Person PreservePreviousUrl() As Boolean
    //    ReadOnly Person PreloadedLanguageCode() As String
    //    ReadOnly Person PreloadedInPopupWindow() As Boolean
    //    ReadOnly Person PreloadedForDownload As Boolean

    //    Person CurrentForUserID() As Integer
    //    Person CurrentCommunityID() As Integer

    //    Person PreservedDownloadUrl() As String
    //    Person CurrentInPopupWindow() As Boolean
    //    Person CurrentForDownload() As Boolean

    //    Sub InitializeView(ByVal CommunityID As Integer)
    //    Sub LoadStandardLoginPage()
    //    Sub LoadInternalLoginPage(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String)
    //    Sub LoadExternalLoginPage(ByVal AuthenticationTypeID As Integer, ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String)
    //    Sub LoadLanguage(ByVal oLanguage As Language)
    //End Interface
}
