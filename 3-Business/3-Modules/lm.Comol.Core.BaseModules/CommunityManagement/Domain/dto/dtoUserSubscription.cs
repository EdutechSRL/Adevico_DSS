
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Communities;
namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable(), CLSCompliant(true)]
    public class dtoUserSubscription : dtoBaseUserSubscription
    {
        public Int32 IdRole { get; set; }
        public Boolean isNew { get { return Id == 0; } }
        public Boolean isToUpdate { get { return IdRole != IdPreviousRole; } }
        public String MostLikelyPath { get; set; }
        public dtoUserSubscription()
        {

        }
    }
}