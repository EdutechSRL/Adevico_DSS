using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Advanced.Domain
{
    /// <summary>
    /// Membro commissione
    /// </summary>
    public class AdvMember : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Commissione di riferimento
        /// </summary>
        public virtual AdvCommission Commission { get; set; }
        /// <summary>
        /// Dati del valutatore corrente
        /// </summary>
        public virtual litePerson Member { get; set; }
        /// <summary>
        /// Indica se il valutatore è il presidente della commissione
        /// </summary>
        public virtual bool IsPresident { get; set; }
        /// <summary>
        /// Verifica se il valutatore è stato sostituito in precedenza
        /// </summary>
        public virtual bool HasSubstitution { get; set; }
    }
}
