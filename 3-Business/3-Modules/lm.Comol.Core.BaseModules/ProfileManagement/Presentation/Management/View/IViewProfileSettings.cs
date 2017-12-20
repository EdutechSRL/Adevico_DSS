using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewProfileSettings : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }
        Int32 IdProfileType { get; set; }

        long idDefaultProvider { get; set; }

        ProfileSettingsTab SelectedTab { get; }

        void LoadProfileInfo(Int32 idProfile, String displayName, String profileAvatar, Int32 idProfileType);
        void LoadTabs(List<ProfileSettingsTab> tabs);
        void DisplayTab(Int32 idProfile, ProfileSettingsTab tab);

        void DisplayProfileUnknown();
        void DisplaySessionTimeout();
        void DisplayNoPermission();
        void DisplayErrorSaving();
        void DisplaySavedInfo();
        Boolean ValidateContent();
    }
}