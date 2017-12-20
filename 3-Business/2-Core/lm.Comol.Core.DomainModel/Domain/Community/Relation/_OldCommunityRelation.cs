
using System;
namespace lm.Comol.Core.Communities
{
	[Serializable(), CLSCompliant(true)]
	public class _OldCommunityRelation : lm.Comol.Core.DomainModel.DomainObject<Int32>
	{
        public virtual Int32 IdDestination { get; set; }
        public virtual Int32 IdSource { get; set; }
        public _OldCommunityRelation()
		{
		}
	}
}