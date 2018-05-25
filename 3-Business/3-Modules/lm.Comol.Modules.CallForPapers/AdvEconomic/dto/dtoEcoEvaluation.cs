using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.dto
{
    /// <summary>
    /// dto Valutazione economica
    /// </summary>
    public class dtoEcoEvaluation
    {
        /// <summary>
        /// Id sottomissione
        /// </summary>
        public long SubmissionId { get; set; }
        /// <summary>
        /// Nome tipo sottomissione (da bando)
        /// </summary>
        public string SubmissionType { get; set; }
        /// <summary>
        /// Nome sottomittore
        /// </summary>
        public string SubmissionName { get; set; }
        /// <summary>
        /// elenco tabelle economiche
        /// </summary>
        public IList<dtoEcoEvTable> Tables { get; set; }
        /// <summary>
        /// Totale richiesto
        /// </summary>
        public double RequestTotal { get; set; }
        /// <summary>
        /// Totale ammesso
        /// </summary>
        public double AdmitTotal { get; set; }

        /// <summary>
        /// Totale ammesso
        /// </summary>
        public double AdmitMax { get; set; }

        /// <summary>
        /// stato valutazione
        /// </summary>
        public EvalStatus status { get; set; }
        /// <summary>
        /// Permessi: modifica
        /// </summary>
        public bool CanModify { get; set; }
        /// <summary>
        /// Permessi: puo' rimettere in bozza la valutazione
        /// </summary>
        public bool CanReopen { get; set; }
        /// <summary>
        /// Permessi: chiusura valutazione
        /// </summary>
        public bool CanClose { get; set; }
        /// <summary>
        /// Permessi: visualizzazione valutazione
        /// </summary>
        public bool CanView { get; set; }

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoEcoEvaluation()
        {
            CanModify = false;
            CanClose = false;
            CanView = false;
            Tables = new List<dtoEcoEvTable>();
        }

        /// <summary>
        /// Costruttore da oggetto di dominio
        /// </summary>
        /// <param name="evaluation">
        /// Oggetto dominio: valutazione economica
        /// </param>
        public dtoEcoEvaluation(lm.Comol.Modules.CallForPapers.AdvEconomic.Domain.EconomicEvaluation evaluation)
        {
            if (evaluation == null)
                return;

            if (evaluation.Submission != null)
            {
                SubmissionId = evaluation.Submission.Id;
                SubmissionType = (evaluation.Submission.Type != null) ? evaluation.Submission.Type.Name : "Unknow";

                SubmissionName = (evaluation.Submission.SubmittedBy != null) ?
                    evaluation.Submission.SubmittedBy.SurnameAndName :
                    evaluation.Submission.CreatedBy.SurnameAndName;

            }
            else
            {
                SubmissionId = 0;
                SubmissionType = "Unknow";
                SubmissionName = "Unknow";
            }

            AdmitMax = (evaluation.Commission != null) ? evaluation.Commission.MaxValue : 0;
            
            status = evaluation.Status;

            RequestTotal = evaluation.RequestTotal;
            AdmitTotal = evaluation.AdmitTotal;

            Tables = evaluation.Tables.Select(t => new dtoEcoEvTable(t)).ToList();

        }
    }
}
