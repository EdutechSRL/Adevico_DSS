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
    /// Presenter controllo gestione commenti
    /// </summary>
    public class AdvUcCommentsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        /// <summary>
        /// Manager
        /// </summary>
        public virtual BaseModuleManager CurrentManager { get; set; }
        /// <summary>
        /// View
        /// </summary>
        protected virtual iView.iViewUcAdvComments View
        {
            get { return (iView.iViewUcAdvComments)base.View; }
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
        /// Service Richeiste adesione
        /// </summary>
        private ServiceRequestForMembership _ServiceRequest;
        /// <summary>
        /// SErvice richieste adesione
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
        /// <param name="oContext">Application Context</param>
        public AdvUcCommentsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application context</param>
        /// <param name="view">View: User Control</param>
        public AdvUcCommentsPresenter(iApplicationContext oContext, iView.iViewUcAdvComments view)
                : base(oContext, view)
            {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion
        /// <summary>
        /// Inizializzazione View
        /// </summary>
        /// <param name="CommissionId">Id Commissione</param>
        /// <param name="SubmissionId">Id Sottomissione</param>
        public void InitView(long CommissionId, long SubmissionId)
        {
            IList<dto.dtoAdvComment> comments = CallService.AdvCommentsGet(CommissionId, SubmissionId);

            View.BindView(comments);
        }
    }
}
