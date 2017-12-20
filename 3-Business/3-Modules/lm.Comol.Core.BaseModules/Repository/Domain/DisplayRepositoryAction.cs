using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Repository.Domain
{
    [Serializable]
    public enum UserRepositoryAction
    {
        None = 0,
        SelectAction = 1,
        LinkForDownload = 2,
        LinkForScorm = 3,
        LinkForMultimedia = 4,
        InternalUploadPlay = 5,
        CommunityUploadPlay = 6,
        UploadFile = 7,
        CreateFolder = 8
    }
}