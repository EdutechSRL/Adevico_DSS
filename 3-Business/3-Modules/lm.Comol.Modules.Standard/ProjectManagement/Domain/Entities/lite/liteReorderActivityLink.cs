using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{

    [Serializable]
    public class liteReorderActivityLink
    {
        public virtual long Id { get; set; }
        public virtual long IdProject { get; set; }
        public virtual long IdSource { get; set; }
        public virtual long IdTarget { get; set; }
        public virtual PmActivityLinkType Type { get; set; }
        public virtual Double LeadLag { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual Boolean isVirtual { get; set; }

        public liteReorderActivityLink()
        {
        }

        public override string ToString()
        {
            return String.Format("{0}->{1} [{2}] {3}",
                IdSource,
                IdTarget,
                this.LeadLag > 0 ? (this.LeadLag != 0 ? "+" + this.LeadLag.ToString() : "") : this.LeadLag.ToString(),
                this.Type
                );
        }
    }
}