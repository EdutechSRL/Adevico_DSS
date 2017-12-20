using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewManageProfileAuthenticationProviders : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean AllowAddprovider { get; set; }
        Boolean AllowEdit { get; set; }
        Boolean AllowRenewPassword { get; set; }
        Boolean AllowSetPassword { get; set; }
        Boolean isAjaxPanel { get; set; }
        long AvailableProvidersCount { get; set; }
        long CurrentIdLoginInfo { get; set; }
        long CurrentIdProvider { get; set; }
        Int32 idProfile { get; set; }
        long idDefaultProvider { get; set; }


        void InitializeControl(Int32 idProfile, Boolean isAjaxPanel);
        void InitializeControlForAdd(Int32 idProfile, Boolean isAjaxPanel);
        void LoadItems(List<dtoUserProvider> providers);
        void SetTitle(String displayName);
        void DisplayProfilerExternalError(lm.Comol.Core.Authentication.ProfilerError message);
        void DisplayProfilerInternalError(lm.Comol.Core.Authentication.ProfilerError message);
        void DisplayError(lm.Comol.Core.Authentication.ProfilerError message);
        void DisplayNoPermission();
        void DisplayWorkinkSessionTimeout();
        void DisplayProfileUnknown();
        
        void EditExternalUserInfo(long idLoginInfo, dtoUserProvider provider, dtoExternalCredentials credentials);
        void EditInternalUserInfo(long idLoginInfo, String login);
        void EditInternalUserPassword(long idLoginInfo, String login,String username, String mail);
        void AddUserProviders(List<dtoBaseProvider> availableProviders);
        void LoadProviderToAdd(dtoBaseProvider provider);
        Boolean SendMail(lm.Comol.Core.Authentication.InternalLoginInfo user, String password);
    }
}