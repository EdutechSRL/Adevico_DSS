using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoRequestedFile :dtoBase 
    {
        public virtual String Name { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual IList<long> SubmittersTypeID { get; set; }

        public dtoRequestedFile() :base() {
            SubmittersTypeID = new List<long>();
        }

        public dtoRequestedFile(RequestedFile requestedFile): base()
        {
            Id = requestedFile.Id;
            Name = requestedFile.Name;
            Mandatory = requestedFile.Mandatory;
            SubmittersTypeID = new List<long>();
        }
        public dtoRequestedFile(RequestedFile requestedFile, IList<long> submittersTypeId)
            : base()
        {
            Id = requestedFile.Id;
            Name = requestedFile.Name;
            Mandatory = requestedFile.Mandatory;
            SubmittersTypeID = submittersTypeId;
        }
    }
}
