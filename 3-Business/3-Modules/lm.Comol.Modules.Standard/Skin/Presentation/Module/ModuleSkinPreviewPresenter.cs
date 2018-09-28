using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.Skin.Domain;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class ModuleSkinPreviewPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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

            public ModuleSkinPreviewPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleSkinPreviewPresenter(iApplicationContext oContext, IViewSkinPreview view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            protected virtual IViewSkinPreview View
            {
                get { return (IViewSkinPreview)base.View; }
            }
        #endregion

            public void InitView(Int32 idModule,Int32 idCommunity, long idItem, SkinDisplayType itemType)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                    LoadSkins(idModule, idCommunity,idItem, itemType);
            }
            private void LoadSkins(Int32 idModule, Int32 idCommunity, long idItem, SkinDisplayType itemType)
            {
                String code = Service.GetModuleCode(idModule);
                View.CurrentModule = code;
                View.DisplayContentByService(code);

                View.InitializeSkin(Service.GetItemSkinSettings(idCommunity,idItem,itemType));
            }
    }
 }