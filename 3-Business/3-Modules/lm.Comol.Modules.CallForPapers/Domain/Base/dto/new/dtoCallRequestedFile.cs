using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoCallRequestedFile :dtoBase 
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String Tooltip { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
         
        public virtual List<long> SubmitterAssignments { get; set; }

        public dtoCallRequestedFile() :base() {
            SubmitterAssignments = new List<long>();
        }

        public dtoCallRequestedFile(RequestedFile requestedFile): base()
        {
            Id = requestedFile.Id;
            Name = requestedFile.Name;
            Mandatory = requestedFile.Mandatory;
            SubmitterAssignments = new List<long>();
        }
        public dtoCallRequestedFile(RequestedFile requestedFile, List<long> idSubmitters)
            : base()
        {
            Id = requestedFile.Id;
            Name = requestedFile.Name;
            Mandatory = requestedFile.Mandatory;
            SubmitterAssignments = idSubmitters;
        }
    }
}
