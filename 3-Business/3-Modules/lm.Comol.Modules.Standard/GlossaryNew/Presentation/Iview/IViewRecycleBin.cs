using System;
using System.Collections.Generic;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewRecycleBin : IViewPageBase
    {
        void SetTitle(String name);
        void LoadViewData(List<DTO_GlossaryDelete> glossaries);
    }
}