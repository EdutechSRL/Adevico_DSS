using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic
{
    /// <summary>
    /// Stato valutazione economica
    /// </summary>
    public enum EvalStatus : int
    {
        /// <summary>
        /// In bozza
        /// </summary>
        draft = 0,
        /// <summary>
        /// Preso in carico (non utilizzato)
        /// </summary>
        take = 1,
        /// <summary>
        /// Valutazione completata
        /// </summary>
        completed = 2,
        /// <summary>
        /// Valutazione confermata dal presidente
        /// </summary>
        confirmed = 3
    }
}
