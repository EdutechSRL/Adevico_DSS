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
    public class InputFieldPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewInputField View
            {
                get { return (IViewInputField)base.View; }
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
            public InputFieldPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public InputFieldPresenter(iApplicationContext oContext, IViewInputField view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idCall, long idSubmission, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, dtoSubmissionValueField field, Boolean disabled, Boolean isPublic)
        {
            if (UserContext.isAnonymous && !isPublic)
                View.DisplaySessionTimeout();
            else
            {
                View.Disabled = disabled;
                View.IdCall = idCall;
                View.IdSubmission = idSubmission;
                View.IdCallCommunity = identifier.IdCommunity;
                if (field == null)
                {
                    View.Mandatory = false;
                    View.DisplayEmptyField();
                }
                else
                {
                    View.Mandatory = (field.Field == null) ? false : field.Field.Mandatory;
                    View.DisclaimerType = (field.Field == null) ? DisclaimerType.None : field.Field.DisclaimerType;
                    View.Options = (field.Field == null) ? new List<dtoFieldOption>() : field.Field.Options;
                    View.SetupView(field, UserContext.CurrentUserID, identifier, isPublic);
                }
            }
        }
        public void AddFile() { 
        
        }
        public void RemoveFile() {
            View.RefreshFileField(null);
        }
    }
}