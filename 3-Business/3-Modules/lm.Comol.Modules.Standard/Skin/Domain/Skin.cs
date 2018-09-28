using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class Skin : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual IList<HeaderLogo> HeaderLogos { get; set; }
        public virtual String MainCss { get; set; }
        public virtual String IECss { get; set; }
        public virtual String AdminCss { get; set; }
        public virtual String LoginCss { get; set; }
        public virtual IList<FooterLogo> FooterLogos { get; set; }
        public virtual IList<FooterText> FooterText { get; set; }
        public virtual Boolean IsPortal { get; set; }
        public virtual IList<Skin_ShareCommunity> Communities { get; set; }
        public virtual IList<Skin_ShareOrganization> Organizations { get; set; }

        public virtual Boolean IsModule { get; set; }

        public virtual HeaderTemplate HeaderTemplate { get; set; }
        public virtual FooterTemplate FooterTemplate { get; set; }

        public virtual bool OverrideVoidFooterLogos { get; set; }

    }
}