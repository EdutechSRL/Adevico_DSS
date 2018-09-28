using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{

    [Serializable()]
    public class PathPersonAssignment : BasePersonAssignment
    {
        public virtual long IdPath{ get; set; }

        public PathPersonAssignment()
        { }

        public PathPersonAssignment(long idPath, long Id, Int32 idPerson, RoleEP RoleEP, Int64 MinCompletion, Int32 idCreatedBy, DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
            : base(Id, RoleEP, MinCompletion, idPerson, idCreatedBy, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
        {
            IdPath = idPath;
        }
    }
}