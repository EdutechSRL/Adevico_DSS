using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Federation.Domain
{
    [Serializable]
    public class dtoUserfederationData
    {
        public int UserId { get; set; }
        public int CommunityId { get; set; }

        public Federation.Enums.FederationResult Result { get; set; }

        public DateTime Creation { get; set; }

        public TimeSpan LifeTime {
            get
            {
                return (DateTime.Now - Creation);
            }
        }

    }
}
