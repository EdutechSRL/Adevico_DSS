using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation.Call
{
    public class CallPrintSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

        private ServiceCallOfPapers _Service;
        public virtual BaseModuleManager CurrentManager { get; set; }

        protected virtual IViewCallPrintSettings View
        {
            get { return (IViewCallPrintSettings)base.View; }
        }

        private ServiceCallOfPapers Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceCallOfPapers(AppContext);
                return _Service;
            }
        }

        public CallPrintSettingsPresenter(iApplicationContext oContext) : base(oContext)
        {
        }

        public CallPrintSettingsPresenter(iApplicationContext oContext, iDomainView view) : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }


        #endregion

        public void InitView(Int64 callId)
        {
            
            CallPrintSettings sets = Service.PrintSettingsGetFromCall(callId);

            View.Initialize(sets, Service.ServiceModuleID());

        }

        public void SaveSetting()
        {
            CallPrintSettings sets = new CallPrintSettings();
            View.UpdateSettings(ref sets);
            long SetId = Service.PrintSettingsSet(sets);

            if (SetId <= 0)
            {
                //save error
            }
            else
            {
                sets = Service.PrintSettingsGet(SetId);
                View.Initialize(sets, Service.ServiceModuleID());
            }
        }
    }
}
