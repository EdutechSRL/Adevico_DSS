using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.WebConferencing.View
{
    public interface iViewWbList : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// Recupera i parametri "di sistema" di una piatatforma di VideoConferenza.
        /// </summary>
        /// <returns>
        /// I parametri di una data piattaforma...
        /// </returns>
        Domain.WbSystemParameter SysParameter { get; }

        /// <summary>
        /// Fa il bind dei dati.
        /// </summary>
        /// <param name="Rooms">Elenco stanze a cui l'utente può almeno accedere</param>
        /// <param name="CanAdd">Se l'utente corrente può aggiungere stanze nel contesto corrente</param>
        /// <param name="CanModify">Se l'utente può modificare stanze nel contesto corrente</param>
        /// <param name="UserId">Id Utente (Per identificare le stanze che lui ha creato e che quindi può modificare)</param>
        void BindList(IList<Domain.WbRoom> Rooms, Boolean CanAdd, Boolean CanModify, Int32 UserId);

        /// <summary>
        /// Mostra errori nella pagina
        /// </summary>
        /// <param name="Message">Tipo di messaggio per internazionalizzazione</param>
        /// <param name="HideContent">Se nascondere o meno il contenuto della pagina (lista/pulsanti)</param>
        void ShowErrorOnPage(Domain.ErrorListMessage Message, Boolean HideContent);

        /// <summary>
        /// Invio azioni utente
        /// </summary>
        /// <param name="CommunityID">ID Comunità corrente (o stanza)</param>
        /// <param name="ModuleID">ID Modulo</param>
        /// <param name="action">Azione</param>
        void SendUserAction(
            int CommunityID, int ModuleID,
            lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType action);

        /// <summary>
        /// ID comunità della view, che deriva da:
        /// Se omesso restituisce -1
        /// </summary>
        Int32 ViewCommunityId { get; set; }

        /// <summary>
        /// Mostra sessione scaduta
        /// </summary>
        void DisplaySessionTimeout();

        /// <summary>
        /// Filtri lista
        /// </summary>
        Domain.DTO.DTO_RoomListFilter Filters { get; }

        /// <summary>
        /// Paginatore
        /// </summary>
        lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
        int DefaultPageSize { get; set; }

        //bool ShowUpdateRoomRecording { get; set; }
    }
}