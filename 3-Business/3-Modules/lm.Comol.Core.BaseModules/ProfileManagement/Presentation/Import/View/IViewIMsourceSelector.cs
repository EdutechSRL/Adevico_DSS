using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Subscriptions;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewIMsourceSelector : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        SourceType CurrentSource { get; set; }
        lm.Comol.Core.DomainModel.Helpers.SearchRange CurrentRange { get; set; }
        Int32 IdCommunityRange { get; set; }
        
        Boolean isInitialized { get; set; }
        void InitializeControl(SourceType defaultSource);
        void InitializeControl(SourceType defaultSource, lm.Comol.Core.DomainModel.Helpers.SearchRange range, Int32 idCommunity = -1);

        void LoadAvailableSources(List<SourceType> items);
        void DisplaySessionTimeout();
    }
}