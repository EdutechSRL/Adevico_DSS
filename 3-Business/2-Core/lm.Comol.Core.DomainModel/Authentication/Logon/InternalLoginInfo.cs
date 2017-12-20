using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]

    public class InternalLoginInfo : BaseLoginInfo
    {
        public virtual String Login {get;set;}
        public virtual String Password { get; set; }
        public virtual DateTime? PasswordExpiresOn { get; set; }
        public virtual EditType ResetType { get; set; }
    }
}