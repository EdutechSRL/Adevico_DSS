using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class TileTranslation : lm.Comol.Core.DomainModel.Languages.ItemBaseTranslation, IDisposable, ITileBaseItem
    {
        public virtual Tile Tile {get;set;}

        public TileTranslation()
        {
            Translation = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
        }

        public virtual TileTranslation Copy(Tile tile, litePerson person, String ipAddress, String ipProxyAddress, String titlePrefix = "", DateTime? createdOn = null)
        {
            TileTranslation t = new TileTranslation();
            t.CreateMetaInfo(person, ipAddress, ipProxyAddress,createdOn);

            t.Tile = tile;
            t.IdLanguage = IdLanguage;
            t.LanguageCode = LanguageCode;
            t.Translation = Translation.Copy();
            if (!String.IsNullOrEmpty(titlePrefix))
                t.Translation.Title = titlePrefix + t.Translation.Title;
            return t;
        }

        public void Dispose()
        {
        }
    }
}