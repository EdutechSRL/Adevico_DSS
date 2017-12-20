using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ModulesLoader.Presentation
{

    public interface IViewExternalModuleLoader: lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        String PreLoadedExternalSource { get; }
        String PreLoadedExternalID { get; }
        String PreLoadedModuleCode { get; }
        String PreloadedModulePage { get; }
        String PreloadedPreviousUrl { get; }
        String PortalHome { get; }
        //System.Guid UniqueAccessId { get; set; }
        //String PreviousUrl { get; set; }
        String GetEncodedIdUser(string urlIdentifier);

        //void AnalyzesRequest(Guid accessId);
        void UnknowAuthenticationProvider();
       // void showAuthenticationInfo(UrlProviderResult result);
        void showLogonInfo(UrlProviderResult result, String loginUrl);
        void ShowAuthenticationResult(AuthenticationResult result);
        void LoadWaitingMessage(Person person, String communityName);
        void LoadModuleWithLogon(Person person, int IdCommunity, String destinationUrl);
        void LoadModule(int IdCommunity, String destinationUrl);
        void LoadUnknowCommunity();
        void LoadUnsubscribedCommunity(String communityName);
        void LoadUserLanguage(int IdLanguage, string code);
    }
}