using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class TileTagAssociation : DomainBaseObjectLiteMetaInfo<long>, IDisposable, ITileBaseItem
    {
        public virtual Tile Tile { get; set; }
        public virtual lm.Comol.Core.Tag.Domain.TagItem Tag { get; set; }

        public virtual TileTagAssociation Copy(Tile tile, litePerson person, String ipAddress, String ipProxyAddress, DateTime? createdOn = null)
        {
            TileTagAssociation t = new TileTagAssociation();
            t.CreateMetaInfo(person, ipAddress, ipProxyAddress, createdOn);

            t.Tag = Tag;
            t.Tile = tile;

            return t;
        }
        public void Dispose()
        {
            
        }
    }
}
