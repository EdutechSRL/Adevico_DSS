using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class PmActivityLink : DomainBaseObjectLiteMetaInfo<long> 
    {
        public virtual Project Project { get; set; }
        public virtual PmActivity Source { get; set; }
        public virtual PmActivity Target { get; set; }
        public virtual PmActivityLinkType Type { get; set; }
        public virtual Double LeadLag { get; set; }

        public PmActivityLink()
        {
        }

        public override string ToString()
        {
            return String.Format("{0}->{1} [{2}] {3}",
                this.Source.Id,
                this.Target.Id,
                this.LeadLag > 0 ? (this.LeadLag != 0 ? "+" + this.LeadLag.ToString() : "") : this.LeadLag.ToString(),
                this.Type
                );
        }
    }
}
