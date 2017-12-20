using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewFolderSelector : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Boolean AutoPostBack { get; set; }
        Boolean AlsoSelectedQuota { get; set; }
        long IdSelectedFolder { get; }

        RepositoryType RepositoryType { get; set; }
        Int32 RepositoryIdCommunity { get; set; }
        void  InitializeControl(long idFolder, String folderName,List<dtoNodeFolderItem> folders );
        void InitializeControl(RepositoryType type, Int32 idCommunity, long idFolder, String folderName, List<dtoNodeFolderItem> folders);
        void InitializeControl(RepositoryType type, Int32 idCommunity);
    }
}