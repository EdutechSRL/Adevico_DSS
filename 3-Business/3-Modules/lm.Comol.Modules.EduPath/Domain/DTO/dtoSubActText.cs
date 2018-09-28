using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoSubActText
    {
        public long Id { get; set; }
        public String Description { get; set; }
        public Status Status { get; set; }
        public Int64 Weight { get; set; }
        public int CommunityId { get; set; }


        public dtoSubActText() { }

    }
}
