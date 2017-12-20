
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class OrganizationAffiliation : DomainBaseObjectMetaInfo<long>
	{
        public virtual Agency Agency { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsEmpty { get; set; }
        public OrganizationAffiliation()
            : base()
		{

		}
	}
}