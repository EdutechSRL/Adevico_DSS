using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class MailTemplatePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
        protected virtual IViewMailTemplate View
        {
            get { return (IViewMailTemplate)base.View; }
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
        public MailTemplatePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public MailTemplatePresenter(iApplicationContext oContext, IViewMailTemplate view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(dtoImportSettings settings)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.SendMailToUsers = true;
                View.DisplayMail(Service.GetProfileTypeMailTemplateAttributes(settings));
                View.isInitialized = true;
            }
        }
    }
}
