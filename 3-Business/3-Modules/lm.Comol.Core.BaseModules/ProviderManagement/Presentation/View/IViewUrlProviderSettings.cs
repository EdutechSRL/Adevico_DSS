using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewUrlProviderSettings : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long PreloadedIdProvider { get; }
        Boolean AllowManagement { set; }
        Boolean AllowEdit { get; set; }
        long IdProvider { get; set; }

        Authentication.Helpers.EncryptionInfo EncryptionInfo { get; set; }

        void LoadProviderInfo(dtoUrlProvider provider);
        void LoadLoginFormats(List<dtoLoginFormat> items);
        void DisplaySessionTimeout();
        void DisplayProviderUnknown();
        void DisplayDeletedProvider(String name, AuthenticationProviderType type);
        void DisplayNoPermission();
        void GotoManagement();
    }
}