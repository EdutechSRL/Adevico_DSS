
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class liteAgencyAffiliation : DomainBaseObject<long>
	{
        public virtual Int32 IdPerson { get; set; }
        public virtual long IdAgency { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual Boolean IsEnabled { get; set; }

        public liteAgencyAffiliation()
            : base()
		{

		}
	}
}