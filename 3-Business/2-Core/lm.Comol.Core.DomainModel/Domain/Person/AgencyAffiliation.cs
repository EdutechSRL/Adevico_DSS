
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class AgencyAffiliation : DomainBaseObjectMetaInfo<long>
	{
        public virtual Person Employee { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual Boolean IsEnabled { get; set; }

        public AgencyAffiliation()
            : base()
		{
            IsEnabled = true;
            FromDate = DateTime.Now;
		}
	}
}