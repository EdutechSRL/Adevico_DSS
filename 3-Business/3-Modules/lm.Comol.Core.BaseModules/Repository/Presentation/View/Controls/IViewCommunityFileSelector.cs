using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewCommunityFileSelector  : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean EnableClientScript { get; set; }

        void InitializeControl(Int32 idCommunity, List<long> selectedFiles, Boolean showHiddenItems, Boolean forAdminPurpose, Boolean disableWaitingFiles, RepositoryItemType type);
        void InitializeControl(Int32 idCommunity, List<long> selectedFiles, Boolean loadIntoTree, Boolean showHiddenItems, Boolean forAdminPurpose, Boolean disableWaitingFiles, RepositoryItemType type);
        void LoadTree(dtoFileFolder tree);
        void UnselectAll();
        void DisplayNoFilesFound();
    }
}