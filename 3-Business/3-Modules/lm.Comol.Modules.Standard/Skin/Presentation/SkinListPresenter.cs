using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using Entity = Comol.Entity;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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


        public SkinListPresenter(iApplicationContext oContext, iViewSkinList view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }


        protected virtual iViewSkinList View
        {
            get { return (iViewSkinList)base.View; }
        }
        #endregion

        public void RefreshList(Domain.SkinShareType Type)
        {
            View.BindSkins(Service.GetSkinsList(Type));
        }

        public void EraseSkin(Int64 SkinId)
        {
            Service.DELETE_Skin(SkinId, View.BasePath);

        }

        public void RemPortal(Int64 SkinId)
        {
            Service.RemPortal(SkinId);
        }

        public void RemComAss(Int64 SkinId, Int32 ComId)
        {
            Service.RemComAss(SkinId, ComId);
        }

        public void RemOrgnAss(Int64 SkinId, Int32 OrgnId)
        {
            Service.RemOrgnAss(SkinId, OrgnId);
        }

        public void Copyskin(Int64 SkinId)
        {
            Service.CopySkin(SkinId, View.BasePath);
        }


        public Domain.HTML.HTMLSkin Test(
            Int32 CommunityId,
            Int32 OrganizationId,
            String VirtualPath,
            String LangCode,
            String DefLangCode,
            Entity.Configuration.SkinSettings Settings,
            String AppBaseUrl
            )
        {
            return Service.GetSkinHTML(CommunityId, OrganizationId, VirtualPath, LangCode, DefLangCode, Settings, AppBaseUrl);
        }

        public void ClearCache()
        {
            Service.CleanCache();
        }

        public string TestCss(Int32 CommunityId, Int32 OrganizationId, String VirtualPath, String DefLangCode, Business.SkinFileManagement.CssType Type, String BaseAppPath, Entity.Configuration.SkinSettings DEF_SkinSettings)
        {
            
            return Service.GetCSSHtml(CommunityId, OrganizationId, VirtualPath, DefLangCode, Type, BaseAppPath, DEF_SkinSettings);
        }
    }
}
