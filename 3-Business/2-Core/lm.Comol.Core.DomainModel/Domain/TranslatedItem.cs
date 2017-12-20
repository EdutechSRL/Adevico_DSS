using System;

namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true), Serializable()]
	public class TranslatedItem<T> : DomainObject<T>
	{
        public string Translation { get; set; }
	}
}