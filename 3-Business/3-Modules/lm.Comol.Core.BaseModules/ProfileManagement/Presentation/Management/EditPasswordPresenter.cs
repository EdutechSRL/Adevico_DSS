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
    public class EditPasswordPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewEditPassword View
            {
                get { return (IViewEditPassword)base.View; }
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
            public EditPasswordPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditPasswordPresenter(iApplicationContext oContext, IViewEditPassword view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.LoadUserUnknown();
            else
            {
                View.DisplayProviders(Service.GetProfileProviders(UserContext.CurrentUserID, false));   
            }
        }

        public void EditPassword(String oldPassword, String newPassword) {
            try
            {
                if (Service.EditPassword(UserContext.CurrentUserID, oldPassword, newPassword))
                    View.DisplayProviders(Service.GetProfileProviders(UserContext.CurrentUserID, false));
                else
                    View.DisplayPasswordNotChanged();
            }
            catch (InvalidPasswordException ex) {
                View.DisplayInvalidPassword();
            }
            catch (Exception ex){
                View.DisplayPasswordNotChanged();
            }
            
        }
    }
}