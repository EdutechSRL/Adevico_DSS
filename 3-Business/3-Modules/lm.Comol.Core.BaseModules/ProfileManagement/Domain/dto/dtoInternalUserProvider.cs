using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoInternalUserProvider : dtoUserProvider
    {
        public virtual DateTime? PasswordExpiresOn { get; set; }
        public virtual EditType ResetType { get; set; }
        public virtual String Login { get; set; }
    }
}