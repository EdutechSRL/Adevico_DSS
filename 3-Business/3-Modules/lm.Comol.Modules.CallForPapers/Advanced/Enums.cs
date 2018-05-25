using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced
{
    /// <summary>
    /// Stati commissione per icone
    /// Rivedere stati per membri/utente/etc...
    /// </summary>
    public enum CommissionStatus : int
    {
        /// <summary>
        /// Draft: possibilità di modifica/appena creata
        /// </summary>
        Draft = 0,
        /// <summary>
        /// Come bozza: possibilità di modifica, ma i valutatori/membri possono accedere per visualizzare le sottomissioni, ma senza possibilità di valutare.
        /// </summary>
        ViewSubmission = 1,
        /// <summary>
        /// Le valutazioni in corso
        /// </summary>
        Started = 2,
        /// <summary>
        /// Bloccata: permette la sostituzione di un membro valutatore
        /// </summary>
        Locked = 6,
        /// <summary>
        /// Valutazione conclusa
        /// </summary>
        ValutationEnded = 8,
        /// <summary>
        /// Valutazione confermata (sostituito da "Closed")
        /// </summary>
        ValutationConfirmed = 9,
        /// <summary>
        /// Commissione chiusa
        /// </summary>
        Closed = 10,
    }

    /// <summary>
    /// Stato dello step
    /// </summary>
    public enum StepStatus : int
    {
        /// <summary>
        /// Bozza
        /// </summary>
        Draft = 0,
        /// <summary>
        /// Unused
        /// </summary>
        Open = 1,
        /// <summary>
        /// Almeno una commissione ha iniziato le valutazioni
        /// </summary>
        Started = 2,
        /// <summary>
        /// Unused
        /// </summary>
        Locked = 4,
        /// <summary>
        /// Le commissioni hanno completato le loro valutazioni.
        /// Il presidente della commissione MASTER deve confermare chi passa allo step successivo
        /// </summary>
        WaitforClosing = 6,
        /// <summary>
        /// Step concluso
        /// </summary>
        Closed = 10
    }

    /// <summary>
    /// Azioni che un utente può compiere nella commissione
    /// </summary>
    [Flags]
    public enum CommissionAction
    {
        /// <summary>
        /// Nessuna azione
        /// </summary>
        None = 0,
        /// <summary>
        /// Rimozione commissinoe
        /// </summary>
        Remove = 1 << 0,
        /// <summary>
        /// Aggiungi nuova commissione
        /// </summary>
        Add = 1 << 1,
        /// <summary>
        /// Modifica commissione
        /// </summary>
        EditCommission = 1 << 2,
        /// <summary>
        /// Modifica membri commissione
        /// </summary>
        EditMembers = 1 << 3,
        /// <summary>
        /// Visualizza commissione
        /// </summary>
        ShowCommission = 1 << 4,
        /// <summary>
        /// Downlad dei report della commissione
        /// </summary>
        DownloadReport = 1 << 5
    }
    /// <summary>
    /// Tipo di step
    /// </summary>
    public enum StepType
    {
        /// <summary>
        /// Validazione: sempre e solo al primo posto
        /// </summary>
        validation = 0,
        /// <summary>
        /// Economico
        /// </summary>
        /// <remarks>
        /// SEMPRE in coda agli altri step.
        /// Presente SOLO se nel bando è presente ALMENO UNA tabella economica
        /// </remarks>
        economics = 1,
        /// <summary>
        /// Step di valutazione
        /// </summary>
        custom = 10
    }
    
    /// <summary>
    /// Schermate di edit di una commissione
    /// </summary>
    public enum CommiteeEditPage : int
    {
        /// <summary>
        /// Nessuna: non in uso
        /// </summary>
        none= 0,
        /// <summary>
        /// Gestione dei membri della commissione
        /// </summary>
        Members = 1,
        /// <summary>
        /// Gestione dei criteri della commissione
        /// </summary>
        Criterion = 2,
        /// <summary>
        /// Per sviluppi futuri: assegnazione sottomissioni ai membri
        /// </summary>
        Submission = 10
    }



    /// <summary>
    /// Permessi commissione
    /// </summary>
    [Flags]
    public enum CommissionPermission
    {
        /// <summary>
        /// Nessun permesso
        /// </summary>
        None = 0,
        /// <summary>
        /// Visualizzazione dati generici commissione (nome e stato)
        /// </summary>
        View = 1 << 0,
        /// <summary>
        /// Modifica commissione (creatore e Presidente)
        /// </summary>
        Edit = 1 << 2,
        /// <summary>
        /// Apertura valutazioni: solo presidente
        /// </summary>
        OpenEvaluation = 1 << 3,
        /// <summary>
        /// Caricamento verbale: presidente
        /// </summary>
        UploadVerbale = 1 << 4,
        /// <summary>
        /// Valutazione: tutti i membri
        /// </summary>
        Evaluate = 1 << 5,
        /// <summary>
        /// Richiesta integrazione: segretario
        /// </summary>
        RequestIntegration = 1 << 6,
        /// <summary>
        /// Chiusura valutazioni: presidente
        /// </summary>
        CloseEvaluation = 1 << 7,
        /// <summary>
        /// Chiusura commissione: presidente
        /// </summary>
        CloseCommission = 1 << 8

    }
    
    /// <summary>
    /// Permessi elenco sottomissioni
    /// </summary>
    [Flags]
    public enum SubmissionListPermission
    {
        /// <summary>
        /// Nessun permesso
        /// </summary>
        None = 0,
        /// <summary>
        /// Sola visualizzazione
        /// </summary>
        View = 1 << 0,
        /// <summary>
        /// Valutazione sottomissioni
        /// </summary>
        Evaluate = 1 << 2,
        /// <summary>
        /// Gestione: Segretario o presidente
        /// </summary>
        Manage = 1 << 3
        
    }

    /// <summary>
    /// Permessi valutazioni avanzate
    /// </summary>
    [Flags]
    public enum EvaluationAdvPermission
    {
        /// <summary>
        /// Nessun permesso
        /// </summary>
        None = 0,
        /// <summary>
        /// Visualizza sottomissione
        /// </summary>
        View = 1 << 0,
        /// <summary>
        /// Valuta sottomissione
        /// </summary>
        Evaluate = 1 << 2
    }

    /// <summary>
    /// Feedback modifica stato commissione
    /// </summary>
    public enum CommissionStatusFeedback
    {
        /// <summary>
        /// Errore sconosciuto/itnerno
        /// </summary>
        Unknow = -2,
        /// <summary>
        /// Commissione non trovata/cancellata
        /// </summary>
        NotFount = -1,
        /// <summary>
        /// Modifica avvenuta con successo
        /// </summary>
        Success = 0,
        /// <summary>
        /// Non ci sono le condizioni necessarie per la modifica (logiche business)
        /// </summary>
        NotPermitted = 1,
        /// <summary>
        /// Non si dispone dei permessi per modificare lo stato della commissione
        /// </summary>
        NoPermission = 2
    }

    /// <summary>
    /// Tipo valutazione
    /// </summary>
    public enum EvalType
    {
        /// <summary>
        /// Nessuna (non usato)
        /// </summary>
        none = 0,
        /// <summary>
        /// Media
        /// </summary>
        Average = 1,
        /// <summary>
        /// Somma
        /// </summary>
        Sum = 2
    }

    /// <summary>
    /// Tipo richiesta di integrazione
    /// </summary>
    public enum IntegrationType : int
    {
        /// <summary>
        /// Solo testo
        /// </summary>
        Text = 1,
        /// <summary>
        /// Testo e file
        /// </summary>
        TextnFile = 2
    }
    /// <summary>
    /// Permessi generici (tipo attore)
    /// </summary>
    [Flags]
    public enum GenericStepPermission
    {
        /// <summary>
        /// Nessun permesso
        /// </summary>
        none = 0,
        /// <summary>
        /// Sottomittore: chi sottomette il bando
        /// </summary>
        Submitter = 1<<0,
        /// <summary>
        /// Membro valutatore
        /// </summary>
        Member = 1 << 1,
        /// <summary>
        /// Segretario
        /// </summary>
        Secretary = 1 << 2,
        /// <summary>
        /// Presidente
        /// </summary>
        President = 1 << 3,
        /// <summary>
        /// Presidente commissione default
        /// </summary>
        MainPresident = 1 << 4,
        /// <summary>
        /// Presidente commissione default
        /// </summary>
        MainSecretary = 1 << 5
    }

}
