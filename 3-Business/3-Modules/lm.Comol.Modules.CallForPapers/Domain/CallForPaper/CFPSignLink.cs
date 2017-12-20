using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class CFPSignLink : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Int64 CallId { get; set; }
        public virtual Int64 RevisionId { get; set; }

        public virtual Int64 LinkId { get; set; }

        //public virtual ModuleLink SignatureLink { get; set; }

    }
}
