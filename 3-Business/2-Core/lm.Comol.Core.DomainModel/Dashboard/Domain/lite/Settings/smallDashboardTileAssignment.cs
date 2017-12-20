using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class smallDashboardTileAssignment 
    {
        public virtual long Id { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual long IdDashboard { get; set; }
        public virtual smallTile Tile { get; set; }
        public virtual AvailableStatus Status { get; set; }
        public virtual long DisplayOrder { get; set; }

    }
}
