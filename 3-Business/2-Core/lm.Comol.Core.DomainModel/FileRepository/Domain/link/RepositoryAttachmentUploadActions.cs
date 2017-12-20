using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Repository
{
    [Serializable]
    public enum RepositoryAttachmentUploadActions : int
    {
        none = 0,
        uploadtomoduleitem = 1,
        uploadtomoduleitemandcommunity = 2,
        linkfromcommunity = 4,
        addurltomoduleitem = 8,
        addurltomoduleitemandcommunity = 16
    }
}