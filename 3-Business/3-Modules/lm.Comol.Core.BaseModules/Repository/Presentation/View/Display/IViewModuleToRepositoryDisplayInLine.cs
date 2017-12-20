using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewModuleToRepositoryDisplayInLine : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        String ServiceCode { get; set; }
        int ServiceID { get; set; }
        Boolean DisplayInlineName { get; set; }
        Boolean OverrideToolTip { get; set; }
        //Boolean ShowOnSingleline  { get; set; }
        String DescriptionBefore { get; set; }
        String DescriptionAfter { get; set; }
        lm.Comol.Core.DomainModel.Helpers.IconSize IconSize { get; set; }

        void DisplayNoAction();
        void DisplayRemovedObject();
        //void DisplayLinkDownload(long IdLink, int IdCommunity, BaseCommunityFile Item);
        //void DisplayLinkForPlay(long IdLink, int IdCommunity, BaseCommunityFile Item);
        //void DisplayLinkForPlayInternal(long IdLink, int IdCommunity, BaseCommunityFile Item, int ServiceActionID);
        void DisplayLink(long IdLink, int IdCommunity, BaseCommunityFile Item);
        void DisplayLinkForModule(long IdLink, int IdCommunity, BaseCommunityFile Item, int ServiceActionID);
    }
}