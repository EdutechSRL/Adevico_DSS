using System;
using System.Collections.Generic;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewGlossarySearch : IViewPageBase
    {
        void SetTitle(String name);
        void LoadViewData(Int32 fromIdCommunity, GlossaryFilter glossaryFilter);
        void BindRepeaterList(IEnumerable<DTO_Term> termList, char letter, Int32 records, Int32 currentPage, List<CharInfo> letters);
    }
}