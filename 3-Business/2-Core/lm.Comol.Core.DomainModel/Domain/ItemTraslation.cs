
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ItemTraslation<T, Y> : lm.Comol.Core.DomainModel.DomainObject<T>
	{
        public virtual string Translation { get; set; }
        public virtual Y Item { get; set; }
        public virtual Language SelectedLanguage { get; set; }
		public ItemTraslation()
		{
		}
	}
}