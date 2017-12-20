using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
{
    [Serializable]
    public enum UserMessageType
    {
        none = 0,
        detailsNoPermissionToSave = 1,
        versionAdded = 2,
        versionVirtualDeleted = 3,
        versionVirtualUndeleted = 4,
        versionPhisicalDeleted = 5,
        versionNotAdded = 6,
        versionNotPromoted = 7,
        versionPromoted = 8,
        versionItemNotFound = 9,
        versionItemNoPermission = 10,
        permissionsSaved = 20,
        permissionsUnableToSave = 21,
        permissionsNoPermissionToSave = 22,
        permissionsNoItemToSave = 23,
        permissionsReloaded = 24,
        multimediaSettingsSaved= 30,
        multimediaSettingsUnableToSave = 31,
        multimediaSettingsInvalidStatus = 32,
        multimediaSettingsInvalidType = 33,
        multimediaSettingsNoPermission = 34,
        multimediaSettingsNoDefaultDocument = 35,
        multimediaSettingsDocumentNotFound = 36,
        scormSettingsSaved = 40,
        scormSettingsUnableToSave = 41,
        scormSettingsInvalidStatus = 42,
        scormSettingsInvalidType = 43,
        scormSettingsNoPermission = 44,
        scormSettingsNotFound = 45,
        scormSettingsWaitingToSet = 46,
        scormSettingsErrors = 47
    }
}
