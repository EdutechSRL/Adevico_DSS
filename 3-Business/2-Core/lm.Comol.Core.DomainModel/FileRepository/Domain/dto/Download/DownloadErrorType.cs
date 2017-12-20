using System;
namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable()]
    public enum DownloadErrorType
    {
        none = 0,
        notExist = 1,
        noPermissions = 2,
        notLoggedIn = 3,
        impersonationFailed = 4,
    }
}