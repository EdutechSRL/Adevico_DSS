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
    public class ManageProfileAuthenticationProvidersPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewManageProfileAuthenticationProviders View
            {
                get { return (IViewManageProfileAuthenticationProviders)base.View; }
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
            public ManageProfileAuthenticationProvidersPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ManageProfileAuthenticationProvidersPresenter(iApplicationContext oContext, IViewManageProfileAuthenticationProviders view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idProfile, Boolean forAdd)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkinkSessionTimeout();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                if (module.AddAuthenticationProviderToProfile || module.Administration)
                {
                    Person person = CurrentManager.GetPerson(idProfile);
                    if (person == null)
                        View.DisplayProfileUnknown();
                    else
                    {
                        View.SetTitle(person.SurnameAndName);
                        View.idDefaultProvider = person.IdDefaultProvider;
                        View.idProfile = person.Id;
                        View.CurrentIdLoginInfo = 0;
                        dtoProfilePermission permission = GetProfilePermission(person.TypeID);
                        //View.AllowAddprovider = permission.ManageAuthentication && (module.RenewPassword || module.Administration);
                        View.AllowEdit = permission.ManageAuthentication;
                        View.AllowRenewPassword = permission.RenewPassword && (module.RenewPassword || module.Administration);
                        View.AllowSetPassword = permission.SetPassword && (module.RenewPassword || module.Administration);

                        List<dtoUserProvider> userProviders = Service.GetProfileProviders(idProfile,true);
                        List<dtoBaseProvider> providers = Service.GetAvailableAuthenticationProviders(UserContext.Language.Id, idProfile);
                        Boolean AllowAddprovider = (providers.Count>0 && ((module.AddAuthenticationProviderToProfile || module.Administration)) && permission.ManageAuthentication);
                        View.AllowAddprovider = AllowAddprovider;
                        View.AvailableProvidersCount = providers.Count;
                        if (forAdd && AllowAddprovider)
                            View.AddUserProviders(providers);
                        else
                            View.LoadItems(userProviders);
                    }
                }
                else
                    View.DisplayNoPermission();
            }
        }

        public void AddNewLoginInfo() {
            View.AddUserProviders(Service.GetAvailableAuthenticationProviders(UserContext.Language.Id, View.idProfile));
        }

        public void LoadAuthenticationItems() {
            View.LoadItems(Service.GetProfileProviders(View.idProfile, true));
        }
        public void SaveExternalProvider(long idProvider,dtoExternalCredentials credentials)
        {
            long idLoginInfo = View.CurrentIdLoginInfo;
            ProfilerError message =  Service.VerifyExternalInfoDuplicate(View.idProfile,idProvider,credentials);

            if (message== ProfilerError.none){
                if (idLoginInfo == 0)
                    message = Service.AddExternalLogin(View.idProfile, idProvider, credentials);
                else
                    message = Service.UpdateExternalLogin(idLoginInfo, credentials);
                
                if (message == ProfilerError.none){
                    if (idLoginInfo == 0)
                    {
                        Person person = CurrentManager.GetPerson(View.idProfile);
                        if (person != null)
                            View.idDefaultProvider = person.IdDefaultProvider;
                        SetupOtherProviders();
                    }
                    LoadAuthenticationItems();
                }
                else if (message == ProfilerError.externalUniqueIDduplicate)
                    View.DisplayProfilerExternalError(message);
                else
                    View.DisplayError(message);
            }
            else
                View.DisplayProfilerExternalError(message);

        }
        public void SaveInternalProvider(String login) {
            ProfilerError message = ProfilerError.internalError;
            long idLoginInfo = View.CurrentIdLoginInfo;
            if (idLoginInfo == 0)
            {
                String password = "";
                InternalLoginInfo loginInfo = null;
                message = Service.AddInternalLogin(View.idProfile, login, ref password , ref loginInfo );
                if (message == ProfilerError.none && !string.IsNullOrEmpty(password) && loginInfo !=null)
                    View.SendMail(loginInfo, password);
                SetupOtherProviders();
            }
            else
                message = Service.UpdateInternalProfile(idLoginInfo, View.idProfile, login);
            if (message == ProfilerError.none)
            {
                if (idLoginInfo == 0) {
                    Person person = CurrentManager.GetPerson(View.idProfile);
                    if (person != null)
                        View.idDefaultProvider = person.IdDefaultProvider;
                }
                LoadAuthenticationItems();
            }
            else if (message == ProfilerError.loginduplicate)
                View.DisplayProfilerInternalError(message);
            else
                View.DisplayError(message);
        }
        private void SetupOtherProviders() {
            Int32 idProfile = View.idProfile;
            dtoProfilePermission permission = new dtoProfilePermission();
            Person person = CurrentManager.GetPerson(idProfile);
            if (person != null)
                permission = GetProfilePermission(person.TypeID);
            ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
            List<dtoUserProvider> userProviders = Service.GetProfileProviders(idProfile, true);
            List<dtoBaseProvider> providers = Service.GetAvailableAuthenticationProviders(UserContext.Language.Id, idProfile);
            Boolean AllowAddprovider = (providers.Count > 0 && ((module.AddAuthenticationProviderToProfile || module.Administration)) && permission.ManageAuthentication);
            View.AllowAddprovider = AllowAddprovider;
            View.AvailableProvidersCount = providers.Count;
        }

        public Boolean SetDefaultProvider(long idProvider) {
            Boolean result = false;
            if (UserContext.isAnonymous)
                View.DisplayWorkinkSessionTimeout();
            else
            {
                result = Service.SetDefaultProvider(idProvider, View.idProfile);
                if (result)
                    View.idDefaultProvider = idProvider;
                LoadAuthenticationItems();
            }
            return result;
        }
        public Boolean ModifyAuthenticationUse(long idLoginInfo, Boolean enable)
        {
            Boolean result = false;
            if (UserContext.isAnonymous)
                View.DisplayWorkinkSessionTimeout();
            else
            {
                result = Service.ModifyAuthenticationActivation(idLoginInfo, enable);
                LoadAuthenticationItems();
            }
            return result;
        }
        public void EditUserProviderInfo(long idLoginInfo) {
            BaseLoginInfo loginInfo = CurrentManager.Get<BaseLoginInfo>(idLoginInfo);
            if (loginInfo!=null){
                View.CurrentIdProvider = 0;
                if (typeof(InternalLoginInfo) == loginInfo.GetType())
                    View.EditInternalUserInfo(idLoginInfo, ((InternalLoginInfo)loginInfo).Login);
                else
                {
                    View.CurrentIdProvider = ((ExternalLoginInfo)loginInfo).Provider.Id;
                    View.EditExternalUserInfo(idLoginInfo, Service.GetProfileProvider(View.idProfile, ((ExternalLoginInfo)loginInfo).Provider.Id, UserContext.Language.Id), new dtoExternalCredentials() { IdentifierLong = ((ExternalLoginInfo)loginInfo).IdExternalLong, IdentifierString = ((ExternalLoginInfo)loginInfo).IdExternalString });
                }
            }

        }
        public void SelectNewUserProvider(long idProvider)
        {
            dtoBaseProvider provider = Service.GetAuthenticationProvider(UserContext.Language.Id, idProvider);
            if (provider != null)
                View.LoadProviderToAdd(provider);
        }
        public void RenewPassword(int IdUser)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkinkSessionTimeout();
            else
            {
                String newPassword = "";
                InternalLoginInfo internalLogin = Service.RenewPassword(IdUser, ref newPassword);

                if (!String.IsNullOrEmpty(newPassword) && internalLogin != null)
                {
                    View.SendMail(internalLogin, newPassword);
                }
            }
        }
        public void SetPassword(long idLoginInfo)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkinkSessionTimeout();
            else
            {
                InternalLoginInfo loginInfo = CurrentManager.Get<InternalLoginInfo>(idLoginInfo);
                if (loginInfo != null && loginInfo.Person !=null)
                {
                    View.CurrentIdProvider = 0;
                    if (typeof(InternalLoginInfo) == loginInfo.GetType())
                        View.EditInternalUserPassword(idLoginInfo, loginInfo.Login, loginInfo.Person.SurnameAndName, loginInfo.Person.Mail );
                }
                else
                    LoadAuthenticationItems();
            }
        }
        public void SaveInternalPassword(String password, Boolean isOneTimePassword)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkinkSessionTimeout();
            else
            {
                long idLoginInfo = View.CurrentIdLoginInfo;
                if (idLoginInfo > 0)
                {
                    Person person = CurrentManager.GetPerson(View.idProfile);
                    InternalLoginInfo loginInfo = Service.SetPassword(person, password, isOneTimePassword);
                    if (loginInfo != null)
                        View.SendMail(loginInfo, password);
                }
                LoadAuthenticationItems();
            }
        }
        public void DeleteLoginInfo(int idLoginInfo)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkinkSessionTimeout();
            else
            {
                Service.DeleteLoginInfo(idLoginInfo);
                LoadAuthenticationItems();
            }
        }
        private dtoProfilePermission GetProfilePermission(Int32 idProfileType) {
            return new dtoProfilePermission(UserContext.UserTypeID, idProfileType);
        }
    }
}