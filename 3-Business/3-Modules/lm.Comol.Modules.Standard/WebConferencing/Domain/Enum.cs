using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{

    ///// <summary>
    ///// Stato dell'invito rispetto all'utente.
    ///// Rivedere i nomi
    ///// Mi pare non sia manco stato usato...
    ///// </summary>
    //public enum WBInvitationUserStatus
    //{
    //    /// <summary>
    //    /// Non inviato (bozza)
    //    /// </summary>
    //    NotSended,
    //    /// <summary>
    //    /// non letto   (solo recapito interno, no mail?)
    //    /// </summary>
    //    NotReaded,
    //    /// <summary>
    //    /// letto       (solo recapito interno, no mail?)
    //    /// </summary>
    //    Readed,
    //    /// <summary>
    //    /// Presenza confermata
    //    /// </summary>
    //    Confirmed,
    //    /// <summary>
    //    /// Presenza rifiutata
    //    /// </summary>
    //    Refused,
    //    /// <summary>
    //    /// Presenza non confermata o sconosciuta...
    //    /// </summary>
    //    Unknow
    //}

    #region Type

    /// <summary>
    /// Available Room Type.
    /// Some parameters are based on this.
    /// </summary>
    [Serializable()]
    public enum RoomType
    {
        /// <summary> 1-1 </summary>
        VideoChat = 1,
        /// <summary> Max 4-5 </summary>
        Meeting = 2,
        /// <summary> One to many </summary>
        Lesson = 3,
        /// <summary> One manager, some speaker, many users</summary>
        Conference = 4,
        /// <summary> Can be configured </summary>
        Custom = -1
    }


    /// <summary>
    /// Tipi di iscrizione a livello di sistema.
    /// Potranno essere integrati con quelli personali
    /// </summary>
    [Serializable()]
    public enum SysSubscriptionType
    {
        /// <summary>
        /// Per utenti comunità
        /// </summary>
        Community,
        /// <summary>
        /// Per iscritti al sistema
        /// </summary>
        System,
        /// <summary>
        /// Utenti esterni
        /// </summary>
        External
    }

    /// <summary>
    /// Permessi iscrizioni utenti (sistema o personali)
    /// </summary>
    [Serializable()]
    public enum SubscriptionType
    {
        /// <summary>
        /// Non è consentita l'iscrizione autonoma
        /// </summary>
        None = 1,
        /// <summary>
        /// Iscrizione consentita, ma l'accesso deve essere abilitato dall'amministratore della stanza
        /// </summary>
        Moderated = 2,
        /// <summary>
        /// Iscrizione ed accesso libero
        /// </summary>
        Free = 3
    }

    #endregion

    #region CheckResults

    /// <summary>
    /// Risultato controllo mail
    /// </summary>
    [Serializable()]
    public enum MailCheck
    {
        /// <summary>
        /// Parametri insufficienti o errati
        /// </summary>
        ParameterError = -1,
        /// <summary>
        /// Mail sconosciuta (non presente nel sistema)
        /// </summary>
        MailUnknow = 0,
        /// <summary>
        /// Mail utilizzata nella stanza (lato implementazioni)
        /// </summary>
        MailInRoom = 1,
        /// <summary>
        /// Mail utilizzata nella stanza (lato dB)
        /// </summary>
        MailInRoomdB = 2,
        /// <summary>
        /// Mail presente in Comol
        /// </summary>
        MailInSystem = 3
    }

    /// <summary>
    /// Risultato controllo utente (Person)
    /// </summary>
    [Serializable()]
    public enum PersonCheck
    {
        /// <summary>
        /// Persona iscritto alla comunità
        /// </summary>
        UserInCommunity,
        /// <summary>
        /// Persona iscritto al sistema
        /// </summary>
        UserInSystem,
        /// <summary>
        /// Persona sconosciuta
        /// </summary>
        UserUnknow,
        /// <summary>
        /// Room non specificata o inesistente
        /// </summary>
        NoRoom,
        /// <summary>
        /// Comunità non specificata o inesistente
        /// </summary>
        NoCommunity
    }

    /// <summary>
    /// Risultato di un iscrizione esterna
    /// </summary>
    [Serializable()]
    public enum ExtSubscriptionStatus
    {
        /// <summary>
        /// Ok. Mail con i dati d'accesso inviata correttamente.
        /// </summary>
        MailSended,
        /// <summary>
        /// In attesa di conferma da parte dell'amministratore
        /// </summary>
        RequestSended,
        /// <summary>
        /// Non è permessa l'iscrizione autonoma
        /// </summary>
        NoPermission,
        /// <summary>
        /// Parametri mancanti o errati (Es. Room code)
        /// </summary>
        ParametersError,
        /// <summary>
        /// Mail già presente nella stanza (non è possibile reiscriversi)
        /// </summary>
        MailInRoom,
        /// <summary>
        /// Mail già presente nel sistema (è necessario login per iscriversi)
        /// </summary>
        MailInSystem,
        /// <summary>
        /// Errore interno (dB o altro)
        /// </summary>
        InternalError
    }
    
    #endregion

    #region Views
    
    /// <summary>
    /// Passi per l'ADD, versione Wizard completo.
    /// </summary>
    /// <remarks>
    /// In parte obsoleto, dopo semplificazioni Wizard creazione:
    /// rimangono solo quelli con V
    /// </remarks>
    [Serializable()]
    public enum WBAddStep
    {
        /// <summary>
        /// V Ballot Screen selezione utenti
        /// </summary>
        ModeSelect,
        /// <summary>
        /// V Dati generici (indipendenti da implementazioni)
        /// </summary>
        GenericData,
        /// <summary>
        /// Dati specifici (in base alle implementazioni)
        /// </summary>
        SpecificData,
        /// <summary>
        /// V Selezione utenti
        /// </summary>
        UserSelect,
        /// <summary>
        /// Inviti
        /// </summary>
        Invitation,
        /// <summary>
        /// Conferma
        /// </summary>
        Confirm,
        /// <summary>
        /// Stanza creata
        /// </summary>
        Done
    }

    /// <summary>
    /// Schermate di modifica
    /// </summary>
    [Serializable()]
    public enum EditViews
    {
        /// <summary>
        /// Dati generici (indipendenti dalla piattaforma)
        /// </summary>
        DataGeneric = 0
        /// <summary>
        /// Dati avanzati (dipendenti dalla piattaforma)
        /// </summary>
        ,DataAdvance = 1
        /// <summary>
        /// Gestione utenti
        /// </summary>
        ,Users = 2
        ///// <summary>
        ///// Gestione inviti utenti iscritti
        ///// </summary>
        //UsersInvitation = 3,
        /// <summary>
        /// Gestione accesso esterno (Url accesso e permessi iscrizioni)
        /// </summary>
        ,ExternalAccess = 6
        ///// <summary>
        ///// Gestione inviti generici (Solo URL)
        ///// </summary>
        //,GenericInvitation = 7
    }

    #endregion

    #region Errors

    /// <summary>
    /// Errori di accesso esterno
    /// </summary>
    [Serializable()]
    public enum ErrorExtAccess
    {
        /// <summary>
        /// Nessun errore
        /// </summary>
        none,
        /// <summary>
        /// Server esterno non raggiungibile
        /// </summary>
        NoServer,
        /// <summary>
        /// Stanza non riconosciuta: chiave non valida/modificata/cancellata, stanza  cancellata 
        /// </summary>
        UnknowRoom,
        /// <summary>
        /// Errore interno sottoscrizione
        /// </summary>
        InternalSubScriptionError,
        /// <summary>
        /// Iscrizione avvenuta, ma necessaria autorizzazione amministratore per l'accesso.
        /// </summary>
        AdminConfirmRequired,
        /// <summary>
        /// Non è permesso l'accesso/iscrizione
        /// </summary>
        NoPermission,
        /// <summary>
        /// Chiave utente errata (errore trascrizione, chiave modificata o cancellata)
        /// </summary>
        WrongUserKey,
        /// <summary>
        /// Errore invio mail conferma iscrizione.
        /// </summary>
        MailSenderError,
        /// <summary>
        /// I template di sistema non sono ancora definiti.
        /// </summary>
        NoTemplate
    }
    
    /// <summary>
    /// Lista, errori visualizzazione
    /// </summary>
    [Serializable()]
    public enum ErrorListMessage
    {
        /// <summary>
        /// Sessione scaduta
        /// </summary>
        NoSession = 0,
        /// <summary>
        /// Permessi insufficienti
        /// </summary>
        NoPermission = 1,
        /// <summary>
        /// Nessuna stanza presente in lista
        /// </summary>
        NoItem = 3,
        /// <summary>
        /// Server esterno non raggiungibile
        /// </summary>
        NoServer = -1
    }
    
    /// <summary>
    /// Errori possibili nell'inserimento multiplo di utenti esterni
    /// </summary>
    [Serializable()]
    public enum ErrorAddExtUser
    {
        /// <summary>
        /// Utente inserito, nessun errore
        /// </summary>
        none,
        /// <summary>
        /// Mail presente nel sistema
        /// </summary>
        MailInSystem,
        /// <summary>
        /// Mail presente nella stanza
        /// </summary>
        MailInRoom,
        /// <summary>
        /// Errore interno
        /// </summary>
        InternalError,
        /// <summary>
        /// Parametri insufficienti o errati
        /// </summary>
        ParameterError,
        /// <summary>
        /// Formato mail non valido
        /// </summary>
        MailFormatError
    }
    
    #endregion

#region RoomList
    public enum RoomListFilterAccess
    {
        /// <summary>
        /// Tutte
        /// </summary>
        all,
        /// <summary>
        /// A cui ho accesso (sono iscritto, mi posso iscrivere e non sono bloccato)
        /// </summary>
        access,
        /// <summary>
        /// A cui sono iscritto
        /// </summary>
        subscribed,
        /// <summary>
        /// Create da me
        /// </summary>
        created
    }

    public enum RoomListFilterType
    {
        /// <summary>
        /// Tutte
        /// </summary>
        all,
        /// <summary>
        /// Solo le Chat
        /// </summary>
        Chat,
        /// <summary>
        /// Solo i Meetings
        /// </summary>
        Meetings,
        /// <summary>
        /// Solo le lezioni
        /// </summary>
        Lesson,
        /// <summary>
        /// Solo le Conference
        /// </summary>
        Conference,
        /// <summary>
        /// Solo le Custom
        /// </summary>
        Custom
    }

    public enum RoomListFilterVisibility
    {
        /// <summary>
        /// Tutte
        /// </summary>
        all,
        /// <summary>
        /// Solo le pubbliche
        /// </summary>
        Public,
        /// <summary>
        /// Solo le private
        /// </summary>
        Private
    }

    public enum RoomListOrder
    {
        /// <summary>
        /// Nome stanza
        /// </summary>
        Name,
        /// <summary>
        /// Tipo stanza
        /// </summary>
        Type,
        ///// <summary>
        ///// Utenti
        ///// </summary>
        //Users,
        /// <summary>
        /// StartDate
        /// </summary>
        Start,
        /// <summary>
        /// Status (Public/Private)
        /// </summary>
        Status
    }

#endregion


#region UserList

    public enum UserListSearchBy
    {
        Name,
        Surename,
        Mail
    }

    public enum UserListOrderBy
    {
        Name,
        SureName,
        Mail
    }

    public enum UserListUsertype
    {
        all = -1,
        system = 1,
        external = 2
    }
#endregion

#region "User Filters For Message"

    /// <summary>
    /// Ordinamento utenti
    /// </summary>
    [Serializable()]
    public enum UserByMessagesOrder
    {
        None = 0,
        ByMail = 1,
        ByName = 2,
        BySurname = 3,
        ByType = 4,
        ByMessageNumber = 5,
        ByAgency = 6,
        ByStatus = 7
    }

    /// <summary>
    /// Filtro in base alla tipologia di utenti
    /// </summary>
    [Serializable()]
    public enum UserTypeFilter
    {
        None = 0,
        Participant = 1,
        Administrator = 2,
        GenericMail = 3,
        WithoutMembers = 4,
        ExternalParticipant = 5,
        InternalParticipant = 6,
        All = 7
    }

    /// <summary>
    /// Status della mail
    /// </summary>
    [Serializable()]
    public enum MailStatus
    {
        None = 0,
        Confirmed = 1,
        WaitingConfirm = 2,
        All = 3
    }
    /// <summary>
    /// Status della mail
    /// </summary>
    [Serializable()]
    public enum UserStatus
    {
        None = 0,
        Locked = 1,
        Unlocked = 2,
        NotSubscribed = 3,
        All = 4
    }

    public enum ColumnMessageGrid
    {
        AgencyName = 0,
        UserStatus = 1,
        Actions = 2,
    }
#endregion
}