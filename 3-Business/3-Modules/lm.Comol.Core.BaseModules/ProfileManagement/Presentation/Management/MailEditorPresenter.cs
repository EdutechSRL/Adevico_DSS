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
    public class MailEditorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewMailEditor View
            {
                get { return (IViewMailEditor)base.View; }
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
            public MailEditorPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MailEditorPresenter(iApplicationContext oContext, IViewMailEditor view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idProfile = idUser;
                View.IdPendingRequest = 0;
                Person person = CurrentManager.GetPerson(idProfile);
                if (person == null)
                    View.DisplayProfileUnknown();
                else
                {
                    View.IdProfile = person.Id;
                    MailEditingPending pending = Service.LastEditingPending(person);
                    if (pending != null)
                    {
                        View.IdPendingRequest = pending.Id;
                        View.DisplayWaitingCode(pending.CreatedOn, pending.Mail);
                    }
                    else
                        View.DisplayMailEditor();
                }
            }
        }

        public MailEditingPending SavePendingChanges(String mail)
        {
            MailEditingPending pending = null;
            if (Service.isPendingMailUnique(View.IdProfile,mail))
            {
                pending = Service.SavePendingChanges(View.IdProfile, mail);

                if (pending == null)
                    View.DisplayError(ErrorMessages.UnsavedRequest);
                else
                    View.IdPendingRequest = pending.Id;
            }
            else
                View.DisplayError(ErrorMessages.MailAlreadyExist);
            return pending;
        }
        public void ActivateMail(String code)
        {
            MailEditingPending pending = Service.ActivateMailPendingChange(View.IdProfile, View.IdPendingRequest, code);
            if (pending == null)
                View.DisplayError(ErrorMessages.NoPendingRequest);
            else if (pending.ActivationCode != code)
                View.DisplayError(ErrorMessages.InvalidCode);
            else
            {
                View.IdPendingRequest = pending.Id;
                View.DisplayActivationComplete(pending.Mail);
            }
        }

        public MailEditingPending SendNewCode()
        {
            return Service.RenewPendingChangesCode(View.IdProfile, View.IdPendingRequest);
        }
        
    }
}