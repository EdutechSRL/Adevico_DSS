using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class BaseForPaperAssignment : DomainBaseObjectLiteMetaInfo<long> 
    {
        public virtual Boolean Deny {get;set;}
        //public virtual Boolean Inherited {get;set;}
        public virtual BaseForPaper BaseForPaper {get;set;}
    }
}