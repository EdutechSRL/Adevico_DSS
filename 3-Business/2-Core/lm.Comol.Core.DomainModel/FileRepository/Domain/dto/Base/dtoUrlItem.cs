using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoUrlItem
    {
        public virtual long Id { get; set; }
        public virtual String Address { get; set; }
        public virtual String Name {get;set;}
        public virtual String DisplayName { get { return (String.IsNullOrEmpty(Name) ? Address : Name); } }
        public virtual String OriginalName { get; set; }
        public virtual Boolean IsVisible { get; set; }
        public virtual Boolean IsValid { get { return !IsEmpty; } }
        public virtual Boolean IsEmpty { get { return String.IsNullOrWhiteSpace(Address); } }
        public dtoUrlItem() { }
    }
}