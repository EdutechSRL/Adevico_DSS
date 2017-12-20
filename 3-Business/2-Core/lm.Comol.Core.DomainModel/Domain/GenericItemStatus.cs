
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class GenericFilterItem<T, Y> : lm.Comol.Core.DomainModel.DomainObject<T>
	{
        public Y Name { get; set; }
	}
}