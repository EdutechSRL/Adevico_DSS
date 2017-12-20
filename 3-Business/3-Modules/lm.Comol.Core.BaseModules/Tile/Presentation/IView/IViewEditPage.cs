using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation 
{
    public interface IViewEditPage : IViewEditPageBase
    {
        Boolean AllowSave{ set; }
        Boolean AllowEnable { set; }
        Boolean AllowDisable { set; }
        Boolean AllowVirtualUndelete { set; }
        Boolean AllowVirtualDelete { set; }
        Boolean PreloadFromAdd { get; }

        void InitalizeEditor(dtoEditTile tile, long idTile, Int32 idCommunity, Int32 idTileCommunity, List< lm.Comol.Core.DomainModel.TranslatedItem<long>> tags, List<TileType> types = null);
        void InitalizeEditor(dtoEditTile tile, long idTile, Int32 idCommunity, Int32 idTileCommunity, List<TileType> types = null);
        void InitalizeEditor(dtoEditTile tile, long idTile, Int32 idCommunity, Int32 idTileCommunity, String itemName);
        void DisplayTileAdded();
    }
}