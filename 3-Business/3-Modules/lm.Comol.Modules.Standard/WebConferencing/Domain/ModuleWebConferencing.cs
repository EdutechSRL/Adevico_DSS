using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// Modulo x gestione permessi/azioni
    /// </summary>
    [Serializable]
    public class ModuleWebConferencing
    {
        #region Property (code/permission)
        
        /// <summary>
        /// Codice univoco servizio
        /// </summary>
        public const String UniqueCode = "SRVWBCF";

        /// <summary>
        /// Permesso: visualizzazione lista stanze
        /// </summary>
        public virtual Boolean ListRoom { get; set; }
        /// <summary>
        /// Permesso: creazione chat (e relativo management)
        /// </summary>
        public virtual Boolean AddChatRoom { get; set; }
        /// <summary>
        /// Permesso: creazione stanze (e relativo management)
        /// </summary>
        public virtual Boolean ManageRoom { get; set; }

        #endregion

        #region Costruttori
        
        /// <summary>
        /// Costruttore
        /// </summary>
        public ModuleWebConferencing()
        {
            ListRoom = true;
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="permission">Permessi (by system)</param>
        public ModuleWebConferencing(long permission)
        {
            ListRoom = PermissionHelper.CheckPermissionSoft((long)Base2Permission.List, permission);
            AddChatRoom = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddChat, permission);
            ManageRoom = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Manage, permission);
        }

        /// <summary>
        /// Costruttore modulo portale
        /// </summary>
        /// <param name="UserTypeID">Tipo utente</param>
        /// <returns>Modulo e relativi permessi per il tipo indicato</returns>
        public static ModuleWebConferencing CreatePortalmodule(int UserTypeID)
        {
            ModuleWebConferencing module = new ModuleWebConferencing();

            module.AddChatRoom = false;
            module.ManageRoom = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.ListRoom = (UserTypeID == (int)UserTypeStandard.AllWithoutGuestUser);

            return module;
        }

        #endregion

        public lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages ToTemplateModule()
        {
            lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages m = new lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(UniqueCode);
            m.Add = ManageRoom || AddChatRoom;
            m.SendMessage = ManageRoom || AddChatRoom;
            m.DeleteMyTemplates = ManageRoom || AddChatRoom;
            m.Edit = ManageRoom;
            m.Administration = ManageRoom;
            m.Clone = ManageRoom;
            m.List = ManageRoom;
            m.DeleteOtherTemplates = ManageRoom;
            m.ManageModulePermission =   ManageRoom;
            return m;
        }

        /// <summary>
        /// converter permessi sistema/permessi Webconference
        /// </summary>
        [Flags, Serializable]
        public enum Base2Permission
        {
            /// <summary>
            /// Visualizzazione lista
            /// </summary>
            List = 1,
            /// <summary>
            /// Creazione e gestione chat
            /// </summary>
            AddChat = 2,
            /// <summary>
            /// Modifica permessi
            /// </summary>
            GrantPermission = 32,
            /// <summary>
            /// Creazione e gestione stanze
            /// </summary>
            Manage = 64
        }

        /// <summary>
        /// Azioni servizio
        /// </summary>
        [Serializable]
        public enum ActionType
        {
            /// <summary>
            /// Nessun azione
            /// </summary>
            None = 73500,
            /// <summary>
            /// Permessi insufficienti
            /// </summary>
            NoPermission = 73501,
            /// <summary>
            /// Server esterno non disponibile
            /// </summary>
            NoServer = 73502,
            /// <summary>
            /// Utente non riconosciuto
            /// </summary>
            NoUser = 73503,
            /// <summary>
            /// Stanza inesistente
            /// </summary>
            NoRoom = 73504,
            /// <summary>
            /// Errore generico
            /// </summary>
            GenericError = 73505,
            
            /// <summary>
            /// Visualizzazione lista stanze
            /// </summary>
            RoomList = 73511,
            /// <summary>
            /// Accesso a stanza
            /// </summary>
            RoomAccess = 73512,
            /// <summary>
            /// Accesso alla gestione stanza
            /// </summary>
            RoomEdit = 73513,
            /// <summary>
            /// Aggiornamento dati stanza
            /// </summary>
            RoomUpdate = 73514,
            /// <summary>
            /// Cancellazione stanza
            /// </summary>
            RoomDelete = 73515,
            /// <summary>
            /// Accesso alla creazione di una stanza
            /// </summary>
            RoomAdding = 73516,
            /// <summary>
            /// Stanza creata
            /// </summary>
            RoomAdd = 73517,    
            //VirtualDelete = 73516,
            //VirtualUnDelete = 73517,
            
            /// <summary>
            /// Utente interno aggiunto
            /// </summary>
            UsersAddInternal = 73521,
            /// <summary>
            /// Utente esterno aggiunto
            /// </summary>
            UsersAddExternal = 73522,
            /// <summary>
            /// Import utenti da CSV (DA implementare)
            /// </summary>
            UsersImportFromCsv = 73523,
            /// <summary>
            /// Aggiornamento dati utente
            /// </summary>
            UserUpdate = 73524,
            /// <summary>
            /// Utente cancellato dalla stanza
            /// </summary>
            UserRemove = 73525,
            /// <summary>
            /// Iscrizione autonoma utente esterno
            /// </summary>
            UserSubscribeSelfInternal = 73526,
            /// <summary>
            /// Accesso a pagina di login/iscrizione esterna
            /// </summary>
            UserSubscribeLogin = 73527,
            /// <summary>
            /// Iscrizione autonoma utente esterno
            /// </summary>
            UserSubscribeSelfExternal = 73527,
            /// <summary>
            /// Utente bloccato
            /// </summary>
            UserLock = 73531,
            /// <summary>
            /// Utente sbloccato
            /// </summary>
            UserUnlock = 73532,
        
            /// <summary>
            /// Mail con i dati d'accesso inviata all'utente
            /// </summary>
            MailSendUser = 73541,
            /// <summary>
            /// Invio invito generico
            /// </summary>
            MailSendInvitation = 73542,
            /// <summary>
            /// Invio mail richiesta dati accesso
            /// </summary>
            MailRequestSend = 73543,
            /// <summary>
            /// Invio mail di conferma iscrizione
            /// </summary>
            MailSubscriptionSend = 73544,

            /// <summary>
            /// Accesso lista file
            /// </summary>
            FileList = 73551,
            /// <summary>
            /// Aggiunto un file alla stanza
            /// </summary>
            FileAdd = 73552,
            /// <summary>
            /// Cancellato file
            /// </summary>
            FileDelete = 73553,
            /// <summary>
            /// File scaricato
            /// </summary>
            FileDownload = 73554,

            /// <summary>
            /// Accesso lista registrazioni stanza
            /// </summary>
            RecList = 73555,
            /// <summary>
            /// Cancellazione registrazioni
            /// </summary>
            RecDelete = 73556,
            /// <summary>
            /// Download registrazione
            /// </summary>
            RecDownload = 73557,

            /// <summary>
            /// Accesso statistiche personali
            /// </summary>
            StatShowPersonal = 73561,
            /// <summary>
            /// Accesso statistiche globali
            /// </summary>
            StatShowGlobal = 73562,
        }

        /// <summary>
        /// Descrizione prefissi:
        /// SYS_    Inviati con automatismi dal sistema
        /// BOH_    Tendenzialmente automatismi di sistema, ma da valutare SE richiedere conferma dall'admin!
        /// USR_    Gestiti dagli utenti (amministratori), tipo inviti
        /// </summary>
        [Serializable]
        public enum MailSenderActionType
        {
            ///// <summary>
            ///// Richiesta iscrizione (al 90% solo messaggio a monitor)
            ///// </summary>
            //SubscriptionRequest = 1,
            /// <summary>
            /// Accettazione richiesta con credenziali accesso (Sia automatica che da admin)
            /// </summary>
           /// SubscriptionAccepted = 2,
            /// <summary>
            /// Invio/recupero credenziali (Su richiesta esplicita di Admin o dell'utente)
            /// </summary>
            Credential = 5,
            /// <summary>
            /// Utente bloccato (da verificare)
            /// </summary>
            LockUser = 11,
            /// <summary>
            /// Utente riabilitato (da verificare)
            /// </summary>
            UnLockUser = 12,
            //USR_InvitationGeneric = 21,     //Invito generico (da Admin, dopo edit)
            //USR_InvitationSpecific = 22     //Invito specifico (da Admin, dopo edit)
            GenericInvitation = 21
        }

        /// <summary>
        /// Oggetti relativi alle azioni (non in uso)
        /// </summary>
        [Serializable]
        public enum ObjectType
        {
            /// <summary>
            /// Nessun oggetto/azione generica
            /// </summary>
            None = 0,
            /// <summary>
            /// Stanza
            /// </summary>
            Room = 1,
            /// <summary>
            /// Utente
            /// </summary>
            User = 2,
            /// <summary>
            /// Mail
            /// </summary>
            Mail = 3,
            /// <summary>
            /// File
            /// </summary>
            File = 4,
            /// <summary>
            /// Registrazioni
            /// </summary>
            Rec = 5,
            /// <summary>
            /// Statistiche
            /// </summary>
            Stat = 6
        }
    }
}


//'    PermissionType
//'    None = -1                                           '    Admin = 6 '64
//'    Read = 0 '1                                         '    Send = 7 ' 128
//'    Write = 1 '2                                        '    Receive = 8 '256
//'    Change = 2 '4                                       '    Synchronize = 9 '512
//'    Delete = 3 '8                                       '    Browse = 10 '1024      ViewCommunityProjects
//'    Moderate = 4 '16                                    '    Print = 11 '2048
//'    Grant = 5 '32                                       '    ChangeOwner = 12 '4096
//'    Add = 13 '8192        AddCommunityProject                                        '    ChangeStatus = 14 '16384
//'    DownLoad = 15 '32768