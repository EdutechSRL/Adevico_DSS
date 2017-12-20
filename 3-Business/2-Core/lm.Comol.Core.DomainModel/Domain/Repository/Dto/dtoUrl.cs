using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class dtoUrl
    {
        public virtual long Id { get; set; }
        public virtual String Address { get; set; }
        public virtual String Name {get;set;}
        public virtual String DisplayName { get { return (String.IsNullOrEmpty(Name) ? Address : Name); } }
        public dtoUrl() { }
    }
}