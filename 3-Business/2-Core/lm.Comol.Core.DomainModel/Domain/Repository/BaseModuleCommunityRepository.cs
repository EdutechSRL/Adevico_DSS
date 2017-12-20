using System;
namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class BaseModuleCommunityRepository
    {

        public bool UploadFile {get;set;}
        public bool DeleteMyFile {get;set;}
        public bool ManagementPermission {get;set;}
        public bool Administration {get;set;}
        public bool ListFiles {get;set;}
        public bool Edit {get;set;}
        public bool DownLoad { get; set; }

        public BaseModuleCommunityRepository()
        {
        }

        public static BaseModuleCommunityRepository CreatePortalmodule(int UserTypeID)
        {
            BaseModuleCommunityRepository oService = new BaseModuleCommunityRepository();
            {
                oService.Administration = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative || UserTypeID == (int)UserTypeStandard.SysAdmin);
                oService.DeleteMyFile = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative || UserTypeID == (int)UserTypeStandard.SysAdmin);
                oService.DownLoad = (UserTypeID != (int)UserTypeStandard.Guest);
                oService.Edit = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative || UserTypeID == (int)UserTypeStandard.SysAdmin);
                oService.ListFiles = (UserTypeID != (int)UserTypeStandard.Guest);
                oService.ManagementPermission = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.SysAdmin);
                oService.UploadFile = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative || UserTypeID == (int)UserTypeStandard.SysAdmin);
            }
            return oService;
        }
    }
}
