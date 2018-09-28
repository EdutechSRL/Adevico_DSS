using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public abstract class BasePersonAssignment : BaseAssignment
    {
        public virtual Int32 IdPerson { get; set; }

        public BasePersonAssignment(long Id, RoleEP RoleEP, Int64 MinCompletion, Int32 idPerson, Int32 IdCreatedBy,
             DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
            : base(Id, RoleEP, MinCompletion, IdCreatedBy, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
        {
            IdPerson = idPerson;
        }
        public BasePersonAssignment() { }
    
    }
}