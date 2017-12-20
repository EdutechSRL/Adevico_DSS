using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewTokenValidation : IViewBaseAuthenticationPages
    {
        //int LoggedUserId { get; set; }
        Boolean AllowAdminAccess { get; }
        Boolean isSystemOutOfOrder { get; }
        Boolean IsFromLogoutPage { get; }
        Boolean HasUrlValues { get; }
        Boolean SubscriptionActive { get; }
        //Boolean AllowExternalWebAuthentication { set; }
        Boolean AllowSubscription { get; set; }
        String PreloadFromUrl { get; }
        Boolean PreloadForDebug { get; }
        String FromLogonUrl { get; set; }

        dtoMacUrlProviderIdentifier GetUrlIdentifier(List<dtoMacUrlProviderIdentifier> availableItems);
        List<dtoMacUrlUserAttribute> GetTokenAttributes(List<dtoMacUrlUserAttribute> attributes);
        String GetUrlAttributeValue(string name);

        void DisplaySystemOutOfOrder();
        void DisplayDebugInfo(dtoMacUrlToken token);
        void DisplayProviderNotFound();
        void DisplayUrlAuthenticationUnavailable();
        void DisplayInvalidMessage(UrlProviderResult message);
        void DisplayInvalidMessage(string username, UrlProviderResult message);
        void DisplayNoToken();
        void DisplayAccountDisabled(String url);
        void DisplayPrivacyPolicy(int userId, long idProvider, String defaultUrl, Boolean internalPage);
        void DisplayTaxCodeAlreadyPresent();
        void DisplayAutoEnrollmentFailed();
        void LoadLanguage(Language language);
        void SetAutoLogonUrl(String url);
        void LogonUser(Person user, long idProvider, String defaultUrl, Boolean internalPage, Int32 idUserDefaultIdOrganization);
        void GoToProfile(dtoMacUrlToken token, string wizardUrl);

        Int32 CreateUserProfile(dtoBaseProfile profile, Int32 idProfileType, Int32 idOrganization,  MacUrlAuthenticationProvider provider,dtoExternalCredentials credentials  );

        void UpdateLogonXml(Int32 idUser);
        Boolean DeletePreviousProfileType(Int32 idProfile, Int32 idOldType, Int32 idNewType);
        Boolean EditProfileType(Int32 idProfile, dtoBaseProfile profile, Int32 idOldType, Int32 idNewType);
        dtoBaseProfile GetOldTypeProfileData(Int32 idProfile, Int32 idOldType);

        void SendMail(dtoBaseProfile profile,Boolean inserted);
        //
        //void DisplayAuthenticationOutOfOrder();
        //void GoToProfile(long idProvider, dtoUrlToken urlToken, string wizardUrl);
        //
        //void DisplayPrivacyPolicy(int userId, long idProvider);
        //
        //void SetAutoLogonUrl(String url);
        //void GoToDefaultPage();

        // DA ADEGUARE
        //void DisplayInvalidToken(String url, int idPerson, dtoUrlToken urlToken, UrlProviderResult status);

        // DA ADEGUARE
        //dtoUrlToken GetUrlToken(List<String> identifiers);

        // DA ADEGUARE
        //void DisplayInvalidToken(String url, int idPerson, dtoUrlToken urlToken, UrlProviderResult status);

       // void SetExternalWebLogonUrl(String url);
    } 
}