
using System.Collections.Generic;
using System;
namespace lm.Comol.Core.DomainModel.Common
{
	[CLSCompliant(true)]
	public interface IViewOtherModuleLinkRepositoryItems : lm.Comol.Core.DomainModel.Common.iDomainView
	{

		bool ViewAlreadySelectedFiles { get; set; }
		List<ModuleActionLink> GetNewRepositoryItemLinks();
		List<long> GetSelectedFolder();
		//        Function GetSelectedItems() As List(Of Long)

		List<long> GetRepositoryItemLinksId();
		void InitializeControl(int IdCommunity, List<iCoreItemFileLink<long>> files, bool ViewSelectedPanel, bool LoadSelectedFilesIntoTree, bool showAlsoHiddenItems, bool adminPurpose);

		void DisableControl();
	}
}