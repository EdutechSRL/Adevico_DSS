using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkinOrganization
    {
        public virtual Int32 Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Url { get; set; }
        public virtual Boolean IsChecked { get; set; }
    }
}
