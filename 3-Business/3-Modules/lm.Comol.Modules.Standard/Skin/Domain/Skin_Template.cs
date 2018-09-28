using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class Skin_Template : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Css { get; set; }
        public virtual Skin Skin { get; set; }
    }

    public class HeaderTemplate : Skin_Template { }
    public class FooterTemplate : Skin_Template { }
}
