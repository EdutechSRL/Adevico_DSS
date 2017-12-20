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
    public class EditProfilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
        protected virtual IViewEditProfile View
        {
            get { return (IViewEditProfile)base.View; }
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
        public EditProfilePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditProfilePresenter(iApplicationContext oContext, IViewEditProfile view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(Int32 idProfile)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = (module.EditProfile || module.Administration || module.ViewProfiles);
                if (module.EditProfile || module.Administration)
                {
                    Person person = CurrentManager.GetPerson(idProfile);
                    if (person == null)
                        View.DisplayProfileUnknown();
                    else
                    {
                        View.IdProfile = person.Id;
                        View.IdProfileType = person.TypeID;
                        dtoProfilePermission permission = new dtoProfilePermission(UserContext.UserTypeID, person.TypeID);
                        View.AllowEdit = permission.Edit;
                        View.AllowManageAuthentications = permission.ManageAuthentication;
                        View.LoadProfileName(person.SurnameAndName);
                        if (permission.Edit)
                        {
                            View.LoadProfile(person.Id, person.TypeID);
                        }
                        else
                            View.DisplayNoPermission();
                    }
                }
                else
                    View.DisplayNoPermission();
            }
        }

        public void ProfileSaved(Boolean saved) {
            if (saved)
                View.GotoManagement();
            else {
                View.DisplayErrorSaving();
            }
        }
    }
}