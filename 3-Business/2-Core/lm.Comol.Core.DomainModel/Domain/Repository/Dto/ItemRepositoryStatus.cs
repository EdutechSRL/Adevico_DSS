
using System;
namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public enum ItemRepositoryStatus
    {
        FolderCreated = 1,
        FileUploaded = 2,
        ChangeSaved = 3,
        Impersonated = 4,
        Deleted = 5,
        EditError = -1,
        UploadError = -2,
        FileExist = -3,
        FolderExist = -4,
        CreationError = -5,
        FolderDoesntExist = -6,
        FileDoesntExist = -7,
        NoPermissionToSeeItem = -8,
        NoItemSpecified = -9,
        NotLoggedIn = -10,
        None = -1000,
        NoPermissionToAdd = -11,
        ImpersonationFailed = -12,
        NoImpersonationRequired = -13
    }
}