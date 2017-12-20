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
    public class dtoToolTipLanguageItem : dtoLanguageItem
    {
        public virtual String ToolTip { get; set; }
        public dtoToolTipLanguageItem() { }

    }
}