using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public abstract class BaseCRoleAssignment : BaseAssignment
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdRole { get; set; }

        public BaseCRoleAssignment() { }

        public BaseCRoleAssignment(long Id, RoleEP RoleEP, Int64 MinCompletion, Int32 idRole, Int32 idCommunity, Int32 IdCreatedBy,
             DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
            : base(Id, RoleEP, MinCompletion, IdCreatedBy, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
        {
            IdCommunity = idCommunity;
            IdRole = idRole;
        }
    }
}
