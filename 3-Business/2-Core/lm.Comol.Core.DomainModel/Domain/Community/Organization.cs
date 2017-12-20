using System;

namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class Organization : DomainObject<int>
	{
        public virtual string Name { get; set; }
	}
}