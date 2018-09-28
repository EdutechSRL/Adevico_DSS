using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkinTemplate
    {
        public virtual Int64 Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Css { get; set; }
        public virtual Boolean IsHeader { get; set; }
    }
}
