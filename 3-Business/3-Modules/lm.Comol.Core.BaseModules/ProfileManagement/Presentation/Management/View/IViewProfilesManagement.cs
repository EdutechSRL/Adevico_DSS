using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewProfilesManagement : IViewBaseProfilesManagement
    {
        Boolean AllowDisplayLoginInfo  { get; set; }
        Boolean CurrentDisplayLoginInfo  { get; set; }
        Dictionary<Int32, String> AvailableLogins { get; set; }
        long SelectedIdProvider { get; set; }
        void LoadAuthenticationProviders(List<dtoBaseProvider> providers, long IdDefaultProvider);
        Boolean SendMail(InternalLoginInfo user, String password);

        void DisplayProfileDisabled(Boolean disabled, String user);
        void DisplayProfileEnabled(Boolean disabled, String user);
        void DisplayProfileActivated(Boolean disabled, String user);
        void DisplayUnableToChangePassword( String user);
        void DisplayUnableToSendPassword( String user);
        void DisplayPasswordChanged(String user);
    }
}