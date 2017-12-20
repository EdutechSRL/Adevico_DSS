using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// Elemento richiesta integrazione
    /// </summary>
    public class dtoIntegrationItem
    {
        /// <summary>
        /// Id Integrazione
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Nome segretario
        /// </summary>
        public string SecretaryName { get; set; }
        /// <summary>
        /// Id segretario
        /// </summary>
        public int SecretaryId { get; set; }
        /// <summary>
        /// Testo richiesta
        /// </summary>
        public string RequestText { get; set; }
        /// <summary>
        /// Tipo integrazione
        /// </summary>
        public IntegrationType Type { get; set; }
        /// <summary>
        /// Se la richiesta è stata inviata
        /// </summary>
        public bool IsSended { get; set; }
        /// <summary>
        /// Data invio richiesta
        /// </summary>
        public DateTime? SendedOn { get; set; }

        /// <summary>
        /// Nome sottomittore
        /// </summary>
        public string SubmitterName { get; set; }
        /// <summary>
        /// Id sottomittore
        /// </summary>
        public int SubmitterId { get; set; }
        /// <summary>
        /// SE ha ricevuto risposta
        /// </summary>
        public bool IsAnswered { get; set; }
        /// <summary>
        /// Data invio risposta
        /// </summary>
        public DateTime? AnsweredOn { get; set; }
        /// <summary>
        /// Testo risposta
        /// </summary>
        public string AswerText { get; set; }
        /// <summary>
        /// Allegati risposta
        /// </summary>
        public lm.Comol.Core.DomainModel.ModuleLink Link { get; set; }
        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoIntegrationItem() { }
        /// <summary>
        /// Costruttore da oggetto dominio
        /// </summary>
        /// <param name="integration">Integrazione</param>
        public dtoIntegrationItem(lm.Comol.Modules.CallForPapers.Advanced.Domain.AdvEvalIntegration integration)
        {
            //CanView = false;
            //IsSecretary = false;
            //IsSubmitter = false;
            Id = integration.Id;

            if (integration.Secretary != null)
            {
                SecretaryName = integration.Secretary.SurnameAndName;
                SecretaryId = integration.Secretary.Id;
            }
            else
            {
                SecretaryName = "";
                SecretaryId = 0;
            }

            RequestText = integration.SecretaryText;
            Type = integration.Type;

            IsSended = integration.ReqSended;
            SendedOn = integration.ReqSendedOn;

            if (integration.Submitter != null)
            {
                SubmitterName = integration.Submitter.SurnameAndName;
                SubmitterId = integration.Submitter.Id;
            }
            else
            {
                SubmitterName = "";
                SubmitterId = 0;
            }

            IsAnswered = integration.AnswerSended;
            AnsweredOn = integration.AnswerSendedOn;

            AswerText = integration.SubmitterText;
            Link = integration.Link;
        }
    }
}
