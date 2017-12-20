using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewTicketEditMan : iViewBase
    {
        /// <summary>
        /// NOTA: Se ERROR != none, non fare BIND dei dati, MA procedere di conseguenza (NO PERMISSION).
        /// </summary>
        /// <param name="TicketData"></param>
        void InitView(Domain.DTO.DTO_ManagerModify TicketData, 
            IList<Domain.DTO.DTO_CategoryTree> Categories,
            Domain.DTO.DTO_CategoryTree CurrentCate,
            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> uploadActions,
            lm.Comol.Core.FileRepository.Domain.ModuleRepository rPermissions, 
            int idCommunity,
            long idMessage,
            bool communityHasManager,
            bool hasCommunity);
        
        Int64 TicketId { get; set; }

        Int64 DraftMsgId { get; set; }

        void ShowSendError(Domain.Enums.TicketMessageSendError Error);

        void ShowReopenError();
        void ShowAssignError(Domain.Enums.CategoryReassignError Error);

        void ShowAssignUsrError(Domain.Enums.UserReassignError Error);

        void ShowCategoryChanged();

        Domain.Enums.EditManResMessagesOrder MessagesOrder { get; set; }
        Domain.Enums.EditManResMessagesShow MassageFilter { get; set; }

        String GetChangeStatusMessage(Domain.Enums.TicketStatus NewStatus);
        String GetChangeConditionMessage(Domain.Enums.TicketCondition Condition);

        String GetChangeCategoryMessage();

        String GetChangeUserMessage();

        Domain.Enums.MessageUserType UserType { get; set; }

        ////void ShowSendError(Domain.Enums.TicketMessageSendError Error);
        ///// <summary>
        ///// ID comunità della view, che deriva da:
        ///// Se omesso restituisce -1
        ///// </summary>
        //Int32 ViewCommunityId { get; set; }

        void ShowNoPermission();

        ////int ViewCommunityId { get; set; }
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
    }
}
