using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class Role : DomainObject<int>, iRole
	{
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

		public Role()
		{
		}

	}
}