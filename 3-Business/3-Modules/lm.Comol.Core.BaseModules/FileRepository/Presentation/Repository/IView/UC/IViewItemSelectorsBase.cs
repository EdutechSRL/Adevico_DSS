using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewItemSelectorsBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Context"
            Int32 IdUserLoader { get; set; }
         
            Boolean isInitialized { get; set; }
            List<long> IdRemovedItems { get; set; }
            Dictionary<Boolean, List<long>> GetCurrentSelection();
            Boolean AllowSelectAll { get; set; }
            Boolean HasAvailableItems { get; }
           
          
            Boolean LoadForModule { get; set; }
            String ModuleCode { get; set; }
            List<long> IdSelectedItems { get; set; }


            List<ItemType> AvailableTypes { get; set; }
            RepositoryIdentifier RepositoryIdentifier { get; set; }
            List<StatisticType> DisplayStatistics { get; set; }
            Boolean AllSelected { get; set; }
            ItemAvailability CurrentAvailability { get; set; }
            Boolean CurrentAdminMode { get; set; }
            Boolean CurrentShowHiddenItems { get; set; }
            Boolean CurrentDisableNotAvailableItems { get; set; }
        #endregion

        void InitializeControl(Int32 idUser, RepositoryIdentifier identifier,Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad,ItemAvailability availability,List<StatisticType> displayStatistics, List<long> idRemovedItems,List<long> idSelectedItems = null,  OrderBy orderBy = OrderBy.name, Boolean ascending=true);
        void DisplaySessionTimeout();
        List<dtoRepositoryItemToSelect> GetSelectedItems();
    }
}