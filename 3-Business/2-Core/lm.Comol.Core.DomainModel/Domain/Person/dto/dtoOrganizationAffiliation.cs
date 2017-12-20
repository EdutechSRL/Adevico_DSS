
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class dtoOrganizationAffiliation : DomainObject<Int32>
	{
        public virtual String Name { get; set; }
        public virtual Boolean IsEmpty { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public dtoOrganizationAffiliation()
            : base()
		{

		}
	}
}