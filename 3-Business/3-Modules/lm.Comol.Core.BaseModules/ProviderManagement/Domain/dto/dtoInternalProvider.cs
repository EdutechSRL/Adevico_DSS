using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class dtoInternalProvider : dtoProvider
    {
        public virtual Int32 ChangePasswordAfterDays { get; set; }
        public dtoInternalProvider() {
            this.Type = Authentication.AuthenticationProviderType.Internal;
        }
    }
}