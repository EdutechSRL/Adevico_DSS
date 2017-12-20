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
    public class ProfileInfoPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewProfileInfo View
            {
                get { return (IViewProfileInfo)base.View; }
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
            public ProfileInfoPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProfileInfoPresenter(iApplicationContext oContext, IViewProfileInfo view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idProfile)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Person person = CurrentManager.GetPerson(idProfile);
                if (person == null)
                    View.DisplayProfileUnknown();
                else
                {
                    ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                    dtoProfilePermission permission = new dtoProfilePermission(UserContext.UserTypeID, person.TypeID);


                    if (module.ViewProfileDetails || module.Administration) {
                        List<ProfileInfoTab> tabs = null;
                        if (permission.Info || permission.AdvancedInfo) {
                            View.idDefaultProvider = person.IdDefaultProvider;
                            View.IdProfile = person.Id;
                            View.IdProfileType = person.TypeID;
                            View.LoadProfileInfo(idProfile, person.SurnameAndName, person.FotoPath, person.TypeID);

                            tabs = GetAvailableTabs(permission);
                        }

                        if (tabs == null || tabs.Count == 0)
                            View.DisplayNoPermissionForProfile();
                        else
                        {
                            View.LoadTabs(tabs);
                            View.DisplayBaseInfo(person.Id,permission.AdvancedInfo);
                        }
                    }
                    else
                        View.DisplayNoPermission();
                }
            }
        }

        private List<ProfileInfoTab> GetAvailableTabs(dtoProfilePermission permission)
        {
            List<ProfileInfoTab> steps = new List<ProfileInfoTab>();
            if (permission.Info) {
                steps.Add(ProfileInfoTab.baseInfo);
                steps.Add(ProfileInfoTab.communityInfo);
            }
            if (permission.AdvancedInfo)
                steps.Add(ProfileInfoTab.advancedInfo);
            return steps;
        }
    }
}