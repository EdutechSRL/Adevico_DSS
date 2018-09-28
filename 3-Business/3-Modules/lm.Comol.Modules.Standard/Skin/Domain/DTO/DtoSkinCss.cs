using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkinCss
    {
        public virtual Int64 Id { get; set; }
        public virtual String MainCss { get; set; }
        public virtual String MainCssUrl { get; set; }

        public virtual String AdminCss { get; set; }
        public virtual String AdminCssUrl { get; set; }

        public virtual String IeCss { get; set; }
        public virtual String IeCssUrl { get; set; }

        public virtual String LoginCss { get; set; }
        public virtual String LoginCssUrl { get; set; }

    }
}
