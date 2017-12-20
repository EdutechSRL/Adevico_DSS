using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true), Serializable()]
	public class FilterBase<T>
	{
        public T OrderBy { get; set; }
        public PagerBase Pager { get; set; }
        public bool Ascending { get; set; }
        public ObjectStatus Status { get; set; }

		public FilterBase()
		{
			Status = ObjectStatus.Active;
		}

	}
}