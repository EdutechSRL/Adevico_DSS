using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.Presentation
{
    public class ModuleItemUserSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private int _ModuleID;
        private ServiceCommunityRepository _Service;

        private int ModuleID
        {
            get
            {
                if (_ModuleID <= 0)
                {
                    _ModuleID = this.Service.ServiceModuleID();
                }
                return _ModuleID;
            }
        }
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewModuleItemUserSelector View
        {
            get { return (IViewModuleItemUserSelector)base.View; }
        }
        private ServiceCommunityRepository Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceCommunityRepository(AppContext);
                return _Service;
            }
        }
        public ModuleItemUserSelectorPresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public ModuleItemUserSelectorPresenter(iApplicationContext oContext, IViewModuleItemUserSelector view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView() {
            View.isInitialized = (!View.AllowByAnonymous && UserContext.isAnonymous);
            View.ShowPreview = !(!View.AllowByAnonymous && UserContext.isAnonymous);
            View.UsersCount = 0;
            View.EnableControl = !(!View.AllowByAnonymous && UserContext.isAnonymous);

            if (!View.AllowByAnonymous && UserContext.isAnonymous) { 
            
            }
        }
    }
}
