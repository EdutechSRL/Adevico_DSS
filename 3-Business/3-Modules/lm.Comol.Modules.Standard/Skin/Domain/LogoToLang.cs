using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class LogoToLang
    {
        public virtual Int64 ID { get; set; }
        public virtual FooterLogo Logo { get; set; }
        public virtual String LangCode { get; set; }
    }
}
