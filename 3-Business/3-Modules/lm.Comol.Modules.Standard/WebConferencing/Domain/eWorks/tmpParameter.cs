using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks
{
    /// <summary>
    /// Classe per recupero parametri, del tipo chiave/valore
    /// </summary>
    /// <remarks>
    /// Forse sostituibile con KeyValuePair(String, string)
    /// </remarks>
    public class tmpParameter
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
