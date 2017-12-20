using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class DashboardTileAssignment : DomainBaseObjectLiteMetaInfo<long>, IDisposable
    {
        public virtual DashboardSettings Dashboard { get; set; }
        public virtual Tile Tile { get; set; }
        public virtual AvailableStatus Status { get; set; }       
        public virtual long DisplayOrder { get; set; }

        public virtual DashboardTileAssignment Copy(DashboardSettings dashboard, litePerson person, String ipAddress, String proxyIpAddress)
        {
            DashboardTileAssignment item = new DashboardTileAssignment();
            item.CreateMetaInfo(person, ipAddress, proxyIpAddress);
            item.Tile = Tile;
            item.Status = Status;
            item.DisplayOrder = DisplayOrder;
            item.Deleted = Deleted;
            item.Dashboard = dashboard;
            return item;
        }

        public void Dispose()
        {
            
        }
    }
}
