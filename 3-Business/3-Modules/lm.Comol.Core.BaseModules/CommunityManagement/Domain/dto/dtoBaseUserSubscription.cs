
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Communities;
namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable(), CLSCompliant(true)]
    public class dtoBaseUserSubscription
    {
        public Int32 Id { get; set; }
        public Int32 IdCommunity { get; set; }
        public Int32 IdCommunityType { get; set; }
        public String CommunityName { get; set; }
        public Int32 IdPreviousRole { get; set; }
        public List<String> Path { get; set; }
        public dtoBaseUserSubscription()
        {

        }
    }
}