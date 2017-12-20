using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewEditProvider : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long PreloadedIdProvider { get; }
        Boolean AllowManagement { set; }
        Boolean AllowEdit { get; set; }
        long IdProvider { get; set; }
        IdentifierField IdentifierFields { get; set; }

        void LoadProviderInfo(dtoProvider provider, String name, AuthenticationProviderType type, Boolean allowAdvancedSettings);
        void LoadTranslations(IdentifierField fields, List<lm.Comol.Core.DomainModel.Language> languages);
      
        void DisplayErrorSaving();
        void DisplaySessionTimeout();
        void DisplayProviderUnknown();
        void DisplayDeletedProvider(String name, AuthenticationProviderType type);
        void DisplayNoPermission();
        void GotoManagement();
    }
}