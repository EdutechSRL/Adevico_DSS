using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewCommunityDetailsPage : IViewPageBase
    {
        Boolean PreloadFromPage { get; }
        Boolean PreloadFromSession { get; }
        void SetBackUrl(String url);
        void SetTitle(String name);
        void InitializeDetails(liteCommunityInfo community);
        void DisplayUnknownCommunity();

    }
}