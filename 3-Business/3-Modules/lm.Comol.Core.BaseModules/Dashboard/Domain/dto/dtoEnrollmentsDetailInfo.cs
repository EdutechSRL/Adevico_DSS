using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Domain
{
    [Serializable]
    public class dtoEnrollmentsDetailInfo 
    {
        public Int32 IdRole { get; set; }
        public String Name { get; set; }
        public long Count { get; set; }
        public long Waiting { get; set; }
        public long Blocked { get; set; }
        public Boolean IsDefault { get; set; }
        public long Active { get { return Count - Waiting - Blocked; } }
        public dtoEnrollmentsDetailInfo()
        {

        }
    }
}