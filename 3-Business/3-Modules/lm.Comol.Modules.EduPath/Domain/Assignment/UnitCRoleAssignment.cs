using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class UnitCRoleAssignment : BaseCRoleAssignment
    {
        public virtual long IdUnit { get; set; }
        public UnitCRoleAssignment() { }

        public UnitCRoleAssignment(long idUnit, long Id, Int32 idRole, Int32 idCommunity, RoleEP RoleEP, Int64 MinCompletion, Int32 idCreatedBy, DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
            : base(Id, RoleEP, MinCompletion, idRole, idCommunity, idCreatedBy, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
        {
            IdUnit = idUnit;
        }
    }
}