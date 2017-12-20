using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// Elmeneto sommario valutazione sottomissione
    /// </summary>
    public class dtoStepSummaryItem
    {
        /// <summary>
        /// Id commissione
        /// </summary>
        public long ComissionId { get; set; }
        /// <summary>
        /// Nome commissione
        /// </summary>
        public string ComissionName { get; set; }
        /// <summary>
        /// Se l'elemento è riferito allo step (globale)
        /// </summary>
        public bool isStep { get; set; }
        /// <summary>
        /// Media valutazioni
        /// </summary>
        public virtual double AverageRating { get; set; }
        /// <summary>
        /// Somma valutazioni
        /// </summary>
        public virtual double SumRating { get; set; }
        /// <summary>
        /// Stringa media valutazioni
        /// </summary>
        /// <param name="decimals">Numero decimali previsti: default = 2</param>
        /// <returns></returns>
        public String AverageRatingToString(int decimals = 2)
        {
            return GetDoubleToString(AverageRating, decimals);
        }
        /// <summary>
        /// Stringa somma valutazioni
        /// </summary>
        /// <param name="decimals">Numero decimali previsti: default = 2</param>
        /// <returns></returns>
        public String SumRatingToString(int decimals = 2)
        {
            return GetDoubleToString(SumRating, decimals);
        }
       /// <summary>
       /// Converte un double in stringa
       /// </summary>
       /// <param name="number">Numero da convertire</param>
       /// <param name="decimals">Numero decimali</param>
       /// <returns></returns>
        private String GetDoubleToString(double number, int decimals = 2)
        {
            Double fractional = number - Math.Floor(number);
            return (fractional == 0) ? String.Format("{0:N0}", number) : String.Format("{0:N" + decimals.ToString() + "}", number);
        }


        /// <summary>
        /// Punteggio minimo
        /// </summary>
        public virtual int minScore { get; set; }
        /// <summary>
        /// Numero criteri booleani superato
        /// </summary>
        public virtual bool BoolRating { get; set; }
        /// <summary>
        /// Numero criteri booleani totali
        /// </summary>
        public virtual int BoolTotal { get; set; }
        /// <summary>
        /// SE ha superato tutti i criteri previsti
        /// </summary>
        public virtual bool Passed { get; set; }
        /// <summary>
        /// Stato commissione
        /// </summary>
        public CommissionStatus Status { get; set; }
        /// <summary>
        /// Se è ammesso allo step successivo
        /// </summary>
        public virtual bool Admit { get; set; }

        /// <summary>
        /// Indica se nella commissione/step ci sonodei criteri numerici
        /// </summary>
        public virtual bool HasScoreCriteria { get; set; }

        public virtual EvalType EvaluationType { get; set; }
    }
}
