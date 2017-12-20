using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class ModuleItemFilesSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
        protected virtual IViewModuleItemFilesSelector View
        {
            get { return (IViewModuleItemFilesSelector)base.View; }
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
        public ModuleItemFilesSelectorPresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public ModuleItemFilesSelectorPresenter(iApplicationContext oContext, IViewModuleItemFilesSelector view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
    }
}
