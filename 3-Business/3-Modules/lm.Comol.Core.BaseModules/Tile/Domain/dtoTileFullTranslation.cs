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
    public class dtoTileFullTranslation
    {
        public virtual String Title { get; set; }
        public virtual String ShortTitle { get; set; }
        public virtual String Description { get; set; }
        public virtual Dictionary<TileItemType, String> ToolTips { get; set; }
        public virtual lm.Comol.Core.DomainModel.Languages.dtoLanguageItem LanguageInfo { get; set; }
        public dtoTileFullTranslation()
        {
            ToolTips = new Dictionary<TileItemType, String>();
        }
        public dtoTileFullTranslation(lm.Comol.Core.DomainModel.Languages.dtoLanguageItem languageInfo, dtoEditTile tile)
        {
            LanguageInfo = languageInfo;
            lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation translation = null;
            if (languageInfo.IsMultiLanguage)
                translation = tile.Translation;
            else if (tile.Translations.Where(t => t.IdLanguage == languageInfo.IdLanguage).Any())
                translation = tile.Translations.Where(t => t.IdLanguage == languageInfo.IdLanguage).Select(i => i.Translation).FirstOrDefault();
            else
                translation = new TitleDescriptionObjectTranslation();

            Title = translation.Title;
            ShortTitle = translation.ShortTitle;
            Description = translation.Description;
            ToolTips = (from TileItemType t in Enum.GetValues(typeof(TileItemType)).AsQueryable() select t).ToDictionary(t => t, t => "");
            foreach (dtoEditTileItem item in tile.SubItems)
            {
                if (languageInfo.IsMultiLanguage)
                    ToolTips[item.Type] = item.ToolTip;
                else if (item.Translations.Where(t => t.IdLanguage == languageInfo.IdLanguage).Any())
                    ToolTips[item.Type] = item.Translations.Where(t => t.IdLanguage == languageInfo.IdLanguage).Select(i => i.ToolTip).FirstOrDefault();
                else
                    ToolTips[item.Type] = "";
            }
        }
    }
}