using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class dtoAgencyAffiliation : DomainObject<long>
    {
        //public virtual Person Employee { get; set; }
        //public virtual Agency Agency { get; set; }
        public virtual KeyValuePair<long, String> Agency { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        
        public dtoAgencyAffiliation()
            : base()
        {
            IsEnabled = true;
            FromDate = DateTime.Now;
            Agency = new KeyValuePair<long, String>();
        }
    }
}