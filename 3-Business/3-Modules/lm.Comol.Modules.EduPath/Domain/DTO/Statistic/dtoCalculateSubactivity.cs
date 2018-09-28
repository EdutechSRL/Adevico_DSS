using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoCalculateSubactivity
    {
        public long IdActivity { get; set; }
        public long IdSubActivity { get; set; }
        public long IdLink { get; set; }
        public long IdObject { get; set; }
        public liteSubActivity Item { get; set; }

        public dtoCalculateSubactivity()
        {

        }
        public dtoCalculateSubactivity(liteSubActivity item)
        {
            IdActivity = item.IdActivity;
            IdSubActivity = item.Id;
            IdLink = (item.ModuleLink != null ? item.ModuleLink.Id : 0);
            IdObject = item.IdObjectLong;
            Item = item;
        }
    }
}