using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewTranslationData : lm.Comol.Core.DomainModel.Common.iDomainView
        {
            long IdTranslation { get; set; }
            long IdProvider { get; set; }
            Boolean isInitialized { get; set; }
            Int32 idLanguage { get; set; }
            dtoProviderTranslation Translation { get;}


            Boolean ValidateContent();
            Boolean SaveData();
            void InitializeControl(long idProvider, Int32 idLanguage, IdentifierField fields);
            void LoadTranslation(dtoProviderTranslation translation);
            void UpdateTranslationView(IdentifierField fields);
            void DisplayProviderUnknown();
        }
}