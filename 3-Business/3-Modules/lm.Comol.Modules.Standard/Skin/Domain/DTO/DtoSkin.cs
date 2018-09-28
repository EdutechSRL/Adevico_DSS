using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkin
    {
        public virtual Int64 Id { get; set; }
        public virtual String Name { get; set; }
        //public virtual Boolean IsActive { get; set; }
        public virtual bool OverrideFootLogos { get; set; }
    }
}
