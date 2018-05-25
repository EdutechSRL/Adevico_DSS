using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ApiToken.Domain
{
    public class ApiToken
    {
        public virtual Int64 Id { get; set; }
        public virtual Int32 PersonId { get; set; }
        public virtual String Token { get; set; }
        public virtual DateTime CreateOn { get; set; }

        public virtual String DeviceId { get; set; }

        public virtual TokenType Type { get; set; }

        public virtual bool IsExpired
        {
            get
            {
                if (CreateOn.AddHours(24) < DateTime.Now)
                    return true;

                return false;
            }
        }
    }


    public enum TokenType : short
    {
        None = 0,
        AdevicoWeb = 1, 
        Mobile = 20
    }
}
