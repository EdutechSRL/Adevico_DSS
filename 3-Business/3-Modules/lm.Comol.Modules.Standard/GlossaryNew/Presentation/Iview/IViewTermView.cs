using System;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewTermView : IViewPageBase
    {
        void SetTitle(String name);
        void LoadViewData(Int32 idCommunity, DTO_Glossary glossary, Term term, Boolean fromViewMap);
    }
}