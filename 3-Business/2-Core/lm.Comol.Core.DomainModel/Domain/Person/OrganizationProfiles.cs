using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class OrganizationProfiles : DomainObject<int>
	{
        public virtual Person Profile { get; set; }
        public virtual bool isDefault { get; set; }
        public virtual int OrganizationID { get; set; }

		public OrganizationProfiles()
		{
		}
	}
}