using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class ModuleSkin : BaseSkin
    {
        public virtual IList<Skin_ShareModule> SharedTo { get; set; }
        public virtual HeaderTemplate HeaderTemplate { get; set; }
        public virtual FooterTemplate FooterTemplate { get; set; }
    }
}