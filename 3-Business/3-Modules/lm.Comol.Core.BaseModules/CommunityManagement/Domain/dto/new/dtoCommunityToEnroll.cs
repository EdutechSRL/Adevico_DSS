using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable]
    public class dtoCommunityToEnroll 
    {
        public Int32  Id { get; set; }
        public String Path { get; set; }
        public Boolean Selected { get; set; }
        public Int32 PageIndex { get; set; }
        public dtoCommunityToEnroll()
        {

        }
    }
}