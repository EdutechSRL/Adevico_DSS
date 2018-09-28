using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class FooterText : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Value { get; set; }
        public virtual String LangCode { get; set; }
        public virtual Skin Skin { get; set; }
    }
}
