using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoRequestedFileAssignment :dtoBase 
    {
        public virtual String Name { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual IList<long> SubmittersTypeID { get; set; }
        public virtual IList<dtoSubmitterType> Types { get; set; }
        
        public dtoRequestedFileAssignment() :base() {
            SubmittersTypeID = new List<long>();
            Types = new List<dtoSubmitterType>();
        }

        public dtoRequestedFileAssignment(RequestedFile requestedFile): base()
        {
            Id = requestedFile.Id;
            Name = requestedFile.Name;
            Mandatory = requestedFile.Mandatory;
            SubmittersTypeID = new List<long>();
            Types = new List<dtoSubmitterType>();
        }
        public dtoRequestedFileAssignment(RequestedFile requestedFile, IList<dtoSubmitterType> submittersType)
            : base()
        {
            Id = requestedFile.Id;
            Name = requestedFile.Name;
            Mandatory = requestedFile.Mandatory;
            Types = submittersType;
            SubmittersTypeID = (from t in Types select t.Id).ToList();
        }
    }
}
