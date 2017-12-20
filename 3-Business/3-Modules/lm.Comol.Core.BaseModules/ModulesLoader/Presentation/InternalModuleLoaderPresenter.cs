using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.ModulesLoader.Presentation
{
    public class InternalModuleLoaderPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        //private int _ModuleID;
        //private ServiceCommunityRepository _Service;

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
        protected virtual IViewInternalModuleLoader View
        {
            get { return (IViewInternalModuleLoader)base.View; }
        }

        public InternalModuleLoaderPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public InternalModuleLoaderPresenter(iApplicationContext oContext, IViewInternalModuleLoader view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView(){
            if (this.UserContext.isAnonymous)
                //  codice per effettuare il login automatico...
                this.View.NoPermissionToAccess();
            else{
                int IdCommunity = View.PreLoadedIdCommunity;
                String url = View.PreLoadedPageUrl;
                Boolean decodeUrl = true;
                if (String.IsNullOrEmpty(url)){
                    url = View.PreLoadedPlainPageUrl;
                    decodeUrl=false;
                }
                if (IdCommunity==0)
                    View.NavigateToUrl(url, decodeUrl);
                else{
                    Community  community = CurrentManager.GetCommunity(IdCommunity);
                    if (community == null && IdCommunity==0) 
                        View.ShowNoCommunityAccess(View.PortalName);
                    else if (CurrentManager.HasActiveSubscription(UserContext.CurrentUserID,IdCommunity))
                        if (IdCommunity== UserContext.CurrentCommunityID)
                            View.NavigateToUrl(url, decodeUrl);
                        else
                            View.NavigateToCommunityUrl(UserContext.CurrentUserID, IdCommunity, url, decodeUrl);
                    else if (community!=null)
                        View.ShowNoCommunityAccess(community.Name);
                    View.PreviousUrl = (decodeUrl) ? View.DecodeUrl(url) : url ;
                }
            }
        }
    }
}