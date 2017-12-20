using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewTicketEditExt : iViewBase
    {
        void InitView(Domain.DTO.DTO_UserModify TicketData);

        Int64 TicketId { get; set; }

        void ShowSendError(Domain.Enums.TicketMessageSendError Error);

        /// <summary>
        /// Gestione errore in caso di modifica stato: chiusura/riapertura.
        /// </summary>
        /// <param name="IsReopen">True: in caso di riapertura; False in caso di chiusura</param>
        void ShowChangeStatusError(Boolean IsReopen);

        String GetChangeStatusMessage(Domain.Enums.TicketStatus NewStatus);

        Domain.DTO.DTO_User CurrentUser { get; }

        void ShowDraft(Int64 TicketId);

        /// <summary>
        /// Imposta la modifica a "ReadOnly"
        /// </summary>
        /// <param name="IsTicketLocked">
        /// False: Edit ticket bloccato da sistema
        /// True: edit ticket bloccato da gestore ticket
        /// </param>
        void SetReadOnly(bool IsTicketLocked);

        Int64 DraftMsgId { get; set; }

        //bool IsOwner { get; set; }

        /// <summary>
        /// Effettua l'upload effettivo e carica i file...
        /// </summary>
        /// <returns></returns>
        List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> GetUploadedItems(Domain.Message draftMessage);
    }
}
