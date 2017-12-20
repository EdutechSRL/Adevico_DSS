using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class liteDashboardTileAssignment : lm.Comol.Core.DomainModel.DomainBaseObject<long>, ICloneable, IDisposable 
    {
        public virtual long IdDashboard { get; set; }
        public virtual liteTile Tile { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual AvailableStatus Status { get; set; } 
        public virtual object Clone()
        {
            liteDashboardTileAssignment item = new liteDashboardTileAssignment();
            item.IdDashboard = IdDashboard;
            item.Tile = Tile;
            item.Deleted = Deleted;
            item.Id = Id;
            item.DisplayOrder = DisplayOrder;
            return item;
        }

        public void Dispose()
        {

        }
    }
}
