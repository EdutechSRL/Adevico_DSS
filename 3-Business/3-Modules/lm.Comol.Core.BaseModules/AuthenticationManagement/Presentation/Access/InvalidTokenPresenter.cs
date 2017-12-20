using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public class InvalidTokenPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ExternalAuthenticationService _Service;
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService _ProfileService;
            private ExternalAuthenticationService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ExternalAuthenticationService(AppContext);
                    return _Service;
                }
            }
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (_ProfileService == null)
                        _ProfileService = new lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(AppContext);
                    return _ProfileService;
                }
            }

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewInvalidToken View
            {
                get { return (IViewInvalidToken)base.View; }
            }

            public InvalidTokenPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public InvalidTokenPresenter(iApplicationContext oContext, IViewInvalidToken view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(int idPerson, long idProvider, UrlProviderResult message) {
            Person person = CurrentManager.GetPerson(idPerson);
            InitializeLanguage(person);

            UrlAuthenticationProvider provider = CurrentManager.Get<UrlAuthenticationProvider>(idProvider);
            if (person != null)
                View.DisplayMessage(person.SurnameAndName, message);
            else
                View.DisplayMessage(message);

            String defaultUrl = (provider != null) ? provider.RemoteLoginUrl : "";

            if (message != UrlProviderResult.ValidToken)
                View.SetAutoLogonUrl(defaultUrl);
        }
        
        private void InitializeLanguage(Person person)
        {
            Language language = null;
            if (person != null)
                language = CurrentManager.GetLanguage(person.LanguageID);
            if (language == null)
                language = CurrentManager.GetDefaultLanguage();

            View.LoadLanguage(language);
        }
    }
}