using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adevico.WebSNMTP.dto
{
    /// <summary>
    /// dto paramtro: coppia Id-Valore
    /// </summary>
    [CLSCompliant(true)]
    public class dtoIdValue
    {
        /// <summary>
        /// Id parametro
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Valore
        /// </summary>
        public string Value { get; set; }
    }
}