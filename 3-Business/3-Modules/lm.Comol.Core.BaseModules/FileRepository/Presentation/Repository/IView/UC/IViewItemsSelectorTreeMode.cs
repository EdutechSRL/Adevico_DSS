using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewItemsSelectorTreeMode : IViewItemSelectorsBase
    {
        TreeMode TreeSelect { get; set; }
        Boolean FolderSelectable { get; set; }
        Boolean RemoveEmptyFolders { get; set; }
        Boolean HasItemsToSelect { get; set; }
        Guid ControlUniqueId { get; set; }
        #region "Translations"
            String GetRootFolderFullname();
            String GetRootFolderName();
        #endregion


        void InitializeControlForModule(Int32 idUser, RepositoryIdentifier identifier, Boolean loadSelectedItems, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, ItemAvailability availability, Boolean allowSelectFolder, List<long> idSelectedItems, List<long> idRemovedItems=null);
        void InitializeControlForModule(Int32 idUser, RepositoryIdentifier identifier, Boolean loadSelectedItems, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad, ItemAvailability availability, Boolean allowSelectFolder, List<long> idSelectedItems, List<long> idRemovedItems = null);
        void LoadItems(List<dtoRepositoryItemToSelect> items);
    }
}