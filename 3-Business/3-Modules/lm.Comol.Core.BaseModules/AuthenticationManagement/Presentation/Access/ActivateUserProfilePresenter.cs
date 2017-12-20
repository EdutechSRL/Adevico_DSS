using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public class ActivateUserProfilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private CoreAuthenticationsService _Service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewActivateUserProfile View
            {
                get { return (IViewActivateUserProfile)base.View; }
            }
            private CoreAuthenticationsService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new CoreAuthenticationsService(AppContext);
                    return _Service;
                }
            }
            public ActivateUserProfilePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ActivateUserProfilePresenter(iApplicationContext oContext, IViewActivateUserProfile view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean accessAvailable = !(View.isSystemOutOfOrder && !View.AllowAdminAccess);
            View.AllowInternalAuthentication = !View.isSystemOutOfOrder;
            if (!accessAvailable)
                View.DisplaySystemOutOfOrder();
            else{
                Int32 iduser = View.PreloadedIdUser;
                Guid urlIdentifier = View.PreloadedUrlIdentifier;

                Person person = CurrentManager.GetPerson(iduser);
                if (person != null) { 
                    Language language = CurrentManager.GetLanguage(person.LanguageID);
                    if (language != null)
                        View.ReloadLanguageSettings(language.Id, language.Code);
                }

                if (iduser <= 0 || urlIdentifier == Guid.Empty)
                    View.DisplayUnknownUser();
                else if (Service.IsActivatedUserMail(iduser, urlIdentifier))
                    View.DisplayAlreadyActivatedInfo();
                else if (Service.ActivateUserMail(iduser, urlIdentifier))
                {
                    View.DisplayActivationInfo();
                    View.SendActivationMail(person);
                }
                else
                    View.DisplayUnknownUser();
                
            }
        }
    }
}