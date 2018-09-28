using System;
using System.Collections.Generic;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewGlossaryView : IViewPageBase
    {
        void SetTitle(String name);
        void LoadViewData(DTO_Glossary glossary, Dictionary<String, CharInfo> words, Int32 fromIdCommunity, Boolean manageEnabled, Boolean manage, Boolean loadFromCookies, string idCookies, int page);
        void BindRepeaterList(IEnumerable<DTO_Term> termList, Dictionary<String, CharInfo> words, char letter, Int32 records, Int32 pageIndex, List<CharInfo> letters);
    }
}