using System;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
{
    public interface IViewUC_GlossaryShare : IView_PageBaseControls
    {
        DTO_Glossary ItemData { get; set; }
        void SetTitle(String name);
        void LoadViewData(int idCommunity, DTO_Glossary glossary, List<DTO_Share> communityShareList);
        void GoToGlossaryList();
        void GoToGlossaryView();
        void ShowErrors(List<string> resourceErrorList, MessageType type = MessageType.alert);
        void ShowErrors(SaveStateEnum saveStateEnum, MessageType type = MessageType.error);
    }
}