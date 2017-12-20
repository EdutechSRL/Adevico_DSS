using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoCallToFind : dtoBase
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Edition { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual System.DateTime? EndDate { get; set; }
        public virtual CallForPaperType Type { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual Boolean IsPublic { get; set; }
        public virtual Boolean IsPortal { get; set; }
        public virtual long SubmittedItems { get; set; }
        public virtual long WaitingItems { get; set; }
        public virtual long AcceptedItems { get; set; }
        public dtoCallToFind() { }

        public dtoCallToFind(liteCommunity community)
        {
            if (community != null)
                Community = community;
        }

    }  
}