using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement.Domain
{
    public static class CacheKeys
    {
        public static String AllTree
        {
            get { return "CommunityTree_"; }
        }
        public static String UserCommunitiesTree(Int32 idUser)
        {
            return AllTree + idUser.ToString() + "_" ;
        }
    }
}