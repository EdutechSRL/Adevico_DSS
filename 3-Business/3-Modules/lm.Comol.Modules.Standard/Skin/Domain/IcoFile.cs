using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public enum IcoType { jpg, png, gif }

    public class IcoFile
    {
        public virtual String Name { get; set; }
        public virtual Int64 SizeKb { get; set; }
        public virtual IcoType Type { get; set; }
    }
}
