using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    public class ModuleDocTemplate
    {
        public const String UniqueCode ="SRVDOCT";
        public virtual Boolean ViewTemplates {get;set;}
        public virtual Boolean AddTemplate {get;set;}
        public virtual Boolean EditTemplates {get;set;}
        public virtual Boolean EditBuiltInTemplates { get; set; }
        public virtual Boolean ManageTemplates {get;set;}
        public virtual Boolean DeleteTemplates {get;set;}

        public ModuleDocTemplate() {
            ViewTemplates = true;
        }
        public static ModuleDocTemplate CreatePortalmodule(int UserTypeID){
            ModuleDocTemplate module = new ModuleDocTemplate();
            module.ViewTemplates = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.AddTemplate=(UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.EditTemplates =(UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.EditBuiltInTemplates= (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative   );
            module.ManageTemplates = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.DeleteTemplates = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            return module;
        }

        public ModuleDocTemplate(long permission)
        {
            ViewTemplates = PermissionHelper.CheckPermissionSoft((long)Base2Permission.List, permission);
            AddTemplate = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add, permission);
            EditTemplates = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Edit, permission);
            EditBuiltInTemplates = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditBuiltIn, permission);
            ManageTemplates = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Manage, permission);
            DeleteTemplates = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Delete, permission);
        }

        [Flags,Serializable]
        public enum Base2Permission{
            List = 1,
            Add = 2 ,
            Edit = 4,
            Delete = 8,
            Manage = 16,
            EditBuiltIn = 64,
        }
//         PermissionType
//'    None = -1                                           '    Admin = 6 '64
//'    Read = 0 '1                                         '    Send = 7 ' 128
//'    Write = 1 '2                                        '    Receive = 8 '256
//'    Change = 2 '4                                       '    Synchronize = 9 '512
//'    Delete = 3 '8                                       '    Browse = 10 '1024      ViewCommunityProjects
//'    Moderate = 4 '16                                    '    Print = 11 '2048
//'    Grant = 5 '32                                       '    ChangeOwner = 12 '4096
//'    Add = 13 '8192        AddCommunityProject                                        '    ChangeStatus = 14 '16384
//'    DownLoad = 15 '32768
        [Serializable]
        public enum ActionType{
            None = 23000,
            NoPermission = 23001,
            GenericError = 23002,
            List = 23003,
            Manage = 23004,
            Edit = 23005,
            Delete = 23006,
            Add = 23007,
            EditTemplate = 23008,
            VirtualDelete = 23009,
            VirtualUndelete = 23010,
            PhisicalDelete = 23011,
            LinkToService = 23012,
            Preview = 23013
        }
        [Serializable]
        public enum ObjectType{
            None = 0,
            Template = 1
        }
    }
}
