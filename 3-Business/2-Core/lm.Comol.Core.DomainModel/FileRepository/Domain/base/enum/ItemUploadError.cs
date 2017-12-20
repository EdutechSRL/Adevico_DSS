using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public enum ItemUploadError
    {
        None = 0,
        UnavailableDiskSpace = 1,
        UnavailableRepositorySpace = 2,
        NoAssignmentAdded =3,
        MissingPermissionsToAddFile = 4,
        UnableToSaveFile = 5,
        UnableToAddFile = 6,
        UnableToAddFileToFolder = 7,
        UnableToAddFileToUnknownFolder = 8,
        InvalidFileSize = 9,
        UnableToSaveVersion = 10,
        UnableToFindFile = 11,
        VersioningNotAllowed = 12,
        VersioningNotAllowedByModule = 13
    }
}