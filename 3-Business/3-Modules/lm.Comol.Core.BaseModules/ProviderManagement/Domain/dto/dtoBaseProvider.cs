using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class dtoBaseProvider
    {
        public virtual long IdProvider { get; set; }
        public virtual AuthenticationProviderType Type { get; set; }
        public virtual LogoutMode LogoutMode { get; set; }
        public virtual Boolean isEnabled { get; set; }
        public virtual Boolean DisplayToUser { get; set; }
        public virtual Boolean AllowAdminProfileInsert { get; set; }
        public virtual IdentifierField IdentifierFields { get; set; }
        public virtual dtoProviderTranslation Translation { get; set; }
    }
}