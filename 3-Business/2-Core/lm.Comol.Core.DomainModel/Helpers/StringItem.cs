using System;

namespace lm.Comol.Core.DomainModel.Helpers
{
	[Serializable(), CLSCompliant(true)]
    public class StringItem<T>
	{
        public string Name {get;set;}
        public T Id {get;set;}
	}
}