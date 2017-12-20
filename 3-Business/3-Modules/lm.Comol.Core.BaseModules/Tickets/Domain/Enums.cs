using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.Enums
{
    /// <summary>
    /// Tipo categoria (visibilità)
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public enum CategoryType
    {
        /// <summary>
        /// Globale
        /// </summary>
        Public = 0,
        /// <summary>
        /// Solo x comunità di tipo Ticket
        /// </summary>
        Ticket = 1,
        /// <summary>
        /// Solo per comunità corrente
        /// </summary>
        Current = 2
    }

    [Serializable(), CLSCompliant(true)]
    public enum CategoryTreeMessageType
    {
        /// <summary>
        /// Nasconde il messaggio
        /// </summary>
        none,
        /// <summary>
        /// Non si dispone dei permessi...
        /// </summary>
        NoPermission,
        /// <summary>
        /// Accesso disabilitato
        /// </summary>
        NoAccess,
        /// <summary>
        /// Non sono presenti categorie
        /// </summary>
        NoCategory,
        /// <summary>
        /// Non è stato effettuato alcun riordino
        /// </summary>
        NoReorder,
        /// <summary>
        /// Categoria di default reimpostata come prima in lista
        /// </summary>
        DefaultReorderWarning,
        /// <summary>
        /// Salvataggio avvenuto con successo
        /// </summary>
        Saved,
        /// <summary>
        /// Errore salvataggio dati!
        /// </summary>
        UnSaved,
        /// <summary>
        /// Notifiche: inviate
        /// </summary>
        MessageSend,
        /// <summary>
        /// Notifiche: disabilitate
        /// </summary>
        MessageUnsend
    }

    [CLSCompliant(true), Serializable]
    public enum CategoryReorderResponse
    {
        Success,
        NoDefaultReorder,
        Error
    }

    [CLSCompliant(true), Serializable]
    public enum CategoryTREEgetType
    {
        System,
        Creation,
        FilterUser,
        FilterManager
    }
    /// <summary>
    /// Stato di un Ticket (ciclo di vita)
    /// </summary>
    [CLSCompliant(true)]
    public enum TicketStatus : int
    {
        /// <summary>
        /// Ticket in bozza. Creato, ma non inviato: visibile SOLO dal mittente.
        /// </summary>
        draft = 0,
        /// <summary>
        /// Ticket aperto. Così anche se creato in DRAFT.
        /// </summary>
        open = 10,
        /// <summary>
        /// In lavorazione. Assegnato o preso in carico da qualcuno.
        /// </summary>
        inProcess = 11,
        /// <summary>
        /// In attesa di risposta da parte dell'utente
        /// </summary>
        userRequest = 12,
        ///// <summary>
        ///// L'utente ha richiesto la chiusura del ticket
        ///// </summary>
        //closeRequest = 4,
        /// <summary>
        /// Chiuso con successo.
        /// </summary>
        closeSolved = 21,
        /// <summary>
        /// Chiuso senza successo
        /// </summary>
        closeUnsolved = 22
    }

    [CLSCompliant(true)]
    public enum TicketUserDateFilter
    {
        Creation,
        LastModify
    }

    [CLSCompliant(true)]
    public enum TicketManagerDateFilter
    {
        Creation,
        LastModify,
        LastAssignment,
        LastMessage
    }

    /// <summary>
    /// Per filtro lista manager/resolver
    /// </summary>
    [Flags, CLSCompliant(true)]
    public enum TicketManagerListOnly // : short
    {
        //Whit cool ninja-tricks: bit shift!
        /// <summary>
        /// Nessun filtro: tutti
        /// </summary>
        None = 0,
        /// <summary>
        /// SOLO Ticket che non hanno ricevuto risposte da manager/resolver
        /// </summary>
        noanswers = 1 << 0, //0x1
        /// <summary>
        /// SOLO Ticket che non sono stati assegnati
        /// </summary>
        notassigned = 1 << 1, //0x2
        /// <summary>
        /// SOLO Ticket aggiornati dall'ultimo accesso utente
        /// </summary>
        withnews = 1 << 2 //0x3
    }

    ///// <summary>
    ///// Indica le proprietà del Ticket che causano notifica in caso di modifica
    ///// </summary>
    ///// <remarks>
    ///// Implementata nuova modalità su più livelli:
    ///// 0. Hard-Coded:      nessuna notifica
    ///// 1. Sistema:         impostazioni globali Ticket
    ///// 2. Generico Utente: impostazioni generiche dell'utente
    ///// 4. Ticket:          a livello di TICKET per il singolo utente (manager/resolver/user)
    ///// </remarks>
    //[Flags]
    //[CLSCompliant(true)]
    //public enum MailSettings
    //{
    //    /// <summary>
    //    /// Indica di utilizzare le impostazioni di default (livello superiore)
    //    /// 0 è riservato == nessuna notifica!
    //    /// </summary>
    //    Default = -1,
    //    ///// <summary>
    //    ///// Imposta tutto
    //    ///// </summary>
    //    //All = 63,
    //    /// <summary>
    //    /// Stato Ticket (aperto, in attesa, ...)
    //    /// </summary>
    //    Status = 2,
    //    /// <summary>
    //    /// Riassegnazione a categoria (SOLO Manager/Resolver, per l'utente è sempre quella di creazione)
    //    /// </summary>
    //    Category = 4,
    //    /// <summary>
    //    /// Aggiunta risposta
    //    /// </summary>
    //    Answer = 8,
    //    /// <summary>
    //    /// Modifiche/aggiunte ai file (non previsto, file legati a messaggi)
    //    /// </summary>
    //    File = 16,
    //    /// <summary>
    //    /// Riassegnazione ad utente (SOLO Manager/Resolver)
    //    /// </summary>
    //    Assignments = 32,

    //    ManResNewTicket = 64,

    //}

    [Flags]
    [CLSCompliant(true)]
    public enum MailSettings : long
    {
        //Uso interno: indica di NON considerare il dato.
        DISABLED = -2,
        /// <summary>
        /// Indica di utilizzare le impostazioni di default (livello superiore)
        /// 0 è riservato == nessuna notifica!
        /// </summary>
        Default = -1,   //VErrà tolto. Al salvataggio usare UserDefault 1 << 6 oppure ManResDefault 1 << 22

        none                    = 0,

        //User (Owner + Creator)
        NewTicketUsr            = 1 << 1,
        NewMessageUsr           = 1 << 2,
        StatusChangedUsr        = 1 << 3,
        ModerationChangedUsr    = 1 << 4,
        TicketResetAssUsr       = 1 << 5,
        UserDefault             = 1 << 6,

        //Creator (behalfer)
        OwnerChanged            = 1 << 8,

        //Assigner
        TicketNewAssignmentAss  = 1 << 10,

        //Manager/Resolver
        NewTicketManager        = 1 << 15,
        NewMessageManager       = 1 << 16,
        StatusChangedManager    = 1 << 17,
        ModerationChangedMan    = 1 << 18,
        TicketAssCategoryMan    = 1 << 19,
        TicketNewAssignmentMan  = 1 << 20,
        TicketResetAssMan       = 1 << 21,
        ManResDefault           = 1 << 22

        //Follower : TOLTI perchè sono le stesse di Man/Res
        //TicketAssCategoryMy
        //NewMessageFollower
        //StatusChangedFollower
        //ModerationChangedFol
        //TicketAssCategoryFol
        //TicketResetAssFol

        //Category Management
        //Intanto tralasciate: probabilmente inviate da Admin Comunità a tutti.
        
        //External : non serve! (X Registrazione: inviate by-system sempre e comunque!)

    }

    /// <summary>
    /// Campo Ticket di ordinamento lista Ticket x Utenti normali
    /// </summary>
    [CLSCompliant(true)]
    public enum TicketOrderUser
    {
        /// <summary>
        /// Per codice (inserimento/unorder)
        /// </summary>
        code = 0,
        /// <summary>
        /// Per Oggetto (titolo)
        /// </summary>
        subject = 1,
        /// <summary>
        /// Per categoria
        /// </summary>
        category = 2,
        /// <summary>
        /// tempo apertura/creation
        /// </summary>
        lifeTime = 3,
        /// <summary>
        /// Ultima modifica
        /// </summary>
        lastModify = 4,
        /// <summary>
        /// Stato
        /// </summary>
        status = 5,
        /// <summary>
        /// Owner (behalf)
        /// </summary>
        behalf = 6
    }

    /// <summary>
    /// Campo Ticket di ordinamento lista Ticket x Manager/Resolver
    /// </summary>
    [CLSCompliant(true)]
    public enum TicketOrderManRes
    {
        /// <summary>
        /// Per codice (inserimento/unorder)
        /// </summary>
        code = 0,
        /// <summary>
        /// Per Oggetto (titolo)
        /// </summary>
        subject = 1,
        /// <summary>
        /// Per lingua
        /// </summary>
        language = 2,
        /// <summary>
        /// Per comunità
        /// </summary>
        community = 3,
        /// <summary>
        /// Per associazione utente
        /// </summary>
        association = 4,
        /// <summary>
        /// Per categoria
        /// </summary>
        category = 5,
        /// <summary>
        /// tempo apertura/creation
        /// </summary>
        lifeTime = 6,
        /// <summary>
        /// Ultima modifica
        /// </summary>
        lastModify = 7,
        /// <summary>
        /// Stato
        /// </summary>
        status = 8
    }

    /// <summary>
    /// Campo per filtro per range di date sulla lista ticket di Manager o Resolver.
    /// </summary>
    [CLSCompliant(true)]
    public enum TicketDateFilter
    {
        /// <summary>
        /// Data invio (creazione in draft)
        /// </summary>
        send = 1,
        /// <summary>
        /// Assegnazione
        /// </summary>
        assignment = 2,
        /// <summary>
        /// Richiesta chiusura
        /// </summary>
        closeRequest = 4,
        /// <summary>
        /// Chiusura
        /// </summary>
        close = 8
    }

    /// <summary>
    /// Possibili errori in fase di creazione o modifica di una categoria
    /// </summary>
    [CLSCompliant(true)]
    public enum CategoryAddModifyError
    {
        /// <summary>
        /// nessun errore
        /// </summary>
        none = 0,
        /// <summary>
        /// Categoria padre con nome esistente
        /// </summary>
        fatherName = 1,
        /// <summary>
        /// Categoria padre senza Manager
        /// </summary>
        fatherNoManager = 2,
        /// <summary>
        /// Errore interno (category == null)
        /// </summary>
        noData = -1,
        /// <summary>
        /// Manca il nome della category
        /// </summary>
        noName = 3,
        /// <summary>
        /// Indica che si tenta di modificare lo stato "Pubblico" di una categoria di DEFAULT!
        /// </summary>
        isDefault = 4
    }

    /// <summary>
    /// Errore impostazioni risorse
    /// </summary>
    [CLSCompliant(true)]
    public enum CategoryAssignersError
    {
        /// <summary>
        /// nessun errore
        /// </summary>
        none,
        /// <summary>
        /// Non è stato impostato alcun manager (roll-back).
        /// </summary>
        noManager,
        /// <summary>
        /// Non è stato impostato alcun manager e la categoria NON aveva manager:
        /// impostato un resolver precedente.
        /// </summary>
        setResolver,
        /// <summary>
        /// Non è stato impostato alcun manager e la categoria NON aveva manager:
        /// impostato utente corrente.
        /// </summary>
        setCurrent,

        deleteError
    }

    [CLSCompliant(true)]
    public enum CategoryAssignersDeleteError
    {
        /// <summary>
        /// nessun errore
        /// </summary>
        none,
        /// <summary>
        /// 
        /// </summary>
        noUsers,
        /// <summary>
        /// 
        /// </summary>
        noManger,
        
    }

    /// <summary>
    /// Errore in creazione utente esterno
    /// </summary>
    [CLSCompliant(true)]
    public enum ExternalUserCreateError
    {
        /// <summary>
        /// Nessun errore
        /// </summary>
        none = 0,
        /// <summary>
        /// Formato mail non valido
        /// </summary>
        invalidMail = 1,
        /// <summary>
        /// Mail già associata ad uno User!
        /// </summary>
        TicketMail = 2,
        /// <summary>
        /// Mail nel sistema (non errore, ma mi serve saperlo)
        /// </summary>
        internalMail = 3,
        /// <summary>
        /// Errore interno: generazione codice, errori dB, invio mail
        /// </summary>
        internalError = 4
        
    }

    /// <summary>
    /// Errore validazione utente (login esterna)
    /// </summary>
    [CLSCompliant(true)]
    public enum ExternalUserValidateError
    {
        /// <summary>
        /// tutto ok
        /// </summary>
        none = 0,
        /// <summary>
        /// Mail non valida (formato mail o mail non riconosciuta)
        /// </summary>
        invalidMail = 1,
        /// <summary>
        /// Codice non valido
        /// </summary>
        invalidCode = 2,

        TokenEmpty = 3,
        TokenInvalid = 4,
        TokenExpired = 5


    }


    [CLSCompliant(true)]
    public enum ExternalUserPasswordErrors
    {
        /// <summary>
        /// Nessun errore
        /// </summary>
        none,
        /// <summary>
        /// UNUSED: Non conforme alle policy: troppo corta, caratteri invalidi, etc...
        /// </summary>
        NoPolicy,
        /// <summary>
        /// Password iniziale non valida
        /// </summary>
        InvalidPassword,
        /// <summary>
        /// Le due nuove password non corrispondono
        /// </summary>
        PasswordNotMatch,
        /// <summary>
        /// Errore invio mail
        /// </summary>
        MailSendError,
        /// <summary>
        /// Errore generico
        /// </summary>
        GenericError,
        ///// <summary>
        ///// Utente interno: vale SOLO per MAIL e dati utente!
        ///// </summary>
        //InternalUSer,
        /// <summary>
        /// SessionTimeout
        /// </summary>
        SessionTimeout,
        /// <summary>
        /// Utente in sessione non corrisponde ad utenti nel dB! (?)
        /// </summary>
        UserNotFound,
        /// <summary>
        /// Campi mancanti
        /// </summary>
        VoidField
    }

    /// <summary>
    /// Indica quali ruoli caricare, relativamente ad una categoria
    /// </summary>
    [CLSCompliant(true)]
    public enum RolesLoad
    {
        /// <summary>
        /// Nessuno = non verrà caricato alcun ruolo
        /// </summary>
        none = 0,
        /// <summary>
        /// Carica SOLO i Manager
        /// </summary>
        Manager = 1,
        /// <summary>
        /// Carica SOLO i Resolver
        /// </summary>
        Resolver = 2,
        /// <summary>
        /// Carica tutti, sia Manager che Resolver
        /// </summary>
        all = -1
    }

    /// <summary>
    /// Errori creazione Ticket (draft o invio)
    /// </summary>
    [CLSCompliant(true)]
    public enum TicketCreateError
    {
        /// <summary>
        /// Nessun errore
        /// </summary>
        none = 0,
        ///// <summary>
        ///// Ticket non valido: non bozza, non dell'utente o non esiste
        ///// </summary>
        //invalidTicket = 1,
        /// <summary>
        /// Troppi ticket in bozza
        /// </summary>
        ToMuchDraft = 9,
        /// <summary>
        /// Troppi ticket aperti
        /// </summary>
        ToMuchTicket = 10,
        /// <summary>
        /// Permessi insufficienti
        /// </summary>
        NoPermission = 11,
        /// <summary>
        /// Categoria non selezionata
        /// </summary>
        NoCategory = 21,
        /// <summary>
        /// Titolo non impostato
        /// </summary>
        NoTitle = 22,
        /// <summary>
        /// Testo non impostato
        /// </summary>
        NoText = 23,
        /// <summary>
        /// Errore nel salvataggio (dB)
        /// </summary>
        dBUnknown = 30,

    }

    [CLSCompliant(true)]
    public enum TicketMessageSendError
    {
        none,
        NoMessage,
        TicketNotFound,
        TicketClosed,
        DraftTicket,
        NoPermission
    }

    /// <summary>
    /// Gestione errori pagina edit utente
    /// </summary>
    [CLSCompliant(true)]
    public enum TicketEditUserErrors
    {
        /// <summary>
        /// Il Ticket non appartiene all'utente corrente
        /// </summary>
        NoPermission,
        /// <summary>
        /// Ticket non trovato (ID errato)
        /// </summary>
        NotFound,
        /// <summary>
        /// E' in DRAFT: non può essere modificato da qui -> redirect su apertura
        /// </summary>
        IsDraft,
        /// <summary>
        /// Nessun errore
        /// </summary>
        none
    }

    /// <summary>
    /// Gestione errori pagina edit utente
    /// </summary>
    [CLSCompliant(true)]
    public enum TicketEditManErrors
    {
        /// <summary>
        /// Il Ticket non appartiene all'utente corrente
        /// </summary>
        NoPermission,
        /// <summary>
        /// Ticket non trovato (ID errato)
        /// </summary>
        NotFound,
        /// <summary>
        /// E' in DRAFT: non può essere modificato da qui -> redirect su apertura
        /// </summary>
        IsDraft,
        /// <summary>
        /// Nessun errore
        /// </summary>
        none
    }

    [Flags, CLSCompliant(true)]
    public enum TicketCondition : int
    {
        /// <summary>
        /// Nessun blocco sul ticket = "none"
        /// </summary>
        active = 0,
        /// <summary>
        /// Segnalato.    Il ticket può continuare il suo ITER, ma viene segnalato ai Manager
        /// </summary>
        flagged = 1,
        /// <summary>
        /// Bloccato.     Il Ticket non può essere modificato (se non da Manager/Resolver)
        /// </summary>
        blocked = 2,
        /// <summary>
        /// Segnalato e bloccato (x interfaccia visualizzazione)
        /// </summary>
        flaggedNblocked = 3,
        /// <summary>
        /// Annullato.    Il ticket non è più visibile "di dafualt" e non rientra nella conta dei Ticket.
        /// </summary>
        cancelled = 4
    }

    [CLSCompliant(true)]
    public enum TicketDraftDeleteError
    {
        /// <summary>
        /// Cancellato con successo
        /// </summary>
        none,
        /// <summary>
        /// Ticket non trovato
        /// </summary>
        TicketNotFound,
        /// <summary>
        /// Ticket NON in BOZZA
        /// </summary>
        TicketNotInDraft,
        /// <summary>
        /// Ticket di altro utente
        /// </summary>
        TicketNotMine,
        /// <summary>
        /// Errore cancellazione file
        /// </summary>
        FileError,
        /// <summary>
        /// Errore cancellazione dati Ticket
        /// </summary>
        dBError,
        /// <summary>
        /// Per nascondere il messaggio
        /// </summary>
        hide
    }


    /// <summary>
    /// Tipo di messaggio:
    /// richiesta, risposta, systema...
    /// </summary>
    /// <example>
    /// Se un MANAGER chiude il messaggio,
    /// QUI setto SYSTEM!
    /// </example>
    [CLSCompliant(true)]
    public enum MessageType : int
    {
        /// <summary>
        /// Richiesta. Di default messaggi del partecipante
        /// </summary>
        Request = 1,
        /// <summary>
        /// Risposta. Di default messaggi del manager/resolver
        /// </summary>
        FeedBack = 2,
        /// <summary>
        /// Da definire...
        /// </summary>
        PersonalFeedBack = 3,
        ///// <summary>
        ///// Da definire...
        ///// </summary>
        //Notice = 4,
        /// <summary>
        /// Di sistema: Ex "notice"
        /// </summary>
        System = 10
    }

    /// <summary>
    /// Tipo utente CREATORE del messaggio
    /// </summary>
    /// <example>
    /// Se un MANAGER chiude il messaggio,
    /// QUI setto MANAGER.
    /// </example>
    [CLSCompliant(true)]
    public enum MessageUserType : int
    {
        /// <summary>
        /// Nessun "permesso" per accedere al Ticket!
        /// </summary>
        none = -1,
        /// <summary>
        /// Partecipante: chi apre il ticket
        /// </summary>
        Partecipant = 1,
        /// <summary>
        /// Manager
        /// </summary>
        Manager = 2,
        /// <summary>
        /// Resolver
        /// </summary>
        Resolver = 3,
        /// <summary>
        /// Se "Hide" dati utente: generico
        /// </summary>
        Category = 4,
        /// <summary>
        /// Se "Hide" dati utente: manager categoria
        /// </summary>
        CategoryManager = 5,
        /// <summary>
        /// Se "Hide" dati utente: Resolver categoria
        /// </summary>
        CategoryResolver = 6,
        /// <summary>
        /// Sistema
        /// </summary>
        System = 10
    }

    /// <summary>
    /// SE al messaggio è legata un'azione, (MessageType = System)
    /// altrimenti "normal"
    /// </summary>
    [CLSCompliant(true)]
    public enum MessageActionType : int
    {
        /// <summary>
        /// Nessuna azione particolare: solo aggiunta messaggio
        /// </summary>
        normal = 0,
        /// <summary>
        /// Aggiunta file
        /// </summary>
        filesAdded = 2,
        /// <summary>
        /// Riassegnazione a categoria
        /// </summary>
        riassignedToCategory = 5,
        /// <summary>
        /// Riassegnazione ad utente
        /// </summary>
        riassignedToUser = 6,
        /// <summary>
        /// Modifica stato Ticket (chiusura, in lavorazione, riapertura, etc..., compresi automatismi)
        /// </summary>
        statusChanged = 10,
        /// <summary>
        /// Modifica stato Ticket (chiusura, in lavorazione, riapertura, etc..., compresi automatismi)
        /// </summary>
        conditionChanged = 11,
        /// <summary>
        /// Nel momento in cui un ticket è stato creato per conto di.
        /// </summary>
        behalfSet = 12,
        behalfMessage_OLD = 13,
        /// <summary>
        /// Rimozine behalf
        /// </summary>
        behalfRemove = 14
    }

    [CLSCompliant(true)]
    public enum AssignmentType : int
    {
        Category = 0,
        Manager = 1,
        Resolver = 2
    }
    //[CLSCompliant(true)]
    //public enum UserType
    //{
    //    Participant,
    //    Manager,
    //    Resolver,
    //    Category,
    //    CategoryManager,
    //    CategoryResolver,
    //    System
    //}

    //[CLSCompliant(true)]
    //public enum MessageType
    //{
    //    request,
    //    notice,
    //    feedback,
    //    personalfeedback
    //}

    [CLSCompliant(true)]
    public enum EditManResMessagesOrder
    {
        recentolder,
        oldertorecent
    }

    [CLSCompliant(true)]
    public enum EditManResMessagesShow
    {
        All,
        MessageOnly,
        NotifiesOnly
    }

    [CLSCompliant(true)]
    public enum CategoryDeleteSteps : int
    {
        Step1_Children = 1,
        Step2_Ticket = 2,
        Step3a_ReassignAll = 3,
        Step3b_ReassignSingle = 4,
        Step4_Confirm = 5,
        Step5_END = 6
    }

    [CLSCompliant(true)]
    public enum GlobalAdminStatus
    {
        none,
        SaveOK,
        ImportTypeOK,
        CategoryDefSetted,
        CategoryDefRemoved,
        InternalError,
        LimitError
    }

    [CLSCompliant(true)]
    public enum GlobalAdminSwitch
    {
        none,
        Service,
        NotificationUser,
        NotificationManager,
        CategoryManagement,
        TicketRead,
        TicketWrite,
        TicketBehalf
    }

#region Access
    ///// <summary>
    ///// Errore accesso esterno
    ///// </summary>
    //[CLSCompliant(true)]
    //public enum AccessError
    //{
    //    /// <summary>
    //    /// Nessun errore
    //    /// </summary>
    //    none,
    //    /// <summary>
    //    /// Formato mail non valido
    //    /// </summary>
    //    MailFormat,
    //    /// <summary>
    //    /// Codice non valido
    //    /// </summary>
    //    InvalidCode,
    //    /// <summary>
    //    /// KEY (Captcha) non valido
    //    /// </summary>
    //    InvalidKEY
    //}

    /// <summary>
    /// Errore registrazione
    /// </summary>
    [CLSCompliant(true)]
    public enum AccessRegistrationError
    {
        /// <summary>
        /// Nessun errore
        /// </summary>
        none,
        /// <summary>
        /// Formato mail non valido
        /// </summary>
        MailFormat,
        /// <summary>
        /// Mail presente nei Ticket
        /// </summary>
        MailInTicket,
        /// <summary>
        /// Mail presente nel sistema (MA non nei Ticket!)
        /// </summary>
        MailInSystem,
        /// <summary>
        /// Errore invio mail
        /// </summary>
        MailSendError,
        /// <summary>
        /// Campi mancanti (Nome, cognome, mail, captcha)
        /// </summary>
        VoidField,
        /// <summary>
        /// Chiave (Captcha) non valida
        /// </summary>
        InvalidKEY
    }

    /// <summary>
    /// Errore recupero chiave accesso
    /// </summary>
    [CLSCompliant(true)]
    public enum AccessRecoverError
    {
        /// <summary>
        /// Nessun errore
        /// </summary>
        none,
        /// <summary>
        /// Formato mail non valido
        /// </summary>
        MailFormat,
        /// <summary>
        /// Mail non trovata
        /// </summary>
        MailNotFound,
        /// <summary>
        /// Chiave (Captcha) non valido
        /// </summary>
        InvalidKEY,
        /// <summary>
        /// Mail non confermata: usare "registrazione"
        /// </summary>
        MailNotChecked,
        /// <summary>
        /// Errore interno: mail non inviata
        /// </summary>
        InternalError

    }

    /// <summary>
    /// Viste pagina di accesso
    /// </summary>
    [CLSCompliant(true)]
    public enum AccessView
    {
        /// <summary>
        /// Login: mail, code, captcha
        /// </summary>
        login,
        /// <summary>
        /// Registrazione: mail, nome, cognome, lingua, captcha
        /// </summary>
        register,
        /// <summary>
        /// Utente registrato, in attesa di MAIL di CONFERMA
        /// </summary>
        registered,
        /// <summary>
        /// Recupero credenziali: mail, captcha
        /// </summary>
        recover,
        /// <summary>
        /// Url per il recupero del codice inviata
        /// </summary>
        recoverRequestSended,
        /// <summary>
        /// Modifica password
        /// </summary>
        changePwd
    }

    [CLSCompliant(true)]
    public enum LoginStatus
    {
        normal,
        registration,
        token
    }

    public enum TokenValidationResult
    {
        TokenNotFound,
        UserNotFound,
        InvalidFormat,
        Exired,
        Validated
    }

    /// <summary>
    /// Indica la tipologia di Enum: per consentire utilizzi diversi,
    /// dalla validazione della mail alla 
    /// </summary>
    /// <remarks>
    /// Al momento SOLO Registration!
    /// Recover manda direttamente mail con PWD!
    /// </remarks>
    [Serializable, CLSCompliant(true)]
    public enum TokenType : short
    {
        Registration = 1,
        Recover = 2
    }
#endregion

    [CLSCompliant(true)]
    public enum FileVisibility : byte
    {
        visible = 0,
        hidden = 2,
        hiddenMessage = 3
    }

    [CLSCompliant(true)]
    public enum CantCreate
    {
        /// <summary>
        /// Non si dispone dei permessi...
        /// </summary>
        permission,
        /// <summary>
        /// Blocco di sistema
        /// </summary>
        System,
        /// <summary>
        /// Troppi Ticket in bozza
        /// </summary>
        MaxDraftNoSend,
        /// <summary>
        /// Troppi ticket aperti
        /// </summary>
        MaxSend,
        /// <summary>
        /// Ticket non valido: non dell'utente, non in bozza o inesistente
        /// </summary>
        InvalidTicket
    }

    [CLSCompliant(true)]
    public enum TicketAddCondition
    {
        CheckCount,
        NoUser,
        NoPermission,
        CanCreate,
        CheckPermission
    }

    public enum CategoryReassignError
    {
        none, 
        noChange,
        noPermission,
        invalidTicket,
        CategoryNotFound,
        error
    }

    public enum UserReassignError
    {
        none,
        Current
    }

    public enum FileDeleteResponse
    {
        NotFound,
        NoPermission,
        NotDraft,
        deleted
    }

    /// <summary>
    /// Tipo di permesso utente/tipo persona.
    /// </summary>
    [CLSCompliant(true), Serializable]
    public enum PermissionType : short
    {
        /// <summary>
        /// UnUsed
        /// </summary>
        none = 0,
        /// <summary>
        /// Crea per conto di
        /// </summary>
        Behalf = 1
    }

    //public enum SystemSettingsSwitch
    //{
    //    //service,
    //    categories,
    //    reading,
    //    writings,
    //    behalf
    //}

    [CLSCompliant(true), Serializable]
    public enum BehalfError
    {
        /// <summary>
        /// Hide Message
        /// </summary>
        none,
        /// <summary>
        /// Success message
        /// </summary>
        success,
        /// <summary>
        /// No permission!
        /// </summary>
        NoPermission,
        /// <summary>
        /// Internal error
        /// </summary>
        dBerror,
        /// <summary>
        /// Cancellazione avvenuta con successo
        /// </summary>
        deleteSuccess,
        /// <summary>
        /// Modifica visibilità avvenuta con successo
        /// </summary>
        visibilitySuccess,
        /// <summary>
        /// Sono il creatore, il ticket è in behalf, ma NON ho più il permesso di "Behalf"
        /// </summary>
        permissionRevoked
    }

    ///// <summary>
    ///// Per UC settings: secondo i flag, imposta cosa mostrare
    ///// </summary>
    //[CLSCompliant(true)]
    //public enum ViewSettingsVisibilityType
    //{
    //    /// <summary>
    //    /// Nasconde il controllo
    //    /// </summary>
    //    none = 0,
    //    /// <summary>
    //    /// Notifiche utente (Owner/Creator)
    //    /// </summary>
    //    User = 1,
    //    /// <summary>
    //    /// Notifiche gestori
    //    /// </summary>
    //    Manager = 2,
    //    /// <summary>
    //    /// Mostra/Nasconde "Default"
    //    /// </summary>
    //    HasDefault = 4,
    //    /// <summary>
    //    /// Mostra/nasconde "Nuovo Ticket"
    //    /// </summary>
    //    HasNewTicket = 8,
    //    /// <summary>
    //    /// Mostra/nasconde "Modifica proprietario"
    //    /// </summary>
    //    HasBehalf = 16
    //}

    [CLSCompliant(true)]
    public enum ViewSettingsUser
    {
        Owner,
        Manager
    }

    [CLSCompliant(true)]
    public enum ViewSettingsUserError
    {
        none,
        usernotfound,
        internalError,
        success
    }

    [CLSCompliant(true)]
    public enum CategoryFilterStatus
    {
        None,
        Current,
        Creation,
        History
    }

    [CLSCompliant(true)]
    public enum MailSettingsMaskType
    {
        none,
        globalUser,
        globalManager,
        globalBoth
    }
}
