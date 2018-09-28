using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewSkinFooterText : IViewSkinEditor
    {
        void LoadItems(IList<Domain.DTO.DtoSkinFooterText> items);
    }
}