using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.DomainModel.Helpers;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewModuleModuleToRepositoryDisplay : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
       
        String ModuleCode { get; set; }
        Int32 IdModule { get; set; }
        String FileDisplayName { get; set; }
        Boolean DisplayAsTable { get; set; }
        Boolean DisplayOnly { get; set; }
        IconSize IconSize { get; set; }


        void DisplayNoAction();
        void DisplayRemovedObject();
        void DisplayLinkDownload(long idLink, Int32 idCommunity, BaseCommunityFile item, iCoreFilePermission permission);
        void DisplayLinkForPlay(long idLink, Int32 idCommunity, BaseCommunityFile item, iCoreFilePermission permission);
        void DisplayLinkForPlayInternal(long idLink, Int32 idCommunity, BaseCommunityFile item, iCoreFilePermission permission,Int32 idAction);
    }
}