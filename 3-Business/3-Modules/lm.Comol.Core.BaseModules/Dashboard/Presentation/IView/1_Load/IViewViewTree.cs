using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewViewTree : IViewPageBase
    {
        Boolean PreloadAdvancedTree { get; }
        Boolean PreloadFromPage { get; }
        Boolean PreloadFromSession { get; }
        lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability PreloadMode { get; }
        void SetBackUrl(String url);
        void SetTitle(String name);
        
        /// <summary>
        /// Load tree
        /// </summary>
        /// <param name="advanced">common tree or communities group by type</param>
        /// <param name="cIdCommunity">current community</param>
        void LoadTree(Boolean advanced, Int32 cIdCommunity=0);
        void DisplayUnknownCommunity();
    }
}