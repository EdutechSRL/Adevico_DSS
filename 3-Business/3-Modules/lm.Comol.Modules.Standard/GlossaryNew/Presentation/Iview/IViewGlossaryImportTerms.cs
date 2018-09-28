using System;
using System.Collections.Generic;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Wizard;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewGlossaryImportTerms : IViewPageBase
    {
        DTO_Glossary ItemData { get; set; }
        void LoadViewData(Int32 idCommunity, DTO_Glossary glossary, List<NavigableWizardItem<int>> steps);
        void DisplayCommunityToAdd(Boolean forAdministration, Dictionary<Int32, Int64> requiredPermissions, List<Int32> unloadIdCommunities, CommunityAvailability availability);
        void ShowCommunity(List<int> idCommunities);
        void ShowInfo(SaveStateEnum saveStateEnum, MessageType messageType);
    }
}