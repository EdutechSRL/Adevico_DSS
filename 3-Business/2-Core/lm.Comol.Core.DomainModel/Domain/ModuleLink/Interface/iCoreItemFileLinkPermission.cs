using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	public interface iCoreItemFileLinkPermission<T>
	{
		T ItemFileLinkId { get; set; }
		iCoreItemFileLink<T> ItemFileLink { get; set; }
		iCoreFilePermission Permission { get; set; }
		Boolean isModified();
		IList<TranslatedItem<long>> AvailableStatus { get; set; }
	}
}