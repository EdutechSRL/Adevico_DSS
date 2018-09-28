using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    public abstract class RuleBase<Telement> : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long> where Telement : IRuleElement
    {
        public virtual String Name { get; set; }

        public virtual Telement Source { get; set; }

        public virtual Telement Destination { get; set; }

        public virtual Int64 SourceId { get; set; }

        public virtual Int64 DestinationId { get; set; }
        
        //public Boolean isCompleted { get; set; }

        public virtual CompletionType CompletionType { get; set; }

        public abstract Boolean Execute();
        
    }
}
