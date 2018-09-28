using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;


namespace lm.Comol.Modules.Standard.WebConferencing.View
{
    public interface iViewWbEdit : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// Id della stanza corrente
        /// </summary>
        Int64 RoomId { get; set; }

        /// <summary>
        /// Recupera i parametri "di sistema" di una piatatforma di VideoConferenza.
        /// </summary>
        /// <returns>
        /// I parametri di una data piattaforma...
        /// </returns>
        Domain.WbSystemParameter SysParameter { get; }

        /// <summary>
        /// Tutti i dati della stanza corrente
        /// </summary>
        Domain.DTO.DTO_GenericRoomData CurrentRoomData { get; set; }
        /// <summary>
        /// Parametri avanzati stanza
        /// </summary>
        Domain.WbRoomParameter CurrentRoomParameters { get; set; }

        bool SYS_HasIdInName { get; }
        
        /// <summary>
        /// Imposta tipo stanza (solo visualizzazione)
        /// </summary>
        Domain.RoomType RoomType { set; }

        /// <summary>
        /// Inizializza la View
        /// </summary>
        /// <param name="eWAvParameters">Parametri avanzati eWorks</param>
        /// <param name="LockUsers">Blocca utenti (in base a permessi, per chat)</param>
        /// <param name="RoomCode">Codice stanza</param>
        void Init(Domain.eWorks.DTO.DTOAvailableParameters eWAvParameters, bool LockUsers, string RoomCode, int CommunityId, Int64 RoomId);

        /// <summary>
        /// Imposta tutti i parametri della VIEW (da sostituire a Init)
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="OwnerId">Id Person proprietario stanza</param>
        /// <param name="Data">Dati generici stanza</param>
        /// <param name="Parameter">Dati avanzati stanza</param>
        /// <param name="Type">Tipo stanza</param>
        /// <param name="Users">Lista utenti stanza</param>
        /// <param name="LockUsers">Per stanza di tipo CHAT (utenti non modificabili)</param>
        /// <param name="RoomCode">Codice stanza</param>
        void SetParameter(
            Int64 RoomId,
            Int32 OwnerId,
            Domain.DTO.DTO_GenericRoomData Data, 
            Domain.WbRoomParameter Parameter, 
            Domain.RoomType Type,
            IList<Domain.WbUser> Users,
            bool LockUsers,
            string RoomCode);

        /// <summary>
        /// Mostra sessione scaduta
        /// </summary>
        void DisplaySessionTimeout();
        /// <summary>
        /// Mostra permessi insufficienti
        /// </summary>
        void DisplayNoPermission();
        /// <summary>
        /// Mostra server non raggiungibile
        /// </summary>
        void DisplayNoServer();

        /// <summary>
        /// Mostra che la mail non è stata spedita
        /// </summary>
        void DisplayMailNotSended();

        /// <summary>
        /// Nel caso di aggiunta utenti esterni, mostra gli utenti che non sono stati inseriti
        /// </summary>
        /// <param name="ErrUsers">Elenco utenti non aggiunti</param>
        /// <remarks>
        /// MANCA gestione errori (per tipologia)
        /// RIVEDERE controlli (mail)
        /// </remarks>
        void ShowErrUsers(IList<Domain.DTO.DTO_ExtUser> ErrUsers);

        /// <summary>
        /// Per impostazione modalità accesso in relazione ai vari tipi
        /// </summary>
        /// <remarks>
        /// Al momento SOLO quelli "si sistema": Comunità, Sistema, Esterni, ma potranno essere estesi con le varie personalizzazioni
        /// </remarks>
        IList<Domain.DTO.DTO_AccessType> RoomAccessTypes { get; set; }

        /// <summary>
        /// ID comunità della view, che deriva da:
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

        /// <summary>
        /// Lingue disponibili nel sistema (attive, in base a configurazione in TopBar)
        /// </summary>
        IDictionary<int, string> SystemLanguages { get; }


        /// <summary>
        /// filtri per lista utenti
        /// </summary>
        Domain.DTO.DTO_UserListFilters UserFilters { get; }

        /// <summary>
        /// Dati per configurazione mail, traduzioni tag e dati sistema
        /// </summary>
        /// <remarks>
        /// Tolto momentaneamente l'ID Lingua...
        /// </remarks>
        Domain.DTO.DTO_MailTagSettings GetMailTagSetting(); // { get; }

        /// <summary>
        /// Paginatore Utenti
        /// </summary>
        lm.Comol.Core.DomainModel.PagerBase UserPager { get; set; }
        int UserDefaultPageSize { get; set; }

        IList<Int64> GetSelectedUsers();
    }
}
