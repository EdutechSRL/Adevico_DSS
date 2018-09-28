using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;


namespace  lm.Comol.Modules.Standard.WebConferencing.View
{
    public interface iViewWbCreate : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// Inizializza la creazione.
        /// </summary>
        /// <param name="Room">Parametri standard per ROOM</param>
        /// <param name="eWAvParameters">Eventuali parametri estesi (eWorks)</param>
        void Init(
            Domain.DTO.DTO_GenericRoomData DefRoomData, 
            Domain.WbRoomParameter DefParameters, 
            Domain.eWorks.DTO.DTOAvailableParameters eWAvParameters);

        /// <summary>
        /// Recupera i parametri "di sistema" di una piatatforma di VideoConferenza.
        /// </summary>
        /// <returns>
        /// I parametri di una data piattaforma...
        /// </returns>
        Domain.WbSystemParameter SysParameter { get; }

        /// <summary>
        /// Recupera i dati generici
        /// </summary>
        Domain.DTO.DTO_GenericRoomData GenericData { get; }
        /// <summary>
        /// Recupera i dati avanzati
        /// </summary>
        Domain.WbRoomParameter Parameters { get; }
        /// <summary>
        /// Recupera il TIPO stanza
        /// </summary>
        Domain.RoomType RoomType { get; }

    #region Visualizzazioni
        /// <summary>
        /// Sessione scaduta
        /// </summary>
        void DisplaySessionTimeout();
        /// <summary>
        /// Permessi insufficienti
        /// </summary>
        void DisplayNoPermission();
        /// <summary>
        /// Attiva la sola creazione della chat
        /// </summary>
        void DisplayOnlyChat();
        /// <summary>
        /// Mostra il ballot screen iniziale per la scelta del tipo di stanza
        /// </summary>
        void DisplayBallot();
        /// <summary>
        /// Server esterno non raggiungibile
        /// </summary>
        void DisplayNoServer();
    #endregion
        
        /// <summary>
        /// ID comunità della view, che deriva dall'URL.
        /// Se omesso restituisce -1
        /// </summary>
        Int32 ViewCommunityId { get; set; }

        /// <summary>
        /// Invio azioni utente
        /// </summary>
        /// <param name="CommunityID">ID Comunità corrente (o stanza)</param>
        /// <param name="ModuleID">ID Modulo</param>
        /// <param name="action">Azione</param>
        void SendUserAction(
                    int CommunityID,
                    int ModuleID,
                    lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType action);
    }


}
