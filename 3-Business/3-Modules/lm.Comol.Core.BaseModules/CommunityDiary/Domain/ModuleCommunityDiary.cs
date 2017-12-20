using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Domain
{
    [Serializable]
    public class ModuleCommunityDiary
    {
        public const string UniqueID = "SRVLEZ";
        public virtual Boolean UploadFile {get;set;}
        public virtual Boolean DeleteItem {get;set;}
        public virtual Boolean ManagementPermission {get;set;}
        public virtual Boolean Administration { get; set; }
        public virtual Boolean AddItem {get;set;}
        public virtual Boolean Edit {get;set;}
        public virtual Boolean PrintList {get;set;}
        public virtual Boolean ViewDiaryItems { get; set; }
        public ModuleCommunityDiary() {
         
        }

        public static ModuleCommunityDiary CreatePortalmodule(int UserTypeID){
            ModuleCommunityDiary module = new ModuleCommunityDiary();
            module.ViewDiaryItems = (UserTypeID != (int)UserTypeStandard.Guest );
            module.PrintList= (UserTypeID !=(int)UserTypeStandard.Guest);
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.DeleteItem = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.Edit = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.AddItem= (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.UploadFile= (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.ManagementPermission  = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            
            return module;
        }

        public ModuleCommunityDiary(String permission) { 
            
        }
        public ModuleCommunityDiary(long permission)
        {
            ViewDiaryItems = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewLessons | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
            Edit = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
            PrintList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewLessons | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
            ManagementPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.GrantPermission , permission);
            UploadFile = PermissionHelper.CheckPermissionSoft((long)Base2Permission.UploadFile | (long)Base2Permission.AddLesson | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
            AddItem = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddLesson | (long)Base2Permission.AdminService, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AdminService, permission);
            DeleteItem  = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteLesson | (long) Base2Permission.AdminService , permission); 
        }

        [Flags]
        public enum Base2Permission
        {
            ViewLessons = 1,
            EditLesson = 4,
            UploadFile = 2,
            DeleteLesson = 8,
            PrintLessons = 2048,
            GrantPermission = 32,
            AdminService = 64,
            AddLesson = 8192
        }

         public enum ActionType{
            None = 0,
            NoPermission = 1,
            GenericError = 2,
            ShowDiary = 10003,
            CreateItem = 10004,
            ChangeItem = 10005,
            VirtualDeleteItem = 10006,
            UndeleteItem = 10007,
            DeleteItem = 10008,
            DeleteDiary = 10009,
            ImportItem = 10010,
            AddFiles = 10011,
            RemoveFiles = 10012,
            MoveItem = 10013,
            AddFile = 10014,
            RemoveFile = 10015,
            AddMultipleFilesNoDate = 10016,
            AddFileToItemNoDate = 10017,
            AddFileScormToItemNoDate = 10018,
            AddFileScorm = 10019,
            InitPublishFileIntoCommunity = 10020,
            InitAddCommunityFiles = 10021,
            InitUploadMultipleFiles = 10022,
            InitAddItem = 10023,
            InitEditItem = 10024,
            CreateItemNoDate = 10025,
            EditItemNoDate = 10026,
            DownloadFileItem = 10027,
            ShowFileItem = 10028,
            HideFileItem = 10029,
            ViewItemsToPrint = 10030,
            OpenItemsToPrintWindow = 10031,
            UnkownDiary = 10032,
            UnkownDiaryItem = 10033,
            ShowDiaryItem = 10034,
            HideDiaryItem = 10035,
            NoPermissionToPublish = 10036,
            NoPermissionToEditMetadata = 10037,
            NoPermissionToViewItemPersonalStatistics  = 10038,
            NoPermissionToViewItemAdvancedStatistics  = 10039,
            AttachmentsNotAddedFiles = 10040,
            AttachmentsAddedFiles = 10041,
         }

        public enum ObjectType{
            None = 0,
            Diary = 1,
            DiaryItem = 2,
            File = 3,
            FileScorm = 4,
            DiaryItemFile = 5,
            DiaryItemLinkedFile=6
        }


    }
}