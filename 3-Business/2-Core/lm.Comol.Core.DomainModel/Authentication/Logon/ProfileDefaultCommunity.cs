using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]

    public class ProfileDefaultCommunity : DomainBaseObject<long>
    {
        public virtual Person Person {get;set;}
        public virtual Int32 IdCommunity { get; set; }
        public virtual Boolean isEnabled { get; set; }
    }
}