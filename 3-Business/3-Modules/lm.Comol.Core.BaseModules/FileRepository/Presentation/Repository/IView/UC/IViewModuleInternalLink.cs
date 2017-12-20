using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
	public interface IViewModuleInternalLink : lm.Comol.Core.DomainModel.Common.iDomainView
	{
        TreeMode TreeSelect { get; set; }
        Boolean FolderSelectable { get; set; }
        Boolean RemoveEmptyFolders { get; set; }
		Boolean ViewAlreadySelectedFiles { get; set; }
		List<lm.Comol.Core.DomainModel.ModuleActionLink> GetNewRepositoryItemLinks();
		List<long> GetSelectedFolders();
		List<long> GetRepositoryItemLinksId();
        void InitializeControlForCommunity(Int32 idCurrentUser,Int32 idCommunity, List<RepositoryItemLinkBase<long>> files, Boolean viewSelectedPanel, Boolean loadSelectedFilesIntoTree, Boolean showAlsoHiddenItems, Boolean adminPurpose);
        void InitializeControl(Int32 idCurrentUser, RepositoryIdentifier identifier, List<RepositoryItemLinkBase<long>> files, Boolean viewSelectedPanel, Boolean loadSelectedFilesIntoTree, Boolean showAlsoHiddenItems, Boolean adminPurpose);

        void InitializeControl(Int32 idCurrentUser, RepositoryIdentifier identifier, List<long> itemsLink, List<ItemType> typesToLoad, ItemAvailability availability, Boolean showAlsoHiddenItems, Boolean adminPurpose);
        void InitializeControlForPortal(Int32 idCurrentUser, List<RepositoryItemLinkBase<long>> files, Boolean viewSelectedPanel, Boolean loadSelectedFilesIntoTree, Boolean showAlsoHiddenItems, Boolean adminPurpose);
		void DisableControl();
	}
}