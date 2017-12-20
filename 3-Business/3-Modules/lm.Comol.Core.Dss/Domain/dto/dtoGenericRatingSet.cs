using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain
{
    [Serializable]
    public class dtoGenericRatingSet : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual List<dtoGenericRatingValue> Values { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean IsForWeights { get; set; }
        public virtual Boolean IsDefault { get; set; }

        public dtoGenericRatingSet()
        {
            Values = new List<dtoGenericRatingValue>();
        }

        public Boolean IsValid()
        {
            return Values != null && Values.Any();
        }
        public String ToString()
        {
            return String.Format("Id: {0} Name: {1} IsFuzzy: {2} ",
                Id,  Name, IsFuzzy);
        }
    }
}