using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoTileItemDisplay : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual String NavigateUrl { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual String CssClass { get; set; }
        public virtual TileItemType Type { get; set; }

        public dtoTileItemDisplay()
        {
            Type = TileItemType.UserDefinedUrl;
        }
        public dtoTileItemDisplay(liteTileItem item,Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            NavigateUrl = item.NavigateUrl;
            ToolTip = item.GetTranslation(idUserLanguage,idDefaultLanguage);
            CssClass = item.CssClass;
            Type = item.Type;
        }

        
    }
}