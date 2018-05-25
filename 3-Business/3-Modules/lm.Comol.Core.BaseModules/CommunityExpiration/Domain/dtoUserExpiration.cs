using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Domain
{
    public class dtoUserExpiration
    {
        public dtoUserExpiration() { }

        public dtoUserExpiration(DateTime? Start, int DayDuration)
        {
            StartDateTime = Start;
            Duration = DayDuration;
        }

        public DateTime? StartDateTime { get; set; }
        public int Duration { get; set; }
        public DateTime? Expiration
        {
            get
            {
                if (StartDateTime == null || Duration <= 0)
                    return null;

                return ((DateTime)StartDateTime).AddDays(Duration);

            }
        }
    }
}
