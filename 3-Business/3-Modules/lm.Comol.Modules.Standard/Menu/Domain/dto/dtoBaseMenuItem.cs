using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [CLSCompliant(true),Serializable]
    public class dtoBaseMenuItem : dtoItem
    {
        public virtual long DisplayOrder { get; set; } 
        public virtual string CssClass { get; set; }  
        public virtual Boolean IsEnabled { get; set; }
        public virtual Boolean ShowDisabledItems { get; set; }
        public virtual String Link { get; set; }
        public virtual String Name { get; set; }
        public virtual TextPosition TextPosition { get; set; }
        public virtual int IdModule { get; set; }
        public virtual long Permission { get; set; }
        public virtual long IdMenubar { get; set; }
    }
}