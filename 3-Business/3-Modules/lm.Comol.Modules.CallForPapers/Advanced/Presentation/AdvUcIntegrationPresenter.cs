using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Domain;


namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation
{
    /// <summary>
    /// Presenter UC integrazioni
    /// </summary>
    public class AdvUcIntegrationPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        /// <summary>
        /// Manager
        /// </summary>
        public virtual BaseModuleManager CurrentManager { get; set; }
        /// <summary>
        /// View: UC
        /// </summary>
        protected virtual iView.iViewUcIntegration View
        {
            get { return (iView.iViewUcIntegration)base.View; }
        }
        /// <summary>
        /// Service Bandi
        /// </summary>
        private ServiceCallOfPapers _ServiceCall;
        /// <summary>
        /// Service Bandi
        /// </summary>
        private ServiceCallOfPapers CallService
        {
            get
            {
                if (_ServiceCall == null)
                    _ServiceCall = new ServiceCallOfPapers(AppContext);
                return _ServiceCall;
            }
        }
        /// <summary>
        /// Service Adesioni
        /// </summary>
        private ServiceRequestForMembership _ServiceRequest;
        /// <summary>
        /// Service adesioni
        /// </summary>
        private ServiceRequestForMembership RequestService
        {
            get
            {
                if (_ServiceRequest == null)
                    _ServiceRequest = new ServiceRequestForMembership(AppContext);
                return _ServiceRequest;
            }
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application context</param>
        public AdvUcIntegrationPresenter(iApplicationContext oContext)
                : base(oContext)
            {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application context</param>
        /// <param name="view">View: User control</param>
        public AdvUcIntegrationPresenter(iApplicationContext oContext, iView.iViewUcIntegration view)
                : base(oContext, view)
            {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion
        /// <summary>
        /// Inizializzazione View
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <param name="submissionId">Id sottomissione</param>
        /// <param name="submissionFieldId">Id campo sottomissione</param>
        /// <param name="submitterId">Id sottomittore</param>
        public void InitView(
            long CommId,
            long submissionId, 
            long submissionFieldId, 
            int submitterId
            )
        {
            if (submitterId == 0)
            {
                submitterId = CallService.IntegrationGetSubmitterId(submissionId);
                View.SubmitterId = submitterId;
            }
                

            dto.dtoIntegration integration = CallService.GetIntegrations(
                CommId,
                submissionId,
                submissionFieldId,
                submitterId);

            View.BindView(integration);
        }


        #region Action management
        /// <summary>
        /// Aggiorna una richiesta di integrazione
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <param name="Text">Testo richiesta</param>
        /// <param name="Type">Tipo richiesta</param>
        /// <param name="SubmissionId">Id sottomissione</param>
        /// <param name="SubmissionFieldId">Id campo sottomissione</param>
        /// <param name="SubmitterId">Id sottomittore</param>
        /// <param name="IntegrationId">Id integrazione</param>
        /// <param name="send">Se a true, la imposta come inviata, altrimenti la lascia in bozza</param>
        public void SaveRequest(
            long CommId,
            string Text,
            IntegrationType Type,
            long SubmissionId,
            long SubmissionFieldId,
            int SubmitterId,
            long IntegrationId,
            bool send)
        {
            bool success = CallService.IntegrationAdd(CommId, Text, Type, SubmissionId, SubmissionFieldId, SubmitterId, IntegrationId, send);

            if (success)
                View.ForceBind();
        }
        /// <summary>
        /// Salva la risposta ad una richiesta di integrazione
        /// </summary>
        /// <param name="IntegrationId">Id integrazione</param>
        /// <param name="Text">Testo integrazione</param>
        /// <param name="send">Invia: se False la lascia in bozza</param>
        public void SaveAnswer(
            long IntegrationId,
            string Text,
            bool send)
        {
            bool success = CallService.IntegrationAnswer(IntegrationId, Text, send);

            if (success)
                View.ForceBind();
        }

        /// <summary>
        /// Recupera l'oggetto AdvEvalIntegration per associarlo al file caricato
        /// </summary>
        /// <param name="integrationId">Id integrazione</param>
        /// <param name="moduleCode">Codice modulo (bandi)</param>
        /// <param name="idModule">Id modulo (bandi)</param>
        /// <param name="moduleAction">Azione</param>
        /// <param name="objectType">Tipo oggetto</param>
        /// <returns></returns>
        public Domain.AdvEvalIntegration GetIntegrationForUpload(
            long integrationId,
            ref string moduleCode,
            ref int idModule,
            ref int moduleAction,
            ref int objectType)
        {

            moduleCode = ModuleCallForPaper.UniqueCode;
            idModule = CallService.ServiceModuleID();
            moduleAction = (int)ModuleCallForPaper.ActionType.DownloadSubmittedFile;
            objectType = (int)ModuleCallForPaper.ObjectType.Integrazioni;

            return CallService.GetIntegration(integrationId);
        }
        /// <summary>
        /// Associa il file caricato con l'integrazione
        /// </summary>
        /// <param name="idIntegration">Id integrazione</param>
        /// <param name="maLink">Module Action Link, restituito dal controllo di upload</param>
        /// <param name="text">Testo risposta</param>
        /// <param name="commId">Id commissione</param>
        /// <param name="SubmissionId">Id sottomissione</param>
        /// <param name="fieldId">Id campo</param>
        /// <param name="submitterId">Id sottomittore</param>
        /// <param name="send">Se a true, la imposta come inviata, altrimenti la lascia in bozza</param>
        public void IntegrationFileUpload(
            long idIntegration, ModuleActionLink maLink, string text,
            long commId, long SubmissionId, long fieldId, int submitterId,
            bool send)
        {
            if (maLink == null || maLink.Link == null)
            {
                return;
            }
            else
            {
                CallService.AdvIntegrationAddFile(idIntegration, maLink, text, send);
            }

            InitView(commId, SubmissionId, fieldId, submitterId);
        }

        #endregion

    }
}
