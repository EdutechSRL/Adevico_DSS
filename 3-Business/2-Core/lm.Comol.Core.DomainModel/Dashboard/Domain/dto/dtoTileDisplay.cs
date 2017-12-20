using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoTileDisplay: lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation Translation { get; set; }
        public virtual List<long> Tags { get; set; }
        public virtual List<Int32> CommunityTypes { get; set; }
        public virtual List<dtoTileItemDisplay> SubItems { get; set; }
        public virtual TileType Type { get; set; }
        public virtual Boolean AutoNavigateUrl { get; set; }
        public virtual String NavigateUrl { get; set; }
        public virtual String ImageUrl { get; set; }
        public virtual String ImageCssClass { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual String CommandUrl { get; set; }
        public virtual Boolean ForPreview { get; set; }
        public dtoTileDisplay()
        {
            Translation = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
            Tags = new List<long>();
            CommunityTypes = new List<Int32>();
            SubItems = new List<dtoTileItemDisplay>();
        }

        public dtoTileDisplay(dtoLiteTile dto, Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean forPreview = false)
        {
            Id = dto.Tile.Id;
            Translation = dto.Tile.GetTranslation(idUserLanguage, idDefaultLanguage);
            Tags = new List<long>();
            CommunityTypes = dto.Tile.CommunityTypes.ToList();
            switch(dto.Tile.Type ){
                case TileType.Module:
                case TileType.UserDefined:
                    SubItems = dto.Tile.SubItems.Where(s => s.Deleted == BaseStatusDeleted.None).Select(s => new dtoTileItemDisplay(s, idUserLanguage, idDefaultLanguage)).ToList();
                    break;
                default:
                    SubItems = new List<dtoTileItemDisplay>();
                    break;
            }
            Type = dto.Tile.Type;
            AutoNavigateUrl = dto.Tile.AutoNavigateUrl;
            NavigateUrl = dto.Tile.NavigateUrl;
            ImageUrl = dto.Tile.ImageUrl;
            ImageCssClass = dto.Tile.ImageCssClass;
            ForPreview = forPreview;
        }
    }
}