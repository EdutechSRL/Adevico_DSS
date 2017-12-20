using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class dtoCreatedProfile
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Surname { get; set; }
        public virtual String Login { get; set; }
        public virtual String Password { get; set; }
        public virtual String Mail { get; set; }
        public virtual String TaxCode { get; set; }
        public virtual String DisplayName { get { return Name + ' ' + Surname; } }
    }
}
