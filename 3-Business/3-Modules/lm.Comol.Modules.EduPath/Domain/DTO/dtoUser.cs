using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoUser
    {
        public virtual Int32 Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Surname { get; set; }
        public virtual String DisplayName { get { return Surname + " " + Name; } }
    }
}
