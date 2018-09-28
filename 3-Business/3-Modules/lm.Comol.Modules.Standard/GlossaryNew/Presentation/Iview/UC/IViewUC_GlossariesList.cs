using System;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
{
    public interface IViewUC_GlossariesList : IView_PageBaseControls
    {
        Boolean ForManagement { get; set; }
        void InitializeControl(Int32 idCommunity, Boolean manageEnabled);
        void LoadViewData(List<DTO_Glossary> glossaries, List<DTO_Glossary> publicGlossaries);
        void SetErrorMessage(MessageType type, String messageKey, params String[] parameters);
        void GoToGlossarySearch();
    }
}