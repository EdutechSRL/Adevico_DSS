using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoExternalUserProvider : dtoUserProvider
    {
        public virtual String RemoteUrl { get; set; }
    }
}