using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public enum ProviderWizardStep
    {
        None = 0,
        SelectType = 1,
        ProviderData = 2,
        DefaultTranslation = 3,
        Summary = 4,
        ErrorMessages = 5
    }
}