using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
   [Serializable]
    public class dtoNavigationActivity
    {
    
    
        public long ActivityId { get; set; }
        public long ParentUnitId { get; set; }
        public string ParentUnitName { get; set; }

        public dtoNavigationActivity()
        {
        }
    }
}
