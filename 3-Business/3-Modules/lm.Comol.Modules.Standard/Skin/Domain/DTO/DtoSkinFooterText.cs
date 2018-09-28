using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkinFooterText
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 SkinId { get; set; }
        public virtual String Text { get; set; }
        public virtual String LangCode { get; set; }
        public virtual String LangName { get; set; }

        public virtual Boolean IsDefault { get; set; }
    }
}
