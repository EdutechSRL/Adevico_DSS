using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    public interface ITileBaseItem
    {
        Tile Tile { get; set; }
        lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }
    }
}
