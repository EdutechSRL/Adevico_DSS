using System;
using System.Collections.Generic;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewImportExport : IViewPageBase
    {
        String GlossaryPath { get; }
        void LoadViewData(Int32 idCommunity);
        void DisplayCommunityToAdd(Boolean forAdministration, Dictionary<Int32, Int64> requiredPermissions, List<Int32> unloadIdCommunities, CommunityAvailability availability);
        void ShowCommunity(List<int> idCommunities);
        void ExportGlossaries(String content, String fileName);
    }
}