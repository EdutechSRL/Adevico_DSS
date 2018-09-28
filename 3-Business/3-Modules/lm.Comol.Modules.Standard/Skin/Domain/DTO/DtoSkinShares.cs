using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkinShares
    {
        public Boolean IsPortal { get; set; }
        public IList<DtoSkinShareItem> Organizations { get; set; }
        public IList<DtoSkinShareItem> Communities { get; set; }
    }
}
