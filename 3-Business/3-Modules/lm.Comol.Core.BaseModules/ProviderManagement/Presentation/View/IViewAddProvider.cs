using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewAddProvider : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Boolean AllowManagement { set; }
        Boolean AllowAdd { get; set; }

        ProviderWizardStep CurrentStep { get; }
        List<ProviderWizardStep> AvailableSteps { get; set; }

        long IdProvider { get; set; }

        AuthenticationProviderType SelectedType { get; set; }
        void LoadAvailableTypes(List<AuthenticationProviderType> types);

        AuthenticationProviderType ProviderInfoType { get;  }
        Boolean ValidateProviderInfo();

        dtoProvider ProviderInfo { get; }
        IdentifierField IdentifierFields { get;}
        void LoadProviderInfo(dtoProvider provider, Boolean allowAdvancedSettings);

        Boolean isTranslationInitialized { get; }
        void InitializeTranslation(long idProvider, Int32 idLanguage, IdentifierField fields);
        void UpdateTranslationView(IdentifierField fields);
        Boolean ValidateDefaultTranslation();


        void LoadSummaryInfo(dtoProvider provider, AuthenticationProviderType type);

        void GotoStep(ProviderWizardStep pStep);

        void DisplayErrorSaving();
        void DisplaySessionTimeout();
        void DisplayNoPermission();
        void GotoManagement(long idProvider);
        void GotoSettings(long idProvider,AuthenticationProviderType type);
    }
}