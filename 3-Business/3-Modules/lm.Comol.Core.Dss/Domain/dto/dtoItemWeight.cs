using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Dss.Domain.Templates
{
     [Serializable]
    public class dtoItemWeightBase
    {
        public virtual long IdObject  { get; set; }
        public virtual String Name { get; set; }
        public virtual String Value  { get; set; } 
        public virtual Boolean IsFuzzyValue  { get; set; }
        public virtual Boolean OrderedItem { get; set; } 
    }

     [Serializable]
    public class dtoItemWeight: dtoItemWeightBase
    {
        public virtual Boolean IsFirst { get; set; }
        public virtual Boolean IsLast { get; set; }
        public virtual DssError Error { get; set; }

        public dtoItemWeight()
        {
            Error = DssError.None;
        }
        public dtoItemWeight(dtoItemWeightBase item)
        {
            IdObject = item.IdObject;
            IsFuzzyValue = item.IsFuzzyValue;
            Name = item.Name;
            Value = item.Value;
            OrderedItem = item.OrderedItem;
            Error = DssError.None;
        }
    }
}