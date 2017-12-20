using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class RoleCommunityTypeTemplate : lm.Comol.Core.DomainModel.DomainObject<int>
	{
        public virtual CommunityType Type { get; set; }
        public virtual Role Role { get; set; }
        public virtual bool isDefault { get; set; }
        public virtual bool AlwaysAvailable { get; set; }
	}
}