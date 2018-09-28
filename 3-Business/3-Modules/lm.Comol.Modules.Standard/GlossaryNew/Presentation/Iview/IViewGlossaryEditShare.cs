using System;
using System.Collections.Generic;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.Wizard;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewGlossaryEditShare : IViewPageBase
    {
        DTO_Glossary ItemData { get; set; }
        void SetTitle(String name);
        void LoadViewData(Int32 idCommunity, DTO_Glossary glossary, List<BaseLanguageItem> languages, List<NavigableWizardItem<Int32>> steps, Boolean fromListGlossary);
        void DisplayCommunityToAdd(Boolean forAdministration, Dictionary<Int32, Int64> requiredPermissions, List<Int32> unloadIdCommunities, CommunityAvailability availability);
        void GoToGlossaryList();
        void GoToGlossaryView();
        void ShowErrors(List<string> resourceErrorList, MessageType type = MessageType.alert);
        void ShowErrors(SaveStateEnum saveStateEnum, MessageType type = MessageType.error);
    }
}