using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkinList
    {
        public Int64 Id { get; set;}
        public String Name { get; set; }
        public Boolean IsPortal { get; set; }

        public IList<Domain.DTO.DtoSkinShareItem> Organizations { get; set; }
        public IList<Domain.DTO.DtoSkinShareItem> Communities { get; set; }

    }
}
