using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.Helpers
{
    /// <summary>
    /// Helper esportazione sommario commissione.
    /// Trasforma i dati di sintesi di una commissione in un HTML per la successiva trasformazione in documenti (PDF, rtf, etc...)
    /// Modificando le stringhe dei formati è possibile modificare l'HTML prodotto oppure ottenere esportazioni di altro tipo.
    /// </summary>
    public class CommissionSummaryHTMLexpHelper
    {
        /// <summary>
        /// "<table>{0}{1}</table>"
        /// 0: header
        /// 1: content (body)
        /// </summary>
        public string ContainerFormat = "<table style='border:1px solid black'>{0}{1}</table>";
        /// <summary>
        /// Formato intestazione tabella
        /// </summary>
        public string HeaderFormat = "<thead><tr>{0}</tr></thead>";
        /// <summary>
        /// Formato elemento
        /// </summary>
        public string HeaderItemFormat = "<th style='border:1px solid black; background-color:#ccc;'><b>{0}</b></th>";
        
        /// <summary>
        /// Formato contenuto tabella (body)
        /// </summary>
        public string ContentFormat = "<tbody>{0}</tbody>";
        /// <summary>
        /// Formato riga (tr)
        /// </summary>
        public string ContentRow = "{0}<tr>{1}</tr>";
        /// <summary>
        /// Formato elemento (td)
        /// </summary>
        public string ContentItemFormat = "<td style='border:1px solid black'>{0}</td>";

        /// <summary>
        /// "{0}{1}{2}{3}{4}{5}{6}{7}"
        /// 0: rank
        /// 1: sottomessa da
        /// 2: Tipo sottomissione
        /// 3: Sottomessa il
        /// 4: Punteggio
        /// 5: Boolan
        /// 6: Approvato
        /// 7: Ammesso
        /// </summary>
        public string ItemContent = "{0}{1}{2}{3}{4}{5}{6}{7}";

        /// <summary>
        /// Internazionalizzione rank
        /// </summary>
        public string SummaryRank = "Rank";
        /// <summary>
        /// Internazionalizzione sottomittore
        /// </summary>
        public string SummarySubmission = "Sottomessa da";
        /// <summary>
        /// Internazionalizzione tipo sottomissione
        /// </summary>
        public string SummarySubmissionType = "Tipo sottomissione";
        /// <summary>
        /// Internazionalizzione data sottomissione
        /// </summary>
        public string SummarySubmitOn = "Sottomessa il";
        /// <summary>
        /// Internazionalizzione punteggio
        /// </summary>
        public string SummaryScore = "Punteggio";
        /// <summary>
        /// Internazionalizzione valutazioni binarie
        /// </summary>
        public string SummaryBool = "Boolean";
        /// <summary>
        /// Internazionalizzione superamento criteri
        /// </summary>
        public string SummarySuccess = "Criteri superati";
        /// <summary>
        /// Internazionalizzione ammissione
        /// </summary>
        public string SummaryAdmit = "Approvato";

        /// <summary>
        /// Internazionalizzione valore ammissione (ammesso)
        /// </summary>
        public string ValueAdmit = "Approvato";
        /// <summary>
        /// Internazionalizzione valore ammissione (non ammesso)
        /// </summary>
        public string ValueNotAdmit = "Respinto";

        /// <summary>
        /// Internazionalizzione valore superamento (superato)
        /// </summary>
        public string ValuePass = "Approvato";
        /// <summary>
        /// Internazionalizzione valore superamento (non superato)
        /// </summary>
        public string ValueNotPass = "Respinto";

        /// <summary>
        /// Recupera l'HTML del sommario di una commissione
        /// </summary>
        /// <param name="Submissions">Elenco oggetti di dominio sottomissione/valutazione</param>
        /// <returns>Stringa di report</returns>
        public string Summary(IList<Advanced.Domain.AdvSubmissionToStep> Submissions)
        {
            string HTML = "";

            if (Submissions == null || !Submissions.Any())
                return HTML;


            string HtmlHeader = string.Format(ItemContent,
                String.Format(HeaderItemFormat, SummaryRank),
                String.Format(HeaderItemFormat, SummarySubmission),
                String.Format(HeaderItemFormat, SummarySubmissionType),
                String.Format(HeaderItemFormat, SummarySubmitOn),
                String.Format(HeaderItemFormat, SummaryScore),
                String.Format(HeaderItemFormat, SummaryBool),
                String.Format(HeaderItemFormat, SummarySuccess),
                String.Format(HeaderItemFormat, SummaryAdmit)
                );

            HtmlHeader = String.Format(HeaderFormat, HtmlHeader);

            string HtmlContent = "";

            bool useSum = false;

            try
            {
                useSum = Submissions.FirstOrDefault().Commission.EvalType == EvalType.Sum;
            } catch { }
                


            foreach(Advanced.Domain.AdvSubmissionToStep sub in Submissions.OrderBy(sub => sub.Rank))
            {
                if(sub != null && sub.Submission != null && sub.Submission.Type != null)
                {
                    HtmlContent = String.Format(ContentRow, HtmlContent,
                    String.Format(ItemContent,
                        String.Format(ContentItemFormat, sub.Rank),
                        String.Format(ContentItemFormat, (sub.Submission.SubmittedBy != null) ? sub.Submission.SubmittedBy.SurnameAndName : sub.Submission.CreatedBy.SurnameAndName),
                        String.Format(ContentItemFormat, sub.Submission.Type.Name),
                        String.Format(ContentItemFormat, (sub.Submission.SubmittedOn != null) ? sub.Submission.SubmittedOn.ToString() : "--"),
                        String.Format(ContentItemFormat, (useSum) ? sub.SumRating : sub.AverageRating),
                        String.Format(ContentItemFormat, (sub.BoolRating) ? ValuePass : ValueNotPass),
                        String.Format(ContentItemFormat, (sub.Passed) ? ValuePass : ValueNotPass),
                        String.Format(ContentItemFormat, (sub.Admitted) ? ValueAdmit : ValueNotAdmit)
                        )
                    );
                }
            }

            HtmlContent = String.Format(ContentFormat, HtmlContent);

            HTML = String.Format(ContainerFormat,
                        HtmlHeader,
                        HtmlContent
                        );
            
            return HTML;
        }

    }
}
