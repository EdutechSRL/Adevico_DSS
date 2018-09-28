using System;
using System.Collections.Generic;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
{
    public interface IViewUC_CommunityGlossaryTerms : IView_PageBaseControls
    {
        void SetTitle(String name);
        void InitViewData(List<Int32> idCommunityList);
        void LoadViewData(List<DTO_ImportCommunity> communityList, Boolean showCommunityMapper);
        void LoadViewData(GlossaryContainer glossaryContainer);
    }
}