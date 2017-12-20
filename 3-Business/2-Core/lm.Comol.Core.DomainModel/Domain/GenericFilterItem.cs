
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class GenericItemStatus<T, Y> : lm.Comol.Core.DomainModel.DomainObject<T>
	{
        public Y Status { get; set; }
	}
}