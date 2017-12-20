using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class TileAssignment : DomainBaseObjectLiteMetaInfo<long>, ICloneable, IDisposable, ITileBaseItem
    {
        public virtual Tile Tile { get; set; }
        public virtual Int32 IdProfileType { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual DashboardAssignmentType Type { get; set; }
        public virtual object Clone()
        {
            TileAssignment clone = new TileAssignment();
            clone.IdProfileType = IdProfileType;
            clone.IdRole = IdRole;
            clone.IdPerson = IdPerson;
            clone.Type = Type;
            return clone;
        }
        public virtual TileAssignment Copy(Tile tile, litePerson person, String ipAddress, String ipProxyAddress, DateTime? createdOn = null)
        {
            TileAssignment t = new TileAssignment();
            t.CreateMetaInfo(person, ipAddress, ipProxyAddress, createdOn);

            t.IdProfileType = IdProfileType;
            t.IdRole = IdRole;
            t.IdPerson = IdPerson;

            return t;
        }
        public void Dispose()
        {
            
        }
    }
}
