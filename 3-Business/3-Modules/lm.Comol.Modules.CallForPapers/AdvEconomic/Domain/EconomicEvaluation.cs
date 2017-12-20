using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using CPDomain = lm.Comol.Modules.CallForPapers.Domain;
using Advance = lm.Comol.Modules.CallForPapers.Advanced;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Domain
{
    /// <summary>
    /// Valutazione economica
    /// </summary>
    public class EconomicEvaluation : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Bando di riferimento
        /// </summary>
        public virtual CPDomain.BaseForPaper Call { get; set; }
        /// <summary>
        /// Step economico di riferimento
        /// </summary>
        public virtual Advance.Domain.AdvStep Step { get; set; }
        /// <summary>
        /// Commissione (economica)
        /// </summary>
        public virtual Advance.Domain.AdvCommission Commission { get; set; }
        /// <summary>
        /// Membro valutatore corrente
        /// </summary>
        public virtual Advance.Domain.AdvMember CurrentMember { get; set; }
        /// <summary>
        /// Commenti
        /// </summary>
        public virtual string Comment { get; set; }
        /// <summary>
        /// Sottomissione
        /// </summary>
        public virtual CPDomain.UserSubmission Submission { get; set; }
        /// <summary>
        /// Elenco tabelle economiche
        /// </summary>
        public virtual IList<EconomicTable> Tables { get; set; }
        /// <summary>
        /// Stato valutazione
        /// </summary>
        public virtual EvalStatus Status { get; set; }
        /// <summary>
        /// Graduatoria (da step precedente)
        /// </summary>
        public virtual Int32 Rank { get; set; }
        /// <summary>
        /// Punteggio medio (da step precedente)
        /// </summary>
        public virtual double AverageRating { get; set; }
        /// <summary>
        /// Punteggio totale (da step precedente)
        /// </summary>
        public virtual double SumRating { get; set; }
        /// <summary>
        /// Totale richiesto
        /// </summary>
        public virtual double RequestTotal
        {
            get
            {
                return Tables.Sum(t => t.RequestTotal);
            }
        }
        /// <summary>
        /// Totale ammesso
        /// </summary>
        public virtual double AdmitTotal
        {
            get
            {
                return Tables.Sum(t => t.AdmitTotal);
            }
        }
    }
}
