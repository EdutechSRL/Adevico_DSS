using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewTicketListExt : iViewBase
    {
        /// <summary>
        /// Inizializzazione filtri
        /// </summary>
        /// <param name="Filters">Valori presentati nei selettori dei filtri</param>
        void InitFilters(Domain.DTO.DTO_ListInit Filters);


        /// <summary>
        /// Imposta l'elenco dei Ticket
        /// </summary>
        /// <param name="Tickets">Elenco dei Ticket in base ai parametri indicati</param>
        /// <param name="PageIndex">Indice corrente. Reimpostato a zero se "out of range"</param>
        /// <param name="RecordTotal">Numero totale record, per paginatore</param>
        void SetTickets(List<Domain.DTO.DTO_TicketListItemUser> Tickets, int PageIndex, int RecordTotal, bool ShowManagement, bool HasCurrentDraft);

        /// <summary>
        /// Mostra informazioni sui ticket...
        /// </summary>
        /// <param name="Info">ticket aperti, chiusi, etc...</param>
        void SetInfo(Domain.DTO.DTO_ListInfo Info);

        Domain.DTO.DTO_User CurrentUser { get; }

        //Boolean CheckSession();

        //void SendAction(
        //    ModuleTicket.ActionType Action,
        //    Int32 idCommunity,
        //    ModuleTicket.InteractionType Type,
        //    IList<KeyValuePair<Int32, String>> Objects = null);
        void ShowDeletMessage(Domain.Enums.TicketDraftDeleteError Error);

        String GetBasePath();

        Domain.DTO.DTO_ListFilterUser GetFilters();
    }
}
