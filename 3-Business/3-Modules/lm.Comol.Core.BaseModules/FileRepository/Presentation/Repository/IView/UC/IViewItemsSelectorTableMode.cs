using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewItemsSelectorTableMode : IViewItemSelectorsBase
    {

        #region "Context"
            OrderBy CurrentOrderBy { get; set; }
            Boolean CurrentAscending { get; set; }
            Boolean AllowPaging { get; set; }
            Int32 CurrentPageSize { get; }
            Int32 DefaultPageSize { get; set; }
            PagerBase Pager { get; set; }        
        #endregion

        void InitializeControlByModule(Int32 idUser, String moduleCode, List<dtoRepositoryItemToSelect> itemsToLoad, List<ItemType> typesToLoad, ItemAvailability availability);
        void LoadItems(List<dtoRepositoryItemToSelect> items);
    }
}