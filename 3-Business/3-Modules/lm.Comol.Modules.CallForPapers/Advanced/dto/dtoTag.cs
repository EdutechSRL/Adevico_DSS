using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto Tag
    /// </summary>
    public class dtoTag
    {
        /// <summary>
        /// Id campo bando
        /// </summary>
        public long FieldId { get; set; }
        /// <summary>
        /// Stringa con i tag, separati da virgola
        /// </summary>
        public string Tags { get; set; }
    }
}
