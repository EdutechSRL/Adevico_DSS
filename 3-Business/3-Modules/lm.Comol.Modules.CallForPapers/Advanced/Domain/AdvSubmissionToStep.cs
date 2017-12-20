using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;


namespace lm.Comol.Modules.CallForPapers.Advanced.Domain
{
        /// <summary>
        /// Legame tra le sottomissioni e gli step
        /// </summary>
        /// <remarks>
        /// - CREATO in APERTURA di uno step
        /// Se STEP di VALIDAZIONE:
        /// - vengono assegnate TUTTE le sottomissioni a TUTTI i membri della commissione
        /// Se ALTRI step:
        /// - vengono assegnate SOLO le sottomissioni che hanno SUPERATO lo step precedente (selezione del presidente)
        /// 
        /// In Futuro potrà essere utilizzata per assegnare sottomissioni specifiche a membri specifici.
        /// In questo caso andrà aggiunta la colonna "Membro valutatore".
        /// </remarks>
    public class AdvSubmissionToStep : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Step di riferiemnto
        /// </summary>
        public virtual AdvStep Step { get; set; }
        /// <summary>
        /// Commissione di riferimento
        /// </summary>
        /// <remarks>
        /// SE è "NULL" si tratta delle sottomissioni che hanno raggiunto lo step di riferimento
        /// </remarks>
        public virtual AdvCommission Commission { get; set; }
        //public AdvMember Member { get; set; }   //Assegnata a singolo membro

        /// <summary>
        /// Sottomissione
        /// </summary>
        public virtual UserSubmission Submission { get; set; }
        /// <summary>
        /// Se la sottomissione ha superato TUTTI i criteri previsti per TUTTI i membri.
        /// Calcolata in fase di valutazione
        /// </summary>
        public virtual bool Passed { get; set; }
        /// <summary>
        /// Media delle valutazioni
        /// </summary>
        public virtual double AverageRating { get; set; }
        /// <summary>
        /// Somma delle valutazioni
        /// </summary>
        public virtual double SumRating { get; set; }
        /// <summary>
        /// SE ha superato TUTTI i criteri booleani
        /// </summary>
        public virtual bool BoolRating { get; set; }
        /// <summary>
        /// Graduatoria
        /// </summary>
        public virtual int Rank { get; set; }
        /// <summary>
        /// Se la sottomissione viene ammessa agli step successivi (Solo se di tipo STEP)
        /// </summary>
        public virtual bool Admitted { get; set; }


    }
}
