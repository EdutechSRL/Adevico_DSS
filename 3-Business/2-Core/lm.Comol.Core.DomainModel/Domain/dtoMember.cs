

using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class dtoMember<T> : DomainObject<T>
	{
        public string Name{get; set;}

		public dtoMember()
		{
		}
	}
}