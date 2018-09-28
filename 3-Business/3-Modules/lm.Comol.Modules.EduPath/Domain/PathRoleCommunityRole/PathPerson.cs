using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class PathPerson:PRoleCRole
    {
        public virtual litePerson AssignedUser { get; set; }
       
        public PathPerson() { }
    }
}
