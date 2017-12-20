using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    /// <summary>
    /// Modulo Bandi. Definizioni oggetti, azini, permessi
    /// </summary>
    [Serializable]
    public class ModuleCallForPaper
    {
        /// <summary>
        /// Codice univoco bandi
        /// </summary>
        public const String UniqueCode ="SRVCFP";
        /// <summary>
        /// Permessi visualizzazione bandi
        /// </summary>
        public virtual Boolean ViewCallForPapers {get;set;}
        /// <summary>
        /// Permesso di sottomettere un bando
        /// </summary>
        public virtual Boolean AddSubmission {get;set;}
        /// <summary>
        /// Permesso creazione bando
        /// </summary>
        public virtual Boolean CreateCallForPaper {get;set;}
        /// <summary>
        /// Permesso modifica bando
        /// </summary>
        public virtual Boolean EditCallForPaper { get; set; }
        /// <summary>
        /// Permesso gestione bandi
        /// </summary>
        public virtual Boolean ManageCallForPapers {get;set;}
        /// <summary>
        /// Permesso cancellazione dei propri bandi (bandi creati)
        /// </summary>
        public virtual Boolean DeleteOwnCallForPaper {get;set;}
        /// <summary>
        /// Permesso amministrazione
        /// </summary>
        public virtual Boolean Administration { get; set; }
        /// <summary>
        /// Permesso generico bando (Default)
        /// </summary>
        public ModuleCallForPaper() {
            ViewCallForPapers = true;
        }
        /// <summary>
        /// Crea l'oggetto per il portale
        /// </summary>
        /// <param name="UserTypeID">Tipo persona utente corrente</param>
        /// <returns></returns>
        public static ModuleCallForPaper CreatePortalmodule(int UserTypeID){
            ModuleCallForPaper module = new ModuleCallForPaper();
            module.ViewCallForPapers = true;
            module.AddSubmission=(UserTypeID != (int)UserTypeStandard.Guest );
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.CreateCallForPaper= (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative   );
            module.EditCallForPaper = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.ManageCallForPapers = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.DeleteOwnCallForPaper = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            return module;
        }
        /// <summary>
        /// Crea il modulo a partire dalla stringa permessi correnti
        /// </summary>
        /// <param name="permission"></param>
        public ModuleCallForPaper(long permission)
        {
            ViewCallForPapers = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ListCalls, permission);
            AddSubmission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddSubmission, permission);
            CreateCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddCall, permission);
            EditCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditCall, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Admin, permission);
            ManageCallForPapers = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManageCalls, permission);
            DeleteOwnCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteCall, permission); 
        }
        /// <summary>
        /// Trasforma i permessi del modulo bandi in permessi del modulo Template (stampa documenti)
        /// </summary>
        /// <returns></returns>
        public lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages ToTemplateModule() {
            lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages m = new lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(UniqueCode);
            m.Add = ManageCallForPapers || Administration || CreateCallForPaper;
            m.Administration = ManageCallForPapers || Administration;
            m.Clone = ManageCallForPapers || Administration;
            m.DeleteMyTemplates = ManageCallForPapers || Administration || CreateCallForPaper;
            m.DeleteOtherTemplates = ManageCallForPapers || Administration;
            m.Edit = ManageCallForPapers || Administration;
            m.List = ManageCallForPapers || Administration;
            m.SendMessage = ManageCallForPapers || Administration || CreateCallForPaper;
            m.ManageModulePermission = ManageCallForPapers || Administration || CreateCallForPaper;
            return m;
        }
        /// <summary>
        /// Recupera le azioni automatiche (notifica)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<ActionType> GetAutomaticActions(lm.Comol.Core.Notification.Domain.NotificationMode mode)
        {
            List<ActionType> actions = new List<ActionType>();
            switch (mode) {
                case lm.Comol.Core.Notification.Domain.NotificationMode.Automatic:
                    break;
                case lm.Comol.Core.Notification.Domain.NotificationMode.Scheduling:
                    break;
                case lm.Comol.Core.Notification.Domain.NotificationMode.Manual:
                    break;
            }
            return actions;
        }
        /// <summary>
        /// Conversione posizione permessi generici in permessi modulo
        /// </summary>
        [Flags,Serializable]
        public enum Base2Permission{
            /// <summary>
            /// Elenco bandi
            /// </summary>
            ListCalls = 1,
            /// <summary>
            /// Sottomissione bando
            /// </summary>
            AddSubmission = 2 ,
            /// <summary>
            /// Modifica bando
            /// </summary>
            EditCall = 4,
            /// <summary>
            /// Cancellazione bando
            /// </summary>
            DeleteCall = 8,
            /// <summary>
            /// Gestione bandi
            /// </summary>
            ManageCalls = 16,
            /// <summary>
            /// Amministrazione
            /// </summary>
            Admin = 64,
            /// <summary>
            /// Aggiunta di un nuovo bando
            /// </summary>
            AddCall = 8192
        }

        /// <summary>
        /// Azioni previste nel servizio bandi
        /// </summary>
        /// <remarks>
        /// Alcune azioni non sono in uso.
        /// 
        /// Convenzioni:
        /// 
        /// Crea: accesso alle pagine di creazione di un oggetto
        /// Modifica: accesso alle pagine di modifica di un oggetto
        /// Salva: salva il nuovo oggetto o le sue modifiche
        /// Visualizza: accesso in sola visualizzazione dell'oggetto
        /// 
        /// Comittee: comitato di valutazione (vecchia versione)
        /// Commission: commissione di valutazione (nuova versione con step)
        /// 
        /// NOTE: 
        /// - fare in modo che siano univoche e facilmente individuabili.
        /// - riportare le azioni nella tabella "ActionType"
        /// </remarks>
        [Serializable]
        public enum ActionType{
            /// <summary>
            /// Nessuna
            /// </summary>
            None = 85000,
            /// <summary>
            /// Non si dispone dei permessi
            /// </summary>
            NoPermission = 85001,
            /// <summary>
            /// Errore generico servizio
            /// </summary>
            GenericError = 85002,
            /// <summary>
            /// Elenco bandi
            /// </summary>
            List = 85003,
            /// <summary>
            /// Elenco bandi (gestione)
            /// </summary>
            Manage = 85004,
            /// <summary>
            /// Modifica bando
            /// </summary>
            EditCall = 85008,
            /// <summary>
            /// Cancellazione bando
            /// </summary>
            DeleteCall = 85009,
            /// <summary>
            /// Nuova sottomissione
            /// </summary>
            AddSubmission = 85010,
            /// <summary>
            /// Modifica sottomissione
            /// </summary>
            EditSubmission = 85011,
            /// <summary>
            /// Invio definitivo sottomissione
            /// </summary>
            ConfirmSubmission = 85012,
            /// <summary>
            /// Cancellazione sottomissione
            /// </summary>
            DeleteSubmission = 85013,
            /// <summary>
            /// Valutazione sottomissione
            /// </summary>
            EvaluateSubmission = 85014,
            /// <summary>
            /// Visualizzazione sottomissione
            /// </summary>
            ViewSubmission = 85015,
            /// <summary>
            /// Visualizzazione sottomissione non riconosciuta
            /// </summary>
            ViewUnknowSubmission = 85016,
            /// <summary>
            /// Visualizzazione sottomissione bando
            /// </summary>
            ViewCallSubmission = 85017,
            /// <summary>
            /// Visualizzazione bando non riconosciuto (canellato/non trovato)
            /// </summary>
            ViewUnknowCallForPaper = 85018,
            /// <summary>
            /// Visualizzazione anteprima bando
            /// </summary>
            ViewPreviewCallForPaper = 85019,
            /// <summary>
            /// Scarica file sottomissione
            /// </summary>
            DownloadSubmittedFile = 85020,
            /// <summary>
            /// Scarica file allegato al bando
            /// </summary>
            DownloadCallForPaperFile =85021,

            /// <summary>
            /// Gestione criteri
            /// </summary>
            ManageCriterion = 85022,
            /// <summary>
            /// Creazione nuovo criterio
            /// </summary>
            AddCriterion = 85023,
            /// <summary>
            /// Modifica criterio
            /// </summary>
            EditCriterion = 85024,
            /// <summary>
            /// Cancellazione fisica criterio
            /// </summary>
            DeleteCriterion = 85025,
            /// <summary>
            /// Cancellazione logica criterio
            /// </summary>
            VirtualDeleteCriterion = 85026,
            /// <summary>
            /// Recupero criterio cancellato
            /// </summary>
            VirtualUndeleteCriterion = 85027,
            /// <summary>
            /// Gestione valutatori
            /// </summary>
            ManageEvaluators = 85028,
            /// <summary>
            /// Aggiunta nuovo valutatore
            /// </summary>
            AddEvaluator = 85029,
            /// <summary>
            /// Modifica valutatore
            /// </summary>
            EditEvaluator = 85030,
            /// <summary>
            /// Cancellazione fisica valutatore
            /// </summary>
            DeleteEvaluator = 85031,
            /// <summary>
            /// Cancellazione logica valutatore
            /// </summary>
            VirtualDeleteEvaluator = 85032,
            /// <summary>
            /// Recupero valutatore cancellato
            /// </summary>
            VirtualUndeleteEvaluator = 85033,
            /// <summary>
            /// Gestione valutazioni
            /// </summary>
            ManageEvaluations = 85034,
            /// <summary>
            /// Visualizzazione valutazione sottomissione
            /// </summary>
            ViewEvaluation = 85035,
            /// <summary>
            /// Modifica assegnazione valutatori
            /// </summary>
            EditAssignedEvaluations = 85036,
            /// <summary>
            /// Visualizza e modifica valutatori
            /// </summary>
            ViewAndEditEvaluators = 85037,
            /// <summary>
            /// Visualizzazione valutatori assegnati
            /// </summary>
            ViewAssignedEvaluations = 85038,
            /// <summary>
            /// Accetta sottomissione
            /// </summary>
            AcceptSubmission = 85039,
            /// <summary>
            /// Rifiuta sottomissione
            /// </summary>
            RejectSubmission = 85040,
            /// <summary>
            /// Inizia nuova sottomissione
            /// </summary>
            StartSubmission = 85041,
            /// <summary>
            /// Cancellazione logica sottomissione
            /// </summary>
            VirtualDeleteSubmission = 85042,
            /// <summary>
            /// Cancellazione logica file sottomesso
            /// </summary>
            VirtualDeleteSubmittedFile = 85043,
            /// <summary>
            /// Cancellazione logica bando
            /// </summary>
            VirtualDeleteCallForPaper = 85044,
            /// <summary>
            /// Recupero bando cancellato
            /// </summary>
            VirtualUndeleteCallForPaper = 85045,
            /// <summary>
            /// Inizio creazione bando
            /// </summary>
            StartCallCreation = 85046,
            /// <summary>
            /// Inizio modifica bando
            /// </summary>
            StartCallEdit = 85047,
            /// <summary>
            /// Creazione bando
            /// </summary>
            CreateCallForPaper = 85048,
            /// <summary>
            /// Cancellazione logica campo bando
            /// </summary>
            VirtualDeleteCallField = 85049,
            /// <summary>
            /// Recupero campo cancellato
            /// </summary>
            VirtualUndeleteCallField = 85050,
            /// <summary>
            /// Cancellazione logica sezione
            /// </summary>
            VirtualDeleteCallSection = 85051,
            /// <summary>
            /// Recupero sezione cancellata
            /// </summary>
            VirtualUndeleteCallSection = 85052,
            /// <summary>
            /// Nuovo campo bando
            /// </summary>
            AddFieldToCall = 85053,
            /// <summary>
            /// Nuova sezione bando
            /// </summary>
            AddSectionToCall = 85054,
            /// <summary>
            /// Salvataggio campo
            /// </summary>
            SaveCallField = 85055,
            /// <summary>
            /// Salvataggio sezione
            /// </summary>
            SaveCallSection = 85056,
            /// <summary>
            /// Modifica assegnazioni sottomissioni
            /// </summary>
            EditAssignedSubmissions = 85057,
            /// <summary>
            /// Visualizza valutazioni non assegnate
            /// </summary>
            ViewUnassignedEvaluation = 85058,
            /// <summary>
            /// Aggiunta impostazioni bando
            /// </summary>
            AddCallSettings = 85059,
            /// <summary>
            /// Salvataggio impostazioni bando
            /// </summary>
            SaveCallSettings = 85060,
            /// <summary>
            /// Visualizza bandi disponibili
            /// </summary>
            ViewCallAvailability = 85061,
            /// <summary>
            /// Modifica disponibilità bandi
            /// </summary>
            SaveCallAvailability = 85062,
            /// <summary>
            /// Modifica tipo sottomittore
            /// </summary>
            EditSubmittersType = 85063,
            /// <summary>
            /// Salva tipo sottomittore
            /// </summary>
            SaveSubmittersType = 85064,
            /// <summary>
            /// Modifica allegato
            /// </summary>
            EditAttachments = 85065,
            /// <summary>
            /// Salva allegato
            /// </summary>
            SaveAttachments = 85066,
            /// <summary>
            /// Nuovo allegato
            /// </summary>
            AddAttachments = 85067,
            /// <summary>
            /// Cancellazione logica allegato
            /// </summary>
            VirtualDeleteAttachment = 85068,
            /// <summary>
            /// Cancellazione logica tipo sottomittore
            /// </summary>
            VirtualDeleteSubmitterType = 85069,
            /// <summary>
            /// Caricamento edito bando (editor domanda)
            /// </summary>
            LoadCallEditor = 85070,
            /// <summary>
            /// Salva modifiche alla sezione
            /// </summary>
            SaveCallSections = 85071,
            /// <summary>
            /// Cancellazione logica opzioni campo
            /// </summary>
            VirtualDeleteFieldOption = 85072,
            /// <summary>
            /// Aggiongi opzione campo
            /// </summary>
            AddFieldOption = 85073,
            /// <summary>
            /// Visualizza template del sottomittore
            /// </summary>
            ViewSubmittersTemplate = 85074,
            /// <summary>
            /// Modifica template del sottomittore
            /// </summary>
            EditSubmitterTemplate = 85075,
            /// <summary>
            /// Nuovo template sottomittore
            /// </summary>
            AddSubmitterTemplate = 85076,
            /// <summary>
            /// Cancellazione logica template sottomittore
            /// </summary>
            VirtualDeleteSubmitterTemplate = 85077,
            /// <summary>
            /// Salvataggio template sottomittore
            /// </summary>
            SaveSubmittersTemplate = 85078,
            /// <summary>
            /// Modifica template
            /// </summary>
            EditManagerTemplate = 85079,
            /// <summary>
            /// Salvataggio template
            /// </summary>
            SaveManagerTemplate = 85080,
            /// <summary>
            /// Visualizza file richiesta
            /// </summary>
            ViewRequestedFiles = 85081,
            /// <summary>
            /// Aggiungi file richiesta
            /// </summary>
            AddRequestedFile = 85082,
            /// <summary>
            /// Mofifica file richiesta
            /// </summary>
            EditRequestedFile = 85083,
            /// <summary>
            /// Cancellazione logica file richiesta
            /// </summary>
            VirtualDeleteRequestedFile = 85084,
            /// <summary>
            /// Visualizza revisione
            /// </summary>
            ViewRevision = 85085,
            /// <summary>
            /// Salva revisione
            /// </summary>
            SaveRevision = 85086,
            /// <summary>
            /// Completa (chiudi) revisione
            /// </summary>
            CompleteRevision = 85087,
            /// <summary>
            /// Modifica associazioni campi
            /// </summary>
            EditFieldsAssociation = 85088,
            /// <summary>
            /// Salva associazione campi
            /// </summary>
            SaveFieldsAssociation = 85089,
            /// <summary>
            /// Carica lista revisioni
            /// </summary>
            LoadRevisionsList = 85090,
            /// <summary>
            /// Carica lista sottomissioni
            /// </summary>
            LoadSubmissionsList = 85091,
            /// <summary>
            /// Modifica comitato di valutazione (old)
            /// </summary>
            ManageCommittee = 85092,
            /// <summary>
            /// Salva impostazioni comitato di valutazione (old)
            /// </summary>
            SaveCommitteeSettings = 85093,
            /// <summary>
            /// Aggiungi valutatore
            /// </summary>
            AddCommittee = 85094,
            /// <summary>
            /// Cancellazione comitato di valutazinoe (old)
            /// </summary>
            VirtualDeleteCommittee = 85095,
            /// <summary>
            /// Recupero 
            /// </summary>
            VirtualUndeleteCommittee = 85096,
            /// <summary>
            /// Cancellazione fisica comitato
            /// </summary>
            PhisicalDeleteCommittee = 85097,
            /// <summary>
            /// Cancellazione logica opzione criterio
            /// </summary>
            VirtualDeleteCriterionOption = 85101,
            /// <summary>
            /// Recupero opzione criterio
            /// </summary>
            VirtualUndeleteCriterionOption = 85102,
            /// <summary>
            /// Cancellazione fisica opzione criterio
            /// </summary>
            PhisicalDeleteCriterionOption = 85103,
            /// <summary>
            /// Nuova opzione criterio
            /// </summary>
            AddCriterionOption = 85104,
            /// <summary>
            /// Salva impostazioni valutatori
            /// </summary>
            SaveEvaluatorsSettings = 85105,
            /// <summary>
            /// Salva assegnazione singola sottomissione
            /// </summary>
            SaveSingleSubmissionAssignmentForEvaluation = 85106,
            /// <summary>
            /// Visualizza sommario valutazioni
            /// </summary>
            ViewEvaluationsSummary = 85107,
            /// <summary>
            /// Visualizza sommario comitati
            /// </summary>
            ViewEvaluationsCommitteesSummary = 85108,
            /// <summary>
            /// Visualizza sommario comitati singoli
            /// </summary>
            ViewEvaluationsCommitteeSummary = 85109,
            /// <summary>
            /// Copia un bando
            /// </summary>
            CloneCall = 85110,
            /// <summary>
            /// Imposta opzione default
            /// </summary>
            SetAsDefaultOption = 85111,
            /// <summary>
            /// Modifica tipo avviso
            /// </summary>
            EditDisclaimerType = 85112,
            /// <summary>
            /// Caricamento file su bando anonimo
            /// </summary>
            UploadFileToUnknownCall = 85113,
            /// <summary>
            /// Allegato senza file
            /// </summary>
            AttachmentsNotAddedFiles= 85114,
            /// <summary>
            /// File allegato aggiunti
            /// </summary>
            AttachmentsAddedFiles= 85114,

            /// <summary>
            /// Cancellazione logica sottomissione
            /// </summary>
            VirtualUndeleteSubmission = 85120,
            /// <summary>
            /// controfirma caricata
            /// </summary>
            SignUploaded = 85130,

            /// <summary>
            /// Visualizzazione Processo valutazione
            /// </summary>
            /// <remarks>
            /// L'ID è riferito al bando.
            /// </remarks>
            AdvStepsView = 85141,
            /// <summary>
            /// Visualizzazione sommario Step
            /// </summary>
            AdvStepSummary = 85144,
            /// <summary>
            /// Crea nuovo step
            /// </summary>
            AdvStepAdd = 85145,
            /// <summary>
            /// Chiudi step, confermando i superamenti
            /// </summary>
            AdvStepClose = 85147,
            
            /// <summary>
            /// Nuova commissione
            /// </summary>
            AdvCommissionAdd = 85150,
            /// <summary>
            /// Modifica commissione
            /// </summary>
            AdvCommissionModify = 85151,
            /// <summary>
            /// Salva commissione
            /// </summary>
            AdvCommissionSave = 85152,
            /// <summary>
            /// Visualizzazione commissione
            /// </summary>
            AdvCommissionView = 85153,
            /// <summary>
            /// Visualizza sommario valutazioni commissione
            /// </summary>
            AdvCommissionSummary = 85154,
            
            /// <summary>
            /// Stato commissione: iniziata
            /// </summary>
            AdvCommissionStart = 85171,
            /// <summary>
            /// Stato commissione: bloccata
            /// </summary>
            AdvCommissionStop = 85173,
            /// <summary>
            /// Stato commissione: chiusa
            /// </summary>            
            AdvCommissionClose = 85177,
            /// <summary>
            /// Caricato verbale
            /// </summary>
            AdvCommissionUploadVerbal = 85178,
            /// <summary>
            /// Nuovo membro commissione
            /// </summary>
            AdvMemberAdd = 85157,
            /// <summary>
            /// Cancellazione membro commissione
            /// </summary>
            AdvMemberDelete = 85158,
            /// <summary>
            /// Modifica membro commissione
            /// </summary>
            AdvMemberModifiy = 85159,
            /// <summary>
            /// Modifica segretario commissione
            /// </summary>
            AdvSegretaryModifiy = 85160,
            /// <summary>
            /// Modifica presidente commissione
            /// </summary>
            AdvPresidentModifiy = 85161,
            /// <summary>
            /// Reimposta valutazione in bozza (presidente)
            /// </summary>
            AdvEvaluationSetDraft = 85181,
            /// <summary>
            /// Conferma valutazione (presidente)
            /// </summary>
            AdvEvaluationSetconfirm = 85182,
        }

        /// <summary>
        /// Tipo di oggetto Bando/Richiesta adesione
        /// </summary>
        [Serializable]
        public enum ObjectType{
            /// <summary>
            /// Nessun oggetto
            /// </summary>
            None = 0,
            /// <summary>
            /// Bando
            /// </summary>
            CallForPaper = 1,
            /// <summary>
            /// Sezione
            /// </summary>
            FieldsSection = 2,
            /// <summary>
            /// Definizione campo bando
            /// </summary>
            FieldDefinition = 3,
            /// <summary>
            /// Compilazione campo bando
            /// </summary>
            FieldValue = 4,
            /// <summary>
            /// File richiesta (bando)
            /// </summary>
            RequestedFile = 5,
            /// <summary>
            /// File sottomesso (sottomissione)
            /// </summary>
            SubmittedFile = 6,
            /// <summary>
            /// Tipo di sottomittore
            /// </summary>
            SubmitterType = 7,
            /// <summary>
            /// Sottomissione
            /// </summary>
            UserSubmission = 8,
            /// <summary>
            /// File allegato (bando)
            /// </summary>
            AttachmentFile = 9,
            /// <summary>
            /// Criterio di valutazione
            /// </summary>
            Criterion = 10,
            /// <summary>
            /// Valutatore/membro
            /// </summary>
            Evaluator = 11,
            /// <summary>
            /// Valutazione
            /// </summary>
            Evaluation = 12,
            /// <summary>
            /// Revisione
            /// </summary>
            Revision = 13,
            /// <summary>
            /// Verbale allegato alla commissione
            /// </summary>
            VerbaliCommissione = 15,
            /// <summary>
            /// Integrazioni al bando
            /// </summary>
            Integrazioni = 18,

            /// <summary>
            /// Step valutazione
            /// </summary>
            AdvStep = 20,
            /// <summary>
            /// Commissione avanzata
            /// </summary>
            AdvCommission = 21,
            /// <summary>
            /// Commissione step economico
            /// </summary>
            AdvEconomicCommission = 22,
            /// <summary>
            /// Valutazione avanzata (conferma presidente)
            /// </summary>
            AdvEvaluation = 23,
            /// <summary>
            /// Membro commissione avanzata
            /// </summary>
            AdvMember = 24,
            /// <summary>
            /// Opzione criterio
            /// </summary>
            CriterionOption = 25,
            /// <summary>
            /// Sommario Step
            /// </summary>
            /// <remarks>
            /// L'Id è relativo allo step.
            /// </remarks>
            AdvStepSummary = 26,
            /// <summary>
            /// Sommario Step Economico
            /// </summary>
            /// <remarks>
            /// L'ID è relativo alla commissione
            /// </remarks>
            AdvStepEcoSummary = 27

        }
    }
}