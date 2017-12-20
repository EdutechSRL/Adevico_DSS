using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class liteCommunityModuleAssociation : DomainObject<int>
	{
        public virtual Int32 IdCommunity { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        public virtual Int32 IdModule { get; set; }

	}
}