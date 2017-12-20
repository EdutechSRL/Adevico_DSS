using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class liteOrganizationProfile : DomainObject<int>
	{
        public virtual Int32 IdPerson { get; set; }
        public virtual Boolean isDefault { get; set; }
        public virtual Int32 IdOrganization { get; set; }

        public liteOrganizationProfile()
		{
		}
	}
}