using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Trap
{
    /// <summary>
    /// Id Action dei bandi
    /// </summary>
    [CLSCompliant(true)]
    public enum CallTrapId : int
    {
        /// <summary>
        /// Elenco bandi (utente)
        /// </summary>
        CallListUser = 59001,
        /// <summary>
        /// Elenco bandi (gestore)
        /// </summary>
        CallListManager = 59002,

        /// <summary>
        /// Visualizzazione bando per modifica
        /// </summary>
        CallEditView = 59010,
        /// <summary>
        /// Salvataggio modifiche bando
        /// </summary>
        CallEditSave = 59011,
        /// <summary>
        /// Salvataggio Tag (bando non in bozza)
        /// </summary>
        CallEditSaveTag = 59012,
        /// <summary>
        /// Modifica stato bando
        /// </summary>
        CallEditChangeStatus = 59013,


        /// <summary>
        /// Accesso a sottomissione
        /// </summary>
        SubmissionCompileView = 59030,
        /// <summary>
        /// Inizio sottomissione
        /// </summary>
        SubmissionCompileStart = 59031,
        /// <summary>
        /// Salvataggio in bozza sottomissione
        /// </summary>
        SubmissionCompileDelete = 59032,
        /// <summary>
        /// Salvataggio in bozza sottomissione
        /// </summary>
        SubmissionCompileSaveDraft = 59033,
        /// <summary>
        /// Sottomissione definitiva
        /// </summary>
        SubmissionCompileSubmit = 59035,
        /// <summary>
        /// Allega controfirma
        /// </summary>
        SubmissionCompileUploadSign = 59036,

        /// <summary>
        /// Visualizza sottomissione
        /// </summary>
        SubmissionView = 59037,

        /// <summary>
        /// Elenco sottomissioni
        /// </summary>
        SubmissionList = 59041,

        /// <summary>
        /// Elenco sottomissioni per valutazione
        /// </summary>
        EvaluationSubmissionList = 59060,
        /// <summary>
        /// Valuta sottomissione
        /// </summary>
        EvaluationSubmissionView = 59061,

        /// <summary>
        /// Visualizzazione valutazione
        /// </summary>
        EvaluationView = 59070,
        /// <summary>
        /// Salva in bozza la valutazione
        /// </summary>
        EvaluationSaveDraft = 59071,
        /// <summary>
        /// Salva definitivamente valutazione
        /// </summary>
        EvaluationSave = 59072,

        /// <summary>
        /// Visualizza processo di valutazione
        /// </summary>
        StepView = 59100,
        /// <summary>
        /// Aggiungi step
        /// </summary>
        SteppAdd = 59102,

        /// <summary>
        /// Visualizza commissione
        /// </summary>
        AdvCommissionView = 59120,
        /// <summary>
        /// Modifica dati commissione
        /// </summary>
        AdvCommissionSave = 59121,
        /// <summary>
        /// Modifica stato commissione
        /// </summary>
        AdvCommissionChangeStatus = 59122,
        /// <summary>
        /// Chiudi commissione
        /// </summary>
        AdvCommissionClose = 59123,

        /// <summary>
        /// Visualizza sommario commmissione
        /// </summary>
        AdvCommissionSummary = 59130,

        /// <summary>
        /// Visualizza elenco valutazioni
        /// </summary>
        AdvEvaluationsView = 59140,
        /// <summary>
        /// Reimposta in bozza una valutazione
        /// </summary>
        AdvEvaluationsSetDraft = 59141,
        /// <summary>
        /// Conferma valutazione
        /// </summary>
        AdvEvaluationsConfirm = 59145,
        
        /// <summary>
        /// Visualizza sommario step
        /// </summary>
        StepSummaryView = 59240,
        /// <summary>
        /// Chiudi lo step confermando le ammissioni
        /// </summary>
        StepClose = 59241
       
    }

    /// <summary>
    /// Tipo di oggetto associato all'azione
    /// </summary>
    [CLSCompliant(true)]
    public enum CallObjectId
    {
        /// <summary>
        /// Bando
        /// </summary>
        CallForPeaper,
        /// <summary>
        /// Sottomissione
        /// </summary>
        UserSubmission,
        /// <summary>
        /// valutazione
        /// </summary>
        Evaluation,
        /// <summary>
        /// Commissione
        /// </summary>
        AdvCommission,
        /// <summary>
        /// Step
        /// </summary>
        Step

    }

    //public enum StepActionType : int
    //{
    //    /// <summary>
    //    /// Nessun azione: non in uso
    //    /// </summary>
    //    none = 0,
    //    /// <summary>
    //    /// Errore azione
    //    /// </summary>
    //    Error = -1,
    //    /// <summary>
    //    /// Elenco oggetti (info base)
    //    /// </summary>
    //    List = 1,
    //    /// <summary>
    //    /// Visualizzazione oggetto (tutti i dati)
    //    /// </summary>
    //    View = 2,
    //    /// <summary>
    //    /// Modifica oggetto
    //    /// </summary>
    //    Update = 6,
    //}
}
