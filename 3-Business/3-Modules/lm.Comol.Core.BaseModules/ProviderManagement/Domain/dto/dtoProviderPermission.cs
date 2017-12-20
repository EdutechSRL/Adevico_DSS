using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class dtoProviderPermission
    {
        public virtual long Id { get; set; }
        public virtual dtoProvider Provider { get; set; }
        public virtual dtoPermission Permission { get; set; }
        
        public dtoProviderPermission(){}
        public dtoProviderPermission(dtoProvider provider) {
            Id = provider.IdProvider;
            Provider = provider;
        }
    }
}