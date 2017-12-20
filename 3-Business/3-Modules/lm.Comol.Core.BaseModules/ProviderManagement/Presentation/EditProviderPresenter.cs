using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProviderManagement.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;


namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public class EditProviderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProviderManagementService _Service;
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
            protected virtual IViewEditProvider View
            {
                get { return (IViewEditProvider)base.View; }
            }
            private ProviderManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProviderManagementService(AppContext);
                    return _Service;
                }
            }
            public EditProviderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditProviderPresenter(iApplicationContext oContext, IViewEditProvider view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView()
            {
                long idProvider = View.PreloadedIdProvider;
                View.IdProvider = idProvider;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    ModuleProviderManagement module = ModuleProviderManagement.CreatePortalmodule(UserContext.UserTypeID);
                    View.AllowManagement = (module.Administration || module.ViewProviders);
                    if (module.EditProvider || module.Administration)
                    {
                        dtoProvider provider = Service.GetAuthenticationProvider(idProvider);
                        if (provider == null)
                            View.DisplayProviderUnknown();
                        else if (provider.Deleted != BaseStatusDeleted.None)
                            View.DisplayDeletedProvider((provider.Translation == null) ? provider.Name : provider.Translation.Name, provider.Type);
                        else
                        {
                            View.LoadProviderInfo(provider, (provider.Translation == null) ? provider.Name : provider.Translation.Name, provider.Type, (provider.Type != AuthenticationProviderType.Internal));
                            List<Language> languages = CurrentManager.GetAll<Language>().ToList();
                            View.LoadTranslations(provider.IdentifierFields, languages);
                            View.AllowEdit = true;
                        }
                    }
                    else
                        View.DisplayNoPermission();
                }
            }
    }
}