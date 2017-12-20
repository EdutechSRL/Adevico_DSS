using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.dto
{
    /// <summary>
    /// dto Commario valutazioni economiche
    /// </summary>
    public class dtoEcoSummary
    {
        /// <summary>
        /// Id sottomissione
        /// </summary>
        public long SubmissionId { get; set; }
        /// <summary>
        /// Id bando
        /// </summary>
        public long CallId { get; set; }
        /// <summary>
        /// Id Step
        /// </summary>
        public long StepId { get; set; }
        /// <summary>
        /// Id Valutazione
        /// </summary>
        public long EvalautionId { get; set; }
        
        /// <summary>
        /// Graduatoria
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// Nome sottomittore
        /// </summary>
        public string SubmissionName { get; set; }
        /// <summary>
        /// Nome tipo sottomissione
        /// </summary>
        public string SubmissionType { get; set; }
        /// <summary>
        /// Data sottomissione
        /// </summary>
        public DateTime? SubmittedOn { get; set; }
        /// <summary>
        /// Membro valutatore corrente
        /// </summary>
        public string CurrentMember { get; set; }
        /// <summary>
        /// Stato valutazione
        /// </summary>
        public EvalStatus status { get; set; }

        /// <summary>
        /// Media votazioni
        /// </summary>
        public virtual double AverageRating { get; set; }
        /// <summary>
        /// Somma votazioni
        /// </summary>
        public virtual double SumRating { get; set; }

        /// <summary>
        /// Totale ammesso
        /// </summary>
        public virtual double Founded { get; set; }

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoEcoSummary() { }

        /// <summary>
        /// Costruttore da oggetto di dominio: valutazione (economica)
        /// </summary>
        /// <param name="evaluation"></param>
        public dtoEcoSummary(lm.Comol.Modules.CallForPapers.AdvEconomic.Domain.EconomicEvaluation evaluation)
        {
            if (evaluation == null)
                return;

            EvalautionId = evaluation.Id;
            status = evaluation.Status;
            
            if (evaluation.Submission != null)
            {
                SubmissionId = evaluation.Submission.Id;
                SubmissionType = (evaluation.Submission.Type != null) ? evaluation.Submission.Type.Name : "Unknow";

                SubmissionName = (evaluation.Submission.SubmittedBy != null) ?
                    evaluation.Submission.SubmittedBy.SurnameAndName :
                    evaluation.Submission.CreatedBy.SurnameAndName;

                SubmittedOn = (evaluation.Submission != null) ? evaluation.Submission.SubmittedOn : null;
            } else
            {
                SubmissionId = 0;
                SubmissionType = "Unknow";
                SubmissionName = "Unknow";
            }

            CallId = (evaluation.Call != null) ? evaluation.Call.Id : 0;
            StepId = (evaluation.Step != null) ? evaluation.Step.Id : 0;

            CurrentMember = (evaluation.CurrentMember != null && evaluation.CurrentMember.Member != null) ?
                evaluation.CurrentMember.Member.SurnameAndName : "";
            
            Rank = evaluation.Rank;

            AverageRating = evaluation.AverageRating;
            SumRating = evaluation.SumRating;

            Founded = evaluation.Tables.Sum(t => t.AdmitTotal);
        }
    }
}
