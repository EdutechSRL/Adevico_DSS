using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewSkinTemplate : IViewSkinEditor
    {
        long SelectedIdHeader { get; }
        long SelectedIdFooter { get; }
        void LoadTemplates(Domain.DTO.DtoSkinTemplates items);
    }
}