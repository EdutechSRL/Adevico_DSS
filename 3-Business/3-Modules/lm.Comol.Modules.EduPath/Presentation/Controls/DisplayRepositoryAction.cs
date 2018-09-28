using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Presentation
{
    [Serializable]
    public enum DisplayRepositoryAction
    {
        none = 0,
        select = 1,
        downloadItem = 2,
        playScormPackage = 3,
        playMultimedia = 4,
        internalDownloadOrPlay = 5,
        repositoryDownloadOrPlay = 6
    }
}