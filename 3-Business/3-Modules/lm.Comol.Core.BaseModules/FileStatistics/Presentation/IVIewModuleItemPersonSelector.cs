using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public interface IVIewModuleItemPersonSelector
    {
        int UserCount { get; }
        Boolean HasFileToSelect { get; }
        Boolean HasPermissionToSelectUser { get; }
        Boolean isInitialized { get; set; }
        List<Int32> SelectedUsersId { get; }

        void InitializeNoPermission(int idCommunity);
        void InitializeView(String moduleCode, int objectId, int objectTypeId, IList<Int32> UsersIds, int idCommunity);
        void UnselectAllUsers();
    }
}
