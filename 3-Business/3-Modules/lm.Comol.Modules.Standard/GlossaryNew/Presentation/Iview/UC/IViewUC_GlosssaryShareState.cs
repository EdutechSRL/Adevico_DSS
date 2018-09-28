using System;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
{
    public interface IViewUC_GlosssaryShareState : IView_PageBaseControls
    {
        DTO_Glossary ItemData { get; set; }
        void SetTitle(String name);
        void LoadViewData(int idCommunity, DTO_Share dto_share, Boolean manageEnabled, Boolean manage);
    }
}