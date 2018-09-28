using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.Standard.Menu 
{
    [Serializable]
    public class ModuleMenu
    {
        public const String UniqueCode = "SRVmenu";
        public virtual Boolean ManagePortalMenubar {get;set;}
        public virtual Boolean ManageAdministrationMenubar { get; set; }
        public virtual Boolean ManageCommunitiesMenubar { get; set; }
        
        public virtual Boolean SetActivePortalMenubar { get; set; }
        public virtual Boolean SetActiveAdministrationMenubar { get; set; }
        public virtual Boolean SetActiveCommunitiesMenubar { get; set; }


        public ModuleMenu() {

        }
        public static ModuleMenu CreatePortalmodule(int UserTypeID)
        {
            ModuleMenu module = new ModuleMenu();
            module.ManagePortalMenubar = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative   );
            module.ManageAdministrationMenubar = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.ManageCommunitiesMenubar = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);

            module.SetActivePortalMenubar = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.SetActiveAdministrationMenubar = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator); 
            module.SetActiveCommunitiesMenubar = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);

            return module;
        }

        public ModuleMenu(long permission)
        {
            //AddPersonal = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddPersonal, permission);
            //AddSubmission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddSubmission, permission);
            //CreateCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddCall, permission);
            //EditCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditCall, permission);
            //Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Admin, permission);
            //ManageCallForPapers = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManageCalls, permission);
            //DeleteOwnCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteCall, permission); 
        }


        [Flags,Serializable]
        public enum Base2Permission{
            View = 1,
            Manage = 2 ,
            Edit = 4,
            Delete = 8,
            AddPersonal = 16
        }

        [Serializable]
        public enum ActionType{
            //None = 87000,
            //NoPermission = 87001,
            //GenericError = 87002,
            //AddSticky = 87003,
            //EditSticky = 87004,
            //VirtualDeleteSticky = 87005,
            //VirtualUndeleteSticky = 87006,
            //DeleteSticky = 87007,
            //HideSticky = 87008,
            //ShowSticky = 87009,
            //AddStickyToUser = 87010,
            //AddStickyToCommunity = 87011,
            //AddStickyFromService = 87012
        }
        [Serializable]
        public enum ObjectType{
            None = 0,
            MenuBar = 1,
            TopMenuItem = 2,
            ColumnItem = 3,
            MenuItem = 4
        }
    }
}