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
    public class ProfileSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewProfileSettings View
            {
                get { return (IViewProfileSettings)base.View; }
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
            public ProfileSettingsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProfileSettingsPresenter(iApplicationContext oContext, IViewProfileSettings view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idProfile = UserContext.CurrentUserID;
                Person person = CurrentManager.GetPerson(idProfile);
                if (person == null)
                    View.DisplayProfileUnknown();
                else
                {
                    List<ProfileSettingsTab> tabs = null;
                    View.idDefaultProvider = person.IdDefaultProvider;
                    View.IdProfile = person.Id;
                    View.IdProfileType = person.TypeID;
                    View.LoadProfileInfo(idProfile, person.SurnameAndName, person.FotoPath, person.TypeID);
                    
                    tabs = GetAvailableTabs();
                    if (tabs == null || tabs.Count == 0)
                        View.DisplayNoPermission();
                    else
                    {
                        View.LoadTabs(tabs);
                        View.DisplayTab(idProfile, ProfileSettingsTab.profileData);
                    }
                }
            }
        }

        private List<ProfileSettingsTab> GetAvailableTabs()
        {
            List<ProfileSettingsTab> tabs = new List<ProfileSettingsTab>();
            tabs.Add(ProfileSettingsTab.profileData);
            tabs.Add(ProfileSettingsTab.mailPolicy);
            tabs.Add(ProfileSettingsTab.istantMessaging);
            
            return tabs;
        }

        public Boolean UpdateAvatar(String avatar) {
            return Service.UpdateAvatar(View.IdProfile, avatar);
        }

        public void ProfileSaved(Boolean saved)
        {
            if (saved)
                View.DisplaySavedInfo();
            else
                View.DisplayErrorSaving();
        }
    }
}