using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoUserProvider : dtoBaseProvider
    {
        public virtual long Id { get; set; }
        public virtual Person ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }

        public virtual Boolean isEnabled { get; set; }
        public virtual long  IdExternalLong { get; set; }
        public virtual String IdExternalString { get; set; }
    }
}