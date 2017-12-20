using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// Sommario sottomissioni
    /// </summary>
    public class dtoStepSummarySubmission
    {
        /// <summary>
        /// Id Sottomissione
        /// </summary>
        public long SubmissionId { get; set; }
        /// <summary>
        /// Id valutazione
        /// </summary>
        public long StepEvalId { get; set; }
        /// <summary>
        /// Nome sottomissione
        /// </summary>
        public string SubmissionName { get; set; }
        /// <summary>
        /// Posizione in classifica sottomissione
        /// </summary>
        public int SubmissionRank { get; set; }
        /// <summary>
        /// Elenco sommari commissioni
        /// </summary>
        public IList<dtoStepSummaryItem> Commissions { get; set; }
        

    }
}
