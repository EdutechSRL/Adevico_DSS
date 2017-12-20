using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoTileForReorder 
    {
        public virtual long IdAssignment{get;set;}
        public virtual long IdTile{get;set;}
        public virtual String Name {get;set;}
        public virtual TileType Type{get;set;}
        public virtual AvailableStatus AssignmentStatus { get; set; }
        public virtual AvailableStatus TileStatus { get; set; }
        public virtual lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }
        public virtual Boolean IsAssigned { get { return IdAssignment > 0 && AssignmentStatus == AvailableStatus.Available && Deleted == DomainModel.BaseStatusDeleted.None; } }
        public virtual Boolean IsNotAssigned { get { return IdAssignment == 0; } }
        public virtual long DisplayOrder { get; set; }
        public dtoTileForReorder()
        {

        }
        public dtoTileForReorder(liteSearchTile tile, Int32 idUserlanguage, Int32 idDefaultLanguage)
        {
            IdTile = tile.Id;
            TileStatus = tile.Status;
            Type = tile.Type;
            Name = tile.GetTitle(idUserlanguage, idDefaultLanguage);
            AssignmentStatus = AvailableStatus.Any;
        }
        
        //public dtoTileForReorder(smallDashboardTileAssignment assignment)
        //{
        //    IdAssignment = assignment.Id 
        //    Deleted = assignment.Deleted;
        //    IdTile = (assignment.Tile == null) ? 0 : assignment.Tile.Id;
        //    AssignmentStatus = assignment.Status;
        //    if (IdTile>0){
        //        TileStatus = assignment.Tile.Status;
        //        Type = assignment.Tile.Type;
        //        Name = assignment.Tile.g
        //    }
        //}
        
    }
}