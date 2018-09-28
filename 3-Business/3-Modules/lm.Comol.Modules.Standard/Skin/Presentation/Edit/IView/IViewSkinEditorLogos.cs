using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.Skin.Domain.DTO;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewSkinEditorLogos : IViewSkinEditor
    {
        String BasePath { get; }
        String VirtualFullPath { get; }
    }
}