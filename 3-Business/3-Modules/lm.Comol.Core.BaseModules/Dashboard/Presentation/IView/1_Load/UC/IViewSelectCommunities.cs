using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Communities;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewSelectCommunities : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean AdministrationMode { get; set; }
        Boolean HasSelectedCommunities { get; }
        Boolean AllowAdd { get; set; }
        Boolean isInitialized { get; set; }
        Boolean AddControlUsed { get; set; }
        Boolean HasAvailableCommunitiesToAdd { get;  }
        List<dtoModulePermission> andServiceClauses { get; set; }
        List<dtoModulePermission> orServiceClauses { get; set; }
        List<Int32> NotLoadIdCommunities { get; set; }
        List<Int32> OnlyFromOrganizations { get; set; }
                
        List<dtoCommunityPlainItem> GetSelectedItems();
        List<dtoCommunityPlainItem> SelectedCommunities { get; set; }
        List<Int32> SelectedIdCommunities { get;  }




        void InitializeControl(String displayInfo, List<dtoCommunityPlainItem> loadCommunities, List<Int32> unloadIdCommunities, List<Int32> onlyFromOrganizations);
        void InitializeControlByServices(String displayInfo, List<dtoCommunityPlainItem> loadCommunities, List<Int32> unloadIdCommunities, List<dtoModulePermission> andClause);
       // void InitializeControlByServices(String displayInfo, List<dtoCommunityPlainItem> loadCommunities, List<Int32> unloadIdCommunities, List<lm.Comol.Core.BaseModules.CommunityManagement.dtoModulePermission> andClause, List<lm.Comol.Core.BaseModules.CommunityManagement.dtoModulePermission> orClause);

        void NoItems();
        void LoadItems(List<dtoCommunityPlainItem> communities);
        void InitializeAddControl(Int32 idProfile,List<Int32> unloadIdCommunities, List<Int32> onlyFromOrganizations);
        void InitializeAddControlByService(Int32 idProfile, List<Int32> unloadIdCommunities, List<dtoModulePermission> andClause);
        void InitializeAddControlByService(Int32 idProfile, List<Int32> unloadIdCommunities, List<dtoModulePermission> andClause, List<dtoModulePermission> orClause);
        void UpdateSelectedCommunities(List<Int32> idCommunities);
        void DisplaySessionTimeout();
    }
}