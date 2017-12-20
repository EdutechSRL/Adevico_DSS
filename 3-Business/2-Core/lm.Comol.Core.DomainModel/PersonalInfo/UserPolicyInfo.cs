using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.PersonalInfo
{
    [Serializable]
    public class UserPolicyInfo : DomainBaseObjectMetaInfo<long>
    {
        public virtual DataPolicy Policy { get; set; }
        public virtual Person Owner { get; set; }
        public virtual Boolean Accepted { get; set; }
    }
}
