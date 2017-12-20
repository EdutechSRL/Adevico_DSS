using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewProviderInfo : lm.Comol.Core.DomainModel.Common.iDomainView
    {

        void InitializeControl(long IdProvider);
        void LoadProvider(dtoProvider provider);
        void DisplayProviderUnknown();
    }
   
}