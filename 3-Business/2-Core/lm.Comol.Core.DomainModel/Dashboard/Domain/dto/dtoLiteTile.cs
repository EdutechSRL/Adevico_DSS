using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoLiteTile 
    {
        public virtual liteTile Tile { get; set; }
        public virtual long DisplayOrder {get;set;}
        public virtual Boolean HasOnlyThisIdTag(long idTag)
        {
            return Tile != null && Tile.Tags != null && Tile.Tags.Where(t => t.Deleted == BaseStatusDeleted.None).Count() == 1 && Tile.Tags.Where(t => t.Deleted == BaseStatusDeleted.None && t.Tag != null && t.Tag.Id == idTag).Any();
        }
        public virtual Boolean HasOnlyOneTag()
        {
            return Tile != null && Tile.Tags != null && Tile.Tags.Where(t => t.Deleted == BaseStatusDeleted.None).Count() == 1;
        }

        public virtual Boolean HasAnyCommunityType(List<Int32> idTypes)
        {
            return Tile != null && Tile.CommunityTypes != null && Tile.CommunityTypes.Where(t => idTypes.Contains(t)).Any();
        }
        public dtoLiteTile()
        {

        }
    }
}