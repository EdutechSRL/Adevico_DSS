using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.Standard.Glossary.Domain
{
    [Serializable]
    public class ModuleGlossary
    {
        public const String UniqueCode ="SRVGLS";
        public virtual Boolean ViewTerm {get;set;}
        public virtual Boolean AddTerm {get;set;}
        public virtual Boolean EditTerm {get;set;}
        public virtual Boolean DeleteTerm { get; set; }
        public virtual Boolean ManageGlossary {get;set;}
        public virtual Boolean Administration { get; set; }
        public ModuleGlossary() {
            ViewTerm = false;
        }
        public static ModuleGlossary CreatePortalmodule(int UserTypeID){
            ModuleGlossary module = new ModuleGlossary();
            module.ViewTerm = (UserTypeID != (int)UserTypeStandard.Guest );
            module.AddTerm=(UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.EditTerm=(UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.DeleteTerm=(UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.ManageGlossary=(UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            return module;
        }

        public ModuleGlossary(long permission)
        {
            ViewTerm = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ListItems, permission);
            AddTerm = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddItem, permission);
            EditTerm = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditItem, permission);
            DeleteTerm = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteItem, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Admin, permission);
            ManageGlossary = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManageGroup, permission);
        }

        [Flags,Serializable]
        public enum Base2Permission{
            ListItems = 1,
            AddItem = 2 ,
            EditItem = 4,
            DeleteItem = 8,
            ManageGroup = 16,
            Admin = 64
        }
        [Serializable]
        public enum ActionType{
            None = 86000,
            NoPermission = 86001,
            GenericError = 86002,
            ViewListByLetter = 86003,
            ViewListByIndex = 86004,
            StartAddItem = 86008,
            AddItem = 86009,
            StartEditItem = 86010,
            SaveItem = 86011,
            VirtualDeleteItem = 86012,
            VirtualUndeleteItem = 86013,
            DeleteItem = 86014,
            StartAddGroup = 86015,
            AddGroup = 86016,
            StartEditGroup = 86017,
            SaveGroup = 86018,
            VirtualDeleteGroup = 86019,
            VirtualUndeleteGroup = 86020,
            DeleteGroup = 86021,
            EditPaging
        }
        [Serializable]
        public enum ObjectType{
            None = 0,
            Item = 1,
            Group = 2
        }
    }
}