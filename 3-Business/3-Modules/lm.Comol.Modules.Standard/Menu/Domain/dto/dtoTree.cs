using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoTree : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
      //  public virtual String ToolTip { get; set; } 
        public virtual String Category { get{return Type.ToString();} }
        public virtual MenuItemType Type { get; set; }
        public virtual IList<dtoTree> Items { get; set; }
    }
}