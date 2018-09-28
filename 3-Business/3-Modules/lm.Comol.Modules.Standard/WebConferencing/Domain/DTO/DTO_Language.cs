using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    /// <summary>
    /// DTO dati lingua si sistema
    /// </summary>
    public class DTO_Language
    {
        /// <summary>
        /// Codice lingua
        /// </summary>
        public String Code { get; set; }
        /// <summary>
        /// Nome lingua
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Icona lingua (non utilizzato)
        /// </summary>
        public String Icon { get; set; }
    }
}
