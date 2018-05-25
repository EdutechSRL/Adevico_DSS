using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Presentation
{
    public class PortalConfigPresenter : DomainPresenter
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


        protected virtual iViewPortalConfig View
        {
            get { return (iViewPortalConfig)base.View; }
        }

        public PortalConfigPresenter(iApplicationContext oContext) : base(oContext)
        {
            this.CurrentManager = new Core.Business.BaseModuleManager(oContext);
        }

        public PortalConfigPresenter(iApplicationContext oContext, iDomainView view) : base(oContext, view)
        {
            this.CurrentManager = new Core.Business.BaseModuleManager(oContext);
        }

        #endregion

        public void initView(int NewTypeId = -1)
        {
            //Check permission!

            if(View.CurrentCommunityTypeId < 0)
            {
                InitCommunityType();
            }

            if(NewTypeId >= 0)
            {
                View.CurrentCommunityTypeId = NewTypeId;
            }

            View.Configs = Service.GetPortalExpirationConfig(
                View.CurrentCommunityTypeId, 
                UserContext.Language.Id, 
                UserContext.Language.Code
                );

            //SendUserAction

        }

        private void InitCommunityType()
        {
            IList<dtoTranslatedCommunityType> ComTypes = CurrentManager.GetTranslatedCommunityTypes(UserContext.Language.Id);

            View.CommunityTypes = (from dtoTranslatedCommunityType ct in ComTypes
                                   select new KeyValuePair<int, string>(ct.Id, ct.Name)).ToList();
            
            View.CurrentCommunityTypeId = ComTypes.FirstOrDefault().Id;
        }

        public void Save(int NewTypeId = -1)
        {
            int success = Service.SetExpirationsPortal(View.Configs, View.CurrentCommunityTypeId);

            if (success < 0)
            {
                View.ShowError();
            } else
            {
                initView(NewTypeId);
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
