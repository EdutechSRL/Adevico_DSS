using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Domain
{
    [Serializable]
    public class ModuleNoticeboard
    {
        public const string UniqueID = "SRVBACH";
        public virtual Boolean DeleteMessage {get;set;}
        public virtual Boolean EditMessage {get;set;}
        public virtual Boolean ManagementPermission {get;set;}
        public virtual Boolean Administration { get; set; }
        public virtual Boolean PrintMessage {get;set;}
        public virtual Boolean RetrieveOldMessage {get;set;}
        public virtual Boolean ViewCurrentMessage {get;set;}
        public virtual Boolean ViewOldMessage { get; set; }

        public ModuleNoticeboard() {
         
        }

        

        public ModuleNoticeboard(String permission) { }
            

        public ModuleNoticeboard(long permission)
        {
            ViewCurrentMessage = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ReadMessages | (long)Base2Permission.EditMessage | (long)Base2Permission.Management, permission);
            ViewOldMessage = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ReadMessages | (long)Base2Permission.EditMessage | (long)Base2Permission.Management, permission);
            PrintMessage = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ReadMessages | (long)Base2Permission.EditMessage | (long)Base2Permission.Management, permission);
            EditMessage = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditMessage | (long)Base2Permission.Management, permission);
            DeleteMessage = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteMessages | (long)Base2Permission.Management | (long)Base2Permission.Management, permission);
            RetrieveOldMessage = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditMessage | (long)Base2Permission.Management | (long)Base2Permission.Management, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Management, permission);
            ManagementPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Management | (long)Base2Permission.ManagePermission, permission);
        }

        public static ModuleNoticeboard CreatePortalmodule(int UserTypeID){
            ModuleNoticeboard module = new ModuleNoticeboard();
            module.ViewCurrentMessage = (UserTypeID != (int)UserTypeStandard.Guest );
            module.ViewOldMessage = (UserTypeID != (int)UserTypeStandard.Guest );
            module.PrintMessage= (UserTypeID !=(int)UserTypeStandard.Guest);
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.ManagementPermission = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.DeleteMessage = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.EditMessage = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.RetrieveOldMessage = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            
            return module;
        }

        [Flags]
        public enum Base2Permission
        {
            ReadMessages = 1,
            EditMessage = 2,
            DeleteMessages = 8,
            ManagePermission = 32,
            Management = 64,
            AddMessage = 8192,
        }

        public enum ActionType{
         	None = 12000,
			NoPermission = 12001,
			GenericError = 12002,
			Show = 12003,
			Created = 12004,
			SavedMessage = 12005,
			Clean = 12006,
			VirtualDelete = 12007,
			Undelete = 12008,
			Delete = 12009,
			ShowHistory = 12010,
			SetDefault = 12011,
            ShowRecycle = 12012,
            UndeleteAndActivate = 12014,
            ViewMessage = 12015,
            ViewEmptyMessage = 12016,
            ViewUnknownMessage = 12017,
            AddingNewMessage = 12018,
            AddingNewMessageWithBasicEditor = 12019,
            EditingMessage = 12020,
            EditingMessageWithBasicEditor = 12021,
            UnableToAddMessage = 12022,
            UnableToSaveMessage = 12023,
         }

        public enum ObjectType{
            None = 0,
            Message = 1,
            Noticeboard = 2
        }

    }
}