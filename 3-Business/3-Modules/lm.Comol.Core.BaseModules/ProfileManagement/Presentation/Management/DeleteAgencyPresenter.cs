using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class DeleteAgencyPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewDeleteAgency View
            {
                get { return (IViewDeleteAgency)base.View; }
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
            public DeleteAgencyPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DeleteAgencyPresenter(iApplicationContext oContext, IViewDeleteAgency view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            long idAgency = View.PreloadedIdAgency;

            if (UserContext.isAnonymous)
                View.NoPermission();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                if (module.DeleteProfile || module.Administration)
                {
                    if (Service.ExistAgency(idAgency)){
                        View.IdAgency = idAgency;
                        dtoAgency agency = Service.GetDtoAgency(idAgency);
                        View.DisplayAgencyInfo(agency);
                        View.AllowDelete = (agency.EmployeeNumber==0);
                    }
                    else
                        View.DisplayAgencyUnknown();
                }
                else
                    View.NoPermission();
            }
        }

        public void DeleteAgency() {
            long idAgency = View.IdAgency;
            Service.PhisicalDeleteAgency(idAgency);
            View.GotoManagementPage();
        }
    }
}