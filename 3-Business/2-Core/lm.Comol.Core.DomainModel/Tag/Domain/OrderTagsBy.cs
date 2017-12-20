using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public enum OrderTagsBy
    {
        None =0,
        Name = 1,
        CreatedBy = 2,
        CreatedOn = 3,
        UsedBy = 4,
        ModifiedOn = 5
    }
}