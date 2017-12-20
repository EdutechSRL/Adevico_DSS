using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum LogoutMode
    {
        none = -1,
        internalLogonPage= 0,
        externalPage = 1,
        logoutMessage = 2,
        logoutMessageAndClose = 3,
        portalPage = 4,
        logoutMessageAndUrl = 5
    }
}
