using System;
using System.Collections.Generic;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewImport : IViewPageBase
    {
        void LoadViewData(Int32 idCommunity, String communityName);
        void DisplayCommunityToAdd(Boolean forAdministration, Dictionary<Int32, Int64> requiredPermissions, List<Int32> unloadIdCommunities, CommunityAvailability availability);
        void ShowCommunity(List<int> idCommunities);
        void ShowInfo(SaveStateEnum saveStateEnum, MessageType type);
    }
}