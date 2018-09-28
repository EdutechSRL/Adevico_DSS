using System;
using System.Collections.Generic;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewGlossaryMapView : IViewPageBase
    {
        void SetTitle(String name);
        void LoadViewData(DTO_Glossary glossary, List<String> words, Int32 fromIdCommunity, Boolean manageEnabled, Boolean manage, Boolean loadFromCookies, string idCookies);
        void BindRepeaterList(List<String> words, List<String> wordsAll, IEnumerable<DTO_TermMap> termList, char letter);
    }
}