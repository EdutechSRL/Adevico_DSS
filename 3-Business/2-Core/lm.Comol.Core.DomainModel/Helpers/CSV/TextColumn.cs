
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class TextColumn 
    {
        public virtual int Number { get; set; }
        public virtual String Value { get; set; }
        public virtual Boolean Empty { get { return string.IsNullOrEmpty(Value); } }
    }
}