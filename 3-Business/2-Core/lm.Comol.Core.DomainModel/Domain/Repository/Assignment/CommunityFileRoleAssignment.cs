using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityFileRoleAssignment : CommunityFileAssignment
	{
        public virtual Role AssignedTo { get; set; }
	}
}