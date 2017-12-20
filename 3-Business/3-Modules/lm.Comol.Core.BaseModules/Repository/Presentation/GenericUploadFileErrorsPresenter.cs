using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{

    public class GenericUploadFileErrorsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
        protected virtual IViewGenericUploadFileErrors View
        {
            get { return (IViewGenericUploadFileErrors)base.View; }
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


        public GenericUploadFileErrorsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public GenericUploadFileErrorsPresenter(iApplicationContext oContext, IViewGenericUploadFileErrors view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public void InitView()
        {
            int IdCommunity = 0;
            if (UserContext.isAnonymous)
                View.ShowSessionTimeout();
            else
            {
                IdCommunity = UserContext.CurrentCommunityID;
                View.DeafultBackUrl = View.PreloadedBackUrl;
                View.LoadFiles(View.PreloadedModuleFiles,View.PreloadedRepositoryFiles );
            }
        }
    }
}
