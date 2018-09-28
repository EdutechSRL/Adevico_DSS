using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class ModuleProjectManagement
    {
        public const String UniqueCode = "SRVTASK";
        public virtual Boolean UploadFile { get; set; }
        public virtual Boolean DeleteTask { get; set; }
        public virtual Boolean ManagementPermission { get; set; }
        public virtual Boolean AddTask { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean PrintList { get; set; }
        public virtual Boolean ViewTasks { get; set; }
        public virtual Boolean Administration { get; set; }
        public virtual Boolean CreatePublicProject { get; set; }
        public virtual Boolean CreatePersonalProject { get; set; }
        public virtual Boolean DownloadAllowed { get; set; }
        public virtual Boolean PrintTaskList { get; set; }
        public virtual Boolean ViewTaskList { get; set; }

        public ModuleProjectManagement()
        {
        }
        public static ModuleProjectManagement CreatePortalmodule(int UserTypeID){
            ModuleProjectManagement module = new ModuleProjectManagement();
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.CreatePublicProject = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.CreatePersonalProject = (UserTypeID != (int)UserTypeStandard.Guest);
           
            module.DownloadAllowed = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.ManagementPermission = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.PrintTaskList = (UserTypeID != (int)UserTypeStandard.Guest);
            module.ViewTaskList = (UserTypeID != (int)UserTypeStandard.Guest);
            
            module.UploadFile= (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            
            return module;
        }

        public ModuleProjectManagement(String permission) { 
            
        }
        public ModuleProjectManagement(long permission)
        {
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration, permission);
            CreatePublicProject = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration | (long)Base2Permission.AddPublicProject, permission);
            CreatePersonalProject = true;   //PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewLessons | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService, permission);
            DownloadAllowed = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewCommunityProjects| (long)Base2Permission.Administration , permission);
            ManagementPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManagementPermission , permission);
            PrintTaskList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration | (long)Base2Permission.ViewCommunityProjects, permission);
            ViewTaskList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration | (long)Base2Permission.ViewCommunityProjects, permission); 
        }
        public long GetPermissions()
        {
            long permission = 0;
            if (Administration)
                permission = permission | (long)Base2Permission.Administration;
            if (CreatePublicProject)
                permission = permission | (long)Base2Permission.AddPublicProject;
            if (CreatePersonalProject)
                permission = permission | (long)Base2Permission.AddPersonalProject;
            if (ManagementPermission)
                permission = permission | (long)Base2Permission.ManagementPermission;
            if (DownloadAllowed)
                permission = permission | (long)Base2Permission.ViewCommunityProjects | (long)Base2Permission.Administration;
            if (ViewTaskList)
                permission = permission | (long)Base2Permission.ViewCommunityProjects | (long)Base2Permission.Administration;
            if (PrintTaskList)
                permission = permission | (long)Base2Permission.ViewCommunityProjects | (long)Base2Permission.Administration;
            return permission;
        }


        public lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages ToTemplateModule()
        {
            lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages m = new lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(UniqueCode);
            m.Add = Administration || CreatePersonalProject || CreatePublicProject;
            m.SendMessage = Administration || CreatePersonalProject || CreatePublicProject;
            m.DeleteMyTemplates = Administration || CreatePersonalProject || CreatePublicProject;
            m.Edit = Administration || CreatePersonalProject || CreatePublicProject;
            m.Administration = Administration;
            m.Clone = Administration;
            m.List = Administration;
            m.DeleteOtherTemplates = Administration;
            m.ManageModulePermission = Administration || ManagementPermission;
            return m;
        }
        public static List<MailSenderActionType> GetMandatoryActions()
        {
            return new List<MailSenderActionType>();
        }
        [Flags, Serializable]
        public enum Base2Permission
        {
            AddPublicProject = 8192, //'13 Add
            Administration = 64, //'6 Admin
            ManagementPermission = 32, //'5 Grant
            ViewCommunityProjects = 1024, //'10 Browse
            AddPersonalProject = 2 // '1 Write
        }

        [Serializable]
        public enum ActionType 
        {
            None = 0,
            NoPermission = 1,
            GenericError = 2,
            ProjectStartAdding = 74000,
            ProjectAdded = 74001,
            ProjectAddingCancel = 74002,
            ActivityAdd = 74003,
            ActivityAdded = 74004,
            TaskAddingCancel = 74005,
            StartVirtualDeleteWithReallocateResource = 74006,
            StartUnDeleteWithReallocateResource = 74007,
            AnnulVirtualDeleteWithReallocateResource = 74008,
            AnnulUnDeleteWithReallocateResource = 74009,
            FinishVirtualDeleteWithReallocateResource = 74010,
            FinishUnDeleteWithReallocateResource = 74011,
            StartUndeleteTask = 74012,
            FinishUndeleteTask = 74013,
            StartVirtualDeleteTask = 74014,
            FinishVirtualDeleteTask = 74015,
            StartDeleteTask = 74016,
            FinishDeleteTask = 74017,
            //'AddedTaskAssignment = 74018
            StartManageTaskAssignment = 74019,
            FinishManageTaskAssignment = 74020,
            StartUpdateTaskDetail = 74021,
            StartViewTaskDetail = 74022,
            FinishUpdateTaskDetail = 74023,
            FinishViewTaskDetail = 74024,
            StartViewProjectMap = 74025,
            FinishViewProjectMap = 74026,
            ViewAssignedTask = 74027,
            ViewTaskManagement = 74028,
            ViewInvolvingProject = 74029,
            ViewGantt = 74030,
            // File
            AssignmentDownload = 74032,
            ShowTaskFile = 74033,
            HideTaskFile = 74034,
            AddFile = 74035,
            RemoveFile = 74036,
            AddFiles = 74037,
            RemoveFiles = 74038,
            EditItemNoDate = 74040,
            InitUploadMultipleFiles = 74041,
            SessionTimeout= 74042,
            MalformedUrl = 74043,
            ResourcesManage = 74044,
            ResourcesView = 74045,
            ResourcesSaveSettings = 74046,
            ResourcesAddInternal = 74047,
            ResourcesAddExternal = 74048,
            ResourceAddInternal = 74049,
            ResourceAddExternal = 74050,
            ResourceVirtualDelete = 74051,
            ResourceVirtualUndelete = 74052,
            ResourceRemoveFromNotCompletedAssignments = 74053,
            ResourceRemoveFromNotStartedAssignments = 74054,
            ResourceRemoveFromAllAndRecalculateCompletion = 74055,
            ResourceRemoveFromNotCompletedAssignmentsAndRecalculateCompletion = 74056,
            ResourceRemoveFromAllAssignments = 74057,
            ProjectUnknown = 74058,
            ProjectTryToAdd = 74059,
            ProjectStartEditing = 74060,
            ProjectViewSettings = 74061,
            ProjectSelectOwnerFromResources = 74062,
            ProjectSelectOwnerFromCommunity = 74063,
            ProjectSaveSettings = 74064,
            ProjectTryToSave = 74065,
            CalendarsLoad = 74066,
            CalendarsAdd = 74067,
            CalendarsSave = 74068,
            CalendarsTryToSave = 74069,
            CalendarsSavingError = 74070,
            CalendarsAddException = 74071,
            CalendarsVirtualDelete = 74072,
            CalendarsVirtualUndelete = 74073,
            ExceptionsSave = 74074,
            ExceptionsTryToSave = 74075,
            ExceptionsSavingError = 74076,
            ExceptionsVirtualDelete = 74077,
            ExceptionsVirtualUndelete = 74078,
            ExceptionVirtualDelete = 74079,
            ExceptionVirtualUndelete = 74080,
            ProjectMapForEditing = 74081,
            ProjectMapSavedActivities = 74082,
            ProjectMapErrorSavingActivities = 74083,
            ProjectMapErrorFromDb = 74084,
            ProjectMapView = 74085,
            ProjectMapAddedActivities = 74086,
            ProjectMapAddingActivityErrors = 74087,
            ActivityVirtualDelete = 74088,
            ActivityVirtualDeleteErrors = 74089,
            ActivityToChild = 74090,
            ActivityToChildErrors = 74091,
            ActivityToFather = 74092,
            ActivityToFatherErrors = 74093,
            ActivitySetResources = 74094,
            ActivityUnableSetResources = 74095,
            ActivitySaved = 74096,
            ActivityUnableToSave = 74097,
            ProjectSavedStartDateDeadline = 74098,
            ProjectSavingErrorStartDateDeadline = 74099,
            MyCompletionSavedForTask = 74100,
            MyCompletionSavedForTasks = 74101,
            MyCompletionUnsaved = 74102,
            CompletionSavedForTask = 74103,
            CompletionUnsavedForTask = 74104,
            LoadProjectsAsResource = 74105,
            LoadProjectsAsManager = 74106,
            LoadProjectsAsAdministrator = 74107,
            LoadDashboardAsResource = 74108,
            LoadDashboardAsManager = 74109,
            LoadProjectDashboardAsResource = 74110,
            LoadProjectDashboardAsManager = 74111,
            LoadProjectsGeneric = 74112,
            LoadProjectsPlainAsManager = 74113,
            LoadProjectsPlainAsResource = 74114,
            LoadProjectsPlainAsAdministrator = 74115,
            LoadProjectsPlain = 74116,
            LoadProjectsGroupAsManager = 74117,
            LoadProjectsGroupAsResource = 74118,
            LoadProjectsGroupAsAdministrator = 74119,
            LoadProjectsGroup = 74120,
            ProjectVirtualDelete = 74121,
            ProjectVirtualUndelete = 74122,
            ProjectPhisicalDelete = 74123,
            LoadTasksAsResource = 74124,
            LoadTasksAsManager = 74125,
            LoadTasksAsAdministrator = 74126,
            LoadTasks = 74127,
            LoadTasksPlain = 74128,
            LoadTasksPlainAsResource = 74129,
            LoadTasksPlainAsManager = 74130,
            LoadTasksGroup = 74131,
            LoadTasksGroupAsResource = 74132,
            LoadTasksGroupAsManager = 74133,
            LoadTasksGroupByCommunityProject = 74134,
            LoadTasksGroupByCommunityProjectAsResource = 74135,
            LoadTasksGroupByCommunityProjectAsManager = 74136,
            LoadTasksPlainAsAdministrator = 74137,
            LoadProjectTasksPlainAsResource = 74138,
            LoadProjectTasksPlainAsManager = 74139,
            LoadTasksGroupAsAdministrator = 74140,
            LoadProjectTasksGroupAsResource = 74141,
            LoadProjectTasksGroupAsManager = 74142,
            LoadProjectTasksGroupByCommunityProjectAsResource = 74143,
            LoadProjectTasksGroupByCommunityProjectAsManager = 74144,
            LoadTasksGroupByCommunityProjectAsAdministrator = 74145,
            ReorderLoadItems = 74146,
            ReorderApplied = 74147,
            ReorderCyclesFound = 74148,
            ReorderAppliedWithAllLinksRemoved = 74149,
            ReorderAppliedWithCyclesInvolvedLinksRemoved = 74150,
            ReorderDataSavingErrors = 74151,
            ReorderProjectMapChanged = 74152,
            ReorderReloadProjectMap = 74153,
            ReorderReloadProjectMapWithReorderedItems = 74154,
            ProjectAttachmentsLoad = 74155,
            AttachmentsAddedUrl = 74156,
            AttachmentsNotAddedUrl = 74157,
            AttachmentsNotAddedFiles = 74158,
            AttachmentsAddedFiles = 74159,
            AttachmentsLinkedFiles = 74160,
            AttachmentsNotLinkedFiles = 74161,
            AttachmentsProjectNotFound =74162,
            AttachmentsActivityNotFound = 74163,
            ProjectAttachmentsNoPermissionToDeleteItems = 74164,
            ProjectAttachmentsVirtualDeletedItems = 74165,
            ProjectAttachmentsUnableToVirtualDeleteItems = 74166,
            ProjectAttachmentsUnableToEditUrl = 74167,
            ProjectAttachmentsNoPermissionToEditUrl = 74168,
            ProjectAttachmentsUrlModified = 74169,
            ProjectAttachmentsUrlUnknown = 74170,
            ProjectAttachmentsReload = 74171,
        }

        [Serializable]
        public enum ObjectType : int
        {
            None = 0,
            Project = 1,
            Task = 2,
            TaskAssignment = 3,
            FileScorm = 4,
            TaskFile = 5,
            TaskLinkedFile = 6,
            ResourceInternal = 7,
            ResourceExternal = 8,
            ProjectAttachment = 9
        }

        [Serializable]
        public enum MailSenderActionType
        {
            projectassigned = 1,
            projectdeleted = 2,
            projectsuspended = 3,
            taskcompleted = 4,
            taskdeleted = 5,
            taskassigned = 6,
            weeksummary = 7,
            taskapproved = 8,
            tasknotapproved = 9
        }
    }
}
