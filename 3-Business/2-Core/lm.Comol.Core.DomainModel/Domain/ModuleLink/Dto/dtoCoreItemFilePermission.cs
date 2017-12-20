
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true), Serializable()]
	public class dtoCoreItemFilePermission<T> : iCoreItemFileLinkPermission<T>
	{
        public T ItemFileLinkId { get; set; }
        public iCoreFilePermission Permission { get; set; }
        public IList<TranslatedItem<long>> AvailableStatus { get; set; }
        public iCoreItemFileLink<T> ItemFileLink { get; set; }

		public dtoCoreItemFilePermission()
		{
			AvailableStatus = new List<TranslatedItem<long>>();
		}
		public dtoCoreItemFilePermission(T pId)
		{
            ItemFileLinkId = pId;
		}
		public dtoCoreItemFilePermission(T pId, IList<TranslatedItem<long>> statusList)
		{
            ItemFileLinkId = pId;
			AvailableStatus = statusList;
		}
		public bool isModified()
		{
			return ItemFileLink.isModified();
		}
	}
}