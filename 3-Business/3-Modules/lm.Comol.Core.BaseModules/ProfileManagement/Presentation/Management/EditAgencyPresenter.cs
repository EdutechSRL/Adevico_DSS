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
    public class EditAgencyPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewEditAgency View
            {
                get { return (IViewEditAgency)base.View; }
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
            public EditAgencyPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditAgencyPresenter(iApplicationContext oContext, IViewEditAgency view)
                            : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idAgency)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = module.Administration;
                if (module.Administration)
                {
                    dtoAgency agency = Service.GetDtoAgency(idAgency);
                    if (agency == null && idAgency>0)
                        View.DisplayAgencyUnknown();
                    else
                    {
                        View.IdAgency = idAgency;
                        View.AllowEdit = true;
                        if (idAgency > 0)
                        {
                            View.LoadAgencyName(agency.Name);
                            View.LoadAgency(agency, Service.GetAgencyAvailableOrganizations(idAgency));
                        }
                        else
                            View.InitializeForAdd(Service.GetAgencyAvailableOrganizations(idAgency));
                    }
                }
                else
                    View.DisplayNoPermission();
            }
        }

        public void Save(dtoAgency agency) {
            agency.Id= View.IdAgency;
            if (ValidateInput(agency)){
                Agency saved = Service.SaveAgency(agency);
                if (saved!=null)
                    View.GotoManagement();
                else
                    View.DisplayErrorSaving();
            }
        }

        public Boolean ValidateInput(dtoAgency agency)
        {
            List<AgencyError> errors = Service.VerifyExistingAgency(agency);
            if (errors.Count > 0)
                View.LoadErrors(errors);
            return (errors.Count == 0);
        }
    }
}