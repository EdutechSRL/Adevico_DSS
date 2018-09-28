using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class MoocCokade
    {
        public virtual Int64 Id { get; set; }
        public virtual int CommunityId { get; set; }
        public virtual bool CokadeEnable { get; set; }
    }
}
