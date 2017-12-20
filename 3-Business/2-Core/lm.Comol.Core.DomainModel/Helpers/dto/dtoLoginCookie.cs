using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class dtoLoginCookie
    {
        public virtual int IdPerson {get;set;}
        public virtual long IdProvider {get;set;}
        public virtual long IdDefaultProvider {get;set;}
        public virtual int AuthenticationTypeID {get;set;}
        public virtual String DefaultUrl { get; set; }

        public dtoLoginCookie()
        {
        }
    }
}