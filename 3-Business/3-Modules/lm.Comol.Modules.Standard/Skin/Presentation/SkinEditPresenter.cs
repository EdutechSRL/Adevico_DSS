using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;


namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinEditPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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


            public SkinEditPresenter(iApplicationContext oContext, iViewSkinEdit view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }


            protected virtual iViewSkinEdit View
            {
                get { return (iViewSkinEdit)base.View; }
            }
        #endregion

            public void Bind(Int64 SkinId)
            {
                this.View.SkinData = Service.GetSkinData(SkinId);
            }

            public void Save()
            {
                this.Service.UpdateBaseData(this.View.SkinData);
            }
    }
}
