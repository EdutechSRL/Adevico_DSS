using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Advanced.Domain
{
    /// <summary>
    /// Step commissioni
    /// </summary>
    public class AdvStep : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Bando di riferimento
        /// </summary>
        public virtual BaseForPaper Call { get; set; }
        /// <summary>
        /// Nome dello step
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Ordine di visualizzazione
        /// </summary>
        public virtual int Order { get; set; }
        /// <summary>
        /// Statp dello step
        /// </summary>
        public virtual StepStatus Status { get; set; }
        /// <summary>
        /// Tipo di step
        /// </summary>
        public virtual StepType Type { get; set; }
        /// <summary>
        /// Tipo di valutazione (Media/Somma)
        /// </summary>
        public virtual EvalType EvalType { get; set; }

        /// <summary>
        /// Commissioni che compongono lo step
        /// </summary>
        public virtual IList<AdvCommission> Commissions { get; set; }
    }
}
