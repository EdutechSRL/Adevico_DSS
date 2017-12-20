using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation 
{
    public interface IViewTile : IViewEditPageBase
    {
        String GetDefaultLanguageName();
        String GetDefaultLanguageCode();
        void LoadTile(dtoEditTile tile, List<lm.Comol.Core.DomainModel.TranslatedItem<long>> tags, List<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem> languages, List<dtoTileFullTranslation> translations);
        void LoadTile(dtoEditTile tile, List<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem> languages, List<dtoTileFullTranslation> translations);
        void LoadTile(dtoEditTile tile, String itemName, List<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem> languages, List<dtoTileFullTranslation> translations);
    }
}