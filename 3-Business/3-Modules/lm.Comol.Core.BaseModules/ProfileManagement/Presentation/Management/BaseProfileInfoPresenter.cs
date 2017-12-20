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
    public class BaseProfileInfoPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewBaseProfileInfo View
            {
                get { return (IViewBaseProfileInfo)base.View; }
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
            public BaseProfileInfoPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public BaseProfileInfoPresenter(iApplicationContext oContext, IViewBaseProfileInfo view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idProfile)
        {
            Person person = CurrentManager.GetPerson(idProfile);
            if (person == null || UserContext.isAnonymous)
                View.DisplayEmpty();
            else
            {
                View.IdProfile = person.Id;
                View.IdProfileType = person.TypeID;

                switch (person.TypeID) { 
                    case (int)UserTypeStandard.Company:
                        View.LoadProfileData(new dtoCompany(((CompanyUser)person)));
                        break;
                    case (int)UserTypeStandard.Employee:
                        View.LoadProfileData(new dtoEmployee(((Employee)person), GetVisibility()));
                        break;
                    default:
                        View.LoadProfileData(person.Id, person.TypeID);
                        break;
                }
                View.LoadOrganizations(Service.GetProfileOrganizations(person));
            }
        }

        private AgencyVisibility GetVisibility() {
            int idType = UserContext.UserTypeID;
            return (idType == (int)UserTypeStandard.Administrative || idType == (int)UserTypeStandard.SysAdmin || idType == (int)UserTypeStandard.Administrator) ?  AgencyVisibility.NotDeleted: AgencyVisibility.Active;
        }

    }
}