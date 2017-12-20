using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum ProfileSettingsTab
    {
        none = 0,
        profileData = 1,
        mailPolicy = 2,
        advancedSettings = 3,
        istantMessaging = 4
    }
}