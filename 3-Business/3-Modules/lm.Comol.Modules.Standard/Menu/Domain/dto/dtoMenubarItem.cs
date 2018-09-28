using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoMenubarItemPermission
    {
        public virtual dtoMenubar MenuBar { get; set; }
        public virtual dtoMenubarPermission Permission { get; set; }

        public dtoMenubarItemPermission() { }
        public dtoMenubarItemPermission(dtoMenubar menubar, dtoMenubarPermission permission)
        {
            this.MenuBar = menubar;
            this.Permission = permission;
        }
        public dtoMenubarItemPermission(dtoMenubar menubar, ModuleMenu module)
        {
            dtoMenubarPermission permission = new dtoMenubarPermission();
            Boolean AllowManage = ((module.ManagePortalMenubar && menubar.MenuBarType == MenuBarType.Portal)
                 || (module.ManageAdministrationMenubar && menubar.MenuBarType == MenuBarType.PortalAdministration)
                 || (module.ManageCommunitiesMenubar && menubar.MenuBarType == MenuBarType.GenericCommunity));
            Boolean AllowSetActive = ((module.SetActivePortalMenubar && menubar.MenuBarType == MenuBarType.Portal)
                || (module.SetActiveAdministrationMenubar && menubar.MenuBarType == MenuBarType.PortalAdministration)
                || (module.SetActiveCommunitiesMenubar && menubar.MenuBarType == MenuBarType.GenericCommunity));

            if (AllowManage) {
                permission.AddItem = AllowManage;
                permission.Delete = (menubar.Deleted != Core.DomainModel.BaseStatusDeleted.None  );
                permission.VirtualDelete = (menubar.Deleted == Core.DomainModel.BaseStatusDeleted.None && !menubar.IsCurrent);
                permission.VirtualUndelete = (menubar.Deleted != Core.DomainModel.BaseStatusDeleted.None);
                permission.Edit = AllowManage;
                permission.SetActive = (AllowManage && AllowSetActive && (menubar.Deleted == Core.DomainModel.BaseStatusDeleted.None));
            }
            this.MenuBar = menubar;
            this.Permission = permission;
        }
    }
}
