using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinFooterLogoPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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

            public SkinFooterLogoPresenter(iApplicationContext oContext, IViewSkinFooterLogo view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            protected virtual IViewSkinFooterLogo View
            {
                get { return (IViewSkinFooterLogo)base.View; }
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
                    View.LoadItems(Service.GetFooterLogosDto(idSkin));
            }

            public void LoadLogos(long idSkin)
            {
                View.LoadItems(Service.GetFooterLogosDto(idSkin));
            }
            public String LogoFullPath(long idLogo, String imageName)
            {
                return Business.SkinFileManagement.GetLogoFullPath(View.BasePath, View.IdSkin, idLogo, imageName);
            }
            public String LogoVirtualPath(long idLogo, String imageName)
            {
                return Business.SkinFileManagement.GetLogoVirtualPath(View.IdSkin, idLogo, imageName);
            }
            public String LogoVirtualFullPath(long idLogo, String imageName)
            {
                return Business.SkinFileManagement.GetLogoVirtualFullPath(View.VirtualFullPath, View.IdSkin, idLogo, imageName);
            }
            public long CreateNewLogo(String imageName, String link, String toolTip, IList<String> assLang, int displayOrder)
            {
                return Service.CreateNewFooterLogo(View.IdSkin, imageName, link, toolTip, assLang, displayOrder);
            }
            public void UpdateImage(long idLogo, String imageName)
            {
                Service.UpdateFooterLogoName(idLogo, imageName);
                View.LoadItems(Service.GetFooterLogosDto(View.IdSkin));
            }
            public void UpdateLogo(Int64 idLogo, String imageName, String link, String toolTip, IList<String> assLang, int displayOrder, bool OverrideVoidFooterLogos)
            {
                Service.UpdateFooterLogo(View.IdSkin, idLogo, imageName, link, toolTip, assLang, displayOrder, View.BasePath, OverrideVoidFooterLogos);
                View.LoadItems(Service.GetFooterLogosDto(View.IdSkin));
            }
            public void DeleteLogo(long idLogo)
            {
                Service.DeleteFooterLogo(idLogo, View.IdSkin, View.BasePath);
                View.LoadItems(Service.GetFooterLogosDto(View.IdSkin));
            }
    }
}
