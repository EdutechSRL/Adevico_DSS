using System;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC
{
    public class UC_GlossaryImportTermsPresenter : DomainPresenter
    {
        public void InitView()
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewUC_GlossaryImportTerms View
        {
            get { return (IViewUC_GlossaryImportTerms) base.View; }
        }

        private ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleGlossaryNew.UniqueCode);
                return currentIdModule;
            }
        }

        public UC_GlossaryImportTermsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public UC_GlossaryImportTermsPresenter(iApplicationContext oContext, IViewUC_GlossaryImportTerms view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}