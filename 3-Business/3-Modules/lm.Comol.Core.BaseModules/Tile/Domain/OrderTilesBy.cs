using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Domain
{
    [Serializable]
    public enum OrderTilesBy
    {
        None =0,
        Name = 1,
        CreatedBy = 2,
        CreatedOn = 3,
        Type = 4,
        ModifiedOn = 5,
        ModifiedBy = 6
    }
}