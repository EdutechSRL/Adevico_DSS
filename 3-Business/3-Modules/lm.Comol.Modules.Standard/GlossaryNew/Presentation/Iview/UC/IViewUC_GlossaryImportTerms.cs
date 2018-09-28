using System;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
{
    public interface IViewUC_GlossaryImportTerms : IView_PageBaseControls
    {
        void SetTitle(String name);
        void LoadViewData(DTO_ImportCommunity community, Boolean showCommunityMapper);
    }
}