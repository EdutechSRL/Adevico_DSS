using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.PolicyManagement.Presentation
{
    public interface IViewAcceptLogonPolicy : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 PreloggedUserId { get; set; }
        long PreloggedProviderId { get; set; }
        String PreloggedProviderUrl { get; set; }


        int LoggedUserId { get; set; }
        long LoggedProviderId { get; set; }
        String LoggedProviderUrl { get; set; }
        void GotoInternalShibbolethAuthenticationPage();
        void GotoInternalAuthenticationPage();
        void GotoShibbolethAuthenticationPage();
        void GotoExternalUrl(String url);
        void LoadPolicySubmission(Int32 idUser);
        void LogonUser(Person user, Int32 idCommunity,long idProvider, String providerUrl);
    }
}
