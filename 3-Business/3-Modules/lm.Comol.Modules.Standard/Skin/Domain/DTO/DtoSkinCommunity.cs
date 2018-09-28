using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkinCommunity
    {
        public virtual Int32 Id { get; set; }
        public virtual String Name { get; set; }
        public virtual Int32 OrganizationId { get; set; }
    }
}
