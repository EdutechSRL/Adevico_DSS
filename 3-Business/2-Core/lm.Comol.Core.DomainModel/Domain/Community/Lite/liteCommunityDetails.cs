
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class liteCommunityDetails : lm.Comol.Core.DomainModel.DomainObject<int>
	{
        public virtual String Description { get; set; }
        public liteCommunityDetails()
		{

		}
	}
}