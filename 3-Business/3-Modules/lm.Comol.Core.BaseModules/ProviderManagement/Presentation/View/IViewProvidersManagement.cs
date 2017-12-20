using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewProvidersManagement : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        int PreLoadedPageSize {get;}
        Int32 CurrentPageSize { get; set; }
        PagerBase Pager { get; set; }

        void NoPermission();
        void DisplaySessionTimeout();
        void LoadProviders(List<dtoProviderPermission> items);
        void LoadProviderInfo(long idProvider);
    }
}