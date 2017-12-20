using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto Info valutazione
    /// </summary>
    public class dtoCommEvalInfo
    {
        /// <summary>
        /// Tipo valutazione (Edit commissione)
        /// </summary>
        public EvalType Type { get; set; }
        /// <summary>
        /// Punteggio minimo per superamento
        /// </summary>
        public int minRange { get; set; }
        /// <summary>
        /// Se i criteri booleani sono vincolanti
        /// </summary>
        public bool LockBool { get; set; }
    }
}
