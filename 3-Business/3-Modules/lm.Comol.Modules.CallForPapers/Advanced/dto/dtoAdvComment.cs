using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto recupero commenti (per visualizzazione Segretario)
    /// </summary>
    public class dtoAdvComment
    {
        /// <summary>
        /// Se la valutazione di riferimento è in bozza
        /// </summary>
        public bool isDraft { get; set; }
        /// <summary>
        /// Anagrafica valutatore
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// Commento
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Se è relativa ad uno specifico criterio
        /// (Se False è il commento generale della valutazione)
        /// </summary>
        public bool IsCriteria { get; set; }
        /// <summary>
        /// Nome cirterio (se riferita a criterio)
        /// </summary>
        public string CriteriaName { get; set; }
        /// <summary>
        /// Data riferimento (ultima modifica o creazione)
        /// </summary>
        public DateTime? SaveOn { get; set; }
    }
}
