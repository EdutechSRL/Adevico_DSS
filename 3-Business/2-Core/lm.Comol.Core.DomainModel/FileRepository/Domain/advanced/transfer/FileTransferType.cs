using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    public enum FileTransferType : int
    {
        Unmanaged = 0,
        Scorm = 1,
        Multimedia = 2,
        VideoStreaming = 3
    }
}