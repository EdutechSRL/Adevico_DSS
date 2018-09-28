using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class ModuleEduPath
    {
        public const String UniqueCode ="SRVEDUP";
        public virtual Boolean ViewPathList {get;set;}
        public virtual Boolean ManagePermission {get;set;}
        public virtual Boolean Administration { get; set; }
        public virtual Boolean ViewMyStatistics { get; set; }

        public ModuleEduPath() {}
        public static ModuleEduPath CreatePortalmodule(int UserTypeID){
            ModuleEduPath module = new ModuleEduPath();
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative   );
            return module;
        }

        public ModuleEduPath(long permission)
        {
            ViewPathList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Browse, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration, permission);
            ManagePermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.GrantPermission, permission);
        }
         [Flags,Serializable]
        public enum Base2Permission{
            GrantPermission = 32, //5
            Administration = 64, //6
            Browse = 1024 // Add'10
        }
        [Serializable]
        public enum ActionType{
            None = 83000,
            NoPermission = 83001,
            GenericError = 83002,
            Create = 83003,
            List = 83004,
            Delete = 83005,
            Edit = 83006,
            Access = 83007,
            Review = 83008,
            DoSubActivity = 83009,
            ViewOwnStat = 83010,
            ViewUserStat = 83011,
            Evaluate = 83012,
            ViewSummaryIndex = 83013,
            ViewSummaryCommunity = 83014,
            ViewSummaryPath = 83015,
            ViewSummaryUser = 83016,
            ExportMyStatistics = 83017,
            ExportUserStatistics = 83018,
            ExportPathStatistics = 83019,
            ExportUsersPathStatistics = 83020,
            ExportComunityStatistics = 83021,
            ExportOrganizationStatistics = 83022,
            DownloadCertification = 83023,
            SendCertificationToUser = 83024,
            NoPermissionForSummary = 83025,
            UnselectedUserForSummaryUser = 83026,
            WrongPageAccess = 83027,
            ViewSummaryOrganization = 83028,
            ViewCerticationList=83029
        }
        [Serializable]
        public enum ObjectType{
            None = 0,
            EduPath = 1,
            Unit = 2,
            Activity = 3,
            SubActivity = 4,
            Assignment = 5
        }
    }
}