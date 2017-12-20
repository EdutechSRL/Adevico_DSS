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
    public class UserProfileAddPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private InternalAuthenticationService _InternalService;
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
            protected virtual IViewUserProfileAdd View
            {
                get { return (IViewUserProfileAdd)base.View; }
            }

            private InternalAuthenticationService InternalService
            {
                get
                {
                    if (_InternalService == null)
                        _InternalService = new InternalAuthenticationService(AppContext);
                    return _InternalService;
                }
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
            public UserProfileAddPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public UserProfileAddPresenter(iApplicationContext oContext, IViewUserProfileAdd view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
           
        }
        public InternalLoginInfo AddInternalProfile(dtoBaseProfile profile, Int32 idPerson)
        {
            return Service.AddInternalProfile(profile, idPerson);
        }
        //public ExternalLoginInfo AddExternalProfile(dtoBaseProfile profile, Int32 idPerson, long idProvider)
        //{
        //    return Service.AddExternalProfile(profile, idPerson, idProvider);
        //}

        public Employee AddEmployee(Employee profile)
        {
            return Service.AddEmployee(profile);
        }
        public Employee AddEmployee(Employee profile, long idProvider, dtoExternalCredentials credentials)
        {
            return Service.AddEmployee(profile, idProvider, credentials);
        }

        public CompanyUser AddCompanyUser(CompanyUser profile)
        {
            return Service.AddCompanyUser(profile);
        }
        public CompanyUser AddCompanyUser(CompanyUser profile, long idProvider, dtoExternalCredentials credentials)
        {
            return Service.AddCompanyUser(profile, idProvider, credentials);
        }
        public ExternalLoginInfo AddExternalProfile(Int32 IdPerson, long idProvider, dtoExternalCredentials credentials)
        {
            return Service.AddExternalProfile(IdPerson, idProvider, credentials);
        }


        public WaitingActivationProfile AddWaitingActivationProfile(Int32 IdPerson)
        {
            return Service.AddWaitingActivationProfile(IdPerson);
        }
    }
}