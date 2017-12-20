using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Repository
{
    [Serializable]
    public class RepositorySettings : DomainObject<long>
    {
        public virtual Person Person { get; set; }
        public virtual Community Community { get; set; }
        public virtual Boolean isPortal { get; set; }
        public virtual Boolean DisplayDescriptions { get; set; }

        public RepositorySettings() { }
    }
}
