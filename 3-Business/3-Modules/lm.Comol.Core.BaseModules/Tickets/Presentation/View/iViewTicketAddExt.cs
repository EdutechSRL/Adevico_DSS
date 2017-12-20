using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewTicketAddExt : iViewBase
    {

        void InitView(Domain.DTO.DTO_AddInit Values);

        Int64 SelectedCatId { get; }

        Int64 CurrentTicketId { get; set; }

        void ShowError(Domain.Enums.TicketCreateError Error);

        void TicketCreated(Int64 TicketId, Boolean IsDraft);

        void refreshCategory(IList<Domain.DTO.DTO_CategoryTree> Categories, Int64 SelectedCategoryId); //Domain.DTO.DTO_CategoryTree SelectedCategory

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NoCategory">Se true, non sono presenti categorie ed il servizio si può considerare disabilitato.</param>
        /// <param name="ToMuchTicket">Se true: troppi ticket in bozza, se FALSE: nessun permesso (no utente o no permessi)</param>
        void ShowCantCreate(bool NoCategory, bool ToMuchTicket);

        Domain.DTO.DTO_User CurrentUser { get; }

        //void RedirectToLogin();
        String GetDraftTitle();
        String GetDraftPreview();

        Int64 DraftMsgId { get; set; }

        /// <summary>
        /// Effettua l'upload effettivo e carica i file...
        /// </summary>
        /// <returns></returns>
        List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> GetUploadedItems(Domain.Message DraftMessage);
    }
}
