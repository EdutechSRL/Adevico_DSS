using System;
using System.Collections.Generic;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class BaseSkin : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual IList<HeaderLogo> HeaderLogos { get; set; }
        public virtual IList<FooterLogo> FooterLogos { get; set; }
        public virtual IList<FooterText> FooterText { get; set; }
        public virtual Boolean IsPortal { get; set; }
        public virtual Boolean IsModule { get; set; }
        public virtual SkinType Type { get; set; }
    }
}