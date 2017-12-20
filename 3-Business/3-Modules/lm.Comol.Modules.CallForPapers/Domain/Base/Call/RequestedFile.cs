using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class RequestedFile : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual BaseForPaper Call { get; set; }
        public virtual String Name {get;set;}
        public virtual String Description { get; set; }
        public virtual String Tooltip { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public virtual Boolean Mandatory {get;set;}
        public RequestedFile()
        {
               Deleted = BaseStatusDeleted.None;
        }
        public RequestedFile(long id, BaseForPaper call)
        {
            Deleted = BaseStatusDeleted.None;
            Call = call;
            Id = id;
        }
        public RequestedFile(String name, Boolean mandatory, BaseForPaper call)
        {
            Call = call;
            Name = name;
            Mandatory = mandatory;
            Deleted = BaseStatusDeleted.None;
        }

    }
}
