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
    public class AdvancedProfileInfoPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewAdvancedProfileInfo View
            {
                get { return (IViewAdvancedProfileInfo)base.View; }
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
            public AdvancedProfileInfoPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AdvancedProfileInfoPresenter(iApplicationContext oContext, IViewAdvancedProfileInfo view)
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
                        View.LoadProfileData(new dtoCompany(((CompanyUser)person)), ((CompanyUser)person).PersonInfo);
                        break;
                    case (int)UserTypeStandard.Employee:
                        View.LoadProfileData(new dtoEmployee(((Employee)person), GetVisibility()), ((Employee)person).PersonInfo);
                        break;
                    default:
                        View.LoadProfileData(person.Id, person.TypeID);
                        break;
                }
            }
        }

        private AgencyVisibility GetVisibility()
        {
            int idType = UserContext.UserTypeID;
            return (idType == (int)UserTypeStandard.Administrative || idType == (int)UserTypeStandard.SysAdmin || idType == (int)UserTypeStandard.Administrator) ? AgencyVisibility.NotDeleted : AgencyVisibility.Active;
        }
    }
}