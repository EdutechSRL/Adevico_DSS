using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class ActivateUserMailPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewActivateUserMail View
            {
                get { return (IViewActivateUserMail)base.View; }
            }
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            public ActivateUserMailPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ActivateUserMailPresenter(iApplicationContext oContext, IViewActivateUserMail view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(System.Guid identifier)
        {
            List<MailEditingPending> pendings = Service.GetEditingPendings(identifier);
            if (pendings.Count==0)
                View.DisplayError(ErrorMessages.NoPendingRequest);
            else if (pendings.Count == 1) {
                if (pendings[0].Deleted == BaseStatusDeleted.Manual)
                    View.DisplayError(ErrorMessages.AlreadyActivated);
                else
                {
                    Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
                    if (UserContext.isAnonymous || person == pendings[0].Person)
                    {
                        Service.ActivateMailPendingChange(pendings[0]);
                        View.DisplayActivationComplete();
                    }
                    else
                        View.DisplayError(ErrorMessages.NoPendingRequest);
                }
            }
          
        }       
    }
}