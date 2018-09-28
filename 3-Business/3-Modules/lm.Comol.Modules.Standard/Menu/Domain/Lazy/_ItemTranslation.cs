using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class _ItemTranslation : DomainBaseObject<long>
    {
        public virtual Language Language { get; set; }
        public virtual String Name { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual long IdItem { get; set; }
        public virtual long IdTopItem { get; set; }
        public virtual long IdMenubar { get; set; }
    }
}
