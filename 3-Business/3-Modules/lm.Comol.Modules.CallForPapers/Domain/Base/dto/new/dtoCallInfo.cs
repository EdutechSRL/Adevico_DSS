using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoCallInfo : dtoBase
    {
        public virtual String Name { get; set; }
        public virtual String Edition { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual System.DateTime? EndDate { get; set; }
        public virtual CallForPaperType Type { get; set; }
        public virtual String CommunityName { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Boolean IsPublic { get; set; }
        public virtual Boolean IsPortal { get; set; }
        public virtual long Submissions { get; set; }
        public dtoCallInfo() { }

    }  
   
}