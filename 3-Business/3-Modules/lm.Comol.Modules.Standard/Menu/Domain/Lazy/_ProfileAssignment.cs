using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class _ProfileAssignment : DomainBaseObject<long>
    {
        public virtual int IdProfileType { get; set; }
        public virtual long IdItemOwner { get; set; }
        public virtual long IdMenubar { get; set; }
    }
}