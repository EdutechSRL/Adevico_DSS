using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.BusinessLogic;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public class CertificationBasePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _IdModule;
        private Service _Service;
        private int IdModule
        {
            get
            {
                if (_IdModule <= 0)
                {
                    _IdModule = this.PathService.ServiceModuleID();
                }
                return _IdModule;
            }
        }

        private Service PathService
        {
            get
            {
                if (_Service == null)
                    _Service = new Service(AppContext);
                return _Service;
            }
        }
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewCertificationList View
        {
            get { return (IViewCertificationList)base.View; }
        }
        public CertificationBasePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public CertificationBasePresenter(iApplicationContext oContext, IViewBaseCertification view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion
    }
}
