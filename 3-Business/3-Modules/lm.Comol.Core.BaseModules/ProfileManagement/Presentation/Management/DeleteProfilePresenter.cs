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
    public class DeleteProfilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewDeleteProfile View
            {
                get { return (IViewDeleteProfile)base.View; }
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
            public DeleteProfilePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DeleteProfilePresenter(iApplicationContext oContext, IViewDeleteProfile view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Int32 idUser = View.PreloadedIdProfile;

            if (UserContext.isAnonymous)
                View.NoPermission();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                if (module.DeleteProfile || module.Administration)
                {
                    Person person = CurrentManager.GetPerson(idUser);
                    if (person==null)
                        View.DisplayProfileUnknown();
                    else{
                        View.IdProfile = idUser;
                        dtoProfilePermission permission =  new dtoProfilePermission(UserContext.UserTypeID, person.TypeID);
                        View.DisplayProfileInfo(person.SurnameAndName);
                        View.AllowDelete = permission.Delete;
                    }
                }
                else
                    View.NoPermission();
            }
        }

        public void DeleteProfileInfo() {
            Int32 idProfile = View.IdProfile;
            if (idProfile != UserContext.CurrentUserID){
                if (Service.VirtualDeleteProfileInfo(idProfile))
                {
                    if (View.DeleteProfile(idProfile))
                        Service.PhisicalDeleteProfileInfo(idProfile);

                }
            }
            View.GotoManagementPage();
        }
    }
}