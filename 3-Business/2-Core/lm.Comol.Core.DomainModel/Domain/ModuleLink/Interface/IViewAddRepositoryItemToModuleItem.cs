
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel.Common
{
	[CLSCompliant(true)]
	public interface IViewAddRepositoryItemToModuleItem<T> : lm.Comol.Core.DomainModel.Common.iDomainView
	{

		T PreloadedItemID { get; }

		int PreloadedCommunityID { get; }
		T CurrentItemID { get; set; }
		int CurrentItemCommunityID { get; set; }

		int ModuleID { get; set; }
		bool AllowCommunityLink { set; }

		void SetBackToManagementUrl(int IdCommunity, T IdItem);

		void NoPermissionToManagementFiles(int IdCommunity, int ModuleID);

		void InitializeFileSelector(int IdCommunity, List<iCoreItemFileLink<long>> itemLinks, bool showAlsoHiddenItems, bool adminPurpose);
		void ReturnToFileManagement(int IdCommunity, T IdItem);
	}
}