using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// Sommario step (graduatorie)
    /// </summary>
    public class dtoStepSummary
    {
        /// <summary>
        /// Id step
        /// </summary>
        public long StepId { get; set; }
        /// <summary>
        /// Nome step
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// Stato Step
        /// </summary>
        public StepStatus Status { get; set; }
        /// <summary>
        /// Id bando di riferiemnto
        /// </summary>
        public long CallId { get; set; }
        /// <summary>
        /// Id commissione
        /// </summary>
        public long CommissionId { get; set; }
        /// <summary>
        /// Tipo valutazione
        /// </summary>
        public EvalType evType { get; set; }
        /// <summary>
        /// Elenco sommario sottomissioni
        /// </summary>
        public IList<dtoStepSummarySubmission> items { get; set; }
        /// <summary>
        /// Lista commissioni
        /// </summary>
        public IList<dtoCommToreport> Commissions { get; set; }
        /// <summary>
        /// Indica se è possibile chiudere lo step, confermando le sottomissioni che superano lo step
        /// SOLO per presidente, SOLO se step in stato di attessa conferma
        /// </summary>
        public bool isForClosing { get; set; }
    }
}
