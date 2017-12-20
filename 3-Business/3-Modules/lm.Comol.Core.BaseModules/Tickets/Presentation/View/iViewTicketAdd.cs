using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewTicketAdd : iViewBase
    {
        void InitView(
            Domain.DTO.DTO_AddInit Values,
            List<DomainModel.Repository.RepositoryAttachmentUploadActions> actions,
            DomainModel.Repository.RepositoryAttachmentUploadActions dAction,
            lm.Comol.Core.FileRepository.Domain.ModuleRepository rPermissions,
            long idMessage);

        Int64 SelectedCatId { get; }

        Int64 CurrentTicketId { get; set; }

        void ShowError(Domain.Enums.TicketCreateError Error);

        void TicketCreated(Int64 TicketId, Boolean IsDraft);

        void UpdateCommunity(Int32 NewComId, String NewComName);

        void refreshCategory(IList<Domain.DTO.DTO_CategoryTree> Categories, Int64 SelectedCateId);

        void ShowCantCreate(Domain.Enums.CantCreate Info);

        String GetDraftTitle();
        String GetDraftPreview();
        ///// <summary>
        ///// ID comunità della view, che deriva da:
        ///// Se omesso restituisce -1
        ///// </summary>
        //Int32 ViewCommunityId { get; set; }
        ////To iView Base Internal
        //void DisplaySessionTimeout(Int32 CommunityId);

        //void SendAction(
        //    ModuleTicket.ActionType Action,
        //    Int32 idCommunity,
        //    ModuleTicket.InteractionType Type,
        //    IList<KeyValuePair<Int32, String>> Objects = null);

        /// <summary>
        /// Effettua l'upload effettivo e carica i file...
        /// </summary>
        /// <returns></returns>
         List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> GetUploadedItems(Domain.Message draftMessage,DomainModel.Repository.RepositoryAttachmentUploadActions action);

    
        Int64 DraftMsgId { get; set; }


        void InitPersonSelector(List<Int32> hidePersonId, int communityId);


        void ShowBehalfError(Domain.Enums.BehalfError behalfError);

        void GotoNewTicketCreated(String newTicketCODE);

        /// <summary>
        /// Inizializza il selettore comunità
        /// </summary>
        /// <param name="forAdministration">SE è un amministratore di piattaforma.</param>
        /// <param name="curretPersonId">Person ID corrente</param>
        /// <param name="unloadIdCommunities">Comunità selezionata</param>
        /// <param name="availability">SE caricarle tutte (admin) o solo quelle a cui si è iscritti (utente)</param>
        /// <param name="requiredPermissions">NON IN USO: per filtrare in base a permessi specifici su specifici servizi.</param>
        void InitializeCommunitySelector(
            Boolean forAdministration,
            int curretPersonId,
            List<Int32> unloadIdCommunities,
            CommunityManagement.CommunityAvailability availability,
            Dictionary<int, long> requiredPermissions);
    }
}
