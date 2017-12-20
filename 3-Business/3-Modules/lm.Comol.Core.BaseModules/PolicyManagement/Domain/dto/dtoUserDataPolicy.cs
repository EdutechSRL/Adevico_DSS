using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.PersonalInfo;

namespace lm.Comol.Core.BaseModules.PolicyManagement
{
    [Serializable]
    public class dtoUserDataPolicy
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Text { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual Int32 DisplayOrder  { get; set; }
        public virtual PolicyType Type { get; set; }
        public virtual dtoUserPolicyInfo UserInfo { get; set; }
    }
}