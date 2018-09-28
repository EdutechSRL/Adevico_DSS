using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinSharePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        
        #region "Initalize"
        private Business.ServiceSkin _Service;
        private Business.ServiceSkin Service
        {
            get
            {
                if (_Service == null)
                    _Service = new Business.ServiceSkin(AppContext);
                return _Service;
            }
        }

        public SkinSharePresenter(iApplicationContext oContext, iViewSkinShare view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }


        protected virtual iViewSkinShare View
        {
            get { return (iViewSkinShare)base.View; }
        }
        #endregion

        public void BindMainList()
        {
            View.BindMainList(Service.GetAllShares(View.SkinId));
        }

        public void BindOrganizationList()
        {
            View.BindOrganizations(Service.GetOrganizationList(View.SkinId));
        }

        public void BindCommunity()
        {
            View.BindCommunities(Service.GetCommunitiesId(View.SkinId));
        }

        public void SetPortal()
        {
            this.Service.SetPortal(View.SkinId);
        }

        public void RemPortal()
        {
            this.Service.RemPortal(View.SkinId);
        }

        public void SetOrganization(IList<Int32> OrganizationIDs)
        {
            this.Service.SetOrganizationAss(View.SkinId, OrganizationIDs);
        }

        //public void SetCommunities(IList<Int32> CommunitiesIDs)
        //{
        //    this.Service.SetCommunitiesAss(View.SkinId, CommunitiesIDs);
        //}
        public void AddCommunities(IList<Int32> CommunitiesIDs)
        {
            this.Service.SetCommunitiesAss(View.SkinId, CommunitiesIDs);
        }

        public void RemOrganizationShare(Int32 OrganizationId)
        {
            this.Service.RemOrgnAss(View.SkinId, OrganizationId);
        }

        public void RemCommunityShare(Int32 CommunityId)
        {
            this.Service.RemComAss(View.SkinId, CommunityId);
        }

    }
}
