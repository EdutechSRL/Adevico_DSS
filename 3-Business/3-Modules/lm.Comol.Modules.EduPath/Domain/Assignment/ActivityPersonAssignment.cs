using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class ActivityPersonAssignment:BasePersonAssignment
    {
        public virtual long IdActivity { get; set; }
        public virtual long IdUnit { get; set; }
        public virtual long IdPath { get; set; }

        public ActivityPersonAssignment() { }

        public ActivityPersonAssignment(long idActivity, long Id, Int32 idPerson, RoleEP RoleEP, Int64 MinCompletion, Int32 idCreatedBy, DateTime? CreatedOn, String CreatorIpAddress, String CreatorProxyIpAddress)
            : base(Id, RoleEP, MinCompletion, idPerson, idCreatedBy, CreatedOn, CreatorIpAddress, CreatorProxyIpAddress)
        {
            IdActivity = idActivity;
        }
    }
}