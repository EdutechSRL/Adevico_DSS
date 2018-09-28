using System;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewTermAdd : IViewPageBase
    {
        DTO_Term ItemData { get; set; }
        void SetTitle(String name);
        void LoadViewData(int idCommunity, long idGlossary);
        void GoToGlossaryView();
        void ShowErrors(List<string> resourceErrorList, MessageType type = MessageType.alert);
        void ShowErrors(SaveStateEnum saveStateEnum, MessageType type = MessageType.error);
    }
}