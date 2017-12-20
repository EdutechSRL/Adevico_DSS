using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class DomainObject<T> : iDomainObject<T>, IEquatable<T>
	{
        public virtual T Id {get;set;}

		public virtual bool Equals(T other)
		{
			return this.Id.Equals(other);
		}
	}
}