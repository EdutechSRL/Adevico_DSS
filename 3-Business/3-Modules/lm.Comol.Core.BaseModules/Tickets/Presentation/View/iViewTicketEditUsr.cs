using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewTicketEditUsr : iViewBase
    {
        void InitView(
            Domain.DTO.DTO_UserModify TicketData,
            List<DomainModel.Repository.RepositoryAttachmentUploadActions> actions,
            DomainModel.Repository.RepositoryAttachmentUploadActions dAction,
            lm.Comol.Core.FileRepository.Domain.ModuleRepository rPermissions,
            int idCommunity,
            long idMessage);

        Int64 TicketId { get; set; }

        void ShowSendError(Domain.Enums.TicketMessageSendError Error);

        /// <summary>
        /// Gestione errore in caso di modifica stato: chiusura/riapertura.
        /// </summary>
        /// <param name="IsReopen">True: in caso di riapertura; False in caso di chiusura</param>
        void ShowChangeStatusError(Boolean IsReopen);

        String GetChangeStatusMessage(Domain.Enums.TicketStatus NewStatus);

        //int ViewCommunityId { get; set; }
        ////To iView Base Internal
        //void DisplaySessionTimeout(Int32 CommunityId);


        //void SendAction(
        //    ModuleTicket.ActionType Action,
        //    Int32 idCommunity,
        //    ModuleTicket.InteractionType Type,
        //    IList<KeyValuePair<Int32, String>> Objects = null);

        /// <summary>
        /// Imposta la modifica a "ReadOnly"
        /// </summary>
        /// <param name="IsTicketLocked">
        /// False: Edit ticket bloccato da sistema
        /// True: edit ticket bloccato da gestore ticket
        /// </param>
        void SetReadOnly(bool IsTicketLocked);

        /// <summary>
        /// Effettua l'upload effettivo e carica i file...
        /// </summary>
        /// <returns></returns>
        List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> GetUploadedItems(Domain.Message draftMessage, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions action);

        /// <summary>
        /// Mantiene l'ID del messaggio in bozza
        /// </summary>
        Int64 DraftMsgId { get; set; }

        void ShowBehalfError(Domain.Enums.BehalfError behalfError);

        void InitPersonSelector(List<Int32> hidePersonId);

        //void ShowInitError(Domain.Enums.TicketEditUserErrors error);

    }
}
