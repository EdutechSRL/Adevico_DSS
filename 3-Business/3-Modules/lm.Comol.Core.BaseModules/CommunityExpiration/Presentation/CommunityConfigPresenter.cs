using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Presentation
{
    public class CommunityConfigPresenter : DomainPresenter
    {
        #region "Initialize"

        public virtual Core.Business.BaseModuleManager CurrentManager { get; set; }

        private Business.ComExpirationService _service;
        private Business.ComExpirationService Service
        {
            get
            {
                if (_service == null)
                    _service = new Business.ComExpirationService(AppContext, View.SysConfig);

                return _service;
            }
        }


        protected virtual iViewCommunityConfig View
        {
            get { return (iViewCommunityConfig)base.View; }
        }

        public CommunityConfigPresenter(iApplicationContext oContext) : base(oContext)
        {
            this.CurrentManager = new Core.Business.BaseModuleManager(oContext);
        }

        public CommunityConfigPresenter(iApplicationContext oContext, iDomainView view) : base(oContext, view)
        {
            this.CurrentManager = new Core.Business.BaseModuleManager(oContext);
        }

        #endregion

        public void initView()
        {
            if (CommunityId <= 0)
                return;

            if(!Service.HasCommunityPermission(CommunityId, UserContext.CurrentUserID))
            {
                View.ShowNoPermission();
                return;
            }


            View.CurrentCommunityName = CurrentManager.GetCommunityName(CommunityId);

            View.Configs = Service.GetCommunityExpirationConfig(
                CommunityId,
                UserContext.Language.Id,
                UserContext.Language.Code
                );

            //ToDo: SendUserAction      
        }

        public void Save()
        {
            if (CommunityId <= 0)
                return;

            if (!Service.HasCommunityPermission(CommunityId, UserContext.CurrentUserID))
            {
                View.ShowNoPermission();
                return;
            }

            int success = Service.SetExpirationsCommunity(View.Configs, CommunityId);

            if (success < 0)
            {
                View.ShowError();
            }
            else
            {
                initView();
                View.ShowSuccessSave();
            }
        }

        public int CommunityId
        {
            get
            {
                int CommunityId = View.CurrentCommunityId;

                if (CommunityId <= 0)
                {
                    CommunityId = UserContext.CurrentCommunityID;
                }

                if (CommunityId <= 0)
                {
                    View.ShowNoCommunity();
                }

                return CommunityId;
            }
        }

        public int ModuleId
        {
            get
            {
                return Service.ModuleId;
            }
        }

    }
}
