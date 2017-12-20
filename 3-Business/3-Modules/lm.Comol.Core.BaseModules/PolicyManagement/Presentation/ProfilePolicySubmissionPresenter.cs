using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.PolicyManagement.Business;
using lm.Comol.Core.Authentication.Business;

namespace lm.Comol.Core.BaseModules.PolicyManagement.Presentation
{
    public class ProfilePolicySubmission : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private PolicyService _PolicyService; 
            private int _ModuleID;
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
            protected virtual IViewProfilePolicySubmission View
            {
                get { return (IViewProfilePolicySubmission)base.View; }
            }
            private PolicyService Service
            {
                get
                {
                    if (_PolicyService == null)
                        _PolicyService = new PolicyService(AppContext);
                    return _PolicyService;
                }
            }
            public ProfilePolicySubmission(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProfilePolicySubmission(iApplicationContext oContext, IViewProfilePolicySubmission view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idUser)
        {
            View.CurrentIdUser = idUser;
            List<dtoUserDataPolicy> items = Service.GetActivePolicy(idUser);
            if (items == null)
                View.LoadItemsError();
            else
                View.LoadItems(items);
        }
        public void InitAnonymousView()
        {
            View.CurrentIdUser = 0;
            List<dtoUserDataPolicy> items = Service.GetActivePolicy();
            if (items == null)
                View.LoadItemsError();
            else
                View.LoadItems(items);
        }

        public Boolean SavePolicy() {
            Boolean result = false;
            Int32 idUser = View.CurrentIdUser;
            Person owner = CurrentManager.GetPerson(idUser);
            if (owner == null)
                View.DisplayUnknownUser();
            else {
                List<dtoUserPolicyInfo> items = View.GetItemsValue();
                try
                {
                    result = Service.SaveUserSelection(owner, items);
                }
                catch (MandatoryError ex)
                {
                    View.DisplayItemsToAccept(ex.Items);
                }
                catch (Exception ex) { 
                }
            }
            return result;
        }
    }
}