using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eco = lm.Comol.Modules.CallForPapers.AdvEconomic;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.dto
{
    /// <summary>
    /// dto contenitore sommario valutazioni economiche
    /// </summary>
    public class dtoEcoSummaryContainer
    {
        /// <summary>
        /// Elenco elemeti valutazione (sommari)
        /// </summary>
        public IList<Eco.dto.dtoEcoSummary> Summaries { get; set; }
        /// <summary>
        /// Id step
        /// </summary>
        public long StepId { get; set; }
        /// <summary>
        /// Id bando
        /// </summary>
        public long CallId { get; set; }

        /// <summary>
        /// Permessi: accesso generico (lettura)
        /// </summary>
        public bool hasPermission { get; set; }
        /// <summary>
        /// Permessi: puo' chiudere la commissione
        /// </summary>
        public bool CanCloseCommission { get; set; }

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoEcoSummaryContainer()
        {
            Summaries = new List<Eco.dto.dtoEcoSummary>();
            hasPermission = false;
            CanCloseCommission = false;
        }


        //Dati Commissione/Call
    }
}
