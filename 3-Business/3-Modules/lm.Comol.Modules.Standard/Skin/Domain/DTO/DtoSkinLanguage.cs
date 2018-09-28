using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    [Serializable]
    public class DtoSkinLanguage
    {
        public virtual Int32 Id { get; set; }
        public virtual String LangName { get; set; }
        public virtual String LangCode { get; set; }
        public virtual Boolean IsDefault { get; set; }
    }
}
