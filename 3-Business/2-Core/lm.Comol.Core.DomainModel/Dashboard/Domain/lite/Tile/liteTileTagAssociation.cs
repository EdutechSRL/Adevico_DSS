using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class liteTileTagAssociation : DomainBaseObject<long>, ICloneable , IDisposable
    {
        public virtual long IdTile { get; set; }
        public virtual lm.Comol.Core.Tag.Domain.liteTagItem Tag { get; set; }

        public virtual object Clone()
        {
            liteTileTagAssociation clone = new liteTileTagAssociation();
            clone.Id = Id;
            clone.Deleted = Deleted;
            clone.Tag = (lm.Comol.Core.Tag.Domain.liteTagItem)Tag.Clone();
            return clone;
        }
        public void Dispose()
        {
            
        }
        
    }
}
