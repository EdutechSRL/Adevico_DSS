using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.Tickets
{
    [Serializable]
    public class ModuleTicket
    {
        public const String UniqueCode = "SRVTCKT";
        //public virtual Boolean AddTicket {get;set;}
        //public virtual Boolean ViewMyTickets { get; set; }
        public virtual Boolean ManageCategory { get; set; }
        public virtual Boolean EditCategory { get; set; }

        public virtual Boolean ManagePermissions { get; set; }
        public virtual Boolean Administration { get; set; }

        public ModuleTicket()
        {
            //ViewMyTickets = true;
        }
        public static ModuleTicket CreatePortalmodule(int UserTypeID)
        {
            ModuleTicket module = new ModuleTicket();
            //module.AddTicket = true;
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            return module;
        }
        public ModuleTicket(long permission)
        {
            //ViewMyTickets = true;
            EditCategory = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditCategory, permission);
            ManageCategory = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManageCategory, permission);
            ManagePermissions = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManagePermissions, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Admin, permission);
        }
        public lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages ToTemplateModule()
        {
            lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages m = new lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(UniqueCode);
            m.Add = Administration;
            m.SendMessage = Administration;
            m.DeleteMyTemplates = Administration;
            m.Edit = Administration;
            m.Administration = Administration;
            m.Clone = Administration;
            m.List = Administration;
            m.DeleteOtherTemplates = Administration;
            m.ManageModulePermission = Administration || ManagePermissions;
            return m;
        }

        public static List<MailSenderActionType> GetMandatoryActions()
        {
            return new List<MailSenderActionType>() { MailSenderActionType.externalPasswordChanged, MailSenderActionType.externalRecover, MailSenderActionType.externalRegistration };
        }
        [Flags, Serializable]
        public enum Base2Permission
        {
            //ListTickets = 1,
            /// <summary>
            /// (Moderate: 4 => 16) Modifica categoria/assegnazione manager, resolver
            /// </summary>
            EditCategory = 16, //1 << 3,
            /// <summary>
            /// (Grant: 5 => 32) Management completo categorie
            /// </summary>
            ManageCategory = 32, //1 << 4,
            /// <summary>
            /// (ChangeOwner: 12 => 4096) Modifica permessi: solo sistema se necessario
            /// </summary>
            ManagePermissions = 4096, //1 << 11,
            /// <summary>
            /// (Admin: 6 => 64) Amministrazione globale: non usato in favore di Person.TypeID
            /// </summary>
            Admin = 64 //1 << 5,
        }

        [Serializable, CLSCompliant(true)]
        public enum ActionType : int
        {
            None = 89500,
            NoPermission = 89501,

            TicketCreateDraft = 89505,
            TicketCreate = 89506,

            TicketAssignPerson = 89507,
            TicketAssignUser = 89508,
            TicketAssignCategory = 89509,
            TicketStatusChanged = 89510,

            TicketLoadEditUser = 89515,
            TicketLoadEditManRes = 89516,
            TicketSettingUserChanged = 89517,

            TicketListUser = 89518,
            TicketListManRes = 89519,

            MessageSend = 89520,
            MessageSendAttach = 89521,
            MessageShow = 89522,
            MessageHide = 89523,

            MessageAttachDel = 89525,
            MessageAttachHide = 89526,
            MessageAttachShow = 89527,
            MessageAttachAdd = 89528,
            MessageAttachDownload = 89528,

            CategoryList = 89530,
            CategoryLoadManage = 89531,
            CategoryCreate = 89532,
            CategoryModify = 89533,
            CategoryReassign = 89534,
            CategoryReorder = 89535,
            CategoryDelete = 89536,
            CategoryUndelete = 89537,

            SettingsUserLoad = 89540,
            SettingsUserModify = 89541,
            SettingsGlobalLoad = 89545,
            SettingsGlobalModify = 89546,

            ExternalUserCreate = 89550,
            ExternalUserValidate = 89551,
            ExternalUserModify = 89552,

            ExternalList = 89555,
            ExternalCreateDraft = 89556,
            ExternalCreate = 89557,
            ExternalMessageSend = 89558,
            ExternalMessageSendAttach = 89559,

            //Behalf di un Ticket - TODO!
            BehalfTicketSet = 89560,
            BehalfTicketRemove = 89561,

            //Gestione utenti BEHALF - TODO!
            PermissionBehalfSet = 89565,
            //BehalfUserRemove = 89566,

            PermissionRemove = 89570
            
        }
        [Serializable]
        public enum ObjectType : int
        {
            None = 0,
            
            Ticket = 1,
            /// <summary>
            /// Utente Ticket
            /// </summary>
            User = 2,
            /// <summary>
            /// Person: per Manager/Resolver
            /// </summary>
            Person = 3,
            Message = 4,
            File = 5,
            Category = 6,
            /// <summary>
            /// Id permesso. Al momento SOLO BEHALF (SYS)
            /// </summary>
            Permission = 7,

            
        }

        /// <summary>
        /// COPIA DI lm.DataAction.InteractionType!!!
        /// </summary>
        [Serializable(), CLSCompliant(true)]
        public enum InteractionType : int
        {
            None = 1,
            /// <summary>
            /// Interaction between users
            /// </summary>
            UserWithUser = 2,
            /// <summary>
            ///  Interaction between user and community administrator
            /// </summary>
            UserWithCommunityAdministrator = 3,
            /// <summary>
            /// Interaction between user and LearingObjects
            /// </summary>
            UserWithLearningObject = 4,
            /// <summary>
            /// Interaction generic
            /// </summary>
            Generic = 5,
            /// <summary>
            /// Interaction betweeen core
            /// </summary>
            SystemToSystem = 6,
            /// <summary>
            /// Interaction from core to user
            /// </summary>
            SystemToUser = 7,
            /// <summary>
            /// Interaction from core to module
            /// </summary>
            SystemToModule = 8,
            /// <summary>
            /// Interaction from module to core
            /// </summary>
            ModuleToSystem = 9,
            /// <summary>
            /// Interaction between modules
            /// </summary>
            ModuleToModule = 10
        }


        // /// <summary>
        ///// Descrizione prefissi:
        ///// SYS_    Inviati con automatismi dal sistema
        ///// BOH_    Tendenzialmente automatismi di sistema, ma da valutare SE richiedere conferma dall'admin!
        ///// USR_    Gestiti dagli utenti (amministratori), tipo inviti
        ///// </summary>
        //[Serializable]
        //public enum MailSenderActionType
        //{
        //    ///// <summary>
        //    ///// Richiesta iscrizione (al 90% solo messaggio a monitor)
        //    ///// </summary>
        //    //SubscriptionRequest = 1,
        //    /// <summary>
        //    /// Accettazione richiesta con credenziali accesso (Sia automatica che da admin)
        //    /// </summary>
        //    /// SubscriptionAccepted = 2,
        //    /// <summary>
        //    /// Invio/recupero credenziali (Accesso Esterno)
        //    /// </summary>
        //    Credential = 5,
        //    /// <summary>
        //    /// Utente bloccato (da verificare)
        //    /// </summary>
        //    LockUser = 11,
        //    /// <summary>
        //    /// Utente riabilitato (da verificare)
        //    /// </summary>
        //    UnLockUser = 12,
        //    //USR_InvitationGeneric = 21,     //Invito generico (da Admin, dopo edit)
        //    //USR_InvitationSpecific = 22     //Invito specifico (da Admin, dopo edit)
        //    GenericInvitation = 21
        //}

        /// <summary>
        /// TIPO di notifica, per l'INVIO!
        /// </summary>
        [Serializable]
        public enum MailSenderActionType
        {
            // PRECEDENTI
            /// <summary>
            /// Reserved
            /// </summary>
            none = 0,
            // TOLTI:
            //statusChange = 2,
            //categoryChange = 4,
            //assignmentChange = 8,
            //categoryAssignedChange = 128,
            /// <summary>
            /// Registrazione utente: invio Token o URL che lo contiene
            /// </summary>
            externalRegistration = 16,
            /// <summary>
            /// Recupero credenziali accesso esterno (Password)
            /// </summary>
            externalRecover = 32,
            /// <summary>
            /// FUTURO: mail di avviso che la password è stata modificata.
            /// </summary>
            externalPasswordChanged = 64,
            
            // Nuovi e rivisti
            //Enum: Name = Value				// ID - Message - NORMALE					// Semplificato

            // Utente
            TicketNewUser = 1,				//  1 - User: nuovo ticket					//  11 - Nuovo Ticket
            TicketSendMessageUser = 2,		//  2 - User: nuovo messaggio + stato		//  12 - Modifica al Ticket
            TicketModeratedUser = 3,		//  3 - User: locked!						//  12 - Modifica al Ticket
            TicketCategoryResetUser = 4,	//  4 - User: category modified.			//  12 - Modifica al Ticket

            // Creatore (behalf)
            TicketOwnerChanged = 8,			//  5 - Creator: proprietario modificato	//  13 - Modifica proprietario

            // Manager/Resolver/Assegnatario
            TicketNewMan = 21,				//  6 - Manager: nuovo ticket				//  14 - Nuovo Ticket
            TicketSendMessageMan = 12,		//  7 - Manager: nuovo messaggio			//  15 - Modifica Ticket
            TicketModeratedMan = 13,		//  8 - Manager: signed/locked				//  15 - Modifica Ticket
            TicketCategoryResetMan = 24,	//  6 - Manager: nuovo ticket				//  14 - Nuovo Ticket

            TicketCategoryAdd = 26,			//  6 - Manager: nuovo ticket				//  14 - Nuovo Ticket
            TicketAssignmentAddAssigner = 27,	// 10 - Assegnatario: new ticket	    //  14 - Nuovo Ticket
            TicketAssignmentAddManager = 28,	// 11 - Manager: new ass.	            //  14 - Nuovo Ticket

            CategoryModified = 70,			//  9 - Manager: nuovi tickets				//  16 - Modifiche Tickets
            CategoryReorder = 71,			//  9 - Manager: nuovi tickets				//  16 - Modifiche Tickets


            TicketStatusChangedUser = 80,
            TicketStatusChangedMan = 81,
        }

        /// <summary>
        /// TIPO di notifica, per l'INVIO!
        /// </summary>
        [Serializable]
        public enum NotificationActionType
        {
            none = 0,
            /// <summary>
            /// Invio Ticket
            /// </summary>
            TicketSend = 1,
            /// <summary>
            /// Nuovo messaggio nel Ticket
            /// </summary>
            MassageSend = 5,
            /// <summary>
            /// Nuova categoria corrente
            /// </summary>
            AssignmentCategory = 10,
            /// <summary>
            /// Nuovo assegnatario
            /// </summary>
            AssignmentUser = 12,
            /// <summary>
            /// Modifica categoria di DEFAULT (TODO)
            /// </summary>
            AssignmentReset = 14,
            /// <summary>
            /// Modifica stato
            /// </summary>
            StatusChanged = 20,
            /// <summary>
            /// Modifica moderazione (segnalato/bloccato)
            /// </summary>
            ModerationChanged = 22,
            /// <summary>
            /// Modifica proprietario
            /// </summary>
            OwnerChanged = 24,

            /// <summary>
            /// Amministrazione categorie: notifica generica
            /// </summary>
            CategoriesNotification = 30,
            /// <summary>
            /// Amministrazione categorie: riordino - SOLO MANAGER
            /// </summary>
            CategoriesReordered = 32

        }

        #region KeyValuePair Action
        public static KeyValuePair<int, String> KVPgetUser(Int64 UserId)
        {
            return new KeyValuePair<int, String>((int)ObjectType.User, UserId.ToString());
        }

        public static KeyValuePair<int, String> KVPgetTicket(Int64 TicketId)
        {
            return new KeyValuePair<int, String>((int)ObjectType.Ticket, TicketId.ToString());
        }

        public static KeyValuePair<int, String> KVPgetPerson(Int32 PersonId)
        {
            return new KeyValuePair<int, String>((int)ObjectType.Person, PersonId.ToString());
        }

        public static KeyValuePair<int, String> KVPgetCategory(Int64 CategoryId)
        {
            return new KeyValuePair<int, String>((int)ObjectType.Category, CategoryId.ToString());
        }

        public static KeyValuePair<int, String> KVPgetMessage(Int64 MessageId)
        {
            return new KeyValuePair<int, String>((int)ObjectType.Message, MessageId.ToString());
        }

        public static KeyValuePair<int, String> KVPgetPermission(Int64 PermissionId)
        {
            return new KeyValuePair<int, String>((int)ObjectType.Permission, PermissionId.ToString());
        }
        #endregion

        [Serializable]
        public enum NotificationActionCategoryUserReceiver : long
        {
            All = -1,
            Managers = -2,
            Resolvers = -3
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