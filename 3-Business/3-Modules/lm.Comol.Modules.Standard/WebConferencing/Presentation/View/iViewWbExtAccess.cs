using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;


namespace lm.Comol.Modules.Standard.WebConferencing.View
{
    public interface iViewWbExtAccess : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// Codice che identifica la stanza o la coppia stanza/utente
        /// </summary>
        string UrlCode { get; }

        /// <summary>
        /// ID della stanza, per uso "interno". In tal caso UrlCode = ""!
        /// </summary>
        Int64 RoomId { get; }

        /// <summary>
        /// Codice utente x login (Chiave)
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Mail utente x login
        /// </summary>
        string Mail { get; }

        /// <summary>
        /// Recupera i parametri "di sistema" di una piatatforma di VideoConferenza.
        /// </summary>
        /// <returns>
        /// I parametri di una data piattaforma...
        /// </returns>
        Domain.WbSystemParameter SysParameter { get; }

       
        /// <summary>
        /// Visualizzazione messaggio di errore
        /// </summary>
        /// <param name="ErrorCode">Codice errore che sarà internazionalizzato</param>
        void ShowError(Domain.ErrorExtAccess ErrorCode);
        /// <summary>
        /// Mostra la conferenza
        /// </summary>
        /// <param name="Url">Url da mostrare nell'IFrame della conferenza</param>
        void ShowConference(String Url);
        /// <summary>
        /// Mostra la schermata di accesso (login stanza)
        /// </summary>
        void ShowAccess();
        /// <summary>
        /// Mostra stato sottoscrizione
        /// </summary>
        /// <param name="Status"></param>
        void ShowSubStatus(Domain.ExtSubscriptionStatus Status);
        /// <summary>
        /// Imposta cosa mostrare nella view
        /// </summary>
        /// <param name="Info">Se mostrare le informazioni della stanza. SEMPRE TRUE.</param>
        /// <param name="Login">Se mostrare la LOGIN. External SEMPRE True.</param>
        /// <param name="Subscribe">Se mostrare pannello iscrizioni. A seconda delle impostazioni di Room.</param>
        /// <param name="Conference">Mostra la Conference. Le precedenti andranno a FALSE!</param>
        /// <param name="Error">Se mostrare eventuali errori. Le precedenti satanno a FALSE!</param>
        void Show(Boolean Info, Boolean Login, Boolean Subscribe, Boolean Conference, Boolean Error);

        /// <summary>
        /// Imposta i parametri stanza (titolo e descrizione)
        /// </summary>
        Domain.WbRoom Room {set; }

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
        /// Solo per pagina esterna.
        /// Comunica i dati per il bind della relativa skin.
        /// </summary>
        /// <param name="ModuleId">ID modulo</param>
        /// <param name="CommunityId">ID Comunità</param>
        /// <param name="OrganizationId">ID Organizzazione</param>
        void BindSkin(int ModuleId, int CommunityId, int OrganizationId);


        /// <summary>
        /// Dati per configurazione mail, traduzioni tag e dati sistema
        /// </summary>
        /// <remarks>
        /// Tolto momentaneamente l'ID Lingua...
        /// </remarks>
        Domain.DTO.DTO_MailTagSettings GetMailTagSetting(); // { get; }
    }
}
