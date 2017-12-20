using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class TileItemTranslation : DomainBaseObjectLiteMetaInfo<long>, IDisposable, ITileBaseItem
    {
        public virtual Tile Tile {get;set;}
        public virtual TileItem ItemOwner {get;set;}
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual String ToolTip { get; set; }

        public TileItemTranslation()
        {

        }

        public virtual TileItemTranslation Copy(Tile tile, TileItem owner, litePerson person, String ipAddress, String ipProxyAddress, DateTime? createdOn = null)
        {
            TileItemTranslation t = new TileItemTranslation();
            t.CreateMetaInfo(person, ipAddress, ipProxyAddress,createdOn);

            t.Tile = tile;
            t.ItemOwner = owner;
            t.IdLanguage = IdLanguage;
            t.LanguageCode = LanguageCode;
            t.ToolTip = ToolTip;
            return t;
        }

        public void Dispose()
        {
        }
    }
}