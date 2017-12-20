using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityFilePersonAssignment : CommunityFileAssignment
	{
        public virtual Person AssignedTo { get; set; }

		public CommunityFilePersonAssignment()
		{
		}
	}
}