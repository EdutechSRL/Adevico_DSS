using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class InputRequiredFilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewInputRequiredFile View
            {
                get { return (IViewInputRequiredFile)base.View; }
            }
            private ServiceCallOfPapers _ServiceCall;
            private ServiceCallOfPapers CallService
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
                }
            }
            private ServiceRequestForMembership _ServiceRequest;
            private ServiceRequestForMembership RequestService
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
            public InputRequiredFilePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public InputRequiredFilePresenter(iApplicationContext oContext, IViewInputRequiredFile view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idCall, long idSubmission, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, dtoCallSubmissionFile file, Boolean disabled, Boolean allowAnonymous)
        {
            if (UserContext.isAnonymous && !allowAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.Disabled = disabled;
                View.IdCall = idCall;
                View.IdSubmission = idSubmission;
                View.IdCallCommunity = identifier.IdCommunity;
                if (file == null)
                    View.DisplayEmptyFile();
                else
                    View.SetupView(file,UserContext.CurrentUserID, identifier, allowAnonymous);
            }
        }

        public void AddFile() { 
        
        }
        public void RemoveFile() {
            View.RefreshFileField();
        }
    }
}