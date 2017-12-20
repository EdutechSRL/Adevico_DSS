using System;
using System.Linq;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.TemplateMessages
{
    [Serializable]
    public class ModuleGenericTemplateMessages
    {
        public String uniqueCode;
        public virtual String UniqueCode { get{ return uniqueCode;}}

        public virtual Boolean List { get; set; }
        public virtual Boolean Add { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Clone { get; set; }
        public virtual Boolean DeleteOtherTemplates { get; set; }
        public virtual Boolean DeleteMyTemplates { get; set; }
        public virtual Boolean Administration { get; set; }
        public virtual Boolean ManageModulePermission { get; set; }
        public virtual Boolean SendMessage { get; set; }
        public ModuleGenericTemplateMessages(string code)
        {
            uniqueCode = code;
        }
        public ModuleGenericTemplateMessages(ModuleTemplateMessages m)
        {
            uniqueCode = ModuleTemplateMessages.UniqueCode;
            List  = m.List;
            Add  = m.Add;
            Edit  = m.Edit;
            Clone  = m.Clone;
            DeleteOtherTemplates  = m.DeleteOtherTemplates;
            Administration  = m.Administration;
            ManageModulePermission  = m.ManageModulePermission;
        }
        public ModuleGenericTemplateMessages(long permission)
        {
            List = PermissionHelper.CheckPermissionSoft((long)ModuleTemplateMessages.Base2Permission.List, permission);
            Add = PermissionHelper.CheckPermissionSoft((long)ModuleTemplateMessages.Base2Permission.Add, permission);
            Edit = PermissionHelper.CheckPermissionSoft((long)ModuleTemplateMessages.Base2Permission.Edit, permission);
            Clone = PermissionHelper.CheckPermissionSoft((long)ModuleTemplateMessages.Base2Permission.Clone, permission);
            DeleteOtherTemplates = PermissionHelper.CheckPermissionSoft(((long)ModuleTemplateMessages.Base2Permission.Administration | (long)ModuleTemplateMessages.Base2Permission.DeleteOther), permission);
            DeleteMyTemplates = PermissionHelper.CheckPermissionSoft((long)ModuleTemplateMessages.Base2Permission.Add, permission);
            Administration = PermissionHelper.CheckPermissionSoft(((long)ModuleTemplateMessages.Base2Permission.Administration | (long)ModuleTemplateMessages.Base2Permission.ManageTemplates), permission);
            ManageModulePermission = PermissionHelper.CheckPermissionSoft(((long)ModuleTemplateMessages.Base2Permission.Administration | (long)ModuleTemplateMessages.Base2Permission.GrantPermission), permission);
        }
        public long GetPermissions()
        {
            long permission = 0;
            if (List)
                permission = permission | (long)ModuleTemplateMessages.Base2Permission.List;
            if (Add)
                permission = permission | (long)ModuleTemplateMessages.Base2Permission.Add;
            if (Edit)
                permission = permission | (long)ModuleTemplateMessages.Base2Permission.Edit;
            if (Clone)
                permission = permission | (long)ModuleTemplateMessages.Base2Permission.Clone;
            if (Administration)
                permission = permission | (long)ModuleTemplateMessages.Base2Permission.Administration;
            if (DeleteOtherTemplates)
                permission = permission | (long)ModuleTemplateMessages.Base2Permission.DeleteOther | (long)ModuleTemplateMessages.Base2Permission.Administration;
            /*if (DeleteMyTemplates)
                permission = permission || (long)Base2Permission.Add;*/
            if (ManageModulePermission)
                permission = permission | (long)ModuleTemplateMessages.Base2Permission.GrantPermission;
            return permission;
        }
    }
}