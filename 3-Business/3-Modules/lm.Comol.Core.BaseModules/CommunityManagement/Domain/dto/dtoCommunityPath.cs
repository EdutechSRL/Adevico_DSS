
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Communities;
namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable(), CLSCompliant(true)]
    public class dtoCommunityPath 
    {
        public String Path { get; set; }
        public List<Int32> Idcommunities { get; set; }
        public Int32 Subscriptions { get; set; }
        public Int32 HiddenSubscriptions { get; set; }
        public long AllSubscriptions { get { return Subscriptions + HiddenSubscriptions; } }
        public dtoCommunityPath()
        {

        }
    }
}