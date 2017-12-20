
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityType : lm.Comol.Core.DomainModel.DomainObject<int>
	{
        public virtual string Logo { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsVisible { get; set; }

		public CommunityType()
		{
		}
		public CommunityType(int iCommunityId)
		{
			base.Id = iCommunityId;
		}
	}
}