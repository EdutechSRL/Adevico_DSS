
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityFileCommunityAssignment : CommunityFileAssignment
	{
        public virtual Community AssignedTo { get; set; }

		public CommunityFileCommunityAssignment()
		{
		}
	}
}