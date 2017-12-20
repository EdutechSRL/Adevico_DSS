using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewTicketListMan : iViewBase//: lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void BindTable(
            IList<Domain.DTO.DTO_TicketListItemManager> Items,
            int TotalRec,
            int CurrentPage,
            bool HasDraft);

        void bindInfo(Domain.DTO.DTO_ListInfo FilterInit);

        void BindFilters(
            IList<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> Languages);

        void BindCategoriesFilter(IList<Domain.DTO.DTO_CategoryTree> Categories,
            Domain.DTO.DTO_CategoryTree SelectedCategory);

        //IList<String> LangCodes,
        //Domain.DTO.DTO_ListFilterManager Filters { get; set; }

        /// <summary>
        /// Mostra informazioni sui ticket...
        /// </summary>
        /// <param name="Info">ticket aperti, chiusi, etc...</param>
        void SetInfo(Domain.DTO.DTO_ListInfo Info);

        ///// <summary>
        ///// Imposta/Aggiorna la DDL con la lista delle categorie selezionabili
        ///// </summary>
        ///// <param name="Categories"></param>
        //void refreshCategory(IList<Domain.DTO.DTO_CategoryTree> Categories);

        String GetChangeStatusMessage(Domain.Enums.TicketStatus NewStatus);

        void ShowNoPermission();

        void DisableAddNew();
        
    }
}
