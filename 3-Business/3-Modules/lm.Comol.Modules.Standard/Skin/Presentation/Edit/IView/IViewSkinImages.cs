using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewSkinImages : IViewSkinEditorLogos
    {
        
        void LoadItems(IList<Domain.DTO.DtoSkinImage> items);
    }
}
