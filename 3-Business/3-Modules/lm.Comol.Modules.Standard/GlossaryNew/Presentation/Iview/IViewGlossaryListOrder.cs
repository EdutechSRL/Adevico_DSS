using System;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewGlossaryListOrder : IViewPageBase
    {
        void SetTitle(String name);
        void LoadViewData(List<DTO_Glossary> glossaryList);
        void ShowMessage(SaveStateEnum saveStateEnum, MessageType type);
    }
}