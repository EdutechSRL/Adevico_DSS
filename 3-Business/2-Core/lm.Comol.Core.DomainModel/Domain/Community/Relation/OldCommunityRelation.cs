
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class OldCommunityRelation : DomainObject<Int32>
	{
        public virtual Community Destination { get; set; }
        public virtual Community Source { get; set; }
        public OldCommunityRelation()
		{
		}
	}
}