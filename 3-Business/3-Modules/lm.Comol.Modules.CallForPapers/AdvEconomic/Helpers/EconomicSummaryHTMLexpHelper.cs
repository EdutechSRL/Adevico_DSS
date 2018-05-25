using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eco = lm.Comol.Modules.CallForPapers.AdvEconomic;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Helpers
{
    /// <summary>
    /// Helper esportazione in HTML per creazione PDF/rtf/etc...
    /// </summary>
    public class EconomicSummaryHTMLexpHelper
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
        /// Formato elemento intestazione
        /// </summary>
        public string HeaderItemFormat = "<th style='border:1px solid black; background-color:#ccc;'><b>{0}</b></th>";

        /// <summary>
        /// Formato content tabella (tbody)
        /// </summary>
        public string ContentFormat = "<tbody>{0}</tbody>";
        /// <summary>
        /// Formato contenitore righa (tr)
        /// </summary>
        public string ContentRow = "{0}<tr>{1}</tr>";
        /// <summary>
        /// Formato contenitore elemento (td)
        /// </summary>
        public string ContentItemFormat = "<td style='border:1px solid black'>{0}</td>";

        /// <summary>
        /// Formato elemento:
        /// "{0}{1}{2}{3}{4}{5}{6}{7}"
        /// 0: rank
        /// 1: sottomessa da
        /// 2: Tipo sottomissione
        /// 3: Sottomessa il
        /// 4: Punteggio
        /// 5: Ammesso
        /// 6: Finanziato
        /// </summary>
        public string ItemContent = "{0}{1}{3}{4}{5}{6}";

        /// <summary>
        /// Localizzazione: rank
        /// </summary>
        public string SummaryRank = "Rank";
        /// <summary>
        /// Localizzazione: sottomittore
        /// </summary>
        public string SummarySubmission = "Sottomessa da";
        /// <summary>
        /// Localizzazione: tipo sottomissione
        /// </summary>
        public string SummarySubmissionType = "Tipo sottomissione";
        /// <summary>
        /// Localizzazione: data sottomissione
        /// </summary>
        public string SummarySubmitOn = "Sottomessa il";
        /// <summary>
        /// Localizzazione: punteggio
        /// </summary>
        public string SummaryScore = "Punteggio";
        /// <summary>
        /// Localizzazione: ammissione (step precedente)
        /// </summary>
        public string SummaryAdmit = "Ammesso";
        /// <summary>
        /// Localizzazione: finanziato
        /// </summary>
        public string SummaryFunded = "Finanziato";

        /// <summary>
        /// Localizzazione: valore ammissione (ammesso)
        /// </summary>
        public string ValueAdmit = "Ammesso";
        /// <summary>
        /// Localizzazione: valore ammissione (non ammesso)
        /// </summary>
        public string ValueNotAdmit = "Non ammesso";

        /// <summary>
        /// Localizzazione: valore finanziato (non finanziato).
        /// Se finanziato viene mostrato il valore finanziato
        /// </summary>
        public string ValueNotFund = "Non finanziato";

        /// <summary>
        /// Formato valuta (Il simbolo Euro viene aggiunto hardcoded)
        /// </summary>
        public string CurrencyFormat = "C2";
        /// <summary>
        /// Formato lingua valuta
        /// </summary>
        public string CurrencyCulture = "it-IT";

        /// <summary>
        /// Trasforma il dtoSummaryContainer in stringa HTML (tabella)
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public string EcoSummaryGetHTMLTable(Eco.dto.dtoEcoSummaryContainer Table)
        {
            string HTML = "";

            string header = EcoSummGetHeader();

            string content = "";

            foreach(Eco.dto.dtoEcoSummary item in Table.Summaries.OrderBy(s => s.Rank))
            {
                string row = string.Format(ItemContent,
                    string.Format(ContentItemFormat, item.Rank),
                    string.Format(ContentItemFormat, item.SubmissionName),
                    string.Format(ContentItemFormat, item.SubmissionType),
                    string.Format(ContentItemFormat, (item.SubmittedOn != null) ? item.SubmittedOn.ToString() : "--"),
                    string.Format(ContentItemFormat, item.AverageRating.ToString("F2")),
                    string.Format(ContentItemFormat, ValueAdmit),
                    string.Format(ContentItemFormat, (item.Founded > 0) ?
                        item.Founded.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture))
                        : ValueNotFund)
                    );
                content = string.Format(ContentRow, content, row);
            }
            
            HTML = string.Format(
                ContainerFormat, 
                header, 
                string.Format(ContentFormat, content)
                );

            return HTML;
        }

        /// <summary>
        /// Crea l'intestazione della tabella
        /// </summary>
        /// <returns></returns>
        private string EcoSummGetHeader()
        {
            string HTML = "";
            
            HTML = string.Format(ItemContent,
                string.Format(HeaderItemFormat, SummaryRank),
                string.Format(HeaderItemFormat, SummarySubmission),
                string.Format(HeaderItemFormat, SummarySubmissionType),
                string.Format(HeaderItemFormat, SummarySubmitOn),
                string.Format(HeaderItemFormat, SummaryScore),
                string.Format(HeaderItemFormat, SummaryAdmit),
                string.Format(HeaderItemFormat, SummaryFunded)
                );
            
            return string.Format(HeaderFormat,
                HTML);
        }

      
    }
}
