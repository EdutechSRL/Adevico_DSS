using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoStatusStatItem
    {

        public long idItem { get; set; }
        public StatusStatistic statusStat { get; set; }
     

        public dtoStatusStatItem()
        {}
    }
}
