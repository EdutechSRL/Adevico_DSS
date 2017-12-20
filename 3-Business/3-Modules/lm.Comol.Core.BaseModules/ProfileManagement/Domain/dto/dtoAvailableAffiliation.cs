using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoAvailableAffiliation
    {
        public virtual long Id { get; set; }
        public virtual Organization Organization  { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsEmpty { get; set; }
        public virtual Boolean IsEditable { get; set; }
        public dtoAvailableAffiliation() {
            IsEditable = true;
        }
    }
}