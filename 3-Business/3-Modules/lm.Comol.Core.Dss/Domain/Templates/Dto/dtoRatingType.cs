using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain
{
    [Serializable ]
    public class dtoRatingType
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public RatingType Type { get; set; }
    }
}