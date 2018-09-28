using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    //public enum Logotype
    //{
    //    header = 1,
    //    footer = 0
    //}
    public class Logo : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Alt { get; set; }
        public virtual String ImageUrl { get; set; }
        public virtual String Link { get; set; }
        //public virtual Logotype IsHeader { get; set; }
        //public virtual Boolean Discriminator { get; protected set; }

        public virtual Skin Skin { get; set; }
    }

    public class HeaderLogo : Logo {
        public virtual String LangCode { get; set; }
    }
    public class FooterLogo : Logo { 
        public virtual Int32 DisplayOrder { get; set; }
        public virtual IList<LogoToLang> Languages { get; set; }
    }
}
