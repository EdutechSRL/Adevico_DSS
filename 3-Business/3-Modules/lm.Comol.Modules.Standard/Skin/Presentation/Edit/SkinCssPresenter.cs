using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinCssPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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


        public SkinCssPresenter(iApplicationContext oContext, IViewSkinCSS view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }


        protected virtual IViewSkinCSS View
        {
            get { return (IViewSkinCSS)base.View; }
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
                LoadCss(idSkin);
        }

        public void LoadCss(long idSkin){
            Domain.DTO.DtoSkinCss dto = Service.GetSkinCss(idSkin);
            String path = View.VirtualFullPath + "/" + idSkin.ToString() ;
            dto.MainCssUrl = Business.SkinFileManagement.GetFullCssName(path, Business.SkinFileManagement.CssType.Main, true);
            dto.IeCssUrl = Business.SkinFileManagement.GetFullCssName(path, Business.SkinFileManagement.CssType.IE, true);
            dto.AdminCssUrl = Business.SkinFileManagement.GetFullCssName(path, Business.SkinFileManagement.CssType.Admin, true);
            dto.LoginCssUrl = Business.SkinFileManagement.GetFullCssName(path, Business.SkinFileManagement.CssType.Login, true);

            View.LoadItems(dto);
        }

        public void UpdateCss(long idSkin, String name, Business.SkinFileManagement.CssType type)
        {
            UpdateCss(idSkin, name, type, true);
        }

        public void UpdateCss(long idSkin, String name, Business.SkinFileManagement.CssType type, Boolean Reload)
        {
            switch (type) { 
                case Business.SkinFileManagement.CssType.Main:
                    Service.UpdateCssMain(idSkin, name);
                    break;
                case Business.SkinFileManagement.CssType.Admin:
                    Service.UpdateCssAdmin(idSkin, name);
                    break;
                case  Business.SkinFileManagement.CssType.IE:
                    Service.UpdateCssIE(idSkin, name);
                    break;
                case Business.SkinFileManagement.CssType.Login:
                    Service.UpdateCssLogin(idSkin, name);
                    break;
            }
            
            if (Reload)
                LoadCss(idSkin);

        }

        public void DeleteCss(long idSkin, Business.SkinFileManagement.CssType type)
        {
            Business.SkinFileManagement.DelCssFile(View.SkinPath, type);
            UpdateCss(idSkin, "", type);
        }

        public String GetCssName(Business.SkinFileManagement.CssType type)
        {
            return Business.SkinFileManagement.GetFullCssName(View.SkinPath, type, false);
        }
    }
}
