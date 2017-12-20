using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoCallSection <T>:dtoBase 
    {
        public virtual long IdCall { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual List<T> Fields { get; set; }

        public dtoCallSection() : base() {
            Fields = new List<T>();
        }
        public dtoCallSection(long idCall, int displayOrder, String name, String description, BaseStatusDeleted deleted)
            : base()
        {
            Deleted = deleted;
            IdCall = idCall;
            DisplayOrder = displayOrder;
            Name = name;
            Description = description;
            Fields = new List<T>();
        }
        public dtoCallSection(long idCall, int displayOrder, String name, String description, BaseStatusDeleted deleted, List<T> fields)
            : base()
        {
            Deleted = deleted;
            IdCall = idCall;
            DisplayOrder = displayOrder;
            Name = name;
            Description = description;
            Fields = fields;
        }
    }
}