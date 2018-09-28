using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinImagesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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


        public SkinImagesPresenter(iApplicationContext oContext, IViewSkinImages view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }


        protected virtual IViewSkinImages View
        {
            get { return (IViewSkinImages)base.View; }
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
                LoadImages(idSkin);
        }

        public void DeleteImage(long idSkin,String imageName)
        {
            if (imageName != "")
                Business.SkinFileManagement.DeleteImage(View.BasePath, idSkin, imageName);
            LoadImages(idSkin);
        }

        public String ImageVirtualPath(long idSkin, String imageName)
        {
            return Business.SkinFileManagement.GetImageVirtualPath(View.VirtualFullPath, idSkin, imageName);
        }

        public String ImageFullPath(long idSkin, String imageName)
        {
            return Business.SkinFileManagement.GetImagePath(View.BasePath, idSkin) + "\\" + imageName;
        }

        public void LoadImages(long idSkin)
        {
            View.LoadItems(Business.SkinFileManagement.GetImages(View.BasePath, idSkin));
        }
    }
}
