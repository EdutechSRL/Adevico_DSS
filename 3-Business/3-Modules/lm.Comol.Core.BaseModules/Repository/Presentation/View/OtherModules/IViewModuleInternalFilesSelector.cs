using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewModuleInternalFilesSelector  : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        int FilesCount {get;}
        Boolean HasFileToPublish {get;}
        List<long> SelectedItemFileLinksId {get;}
        List<long> SelectedModuleFileId { get; }
        Boolean HasPermissionToSelectFile { get; }
        int SelectorIdCommunity { get; }

        void InitializeNoPermission(int idCommunity);
        void InitializeView(IList<iCoreItemFileLink<long>> links, long selectedLinkId, int idCommunity);
        void InitializeView(IList<iCoreItemFileLink<long>> links, List<long> selectedLinksId, int idCommunity);
        void UpdateSelectedFilesId(List<long> filesID);
    }
}