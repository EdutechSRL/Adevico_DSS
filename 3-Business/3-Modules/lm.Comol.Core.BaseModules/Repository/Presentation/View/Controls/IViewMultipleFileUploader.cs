using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewMultipleFileUploader  : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 MaxFileInputsCount { get; set; }
        long IdFolder{ get; set; }
        Int32 IdCommunityRepository { get; set; }

        String Portalname {get;}
        String BaseFolder {get;}

        String CommunityName {set;}
        String FilePath {set;}
        Boolean AllowFolderChange { set; }
        Boolean VisibleToDonwloaders { get; set; }
        Boolean DownlodableByCommunity { get; set; }
        Boolean AllowPersonalPermission { get; set; }
        CoreModuleRepository Permission { get; set; }

        void InitializeControl(long idFolder, Int32 idCommunity,CoreModuleRepository permission);
        void LoadFolderSelector(long idFolder, Int32 idCommunity, Boolean showHiddenItems, Boolean forAdminPurpose);
    }
}