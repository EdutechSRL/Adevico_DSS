using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Domain
{
    [Serializable]
    public class dtoEditTile : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation Translation { get; set; }
        public virtual List<dtoBaseObjectTranslation> Translations { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual TileType Type { get; set; }
        public virtual List<long> IdTags { get; set; }
        public virtual List<Int32> IdCommunityTypes { get; set; }
        public virtual List<dtoEditTileItem> SubItems { get; set; }
        public virtual Boolean AutoNavigateUrl { get; set; }
        public virtual String NavigateUrl { get; set; }
        public virtual String ImageUrl { get; set; }
        public virtual String ImageCssClass { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual long IdModulePage { get; set; }
        public virtual Boolean HasModulePage { get { return String.IsNullOrEmpty(ImageUrl) && (IdModulePage >0 || Type == TileType.Module|| Type == TileType.UserDefined); }}

        public dtoEditTile()
        {
            Translations = new List<dtoBaseObjectTranslation>();
            IdTags = new List<long>();
            SubItems = new List<dtoEditTileItem>();
            Translation = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
        }

        public dtoEditTile(liteTile tile, List<Language> languages)
        {
            Id = tile.Id;
            Deleted = tile.Deleted;
            Status = tile.Status;
            IdTags = tile.GetAvailableIdTags();
            AutoNavigateUrl = tile.AutoNavigateUrl;
            IdCommunityTypes = (tile.CommunityTypes==null) ? new List<Int32>() : tile.CommunityTypes.ToList();
            NavigateUrl = tile.NavigateUrl;
            ImageUrl = tile.ImageUrl;
            ImageCssClass = tile.ImageCssClass;
            IdCommunity = tile.IdCommunity;
            SubItems = tile.SubItems.Where(s => s.Deleted == BaseStatusDeleted.None).Select(s => new dtoEditTileItem(s, languages)).ToList();

            Translation = tile.DefaultTranslation;
            Translations = languages.Select(l=> new dtoBaseObjectTranslation() { 
                             IdLanguage= l.Id,
                             LanguageCode= l.Code,
                             Translation= (tile.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage== l.Id).Any()) ? tile.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage== l.Id).Select(t=> t.Translation).FirstOrDefault() : new TitleDescriptionObjectTranslation()
                            }
                            ).ToList();
            Type = tile.Type;
        }
    }
}