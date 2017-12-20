using System;
namespace lm.Comol.Core.Communities
{
	[Serializable(), CLSCompliant(true)]
    public class _RoleCommunityTypeTemplate : lm.Comol.Core.DomainModel.DomainObject<int>
	{
        public virtual Int32 IdCommunityType { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Boolean isDefault { get; set; }
        public virtual Boolean AlwaysAvailable { get; set; }
	}
}