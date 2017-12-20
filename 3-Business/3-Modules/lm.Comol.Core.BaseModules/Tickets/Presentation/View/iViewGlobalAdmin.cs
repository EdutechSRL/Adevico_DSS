using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewGlobalAdmin : iViewBase
    {
        Domain.SettingsPortal Settings { get; set; }
        Domain.DTO.DTO_SysCategoryInfo CategoryInfo { set; }

        void ShowNoPermission();

        void ShowMessage(Domain.Enums.GlobalAdminStatus Status, bool draftLimitErr = false, bool intLimitErr = false, bool extLimitErr = false);

        Domain.DTO.DTO_SettingsPermissionList Permissions { get; set; }


        void ShowSwitchChanged(Domain.DTO.DTO_PortalSettingsSwitch SwitchStatus,  Domain.Enums.GlobalAdminSwitch setSwitch, bool status, bool hasError);

        void SetCategories(Domain.SettingsPortal value);

        Int64 CurrentCategorySelection { get; }
        

        //PARAMETRI PER LEGGERE/SCRIVERE PERMESSI


        //void SetInfo(int CategoryPublic, int CategoryCommunity, int CategoryTicket);

        //int ViewCommunityId { get; set; }
        ////To iView Base Internal
        //void DisplaySessionTimeout();

        //void SendAction(
        //    ModuleTicket.ActionType Action,
        //    Int32 idCommunity,
        //    ModuleTicket.InteractionType Type,
        //    IList<KeyValuePair<Int32, String>> Objects = null);
        // void InitializeUsersSelector(List<Int32> removeUsers);
    }
}
