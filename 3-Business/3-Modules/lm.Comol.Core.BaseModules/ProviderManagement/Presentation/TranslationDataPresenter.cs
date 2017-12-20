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
    public class TranslationDataPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewTranslationData View
            {
                get { return (IViewTranslationData)base.View; }
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
            public TranslationDataPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TranslationDataPresenter(iApplicationContext oContext, IViewTranslationData view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(long IdProvider,Int32 idLanguage )
            {
                View.IdProvider = IdProvider;
                if (UserContext.isAnonymous)
                    View.DisplayProviderUnknown();
                else if (IdProvider>0)
                    View.LoadTranslation(Service.GetAuthenticationTranslation(IdProvider, idLanguage));
                else
                    View.LoadTranslation(new dtoProviderTranslation() { IdAuthenticationProvider = 0, idLanguage = idLanguage, Id = 0 });
            }

            public Boolean SaveTranslation() {
                AuthenticationProviderTranslation translation = Service.SaveTranslation(View.Translation);

                return (translation!=null);
            }
    }
}