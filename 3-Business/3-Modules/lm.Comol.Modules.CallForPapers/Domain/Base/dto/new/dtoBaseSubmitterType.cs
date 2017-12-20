using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoBaseSubmitterType :dtoBase 
    {
        public virtual String Name { get; set; }
        public dtoBaseSubmitterType() :base() {}

        public dtoBaseSubmitterType(SubmitterType submitter)
        {
            Id = submitter.Id;
            Name = submitter.Name;
            Deleted = submitter.Deleted;
        }
    }
}