using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmitterType :dtoBase 
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean AllowMultipleSubmissions { get; set; }
        public virtual Int32 MaxMultipleSubmissions { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public dtoSubmitterType() :base() {}

        public dtoSubmitterType(SubmitterType submitter)
        {
            Id = submitter.Id;
            Name = submitter.Name;
            Description = submitter.Description;
            Deleted = submitter.Deleted;
            AllowMultipleSubmissions = submitter.AllowMultipleSubmissions;
            MaxMultipleSubmissions = (submitter.MaxMultipleSubmissions > 1 && !submitter.AllowMultipleSubmissions) ? 1 : submitter.MaxMultipleSubmissions;
            DisplayOrder = submitter.DisplayOrder;
        }
    }
}