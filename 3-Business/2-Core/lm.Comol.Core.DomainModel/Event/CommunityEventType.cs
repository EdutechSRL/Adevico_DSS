using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityEventType : DomainObject<int>
	{
        public virtual string Name { get; set; }
		public CommunityEventType()
		{
		}
	}
}