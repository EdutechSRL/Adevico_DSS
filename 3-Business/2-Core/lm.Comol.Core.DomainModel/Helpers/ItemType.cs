using System;

namespace lm.Comol.Core.DomainModel.Helpers
{
	[Serializable(), CLSCompliant(true)]
	public class ItemType<T>
	{
        public string Item {get;set;}
        public T Type {get;set;}
	}
}