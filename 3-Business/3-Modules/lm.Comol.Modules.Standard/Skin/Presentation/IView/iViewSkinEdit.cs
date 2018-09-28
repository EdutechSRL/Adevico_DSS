using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface iViewSkinEdit : iDomainView
    {
        Domain.DTO.DtoSkin SkinData { get; set; }
    }
}
