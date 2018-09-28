using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class MenuItemTranslation : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Language Language { get; set; }
        public virtual String Name { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual Menubar Menubar { get; set; }
        public virtual BaseMenuItem Item { get; set; }
        public virtual TopMenuItem TopMenuItem { get; set; }
    }
}
