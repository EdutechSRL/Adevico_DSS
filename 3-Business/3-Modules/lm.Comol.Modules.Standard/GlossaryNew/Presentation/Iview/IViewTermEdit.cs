using System;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewTermEdit : IViewPageBase
    {
        DTO_Term ItemData { get; set; }
        void SetTitle(String name);
        void LoadViewData(int idCommunity, long idGlossary, DTO_Term term, Int32 fromType, Int32 pageIndex, Boolean loadCookies, String idCookies);

        #region Attachment

        void GoToGlossaryView();
        void ShowErrors(List<string> resourceErrorList, MessageType type = MessageType.alert);
        void ShowInfo(SaveStateEnum saveStateEnum, MessageType type);

        #endregion
    }
}