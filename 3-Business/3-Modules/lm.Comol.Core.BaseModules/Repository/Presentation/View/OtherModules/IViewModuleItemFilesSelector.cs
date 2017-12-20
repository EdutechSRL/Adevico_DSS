using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewModuleItemFilesSelector : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        int FilesCount { get; }
        Boolean HasFileToSelect { get; }
        Boolean HasPermissionToSelectFile { get; }
        Boolean isInitialized {get;set;}
        StatTreeNode<StatFileTreeLeaf> LoadedItems { get; set; }
        List<StatFileTreeLeaf> LeafNodes { get; set; }
        List<StatFileTreeLeaf> SelectedFiles { get; }

        void InitializeNoPermission(int idCommunity);
        void InitializeView(String moduleCode, int objectId, int objectTypeId, IList<long> FilesIds, int idCommunity);
        void ViewFileByType(StatTreeLeafType type);
        void UnselectAllFiles();
    }
}