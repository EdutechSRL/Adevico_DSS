using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;


namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinHeaderLogoPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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


        public SkinHeaderLogoPresenter(iApplicationContext oContext, IViewSkinHeaderLogo view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }


        protected virtual IViewSkinHeaderLogo View
        {
            get { return (IViewSkinHeaderLogo)base.View; }
        }
        #endregion

        public void InitView(long idSkin, Boolean allowEdit)
        {
            View.IdSkin = idSkin;
            View.AllowEdit = allowEdit;
            if (UserContext.isAnonymous)
            {
                View.LoadNoItems();
                View.AllowEdit = false;
            }
            else
                LoadLogos(idSkin);
        }

        public long SaveLogo(long idLogo, String logoImage, String alternateText, String link, String languageCode)
        {
            return Service.SaveHeadLogo(idLogo, logoImage, link, alternateText, View.IdSkin, languageCode, View.BasePath);
        }
        public void DeleteLogo(long idSkin, long idLogo)
        {
            Service.DelHeaderLogo(idLogo, View.BasePath, idSkin);
            LoadLogos(idSkin);
        }


        public String LogoFullPath(long idLogo, String logoImage)
        {
            return Business.SkinFileManagement.GetLogoFullPath(View.BasePath, View.IdSkin, idLogo, logoImage);
        }

        public String LogoVirtualPath(long idLogo, String logoImage)
        {
            return Business.SkinFileManagement.GetLogoVirtualPath(View.IdSkin, idLogo, logoImage);
        }

        public String LogoVirtualFullPath(long idLogo, String logoImage)
        {
            return Business.SkinFileManagement.GetLogoVirtualFullPath(View.VirtualFullPath, View.IdSkin, idLogo, logoImage);
        }

        public void CloneDefault(long idSkin,long destIdLogo, String destLanguageCode)
        {
            Service.CloneLogo(idSkin, View.IdLogoDefaultLanguage, destIdLogo, View.BasePath, destLanguageCode);
            LoadLogos(idSkin);
        }
        public void LoadLogos(long idSkin)
        {
            
            View.LoadItems(Service.GetDtoHederLogos(idSkin));
        }

        public void ClearCache()
        {
            Service.CleanCache();
        }
    }
}
