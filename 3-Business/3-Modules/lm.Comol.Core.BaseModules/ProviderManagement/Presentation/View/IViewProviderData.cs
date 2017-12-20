using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewProviderData : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long IdProvider { get; set; }
        AuthenticationProviderType ProviderType { get; set; }
        dtoProvider Current { get; set; }
        IdentifierField IdentifierFields { get; set; }

        Boolean ValidateContent();
        Boolean SaveData();

        void InitializeControl(dtoProvider provider, Boolean allowAdvancedSettings);
        void LoadProvider(dtoProvider provider, Boolean showAdvancedSettings);
        void DisplayDuplicateCode();
        void DisplayProviderUnknown();
    }
}