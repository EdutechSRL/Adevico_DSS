using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class liteRoleCommunityTypeTemplate : lm.Comol.Core.DomainModel.DomainObject<int>
	{
        public virtual Int32 IdCommunityType { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Boolean isDefault { get; set; }
        public virtual Boolean AlwaysAvailable { get; set; }
	}
}