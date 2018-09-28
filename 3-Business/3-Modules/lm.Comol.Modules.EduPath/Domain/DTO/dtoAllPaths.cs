using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class dtoAllPaths
    {

        public long NotStarted { get; set; }

        public long Started { get; set; }

        public long Completed { get; set; }

        public long Total { get; set; }

        public IList<dtoPathInfo> Paths { get; set; }

        public ILookup<int, List<dtoPathInfo>> PathsByCommunity
        {
            get
            {
                return (from item in Paths group item by item.IdCommunity into gr select gr).ToLookup(x => x.Key, x => x.ToList());
            }
        }
    }

    public class dtoPathInfo
    {
        
        public Int64 IdPath { get; set; }
        public String PathName { get; set; }
        public Int32 IdCommunity { get; set; }
        public String CommunityName { get; set; }
        public Boolean PathLocked { get; set; }
        public Boolean IsMooc { get; set; }
        public EPType PathType { get; set; }
        //public StatusStatistic Status { get; set; }

        public String Status { get; set; }        
        public String Deadline { get; set; }

        public PathStatistic Ps { get; set; }

        public Boolean CanManage { get; set; }        
        public Boolean CanStat { get; set; }

        public long NotStarted { get; set; }

        public long Started { get; set; }

        public long Completed { get; set; }
    }
}
