using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinFooterTextPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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

            public SkinFooterTextPresenter(iApplicationContext oContext, IViewSkinFooterText view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            protected virtual IViewSkinFooterText View
            {
                get { return (IViewSkinFooterText)base.View; }
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
                    View.LoadItems(Service.GetFooterTexts(idSkin));
            }

            public void SaveFooterText(long idSkin, String languageCode, String text)
            {
                Service.UpdateText(idSkin, languageCode, text);
                View.LoadItems(Service.GetFooterTexts(idSkin));
            }


    }
}