using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class SubmitterType : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual BaseForPaper Call { get; set; }
        public virtual IList<RequestedFileAssignment> RequiredFiles { get; set; }
        public virtual Boolean AllowMultipleSubmissions { get; set; }
        public virtual Int32 MaxMultipleSubmissions { get; set; }
        public virtual Int32 DisplayOrder { get; set; }

        public SubmitterType(){
            Name = "";
            Description = "";
            MaxMultipleSubmissions = 1;
            AllowMultipleSubmissions = false;
            RequiredFiles = new List<RequestedFileAssignment>();
        }
        public SubmitterType(String name, BaseForPaper call)
        {
            Name = name;
            Call = call;
            RequiredFiles = new List<RequestedFileAssignment>();
        }
        public SubmitterType(String name, String description)
        {
            Name = name;
            Description = description;
            RequiredFiles = new List<RequestedFileAssignment>();
        }
    }
}
