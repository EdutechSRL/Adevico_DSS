using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class EditProfileAuthenticationsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
        protected virtual IViewEditProfileAuthentications View
        {
            get { return (IViewEditProfileAuthentications)base.View; }
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
        public EditProfileAuthenticationsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditProfileAuthenticationsPresenter(iApplicationContext oContext, IViewEditProfileAuthentications view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView()
        {
            Int32 IdProfile = View.PreloadedIdProfile;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = (module.ViewProfiles || module.Administration);
                
                if (module.AddAuthenticationProviderToProfile || module.Administration)
                {
                    Person person = CurrentManager.GetPerson(IdProfile);
                    View.IdProfile = (person == null) ? 0 : IdProfile;
                    if (person == null)
                        View.DisplayProfileUnknown();
                    else
                    {
                        View.SetTitle(person.SurnameAndName);
                        View.IdProfileType = person.TypeID;

                        dtoProfilePermission permission = new dtoProfilePermission(UserContext.UserTypeID, person.TypeID);

                        List<dtoBaseProvider> providers = Service.GetAvailableAuthenticationProviders(UserContext.Language.Id, IdProfile);
                        Boolean AllowAddprovider = (providers.Count > 0 && ((module.AddAuthenticationProviderToProfile || module.Administration)) && permission.ManageAuthentication);
                        View.AllowEditProfile = (permission.Edit);
                        View.AllowAddprovider = AllowAddprovider;
                        View.InitializeControl(IdProfile);
                    }
                }
                else
                    View.DisplayNoPermission();
            }
        }
    }
}