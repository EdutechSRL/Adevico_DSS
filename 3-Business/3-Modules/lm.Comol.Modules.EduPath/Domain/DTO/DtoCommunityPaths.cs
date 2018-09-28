using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class DtoCommunityPaths
    {
        public Int32 IdCommunity { get; set; }

        public string CommunityName { get; set; }

        public Int32 PathsCount { get; set; }
        public Int32 UnlockedCount { get; set; }
        public Int32 LockedCount { get; set; }
        public Int32 DraftCount { get; set; }
    }

    public class DtoCommunitiesPaths
    {
        public IList<DtoCommunityPaths> CommunityPaths { get; set; }

        public Int32 PathsCount { get; set; }
        public Int32 UnlockedCount { get; set; }
        public Int32 LockedCount { get; set; }
        public Int32 DraftCount { get; set; }
    }
}
