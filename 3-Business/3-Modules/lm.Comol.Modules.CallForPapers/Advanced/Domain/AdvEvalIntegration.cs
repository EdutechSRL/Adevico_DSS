using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Advanced.Domain
{
    /// <summary>
    /// Richieste di integrazione (per segretario)
    /// </summary>
    public class AdvEvalIntegration : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Id sottomissione
        /// </summary>
        public virtual long SubmissionId { get; set; }
        /// <summary>
        /// Id campo sottomesso
        /// </summary>
        public virtual long SubmissionFieldId { get; set; }
        /// <summary>
        /// Commissione di riferimento
        /// </summary>
        public virtual AdvCommission Commission { get; set; }
        /// <summary>
        /// Segretario (chi richiede le integrazioni)
        /// </summary>
        public virtual litePerson Secretary { get; set; }
        /// <summary>
        /// Testo della richiesta
        /// </summary>
        public virtual string SecretaryText { get; set; }
        /// <summary>
        /// Tipo richiesta (testo/allegato)
        /// </summary>
        public virtual IntegrationType Type { get; set; }
        /// <summary>
        /// Richiesta inviata
        /// </summary>
        public virtual bool ReqSended { get; set; }
        /// <summary>
        /// Data INVIO richiesta
        /// </summary>
        public virtual DateTime? ReqSendedOn { get; set; }
        /// <summary>
        /// Sottomittore (chi ha sottomesso il bando)
        /// </summary>
        public virtual litePerson Submitter { get; set; }
        /// <summary>
        /// Testo risposta
        /// </summary>
        public virtual string SubmitterText { get; set; }
        /// <summary>
        /// File allegato alla risposta
        /// </summary>
        public virtual ModuleLink Link { get; set; }
        /// <summary>
        /// Risposta inviata
        /// </summary>
        public virtual bool AnswerSended { get; set; }
        /// <summary>
        /// Data invio risposta
        /// </summary>
        public virtual DateTime? AnswerSendedOn { get; set; }
    }
}
